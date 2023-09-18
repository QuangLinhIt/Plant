using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class BlogCategory
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int CategoryNewId { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual CategoryNews CategoryNew { get; set; }
    }
}
