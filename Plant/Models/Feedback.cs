using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class Feedback
    {
        public Feedback()
        {
            Orders = new HashSet<Order>();
        }

        public int FeedbackId { get; set; }
        public string FeedbackContent { get; set; }
        public string StoreFeedback { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
