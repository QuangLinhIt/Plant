using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class Feedback
    {
        public Feedback()
        {
            FeedbackImages = new HashSet<FeedbackImage>();
            ProductOrders = new HashSet<ProductOrder>();
        }

        public int FeedbackId { get; set; }
        public string FeedbackContent { get; set; }
        public DateTime? CreateDay { get; set; }
        public int Star { get; set; }
        public int? ShopFeedbackId { get; set; }

        public virtual ShopFeedback ShopFeedback { get; set; }
        public virtual ICollection<FeedbackImage> FeedbackImages { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
