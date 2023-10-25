using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.ViewModels
{
    public class CategoryVm
    {
        public int CategoryId { get; set; }
        public int? ParentId { get; set; }
        public int CategoryTranslationId { get; set; }
        public int LangId { get; set; }
        public string CategoryName { get; set; }
        public string SignLanguages { get; set; }

    }
}
