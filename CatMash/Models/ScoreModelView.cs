using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatMash.Models
{
    public class ScoreModelView
    {
        public List<ScoreCat> Cats { get; set; }

        public ScoreModelView()
        {
            Cats = new List<ScoreCat>();
        }
    }
}
