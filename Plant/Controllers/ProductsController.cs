using System;
using System.Collections.Generic;
using System.Linq;
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
        public IActionResult Index( int page = 1)
        {
            //get cookie languages
            string culture = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            int cultureStartIndex = culture.IndexOf("c=") + 2;
            int cultureEndIndex = culture.IndexOf("|");
            string extractedCulture = culture.Substring(cultureStartIndex, cultureEndIndex - cultureStartIndex);
            // value languages= extractedCulture
            var pageNumber = page;
            var pageSize = 12;
           
                //view data list category start
                var category = (from c in _context.Categories
                                join ct in _context.CategoryTranslations on c.CategoryId equals ct.CategoryId
                                join l in _context.Languages on ct.LangId equals l.LangId
                                where l.SignLanguages == extractedCulture
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

                var result = (from p in _context.Products
                              join pt in _context.ProductTranslations on p.ProductId equals pt.ProductId
                              join l in _context.Languages on pt.LangId equals l.LangId
                              where l.SignLanguages == extractedCulture
                              select new ProductVm()
                              {
                                  ProductId = p.ProductId,
                                  Image = p.Image,
                                  Voucher = p.Voucher,
                                  ProductTranslationId = pt.ProductTranslationId,
                                  LangId = l.LangId,
                                  SignLanguages = l.SignLanguages,
                                  CurrencyUnit = l.CurrencyUnit,
                                  ProductName = pt.ProductName,
                                  Price = pt.Price,
                                  OriginalPrice = pt.OriginalPrice,
                                  ShortDes = pt.ShortDes,
                              }).ToList();
                PagedList<ProductVm> models = new PagedList<ProductVm>(result.AsQueryable(), pageNumber, pageSize);
                ViewBag.CurrentPage = pageNumber;
                return View(models);
        }
       
        //GET: ListProducts
        public IActionResult GetListProducts(int page = 1, int id = 0)
        {
            //get cookie languages
            string culture = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            int cultureStartIndex = culture.IndexOf("c=") + 2;
            int cultureEndIndex = culture.IndexOf("|");
            string extractedCulture = culture.Substring(cultureStartIndex, cultureEndIndex - cultureStartIndex);
            // value languages= extractedCulture
            var pageNumber = page;
            var pageSize = 12;
          
                //view data list category start
                var category = (from c in _context.Categories
                                join ct in _context.CategoryTranslations on c.CategoryId equals ct.CategoryId
                                join l in _context.Languages on ct.LangId equals l.LangId
                                where l.SignLanguages == extractedCulture
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

                var result = (from p in _context.Products
                              join pt in _context.ProductTranslations on p.ProductId equals pt.ProductId
                              join pc in _context.ProductCategories on p.ProductId equals pc.ProductId
                              join l in _context.Languages on pt.LangId equals l.LangId
                              where l.SignLanguages == extractedCulture && pc.CategoryId==id
                              select new ProductVm()
                              {
                                  ProductId = p.ProductId,
                                  Image = p.Image,
                                  Voucher = p.Voucher,
                                  ProductTranslationId = pt.ProductTranslationId,
                                  LangId = l.LangId,
                                  SignLanguages = l.SignLanguages,
                                  CurrencyUnit = l.CurrencyUnit,
                                  ProductName = pt.ProductName,
                                  Price = pt.Price,
                                  OriginalPrice = pt.OriginalPrice,
                                  ShortDes = pt.ShortDes,
                                  CategoryId=pc.CategoryId
                              }).ToList();
                PagedList<ProductVm> models = new PagedList<ProductVm>(result.AsQueryable(), pageNumber, pageSize);
                ViewBag.CurrentPage = pageNumber;
                return View(models);
        }

        //GET: DetailProduct
        public IActionResult GetDetailProduct(int id = 0)
        {
            //get cookie languages
            string culture = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            int cultureStartIndex = culture.IndexOf("c=") + 2;
            int cultureEndIndex = culture.IndexOf("|");
            string extractedCulture = culture.Substring(cultureStartIndex, cultureEndIndex - cultureStartIndex);
            // value languages= extractedCulture
           
                var result = (from p in _context.Products
                              join pt in _context.ProductTranslations on p.ProductId equals pt.ProductId
                              join l in _context.Languages on pt.LangId equals l.LangId
                              where l.SignLanguages == extractedCulture && p.ProductId == id
                              select new ProductVm()
                              {
                                  ProductId = p.ProductId,
                                  Image = p.Image,
                                  Voucher = p.Voucher,
                                  ProductTranslationId = pt.ProductTranslationId,
                                  LangId = l.LangId,
                                  SignLanguages = l.SignLanguages,
                                  CurrencyUnit = l.CurrencyUnit,
                                  ProductName = pt.ProductName,
                                  Price = pt.Price,
                                  OriginalPrice = pt.OriginalPrice,
                                  ShortDes = pt.ShortDes,
                                  Description = pt.Description,
                                  TakeCare=pt.TakeCare,
                                  Application=pt.Application
                              }).FirstOrDefault();
                var listImage = _context.ProductImgs.Where(x => x.ProductId == id).ToList();
                ViewData["ProductImg"] = listImage;

                var listColor = _context.ProductColors.Where(x => x.ProductId == id).ToList();
                ViewData["ProductColor"] = listColor;

                var productDifferent = (from p in _context.Products
                              join pt in _context.ProductTranslations on p.ProductId equals pt.ProductId
                              join l in _context.Languages on pt.LangId equals l.LangId
                              where l.SignLanguages == extractedCulture && p.ProductId!=id
                              select new ProductVm()
                              {
                                  ProductId = p.ProductId,
                                  Image = p.Image,
                                  Voucher = p.Voucher,
                                  ProductTranslationId = pt.ProductTranslationId,
                                  LangId = l.LangId,
                                  SignLanguages = l.SignLanguages,
                                  CurrencyUnit = l.CurrencyUnit,
                                  ProductName = pt.ProductName,
                                  Price = pt.Price,
                                  OriginalPrice = pt.OriginalPrice,
                                  ShortDes = pt.ShortDes,
                              }).ToList();
                ViewData["ProductDifferent"] = productDifferent;
                return View(result);
        }
    }
}
