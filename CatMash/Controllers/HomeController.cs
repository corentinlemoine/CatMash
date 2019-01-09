using CatMash.Data;
using CatMash.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CatMash.Controllers
{
    public class HomeController : Controller
    {
        private HomeManager _HM;

        public HomeController(ApplicationDbContext context)
        {
            _HM = new HomeManager(context);
        }

        public IActionResult Index()
        {
            //FirstInit();

            return View(_HM.GetNextCats());
        }

        // A modifier pour faire des pages de ~20 et faire une fonction pour les 20 prochain etc
        public IActionResult Scores()
        {
            return View(_HM.GetScoreModelView());
        }

        [HttpPost]
        public IActionResult AddScoreToCat([FromBody] string catID)
        {
            var result = new NextCats();

            _HM.SaveCat(catID);

            return new JsonResult(_HM.GetNextCats());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
