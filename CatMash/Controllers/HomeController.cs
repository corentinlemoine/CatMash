using CatMash.Data;
using CatMash.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;

namespace CatMash.Controllers
{
    public class HomeController : Controller
    {
        public Random rand = new Random();
        private ApplicationDbContext _context;

        private int _id1Selected = 0;
        private int _id2Selected = 0;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cats = GetAllCats();
            if (FirstInit(cats))
                cats = GetAllCats();

            int id1 = 0;
            int id2 = 0;

            while (id1 == id2)
            {
                id1 = rand.Next(0, cats.Count);
                id2 = rand.Next(0, cats.Count);
            }

            int votes = 0;
            foreach (var c in cats)
                votes += c.Score;

            _id1Selected = id1;
            _id2Selected = id2;

            return View(new VoteModelView() { Cats = cats, SelectedCats = new List<Cat>() { cats[id1], cats[id2] }, Votes = votes });
        }

        public IActionResult Scores()
        {
            var cats = GetAllCats();
            cats.Sort((x, y) => y.Score.CompareTo(x.Score));

            return View(new VoteModelView() { Cats = cats });
        }

        private bool FirstInit(List<Cat> catsDB)
        {
            string json = System.IO.File.ReadAllText("./Data/cats.json");
            var cats = JsonConvert.DeserializeObject<JSONCats>(json);

            // Ajoute les scores pour ne pas les perdres en cas de reset
            foreach (var cDB in catsDB)
                foreach (var c in cats.Images)
                    if (c.ID.Equals(cDB))
                    {
                        c.Score = cDB.Score;
                        break;
                    }

            if (catsDB.Count < cats.Images.Count)
            {
                foreach (var c in cats.Images)
                    Save(c);
                return true;
            }
            return false;
        }

        public List<Cat> GetAllCats()
        {
            return _context.Cat.ToList();
        }

        public void Save(Cat cat)
        {
            var catsInDB = _context.Cat.ToList();

            bool b = false;
            foreach (var c in catsInDB)
            {
                if (c.ID.Equals(cat.ID))
                {
                    c.Score = cat.Score;
                    b = true;
                }
            }
            if (!b)
                _context.Add(cat);

            _context.SaveChanges();
        }

        [HttpPost]
        public IActionResult AddScoreToCat()
        {
            var result = new List<Cat>();

            try
            {
                var json = "";
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    json = reader.ReadToEnd();
                }

                var catID = JsonConvert.DeserializeObject<Cat>(json);

                // Modifier le score
                var cats = GetAllCats();
                foreach (var c in cats)
                    if (c.ID.Equals(catID.ID))
                    {
                        c.Score++;
                        Save(c);
                        break;
                    }

                // Les prochaines images
                int id1 = 0;
                int id2 = 0;

                while (id1 == id2 || (id1 == _id1Selected && id2 == _id2Selected))
                {
                    id1 = rand.Next(0, cats.Count);
                    id2 = rand.Next(0, cats.Count);
                }

                _id1Selected = id1;
                _id2Selected = id2;

                // Pr mettre à jours le nombre de vote
                int votes = 0;
                foreach (var c in cats)
                    votes += c.Score;

                result = new List<Cat>() { cats[id1], cats[id2], new Cat() { Score = votes } };
            }
            catch { }

            return new JsonResult(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
