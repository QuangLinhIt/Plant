using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class ShopFeedback
    {
        public ShopFeedback()
        {
            Feedbacks = new HashSet<Feedback>();
        }

        public int ShopFeedbackId { get; set; }
        public string ShopFeedbackContent { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
