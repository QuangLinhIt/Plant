using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc;
using Plant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Plant.ViewModels;

namespace Plant.Components
{
    public class BlogComponent1 : ViewComponent
    {
        private readonly plantContext _context;

        public BlogComponent1(plantContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(string id = "vi")
        {
            //get cookie languages
            string culture = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            int cultureStartIndex = culture.IndexOf("c=") + 2;
            int cultureEndIndex = culture.IndexOf("|");
            string extractedCulture = culture.Substring(cultureStartIndex, cultureEndIndex - cultureStartIndex);
            // value languages= extractedCulture
            if (extractedCulture == id)
            {
                var result = (from cn in _context.CategoryNews
                              join cnt in _context.CategoryNewTranslations on cn.CategoryNewId equals cnt.CategoryNewId
                              join l in _context.Languages on cnt.LangId equals l.LangId
                              where l.SignLanguages == id
                              select new CategoryNewViewModels()
                              {
                                  CategoryNewId = cn.CategoryNewId,
                                  ParentNewId = cn.ParentNewId,
                                  CategoryNewTranslationId = cnt.CategoryNewTranslationId,
                                  CategoryNewName = cnt.CategoryNewName,
                                  LangId = l.LangId,
                                  SignLanguages = l.SignLanguages
                              }).ToList();
                return View(result);
            }
            else
            {
                var result = (from cn in _context.CategoryNews
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
                return View(result);
            }
        }
    }
    public class BlogComponent2 : ViewComponent
    {
        private readonly plantContext _context;

        public BlogComponent2(plantContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(string id = "vi")
        {
            //get cookie languages
            string culture = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            int cultureStartIndex = culture.IndexOf("c=") + 2;
            int cultureEndIndex = culture.IndexOf("|");
            string extractedCulture = culture.Substring(cultureStartIndex, cultureEndIndex - cultureStartIndex);
            // value languages= extractedCulture
            if (extractedCulture == id)
            {
                var result = (from cn in _context.CategoryNews
                              join cnt in _context.CategoryNewTranslations on cn.CategoryNewId equals cnt.CategoryNewId
                              join l in _context.Languages on cnt.LangId equals l.LangId
                              where l.SignLanguages == id
                              select new CategoryNewViewModels()
                              {
                                  CategoryNewId = cn.CategoryNewId,
                                  ParentNewId = cn.ParentNewId,
                                  CategoryNewTranslationId = cnt.CategoryNewTranslationId,
                                  CategoryNewName = cnt.CategoryNewName,
                                  LangId = l.LangId,
                                  SignLanguages = l.SignLanguages
                              }).ToList();
                return View(result);
            }
            else
            {
                var result = (from cn in _context.CategoryNews
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
                return View(result);
            }
        }
    }

}
