using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.ViewModels
{
    public class CategoryNewViewModels
    {
        public int CategoryNewId { get; set; }
        public int? ParentNewId { get; set; }
        public int CategoryNewTranslationId { get; set; }
        public string CategoryNewName { get; set; }
        public int LangId { get; set; }
        public string SignLanguages { get; set; }
    }
}
