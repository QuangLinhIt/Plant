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
using PagedList.Core;
using Plant.Models;
using Plant.ViewModels;

namespace Plant.Controllers
{
    public class ProductsController : Controller
    {
        private readonly plantContext _context;

        public ProductsController(plantContext context)
        {
            _context = context;
        }

        // GET: Products
        public IActionResult Index(int page = 1)
        {
            //get selected languages
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            var culture = cultureInfo.Name;

            var pageNumber = page;
            var pageSize = 12;

            //hiển thị danh sách danh mục sản phẩm
            var category = (from c in _context.Categories
                            join ct in _context.CategoryTranslations on c.CategoryId equals ct.CategoryId
                            join l in _context.Languages on ct.LangId equals l.LangId
                            where l.SignLanguages == culture
                            select new CategoryVm()
                            {
                                CategoryId = c.CategoryId,
                                ParentId = c.ParentId,
                                CategoryTranslationId = ct.CategoryTranslationId,
                                LangId = l.LangId,
                                CategoryName = ct.CategoryName,
                                SignLanguages = l.SignLanguages
                            }).ToList();
            ViewData["Category"] = category;

            //hiển thị danh sách thông tin sản phẩm
            var result = (from p in _context.Products
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

            //cập nhật số sao trung bình và số sản phẩm đã bán cho từng sản phẩm trong danh sách
            foreach (var item in result)
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
                    item.CountProductSell = totalProductSell;
                    item.AverageStar = averageStar;
                }
                else
                {
                    item.CountProductSell = 0;
                    item.AverageStar = 0;
                }
            }
            PagedList<ProductVm> models = new PagedList<ProductVm>(result.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        //GET: ListProducts
        public IActionResult GetListProducts(int page = 1, int id = 0)
        {
            //get selected languages
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            var culture = cultureInfo.Name;

            var pageNumber = page;
            var pageSize = 12;

            //hiển thị danh sách danh mục sản phẩm
            var category = (from c in _context.Categories
                            join ct in _context.CategoryTranslations on c.CategoryId equals ct.CategoryId
                            join l in _context.Languages on ct.LangId equals l.LangId
                            where l.SignLanguages == culture
                            select new CategoryVm()
                            {
                                CategoryId = c.CategoryId,
                                ParentId = c.ParentId,
                                CategoryTranslationId = ct.CategoryTranslationId,
                                LangId = l.LangId,
                                CategoryName = ct.CategoryName,
                                SignLanguages = l.SignLanguages
                            }).ToList();
            ViewData["Category"] = category;

            //hiển thị danh sách thông tin sản phẩm
            var result = (from p in _context.Products
                          join pt in _context.ProductTranslations on p.ProductId equals pt.ProductId
                          join pc in _context.ProductCategories on p.ProductId equals pc.ProductId
                          join l in _context.Languages on pt.LangId equals l.LangId
                          where l.SignLanguages == culture && pc.CategoryId == id
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
                              CategoryId = pc.CategoryId
                          }).ToList();

            //cập nhật số sao trung bình và số sản phẩm đã bán cho từng sản phẩm trong danh sách
            foreach (var item in result)
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
                    item.CountProductSell = totalProductSell;
                    item.AverageStar = averageStar;
                }
                else
                {
                    item.CountProductSell = 0;
                    item.AverageStar = 0;
                }
            }

            PagedList<ProductVm> models = new PagedList<ProductVm>(result.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        //GET: DetailProduct
        public IActionResult GetDetailProduct(int id = 0, int htmlStar = 0,int page=1)
        {
            var pageNumber = page;
            var pageSize = 5;

            ViewData["CurrentChooseStar"] = htmlStar;
            //get selected languages
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            var culture = cultureInfo.Name;
            var language = _context.Languages.Where(x => x.SignLanguages == culture).FirstOrDefault();

            //thông tin sản phẩm
            var result = (from p in _context.Products
                          join pt in _context.ProductTranslations on p.ProductId equals pt.ProductId
                          join l in _context.Languages on pt.LangId equals l.LangId
                          where l.SignLanguages == culture && p.ProductId == id
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
                              Description = pt.Description,
                              TakeCare = pt.TakeCare,
                              Application = pt.Application
                          }).FirstOrDefault();

            //danh sách phản hồi của sản phẩm
            var listFeedback = (from f in _context.Feedbacks
                                join po in _context.ProductOrders on f.FeedbackId equals po.FeedbackId
                                join p in _context.Products on po.ProductId equals p.ProductId
                                join pt in _context.ProductTranslations on p.ProductId equals pt.ProductId
                                join o in _context.Orders on po.OrderId equals o.OrderId
                                join c in _context.Customers on o.CustomerId equals c.CustomerId
                                where po.ProductId == result.ProductId && po.FeedbackId != null && pt.LangId == language.LangId
                                select new ReviewFeedbackVm()
                                {
                                    FirstName = c.FirstName,
                                    LastName = c.LastName,
                                    CreateDay = f.CreateDay,
                                    ProductOrderId = po.Id,
                                    ProductId = po.ProductId,
                                    ProductName = pt.ProductName,
                                    FeedbackId = f.FeedbackId,
                                    Star = f.Star,
                                    Quantity = po.Quantity,
                                    FeedBackContent = f.FeedbackContent,
                                    ShopFeedbackId = f.ShopFeedbackId,
                                    Color = po.Color
                                }).ToList();
            if (listFeedback.Count != 0)
            {
                result.CountFeedback = listFeedback.Count();
                foreach (var item in listFeedback)
                {
                    if (item.ShopFeedbackId != null)
                    {
                        var shopFeedback = _context.ShopFeedbacks.Where(x => x.ShopFeedbackId == item.ShopFeedbackId).FirstOrDefault();
                        item.ShopFeedbackContent = shopFeedback.ShopFeedbackContent;
                    }
                    var listFeedbackImg = new List<string>();
                    var feedbackImg = _context.FeedbackImages.Where(x => x.FeedbackId == item.FeedbackId).ToList();
                    foreach (var img in feedbackImg)
                    {
                        listFeedbackImg.Add(img.FeedbackImg);
                    }
                    item.ListImageName = listFeedbackImg.ToArray();
                }
                var listFeedbackHtml = new List<ReviewFeedbackVm>();
                foreach (var item in listFeedback)
                {
                    if (item.Star == htmlStar)
                    {
                        listFeedbackHtml.Add(item);
                    }
                }
                if (htmlStar == 0)
                {
                    var models = new PagedList<ReviewFeedbackVm>(listFeedback.AsQueryable(), pageNumber, pageSize);
                    ViewData["ListFeedback"] = models;
                }
                else
                {
                    var models = new PagedList<ReviewFeedbackVm>(listFeedbackHtml.AsQueryable(), pageNumber, pageSize);
                    ViewData["ListFeedback"] = models;
                }
                ViewData["CountListFeedback"] = listFeedback;
            }

            //từ danh sách đánh giá cập nhật số sao trung bình và số sản phẩm đã bán cho thông tin sản phẩm
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
                result.CountProductSell = totalProductSell;
                result.AverageStar = averageStar;
            }
            else
            {
                result.CountProductSell = 0;
                result.AverageStar = 0;
            }

            // hiển thị danh sách hình ảnh liên quan của sản phẩm
            var listImage = _context.ProductImgs.Where(x => x.ProductId == id).ToList();
            ViewData["ProductImg"] = listImage;

            //hiển thị danh sách màu sắc
            var listColor = _context.ProductColors.Where(x => x.ProductId == id).ToList();
            ViewData["ProductColor"] = listColor;

            //hiển thị danh sách sản phẩm khác
            var productDifferent = (from p in _context.Products
                                    join pt in _context.ProductTranslations on p.ProductId equals pt.ProductId
                                    join l in _context.Languages on pt.LangId equals l.LangId
                                    where l.SignLanguages == culture && p.ProductId != id
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
            foreach (var item in productDifferent)
            {
                var listFeedbackDifferent = (from f in _context.Feedbacks
                                             join po in _context.ProductOrders on f.FeedbackId equals po.FeedbackId
                                             where po.ProductId == item.ProductId && po.FeedbackId != null
                                             select new ReviewFeedbackVm()
                                             {
                                                 ProductId = po.ProductId,
                                                 FeedbackId = f.FeedbackId,
                                                 Star = f.Star,
                                                 Quantity = po.Quantity
                                             }).ToList();
                if (listFeedbackDifferent.Count != 0)
                {
                    decimal totalProductSellDifferent = 0;
                    decimal averageStarDifferent = 0;
                    for (var k = 0; k < listFeedbackDifferent.Count; k++)
                    {
                        totalProductSellDifferent += listFeedbackDifferent[k].Quantity;
                    }
                    for (var i = 0; i < listFeedbackDifferent.Count; i++)
                    {
                        averageStarDifferent += (decimal)(listFeedbackDifferent[i].Star * listFeedbackDifferent[i].Quantity) / totalProductSellDifferent;
                    }
                    item.CountProductSell = totalProductSellDifferent;
                    item.AverageStar = averageStarDifferent;
                }
                else
                {
                    item.CountProductSell = 0;
                    item.AverageStar = 0;
                }
            }

            ViewData["ProductDifferent"] = productDifferent;
            return View(result);
        }
    }
}
