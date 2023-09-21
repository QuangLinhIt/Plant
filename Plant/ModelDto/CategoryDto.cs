using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.ModelDto
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public int? ParentId { get; set; }
        public int CategoryTranslationId { get; set; }
        public int LangId { get; set; }
        public string LangName { get; set; }
        public string CategoryName { get; set; }

    }
}
