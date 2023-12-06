using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.ViewModels
{
    public class ListCartVm
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public int FeedbackId { get; set; }
        public int Star { get; set; }
    }
}
