using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Plant.ModelDto;
using Plant.Models;

namespace Plant.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class AdminCategoryNewsController : Controller
    {
        private readonly plantContext _context;

        public AdminCategoryNewsController(plantContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminCategoryNews
        public IActionResult Index(int LangId = 0)
        {
            ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", LangId);
            if (LangId != 0)
            {
                var result = (from cn in _context.CategoryNews
                              join cnl in _context.CategoryNewTranslations on cn.CategoryNewId equals cnl.CategoryNewId
                              join l in _context.Languages on cnl.LangId equals l.LangId
                              where cnl.LangId == LangId
                              orderby cn.CategoryNewId descending
                              select new CategoryNewDto()
                              {
                                  CategoryNewTranslationId = cnl.CategoryNewTranslationId,
                                  CategoryNewId = cn.CategoryNewId,
                                  LangId = cnl.LangId,
                                  LangName = l.LangName,
                                  CategoryNewName = cnl.CategoryNewName,
                                  ParentNewId = cn.ParentNewId
                              }).ToList();
                if (result == null)
                {
                    return NotFound();
                }
                return View(result);
            }
            else
            {
                var result = (from cn in _context.CategoryNews
                              join cnl in _context.CategoryNewTranslations on cn.CategoryNewId equals cnl.CategoryNewId
                              join l in _context.Languages on cnl.LangId equals l.LangId
                              orderby cn.CategoryNewId descending
                              select new CategoryNewDto()
                              {
                                  CategoryNewTranslationId = cnl.CategoryNewTranslationId,
                                  CategoryNewId = cn.CategoryNewId,
                                  LangId = cnl.LangId,
                                  LangName = l.LangName,
                                  CategoryNewName = cnl.CategoryNewName,
                                  ParentNewId = cn.ParentNewId
                              }).ToList();
                if (result == null)
                {
                    return NotFound();
                }
                return View(result);
            }


        }
        public IActionResult Filtter(int LangId = 0)
        {
            var url = $"/Admin/AdminCategoryNews/Index?LangId={LangId}";
            if (LangId == 0)
                url = $"/Admin/AdminCategoryNews/Index";
            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Admin/AdminCategoryNews/Create
        public IActionResult Create()
        {
            ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName");
            return View();
        }

        // POST: Admin/AdminCategoryNews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryNewDto dto)
        {
            if (ModelState.IsValid)
            {
                ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", dto.LangId);
                var catgoryNew = new CategoryNews()
                {
                    ParentNewId = dto.ParentNewId
                };
                if (catgoryNew != null)
                {
                    _context.CategoryNews.Add(catgoryNew);
                    await _context.SaveChangesAsync();
                    var categoryNewTran = new CategoryNewTranslation()
                    {
                        LangId = dto.LangId,
                        CategoryNewId = catgoryNew.CategoryNewId,
                        CategoryNewName = dto.CategoryNewName
                    };
                    _context.CategoryNewTranslations.Add(categoryNewTran);
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        //GET: Admin/AdmnCategoryNews/AddLanguages
        public IActionResult AddLanguages(int? id, int? langId)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", langId);
            var result = (from cnt in _context.CategoryNewTranslations
                          join cn in _context.CategoryNews on cnt.CategoryNewId equals cn.CategoryNewId
                          join l in _context.Languages on cnt.LangId equals l.LangId
                          where cnt.CategoryNewId == id && cnt.LangId == langId
                          select new CategoryNewTranslation()
                          {
                              CategoryNewTranslationId = cnt.CategoryNewTranslationId,
                              LangId = cnt.LangId,
                              CategoryNewId = cnt.CategoryNewId
                          }).FirstOrDefault();
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }
        //POST:Admin/AdminCategoryNews/AddLanguages
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddLanguages(int id, int langId, CategoryNewTranslation data)
        {
            if (id != data.CategoryNewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", data.LangId);
                    var exist = _context.CategoryNewTranslations
                        .Include(x => x.CategoryNew)
                        .Include(x => x.Lang)
                        .Where(x => x.CategoryNewId == data.CategoryNewId && x.LangId == data.LangId)
                        .AsNoTracking()
                        .FirstOrDefault();
                    if (exist != null)
                    {
                        _context.CategoryNewTranslations.Remove(exist);
                        _context.SaveChanges();
                        var result = new CategoryNewTranslation()
                        {
                            LangId = data.LangId,
                            CategoryNewId = data.CategoryNewId,
                            CategoryNewName = data.CategoryNewName
                        };
                        _context.CategoryNewTranslations.Add(result);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var result = new CategoryNewTranslation()
                        {
                            LangId = data.LangId,
                            CategoryNewId = data.CategoryNewId,
                            CategoryNewName = data.CategoryNewName
                        };
                        _context.CategoryNewTranslations.Add(result);
                        _context.SaveChanges();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(data);
        }
        // GET: Admin/AdminCategoryNews/Edit/5
        public IActionResult Edit(int? id, int? langId)
        {
            if (id == null || langId == null)
            {
                return NotFound();
            }
            var result = (from cn in _context.CategoryNews
                          join cnl in _context.CategoryNewTranslations on cn.CategoryNewId equals cnl.CategoryNewId
                          join l in _context.Languages on cnl.LangId equals l.LangId
                          where cn.CategoryNewId == id && cnl.LangId == langId
                          orderby cn.CategoryNewId descending
                          select new CategoryNewDto()
                          {
                              CategoryNewTranslationId = cnl.CategoryNewTranslationId,
                              CategoryNewId = cn.CategoryNewId,
                              LangId = cnl.LangId,
                              LangName = l.LangName,
                              CategoryNewName = cnl.CategoryNewName,
                              ParentNewId = cn.ParentNewId
                          }).FirstOrDefault();
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Admin/AdminCategoryNews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int langId, CategoryNewDto dto)
        {
            if (id != dto.CategoryNewId || langId != dto.LangId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var catgoryNew = _context.CategoryNews.Where(x => x.CategoryNewId == dto.CategoryNewId).FirstOrDefault();
                    if (catgoryNew != null)
                    {
                        catgoryNew.CategoryNewId = dto.CategoryNewId;
                        catgoryNew.ParentNewId = dto.ParentNewId;
                        _context.CategoryNews.Update(catgoryNew);
                        await _context.SaveChangesAsync();
                        var categoryNewTran = new CategoryNewTranslation()
                        {
                            CategoryNewTranslationId = dto.CategoryNewTranslationId,
                            LangId = dto.LangId,
                            CategoryNewId = catgoryNew.CategoryNewId,
                            CategoryNewName = dto.CategoryNewName
                        };
                        _context.CategoryNewTranslations.Update(categoryNewTran);
                        _context.SaveChanges();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        // GET: Admin/AdminCategoryNews/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = _context.CategoryNewTranslations.Include(x => x.CategoryNew).Include(x => x.Lang).Where(x => x.CategoryNewId == id).FirstOrDefault();
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // POST: Admin/AdminCategoryNews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var translation = _context.CategoryNewTranslations.Where(x => x.CategoryNewId == id).ToList();
            _context.CategoryNewTranslations.RemoveRange(translation);
            await _context.SaveChangesAsync();
            var categoryNews = _context.CategoryNews.Where(x => x.CategoryNewId == id).FirstOrDefault();
            _context.CategoryNews.Remove(categoryNews);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryNewsExists(int id)
        {
            return _context.CategoryNews.Any(e => e.CategoryNewId == id);
        }
    }
}
