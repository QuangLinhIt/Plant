using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class CategoryNewTranslation
    {
        public int CategoryNewTranslationId { get; set; }
        public int CategoryNewId { get; set; }
        public int LangId { get; set; }
        public string CategoryNewName { get; set; }

        public virtual CategoryNews CategoryNew { get; set; }
        public virtual Language Lang { get; set; }
    }
}
