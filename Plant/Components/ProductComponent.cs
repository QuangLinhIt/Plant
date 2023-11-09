using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Plant.Models;
using Plant.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
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

        public IViewComponentResult Invoke()
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            var culture = cultureInfo.Name;

            var result = (from c in _context.Categories
                          join ct in _context.CategoryTranslations on c.CategoryId equals ct.CategoryId
                          join l in _context.Languages on ct.LangId equals l.LangId
                          where l.SignLanguages == culture
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
    public class ProductComponent2 : ViewComponent
    {
        private readonly plantContext _context;

        public ProductComponent2(plantContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            //get selected languages
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            var culture = cultureInfo.Name;

            var result = (from c in _context.Categories
                          join ct in _context.CategoryTranslations on c.CategoryId equals ct.CategoryId
                          join l in _context.Languages on ct.LangId equals l.LangId
                          where l.SignLanguages == culture
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
