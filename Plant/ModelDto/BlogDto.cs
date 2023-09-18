using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.ModelDto
{
    public class BlogDto
    {
        public int BlogId { get; set; }
        public string ImageName { get; set; }
        public IFormFile ImageFile { get; set; }
        public int LangId { get; set; }
        public string LangName { get; set; }
        public string Title { get; set; }
        public string ShortDes { get; set; }
        public string Description { get; set; }
        public List<SelectListItem> ListCategoryNews { get; set; }
        public int[] CategoryNewIds { get; set; }
    }
}
