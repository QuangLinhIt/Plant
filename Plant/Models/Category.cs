using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class Category
    {
        public Category()
        {
            CategoryTranslations = new HashSet<CategoryTranslation>();
            ProductCategories = new HashSet<ProductCategory>();
        }

        public int CategoryId { get; set; }
        public int? ParentId { get; set; }

        public virtual ICollection<CategoryTranslation> CategoryTranslations { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
