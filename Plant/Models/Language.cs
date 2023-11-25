using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class Language
    {
        public Language()
        {
            BlogTranslations = new HashSet<BlogTranslation>();
            CategoryNewTranslations = new HashSet<CategoryNewTranslation>();
            CategoryTranslations = new HashSet<CategoryTranslation>();
            Orders = new HashSet<Order>();
            ProductTranslations = new HashSet<ProductTranslation>();
        }

        public int LangId { get; set; }
        public string LangName { get; set; }
        public string SignLanguages { get; set; }

        public virtual ICollection<BlogTranslation> BlogTranslations { get; set; }
        public virtual ICollection<CategoryNewTranslation> CategoryNewTranslations { get; set; }
        public virtual ICollection<CategoryTranslation> CategoryTranslations { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ProductTranslation> ProductTranslations { get; set; }
    }
}
