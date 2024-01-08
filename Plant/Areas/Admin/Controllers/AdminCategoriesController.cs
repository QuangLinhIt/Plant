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
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Plant.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]
    public class AdminCategoriesController : Controller
    {
        private readonly plantContext _context;
        public INotyfService _notyfService { get; }
        public AdminCategoriesController(plantContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminCategories
        public IActionResult Index(int LangId = 0)
        {
            ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", LangId);
            if (LangId != 0)
            {
                var result = _context.CategoryTranslations
                    .Include(x => x.Category)
                    .Include(x => x.Lang)
                    .Where(x => x.LangId == LangId)
                    .OrderBy(x=>x.CategoryId)
                    .ToList();
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(result);
                }
            }
            else
            {
                var result = _context.CategoryTranslations
                    .Include(x => x.Category)
                    .Include(x => x.Lang)
                    .OrderBy(x => x.CategoryId)
                    .ToList();
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(result);
                }
            }
        }
        //GET:Admin/AdminCategories/Filtter
        public IActionResult Filtter(int LangId = 0)
        {
            var url = $"/Admin/AdminCategories/Index?LangId={LangId}";
            if (LangId == 0)
                url = $"/Admin/AdminCategories/Index";
            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Admin/AdminCategories/Create
        public IActionResult Create()
        {
            ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName");
            return View();
        }

        // POST: Admin/AdminCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto dto)
        {
            if (ModelState.IsValid)
            {
                ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", dto.LangId);
                var category = new Category()
                {
                    ParentId = dto.ParentId
                };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                var categoryTranslation = new CategoryTranslation()
                {
                    CategoryId = category.CategoryId,
                    CategoryName = dto.CategoryName,
                    LangId = dto.LangId
                };
                _context.CategoryTranslations.Add(categoryTranslation);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới danh mục sản phẩm thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        //GET: Admin/AdminCategories/AddLanguages
        public IActionResult AddLanguages(int? id,int? langId)
        {
            if(id==null && langId == null)
            {
                return NotFound();
            }
            ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", langId);
            var result = (from ct in _context.CategoryTranslations
                          where ct.CategoryId == id && ct.LangId == langId
                          select new CategoryTranslation()
                          {
                              CategoryTranslationId = ct.CategoryTranslationId,
                              CategoryId = ct.CategoryId,
                              LangId = ct.LangId
                          }).FirstOrDefault();
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return View(result);
            }
        }
        //POST:Admin/AdminCategories/AddLanguages
        [HttpPost]
        public IActionResult AddLanguages(int id,int langId,CategoryTranslation data)
        {
            if (id != data.CategoryId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", data.LangId);
                var exist = _context.CategoryTranslations
                    .Where(x => x.CategoryId == data.CategoryId && x.LangId == data.LangId)
                    .FirstOrDefault();
                if (exist != null)
                {
                    _context.CategoryTranslations.Remove(exist);
                    _context.SaveChanges();
                    var result = new CategoryTranslation()
                    {
                        CategoryId = data.CategoryId,
                        LangId = data.LangId,
                        CategoryName = data.CategoryName
                    };
                    _context.CategoryTranslations.Add(result);
                    _context.SaveChanges();
                    _notyfService.Success("Cập nhật bản dịch danh mục sản phẩm thành công");
                }
                else
                {
                    var result = new CategoryTranslation()
                    {
                        CategoryId = data.CategoryId,
                        LangId = data.LangId,
                        CategoryName = data.CategoryName
                    };
                    _context.CategoryTranslations.Add(result);
                    _context.SaveChanges();
                    _notyfService.Success("Thêm mới bản dịch danh mục sản phẩm thành công");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(data);
        }
        // GET: Admin/AdminCategories/Edit/5
        public IActionResult Edit(int? id, int? langId)
        {
            if (id == null || langId == null)
            {
                return NotFound();
            }
            var result = (from c in _context.Categories
                          join ct in _context.CategoryTranslations on c.CategoryId equals ct.CategoryId
                          join l in _context.Languages on ct.LangId equals l.LangId
                          where ct.CategoryId == id && l.LangId == langId
                          select new CategoryDto()
                          {
                              CategoryId = c.CategoryId,
                              ParentId = c.ParentId,
                              CategoryTranslationId = ct.CategoryTranslationId,
                              LangId = l.LangId,
                              LangName = l.LangName,
                              CategoryName = ct.CategoryName
                          }).FirstOrDefault();
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Admin/AdminCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, int langId, CategoryDto dto)
        {
            if (id != dto.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var category = _context.Categories.Where(x => x.CategoryId == dto.CategoryId).FirstOrDefault();
                    if (category == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        category.CategoryId = dto.CategoryId;
                        category.ParentId = dto.ParentId;
                        _context.Categories.Update(category);
                        await _context.SaveChangesAsync();
                        var cartegoryTranslation = _context.CategoryTranslations.Where(x => x.CategoryId == dto.CategoryId && x.LangId == dto.LangId).FirstOrDefault();
                        if (cartegoryTranslation == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            cartegoryTranslation.CategoryTranslationId = dto.CategoryTranslationId;
                            cartegoryTranslation.CategoryId = dto.CategoryId;
                            cartegoryTranslation.LangId = dto.LangId;
                            cartegoryTranslation.CategoryName = dto.CategoryName;
                            _context.CategoryTranslations.Update(cartegoryTranslation);
                            await _context.SaveChangesAsync();
                            _notyfService.Success("Chỉnh sửa danh mục sản phẩm thành công");
                        }
                    }
                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Warning("Chỉnh sửa danh mục sản phẩm thất bại");
            return View(dto);
        }

        // GET: Admin/AdminCategories/Delete/5
        public async Task<IActionResult> Delete(int? id, int? langId)
        {
            if (id == null || langId == null)
            {
                return NotFound();
            }

            var result = await _context.CategoryTranslations.Include(x => x.Category).Include(x => x.Lang).Where(x => x.CategoryId == id && x.LangId == langId).FirstOrDefaultAsync();
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // POST: Admin/AdminCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, int langId)
        {
            var categoryTranslation = await _context.CategoryTranslations.Where(x => x.CategoryId == id).ToListAsync();
            if (categoryTranslation == null)
            {
                var category = await _context.Categories.Where(x => x.CategoryId == id).FirstOrDefaultAsync();
                if (category == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                _context.CategoryTranslations.RemoveRange(categoryTranslation);
                await _context.SaveChangesAsync();
                var category = await _context.Categories.Where(x => x.CategoryId == id).FirstOrDefaultAsync();
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            _notyfService.Success("Xóa danh mục sản phẩm thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
