using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class FeedbackImage
    {
        public int Id { get; set; }
        public int FeedbackId { get; set; }
        public string FeedbackImg { get; set; }

        public virtual Feedback Feedback { get; set; }
    }
}
