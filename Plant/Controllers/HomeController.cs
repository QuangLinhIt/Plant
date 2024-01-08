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
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly plantContext _context;

        public HomeController(plantContext context)
        {
            _context = context;
        }
        [HttpGet]
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
            //cập nhật số sao trung bình và số sản phẩm đã bán cho từng sản phẩm trong danh sách
            foreach (var item in product)
            {
                var listFeedback = (from f in _context.Feedbacks
                                    join po in _context.ProductOrders on f.FeedbackId equals po.FeedbackId
                                    where po.ProductId == item.ProductId && po.FeedbackId != null
                                    select new ReviewFeedbackVm()
                                    {
                                        ProductId = po.ProductId,
                                        FeedbackId = f.FeedbackId,
                                        Star = f.Star,
                                        Quantity = po.Quantity
                                    }).ToList();
                if (listFeedback.Count != 0)
                {
                    decimal totalProductSell = 0;
                    decimal averageStar = 0;
                    for (var k = 0; k < listFeedback.Count; k++)
                    {
                        totalProductSell += listFeedback[k].Quantity;
                    }
                    for (var i = 0; i < listFeedback.Count; i++)
                    {
                        averageStar += (decimal)(listFeedback[i].Star * listFeedback[i].Quantity) / totalProductSell;
                    }
                    item.AverageStar = averageStar;
                }
                else
                {
                    item.AverageStar = 0;
                }
                var productSell = (from p in _context.ProductOrders
                                   join o in _context.Orders on p.OrderId equals o.OrderId
                                   where p.ProductId == item.ProductId && o.OrderStatus == "Giao hàng thành công"
                                   select new ProductVm()
                                   {
                                       ProductId = p.ProductId,
                                       CountProductSell = p.Quantity
                                   }).ToList();
                foreach (var ps in productSell)
                {
                    item.CountProductSell += ps.CountProductSell;
                }
            }
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

        [HttpGet]
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
