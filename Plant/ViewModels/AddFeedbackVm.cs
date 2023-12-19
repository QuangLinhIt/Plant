using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.ViewModels
{
    public class AddFeedbackVm
    {
        public int ProductOrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Color { get; set; }
        public string ProductImage { get; set; }
        public int? FeedbackId { get; set; }
        public int Star { get; set; }
        public string FeedBackContent { get; set; }
        public List<IFormFile> ListImageFile { get; set; }
    }
}
