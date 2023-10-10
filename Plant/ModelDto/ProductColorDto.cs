using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.ModelDto
{
    public class ProductColorDto
    {
        public int ProductColorId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int LangId { get; set; }
        public string Color { get; set; }
        public int Stock { get; set; }

    }
}
