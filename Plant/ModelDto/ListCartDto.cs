using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.ModelDto
{
    public class ListCartDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImg { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
    }
}
