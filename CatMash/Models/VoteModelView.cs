using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatMash.Models
{
    public class VoteModelView
    {
        public List<Cat> Cats { get; set; }
        public List<Cat> SelectedCats { get; set; }
        public int Votes { get; set; }
    }
    public class MyTestModel
    {
        public int ID { get; set; }

    }
}
