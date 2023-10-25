using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Plant.Models;
using Plant.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.Components
{
        public class ProductComponent1 : ViewComponent
        {
            private readonly plantContext _context;

            public ProductComponent1(plantContext context)
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
                    var result = (from c in _context.Categories
                                  join ct in _context.CategoryTranslations on c.CategoryId equals ct.CategoryId
                                  join l in _context.Languages on ct.LangId equals l.LangId
                                  where l.SignLanguages == id
                                  select new CategoryVm()
                                  {
                                      CategoryId = c.CategoryId,
                                      ParentId = c.ParentId,
                                      CategoryTranslationId = ct.CategoryTranslationId,
                                      CategoryName = ct.CategoryName,
                                      LangId = l.LangId,
                                      SignLanguages = l.SignLanguages
                                  }).ToList();
                    return View(result);
                }
                else
                {
                    var result = (from c in _context.Categories
                                  join ct in _context.CategoryTranslations on c.CategoryId equals ct.CategoryId
                                  join l in _context.Languages on ct.LangId equals l.LangId
                                  where l.SignLanguages == extractedCulture
                                  select new CategoryVm()
                                  {
                                      CategoryId = c.CategoryId,
                                      ParentId = c.ParentId,
                                      CategoryTranslationId = ct.CategoryTranslationId,
                                      CategoryName = ct.CategoryName,
                                      LangId = l.LangId,
                                      SignLanguages = l.SignLanguages
                                  }).ToList();
                    return View(result);
                }
            }
        }
        public class ProductComponent2 : ViewComponent
        {
            private readonly plantContext _context;

            public ProductComponent2(plantContext context)
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
                    var result = (from c in _context.Categories
                                  join ct in _context.CategoryTranslations on c.CategoryId equals ct.CategoryId
                                  join l in _context.Languages on ct.LangId equals l.LangId
                                  where l.SignLanguages == id
                                  select new CategoryVm()
                                  {
                                      CategoryId = c.CategoryId,
                                      ParentId = c.ParentId,
                                      CategoryTranslationId = ct.CategoryTranslationId,
                                      CategoryName = ct.CategoryName,
                                      LangId = l.LangId,
                                      SignLanguages = l.SignLanguages
                                  }).ToList();
                    return View(result);
                }
                else
                {
                    var result = (from c in _context.Categories
                                  join ct in _context.CategoryTranslations on c.CategoryId equals ct.CategoryId
                                  join l in _context.Languages on ct.LangId equals l.LangId
                                  where l.SignLanguages == extractedCulture
                                  select new CategoryVm()
                                  {
                                      CategoryId = c.CategoryId,
                                      ParentId = c.ParentId,
                                      CategoryTranslationId = ct.CategoryTranslationId,
                                      CategoryName = ct.CategoryName,
                                      LangId = l.LangId,
                                      SignLanguages = l.SignLanguages
                                  }).ToList();
                    return View(result);
                }
            }
        }
}
