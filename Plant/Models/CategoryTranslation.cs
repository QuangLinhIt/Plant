using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class CategoryTranslation
    {
        public int CategoryTranslationId { get; set; }
        public int LangId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual Category Category { get; set; }
        public virtual Language Lang { get; set; }
    }
}
