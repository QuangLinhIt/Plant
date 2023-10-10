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
    public class BlogsController : Controller
    {
        private readonly plantContext _context;

        public BlogsController(plantContext context)
        {
            _context = context;
        }

        // GET: Blogs
        public IActionResult Index(string langId = "vi", int page = 1)
        {
            //get cookie languages
            string culture = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            int cultureStartIndex = culture.IndexOf("c=") + 2;
            int cultureEndIndex = culture.IndexOf("|");
            string extractedCulture = culture.Substring(cultureStartIndex, cultureEndIndex - cultureStartIndex);
            // value languages= extractedCulture
            var pageNumber = page;
            var pageSize = 5;
            if (extractedCulture == langId)
            {
                //view data list category start
                var category = (from cn in _context.CategoryNews
                                join cnt in _context.CategoryNewTranslations on cn.CategoryNewId equals cnt.CategoryNewId
                                join l in _context.Languages on cnt.LangId equals l.LangId
                                where l.SignLanguages == extractedCulture
                                select new CategoryNewViewModels()
                                {
                                    CategoryNewId = cn.CategoryNewId,
                                    ParentNewId = cn.ParentNewId,
                                    CategoryNewTranslationId = cnt.CategoryNewTranslationId,
                                    CategoryNewName = cnt.CategoryNewName,
                                    LangId = l.LangId,
                                    SignLanguages = l.SignLanguages
                                }).ToList();
                ViewData["CategoryNew"] = category;

                var result = (from b in _context.Blogs
                              join bt in _context.BlogTranslations on b.BlogId equals bt.BlogId
                              join l in _context.Languages on bt.LangId equals l.LangId
                              where l.SignLanguages == extractedCulture
                              orderby b.BlogId descending
                              select new BlogVm()
                              {
                                  BlogId = b.BlogId,
                                  BlogImg = b.BlogImg,
                                  LangId = l.LangId,
                                  SignLanguages = l.SignLanguages,
                                  Title = bt.Title,
                                  ShortDes = bt.ShortDes,
                                  Description = bt.Description
                              }).ToList();
                PagedList<BlogVm> models = new PagedList<BlogVm>(result.AsQueryable(), pageNumber, pageSize);
                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            else
            {
                //view data list category start
                var category = (from cn in _context.CategoryNews
                                join cnt in _context.CategoryNewTranslations on cn.CategoryNewId equals cnt.CategoryNewId
                                join l in _context.Languages on cnt.LangId equals l.LangId
                                where l.SignLanguages == extractedCulture
                                select new CategoryNewViewModels()
                                {
                                    CategoryNewId = cn.CategoryNewId,
                                    ParentNewId = cn.ParentNewId,
                                    CategoryNewTranslationId = cnt.CategoryNewTranslationId,
                                    CategoryNewName = cnt.CategoryNewName,
                                    LangId = l.LangId,
                                    SignLanguages = l.SignLanguages
                                }).ToList();
                ViewData["CategoryNew"] = category;
                var result = (from b in _context.Blogs
                              join bt in _context.BlogTranslations on b.BlogId equals bt.BlogId
                              join l in _context.Languages on bt.LangId equals l.LangId
                              where l.SignLanguages == extractedCulture
                              orderby b.BlogId descending
                              select new BlogVm()
                              {
                                  BlogId = b.BlogId,
                                  BlogImg = b.BlogImg,
                                  LangId = l.LangId,
                                  SignLanguages = l.SignLanguages,
                                  Title = bt.Title,
                                  ShortDes = bt.ShortDes,
                                  Description = bt.Description
                              }).ToList();
                PagedList<BlogVm> models = new PagedList<BlogVm>(result.AsQueryable(), pageNumber, pageSize);
                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
        }
        //GET: list blog
        public IActionResult GetListBlog(string langId = "vi", int page = 1, int id = 0)
        {
            //get cookie languages
            string culture = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            int cultureStartIndex = culture.IndexOf("c=") + 2;
            int cultureEndIndex = culture.IndexOf("|");
            string extractedCulture = culture.Substring(cultureStartIndex, cultureEndIndex - cultureStartIndex);
            // value languages= extractedCulture
            var pageNumber = page;
            var pageSize = 5;
            if (extractedCulture == langId)
            {
                //view data list category start
                var category = (from cn in _context.CategoryNews
                                join cnt in _context.CategoryNewTranslations on cn.CategoryNewId equals cnt.CategoryNewId
                                join l in _context.Languages on cnt.LangId equals l.LangId
                                where l.SignLanguages == extractedCulture
                                select new CategoryNewViewModels()
                                {
                                    CategoryNewId = cn.CategoryNewId,
                                    ParentNewId = cn.ParentNewId,
                                    CategoryNewTranslationId = cnt.CategoryNewTranslationId,
                                    CategoryNewName = cnt.CategoryNewName,
                                    LangId = l.LangId,
                                    SignLanguages = l.SignLanguages
                                }).ToList();
                ViewData["CategoryNew"] = category;

                var result = (from b in _context.Blogs
                              join bt in _context.BlogTranslations on b.BlogId equals bt.BlogId
                              join bc in _context.BlogCategories on b.BlogId equals bc.BlogId
                              join l in _context.Languages on bt.LangId equals l.LangId
                              where l.SignLanguages == extractedCulture && bc.CategoryNewId == id
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
                PagedList<BlogVm> models = new PagedList<BlogVm>(result.AsQueryable(), pageNumber, pageSize);
                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            else
            {
                //view data list category start
                var category = (from cn in _context.CategoryNews
                                join cnt in _context.CategoryNewTranslations on cn.CategoryNewId equals cnt.CategoryNewId
                                join l in _context.Languages on cnt.LangId equals l.LangId
                                where l.SignLanguages == extractedCulture
                                select new CategoryNewViewModels()
                                {
                                    CategoryNewId = cn.CategoryNewId,
                                    ParentNewId = cn.ParentNewId,
                                    CategoryNewTranslationId = cnt.CategoryNewTranslationId,
                                    CategoryNewName = cnt.CategoryNewName,
                                    LangId = l.LangId,
                                    SignLanguages = l.SignLanguages
                                }).ToList();
                ViewData["CategoryNew"] = category;
                var result = (from b in _context.Blogs
                              join bt in _context.BlogTranslations on b.BlogId equals bt.BlogId
                              join bc in _context.BlogCategories on b.BlogId equals bc.BlogId
                              join l in _context.Languages on bt.LangId equals l.LangId
                              where l.SignLanguages == extractedCulture && bc.CategoryNewId == id
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
                PagedList<BlogVm> models = new PagedList<BlogVm>(result.AsQueryable(), pageNumber, pageSize);
                ViewBag.CurrentPage = pageNumber;
                return View(models);

            }

        }
        //GET:detail blog
        public IActionResult GetDetailBlog(string langId = "vi", int id = 0)
        {
            //get cookie languages
            string culture = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            int cultureStartIndex = culture.IndexOf("c=") + 2;
            int cultureEndIndex = culture.IndexOf("|");
            string extractedCulture = culture.Substring(cultureStartIndex, cultureEndIndex - cultureStartIndex);
            // value languages= extractedCulture

            if (extractedCulture == langId)
            {
                //view data list category start
                var category = (from cn in _context.CategoryNews
                                join cnt in _context.CategoryNewTranslations on cn.CategoryNewId equals cnt.CategoryNewId
                                join l in _context.Languages on cnt.LangId equals l.LangId
                                where l.SignLanguages == extractedCulture
                                select new CategoryNewViewModels()
                                {
                                    CategoryNewId = cn.CategoryNewId,
                                    ParentNewId = cn.ParentNewId,
                                    CategoryNewTranslationId = cnt.CategoryNewTranslationId,
                                    CategoryNewName = cnt.CategoryNewName,
                                    LangId = l.LangId,
                                    SignLanguages = l.SignLanguages
                                }).ToList();
                ViewData["CategoryNew"] = category;
                //view data list category end

                var blogdifferent = (from b in _context.Blogs
                                     join bt in _context.BlogTranslations on b.BlogId equals bt.BlogId
                                     join bc in _context.BlogCategories on b.BlogId equals bc.BlogId
                                     join l in _context.Languages on bt.LangId equals l.LangId
                                     where l.SignLanguages == extractedCulture && b.BlogId != id 
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
                                     }).Take(3).ToList();
                ViewData["BlogDifferent"] = blogdifferent;
                var result = (from b in _context.Blogs
                              join bt in _context.BlogTranslations on b.BlogId equals bt.BlogId
                              join bc in _context.BlogCategories on b.BlogId equals bc.BlogId
                              join l in _context.Languages on bt.LangId equals l.LangId
                              where l.SignLanguages == extractedCulture && b.BlogId == id
                              select new BlogVm()
                              {
                                  BlogId = b.BlogId,
                                  BlogImg = b.BlogImg,
                                  LangId = l.LangId,
                                  SignLanguages = l.SignLanguages,
                                  Title = bt.Title,
                                  ShortDes = bt.ShortDes,
                                  Description = bt.Description,
                                  CategoryNewId = bc.CategoryNewId
                              }).FirstOrDefault();
                return View(result);
            }
            else
            {
                //view data list category start
                var category = (from cn in _context.CategoryNews
                                join cnt in _context.CategoryNewTranslations on cn.CategoryNewId equals cnt.CategoryNewId
                                join l in _context.Languages on cnt.LangId equals l.LangId
                                where l.SignLanguages == extractedCulture
                                select new CategoryNewViewModels()
                                {
                                    CategoryNewId = cn.CategoryNewId,
                                    ParentNewId = cn.ParentNewId,
                                    CategoryNewTranslationId = cnt.CategoryNewTranslationId,
                                    CategoryNewName = cnt.CategoryNewName,
                                    LangId = l.LangId,
                                    SignLanguages = l.SignLanguages
                                }).ToList();
                ViewData["CategoryNew"] = category;
                //view data list category end
                var blogdifferent = (from b in _context.Blogs
                                     join bt in _context.BlogTranslations on b.BlogId equals bt.BlogId
                                     join bc in _context.BlogCategories on b.BlogId equals bc.BlogId
                                     join l in _context.Languages on bt.LangId equals l.LangId
                                     where l.SignLanguages == extractedCulture && b.BlogId != id
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
                                     }).Take(3).ToList();
                ViewData["BlogDifferent"] = blogdifferent;
                var result = (from b in _context.Blogs
                              join bt in _context.BlogTranslations on b.BlogId equals bt.BlogId
                              join bc in _context.BlogCategories on b.BlogId equals bc.BlogId
                              join l in _context.Languages on bt.LangId equals l.LangId
                              where l.SignLanguages == extractedCulture && b.BlogId == id
                              select new BlogVm()
                              {
                                  BlogId = b.BlogId,
                                  BlogImg = b.BlogImg,
                                  LangId = l.LangId,
                                  SignLanguages = l.SignLanguages,
                                  Title = bt.Title,
                                  ShortDes = bt.ShortDes,
                                  Description = bt.Description,
                                  CategoryNewId = bc.CategoryNewId
                              }).FirstOrDefault();
                return View(result);

            }
        }
    }
}
