using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class Blog
    {
        public Blog()
        {
            BlogCategories = new HashSet<BlogCategory>();
            BlogTranslations = new HashSet<BlogTranslation>();
        }

        public int BlogId { get; set; }
        public string BlogImg { get; set; }

        public virtual ICollection<BlogCategory> BlogCategories { get; set; }
        public virtual ICollection<BlogTranslation> BlogTranslations { get; set; }
    }
}
