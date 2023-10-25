using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class ProductTranslation
    {
        public int ProductTranslationId { get; set; }
        public int LangId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public string ShortDes { get; set; }
        public string Description { get; set; }
        public string TakeCare { get; set; }
        public string Application { get; set; }

        public virtual Language Lang { get; set; }
        public virtual Product Product { get; set; }
    }
}
