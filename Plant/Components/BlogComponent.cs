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
using System.Globalization;
using System.Threading;

namespace Plant.Components
{
    [ViewComponent]
    public class BlogComponent1 : ViewComponent
    {
        private readonly plantContext _context;

        public BlogComponent1(plantContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            var culture = cultureInfo.Name;


            var result = (from cn in _context.CategoryNews
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
            return View(result);

        }
    }
    public class BlogComponent2 : ViewComponent
    {
        private readonly plantContext _context;

        public BlogComponent2(plantContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            var culture = cultureInfo.Name;

            var result = (from cn in _context.CategoryNews
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
            return View(result);

        }
    }

}
