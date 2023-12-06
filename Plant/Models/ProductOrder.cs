using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class ProductOrder
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int? FeedbackId { get; set; }
        public string Color { get; set; }

        public virtual Feedback Feedback { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
