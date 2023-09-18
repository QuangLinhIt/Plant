using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductCategories = new HashSet<ProductCategory>();
            ProductColors = new HashSet<ProductColor>();
            ProductImgs = new HashSet<ProductImg>();
            ProductOrders = new HashSet<ProductOrder>();
            ProductTranslations = new HashSet<ProductTranslation>();
        }

        public int ProductId { get; set; }
        public int Voucher { get; set; }
        public string ProductImg { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
        public virtual ICollection<ProductColor> ProductColors { get; set; }
        public virtual ICollection<ProductImg> ProductImgs { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
        public virtual ICollection<ProductTranslation> ProductTranslations { get; set; }
    }
}
