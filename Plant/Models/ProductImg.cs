using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class ProductImg
    {
        public int ProductImgId { get; set; }
        public int ProductId { get; set; }
        public string Img { get; set; }

        public virtual Product Product { get; set; }
    }
}
