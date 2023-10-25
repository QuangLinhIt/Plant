using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.ModelDto
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public int LangId { get; set; }
        public string LangName { get; set; }
        public int? Voucher { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public string ShortDes { get; set; }
        public string Description { get; set; }
        public string TakeCare { get; set; }
        public string Application { get; set; }

        public IFormFile ImageFile { get; set; }
        public string ImageName { get; set; }
        public List<SelectListItem> ListCategory { get; set; }
        public int[] CategoryIds { get; set; }
    }
}
