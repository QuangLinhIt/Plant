using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class ProductColor
    {
        public int ProductColorId { get; set; }
        public int ProductId { get; set; }
        public string Color { get; set; }
        public int Stock { get; set; }

        public virtual Product Product { get; set; }
    }
}
