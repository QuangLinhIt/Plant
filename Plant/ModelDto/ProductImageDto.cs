using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.ModelDto
{
    public class ProductImageDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int LangId { get; set; }
        public List<IFormFile> ListImageFile { get; set; }
        public string[] ListImageName { get; set; }
    }
}
