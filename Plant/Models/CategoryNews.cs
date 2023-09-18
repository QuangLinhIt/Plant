using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class CategoryNews
    {
        public CategoryNews()
        {
            BlogCategories = new HashSet<BlogCategory>();
            CategoryNewTranslations = new HashSet<CategoryNewTranslation>();
        }

        public int CategoryNewId { get; set; }
        public int? ParentNewId { get; set; }

        public virtual ICollection<BlogCategory> BlogCategories { get; set; }
        public virtual ICollection<CategoryNewTranslation> CategoryNewTranslations { get; set; }
    }
}
