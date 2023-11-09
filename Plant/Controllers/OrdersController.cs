using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Plant.Models;
using Plant.ViewModels;

namespace Plant.Controllers
{
    public class OrdersController : Controller
    {
        private readonly plantContext _context;

        public OrdersController(plantContext context)
        {
            _context = context;
        }

        // GET: Orders
        public IActionResult Cart()
        {
            return View();
        }
        public JsonResult GetProductById(int productId, string color)
        {
            //get selected languages
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            var culture = cultureInfo.Name;

            var product = (from p in _context.Products
                           join pc in _context.ProductColors on p.ProductId equals pc.ProductId
                           join pt in _context.ProductTranslations on p.ProductId equals pt.ProductId
                           join l in _context.Languages on pt.LangId equals l.LangId
                           where p.ProductId == productId && l.SignLanguages == culture && pc.Color==color
                           select new ProductVm()
                           {
                               ProductId = p.ProductId,
                               Image = p.Image,
                               ProductName = pt.ProductName,
                               Price=p.Price,
                               OriginalPrice=p.OriginalPrice,
                               Color=pc.Color,
                               Stock=pc.Stock
                           }).FirstOrDefault();
            return Json(product);
        }
        public IActionResult CartDone()
        {
            return View();
        }
    }
}
