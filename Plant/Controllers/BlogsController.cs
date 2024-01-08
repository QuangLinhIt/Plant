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
    public class BlogsController : Controller
    {
        private readonly plantContext _context;

        public BlogsController(plantContext context)
        {
            _context = context;
        }

        // GET: Blogs
        [HttpGet]
        public IActionResult Index(int page = 1)
        {
            //get selected languages
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            var culture = cultureInfo.Name;

            var pageNumber = page;
            var pageSize = 5;

            //view data list category start
            var category = (from cn in _context.CategoryNews
                            join cnt in _context.CategoryNewTranslations on cn.CategoryNewId equals cnt.CategoryNewId
                            join l in _context.Languages on cnt.LangId equals l.LangId
                            where l.SignLanguages == culture
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
                              Description = bt.Description
                          }).ToList();
            PagedList<BlogVm> models = new PagedList<BlogVm>(result.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        //GET: list blog
        [HttpGet]
        public IActionResult GetListBlog(int page = 1, int id = 0)
        {
            //get selected languages
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            var culture = cultureInfo.Name;

            var pageNumber = page;
            var pageSize = 5;

            //view data list category start
            var category = (from cn in _context.CategoryNews
                            join cnt in _context.CategoryNewTranslations on cn.CategoryNewId equals cnt.CategoryNewId
                            join l in _context.Languages on cnt.LangId equals l.LangId
                            where l.SignLanguages == culture
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
                          where l.SignLanguages == culture && bc.CategoryNewId == id
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
        //GET:detail blog
        [HttpGet]
        public IActionResult GetDetailBlog(int id = 0)
        {
            //get selected languages
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            var culture = cultureInfo.Name;

            //view data list category start
            var category = (from cn in _context.CategoryNews
                            join cnt in _context.CategoryNewTranslations on cn.CategoryNewId equals cnt.CategoryNewId
                            join l in _context.Languages on cnt.LangId equals l.LangId
                            where l.SignLanguages == culture
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
                                 where l.SignLanguages == culture && b.BlogId != id
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
                          where l.SignLanguages == culture && b.BlogId == id
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
