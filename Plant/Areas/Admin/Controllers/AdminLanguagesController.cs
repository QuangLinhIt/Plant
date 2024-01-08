using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Plant.Models;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Plant.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]

    public class AdminLanguagesController : Controller
    {
        private readonly plantContext _context;
        public INotyfService _notyfService { get; }
        public AdminLanguagesController(plantContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminLanguages
        public async Task<IActionResult> Index()
        {
            return View(await _context.Languages.ToListAsync());
        }

        // GET: Admin/AdminLanguages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminLanguages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create( Language language)
        {
            if (ModelState.IsValid)
            {
                _context.Add(language);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới ngôn ngữ thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(language);
        }

        // GET: Admin/AdminLanguages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _context.Languages.FindAsync(id);
            if (language == null)
            {
                return NotFound();
            }
            return View(language);
        }

        // POST: Admin/AdminLanguages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Language language)
        {
            if (id != language.LangId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(language);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa ngôn ngữ thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LanguageExists(language.LangId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Warning("Chỉnh sửa ngôn ngữ thất bại");
            return View(language);
        }

        // GET: Admin/AdminLanguages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _context.Languages
                .FirstOrDefaultAsync(m => m.LangId == id);
            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        // POST: Admin/AdminLanguages/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var language = await _context.Languages.FindAsync(id);
            _context.Languages.Remove(language);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa ngôn ngữ thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool LanguageExists(int id)
        {
            return _context.Languages.Any(e => e.LangId == id);
        }
    }
}
