using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.ModelDto
{
    public class FeedbackDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? CreateDay { get; set; }
        public string ProductName { get; set; }
        public string Color { get; set; }
        public string ProductImage { get; set; }
        public int? FeedbackId { get; set; }
        public int Star { get; set; }
        public string FeedBackContent { get; set; }
        public string[] ListImageName { get; set; }
        public int? ShopFeedbackId { get; set; }
        public string ShopFeedbackContent { get; set; }
    }
}
