using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.ViewModels
{
    public class BlogVm
    {
        public int BlogId { get; set; }
        public string BlogImg { get; set; }
        public int LangId { get; set; }
        public string SignLanguages { get; set; }
        public string Title { get; set; }
        public string ShortDes { get; set; }
        public string Description { get; set; }
        public int CategoryNewId{ get; set; }
    }
}
