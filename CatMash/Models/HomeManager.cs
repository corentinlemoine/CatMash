using CatMash.Data;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace CatMash.Models
{
    public class HomeManager
    {
        public Random rand = new Random();
        private ApplicationDbContext _context;

        public HomeManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool FirstInit()
        {
            string json = System.IO.File.ReadAllText("./Data/cats.json");
            var cats = JsonConvert.DeserializeObject<JSONCats>(json);

            foreach (var cat in cats.Images)
                if (_context.Cat.Select(c => c.ID.Equals(cat.ID)).Count() <= 0)
                    _context.Cat.Add(cat);

            return false;
        }

        public NextCats GetNextCats()
        {
            var catCount = _context.Cat.Count();
            // Les prochaines images
            int id1 = 0;
            int id2 = 0;

            while (id1 == id2)
            {
                id1 = rand.Next(0, catCount);
                id2 = rand.Next(0, catCount);
            }

            var c1 = _context.Cat.AsEnumerable().ElementAt(id1);
            var c2 = _context.Cat.AsEnumerable().ElementAt(id2);

            // Pr mettre à jours le nombre de vote
            NextCat nc1 = new NextCat() { ID = c1.ID, URL = c1.URL };
            NextCat nc2 = new NextCat() { ID = c2.ID, URL = c2.URL };

            return new NextCats()
            {
                NextCat1 = nc1,
                NextCat2 = nc2,
                Votes = _context.Cat.Sum(c => c.Score)
            };
        }

        public ScoreModelView GetScoreModelView()
        {
            var scoreCats = new ScoreModelView();

            foreach (var c in _context.Cat.OrderByDescending(c => c.Score).AsEnumerable())
                scoreCats.Cats.Add(new ScoreCat() { URL = c.URL, Score = c.Score });

            return scoreCats;
        }

        public bool SaveCat(string catID)
        {
            try
            {
                var cat = _context.Cat.Single(c => c.ID.Equals(catID)).Score++;
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
