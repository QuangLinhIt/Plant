using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class BlogTranslation
    {
        public int BlogTranslationId { get; set; }
        public int BlogId { get; set; }
        public int LangId { get; set; }
        public string Title { get; set; }
        public string ShortDes { get; set; }
        public string Description { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual Language Lang { get; set; }
    }
}
