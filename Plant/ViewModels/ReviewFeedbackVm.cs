using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.ViewModels
{
    public class ReviewFeedbackVm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? CreateDay { get; set; }
        public int ProductOrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Color { get; set; }
        public string ProductImage { get; set; }
        public int? FeedbackId { get; set; }
        public int Quantity { get; set; }
        public int Star { get; set; }
        public string FeedBackContent { get; set; }
        public string[] ListImageName { get; set; }
        public int? ShopFeedbackId { get; set; }
        public string ShopFeedbackContent { get; set; }
    }
}
