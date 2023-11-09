using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class Order
    {
        public Order()
        {
            ProductOrders = new HashSet<ProductOrder>();
        }

        public int OrderId { get; set; }
        public int LangId { get; set; }
        public int CustomerId { get; set; }
        public int PaymentId { get; set; }
        public int PaymentStatus { get; set; }
        public int? FeedbackId { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal Money { get; set; }
        public decimal ShipFee { get; set; }
        public decimal Total { get; set; }
        public string OrderStatus { get; set; }
        public bool Deleted { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Feedback Feedback { get; set; }
        public virtual Language Lang { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
