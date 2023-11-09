using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Plant.Models;
using Plant.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Plant.Controllers
{
    public class HomeController : Controller
    {
        private readonly plantContext _context;

        public HomeController(plantContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //get selected languages
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            var culture = cultureInfo.Name;
           
            // value languages= extractedCulture
            var product = (from p in _context.Products
                           join pt in _context.ProductTranslations on p.ProductId equals pt.ProductId
                           join l in _context.Languages on pt.LangId equals l.LangId
                           where l.SignLanguages == culture
                           select new ProductVm()
                           {
                               ProductId = p.ProductId,
                               Image = p.Image,
                               Voucher = p.Voucher,
                               ProductTranslationId = pt.ProductTranslationId,
                               LangId = l.LangId,
                               SignLanguages = l.SignLanguages,
                               ProductName = pt.ProductName,
                               Price = p.Price,
                               OriginalPrice = p.OriginalPrice,
                               ShortDes = pt.ShortDes,
                           }).ToList();
            ViewData["ListProduct"] = product;

            var blog = (from b in _context.Blogs
                        join bt in _context.BlogTranslations on b.BlogId equals bt.BlogId
                        join bc in _context.BlogCategories on b.BlogId equals bc.BlogId
                        join l in _context.Languages on bt.LangId equals l.LangId
                        where l.SignLanguages == culture
                        orderby b.BlogId descending
                        select new BlogVm()
                        {
                            BlogId = b.BlogId,
                            BlogImg = b.BlogImg,
                            LangId = l.LangId,
                            SignLanguages = l.SignLanguages,
                            Title = bt.Title,
                            ShortDes = bt.ShortDes,
                            Description = bt.Description,
                            CategoryNewId = bc.CategoryNewId,
                        }).ToList();
            ViewData["ListBlog"] = blog;
            return View();

        }
        public IActionResult Introduction()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
