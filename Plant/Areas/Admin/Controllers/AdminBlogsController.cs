using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Plant.ModelDto;
using Plant.Models;

namespace Plant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminBlogsController : Controller
    {
        private readonly plantContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdminBlogsController(plantContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;

        }

        // GET: Admin/AdminBlogs
        public IActionResult Index(int LangId = 0)
        {
            ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", LangId);
            if (LangId == 0)
            {
                var result = (from b in _context.Blogs
                              join bt in _context.BlogTranslations on b.BlogId equals bt.BlogId
                              join l in _context.Languages on bt.LangId equals l.LangId
                              select new BlogDto()
                              {
                                  BlogId = b.BlogId,
                                  LangId = bt.LangId,
                                  LangName = l.LangName,
                                  Title = bt.Title,
                                  ShortDes = bt.ShortDes,
                                  Description = bt.Description,
                                  ImageName = b.BlogImg
                              }).ToList();
                if (result != null)
                {
                    return View(result);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                var result = (from b in _context.Blogs
                              join bt in _context.BlogTranslations on b.BlogId equals bt.BlogId
                              join l in _context.Languages on bt.LangId equals l.LangId
                              where bt.LangId == LangId
                              select new BlogDto()
                              {
                                  BlogId = b.BlogId,
                                  LangId = bt.LangId,
                                  LangName = l.LangName,
                                  Title = bt.Title,
                                  ShortDes = bt.ShortDes,
                                  Description = bt.Description,
                                  ImageName = b.BlogImg
                              }).ToList();
                if (result != null)
                {
                    return View(result);
                }
                else
                {
                    return NotFound();
                }
            }
        }
        public IActionResult Filtter(int LangId = 0)
        {
            var url = $"/Admin/AdminBlogs/Index?LangId={LangId}";
            if (LangId == 0)
                url = $"/Admin/AdminBlogs/Index";
            return Json(new { status = "success", redirectUrl = url });
        }


        // GET: Admin/AdminBlogs/Create
        public IActionResult Create()
        {
            ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName");
            var blogDto = new BlogDto();
            var categoryNewIds = new List<int>();

            blogDto.ListCategoryNews = _context.CategoryNewTranslations
                .Include(x => x.CategoryNew)
                .Where(x => x.LangId == 1)
                .Select(x => new SelectListItem { Text = x.CategoryNewName, Value = x.CategoryNewId.ToString() }).ToList();

            return View(blogDto);
        }

        // POST: Admin/AdminBlogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BlogDto dto)
        {
            if (ModelState.IsValid)
            {
                ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", dto.LangId);
                var blog = new Blog();
                var blogCategory = new List<BlogCategory>();
                //blog
                blog.BlogImg = ProcessUploadedFile(dto);

                if (dto.CategoryNewIds.Length > 0)
                {
                    foreach (var categoryNewIds in dto.CategoryNewIds)
                    {
                        blogCategory.Add(new BlogCategory { BlogId = blog.BlogId, CategoryNewId = categoryNewIds });
                    }
                    blog.BlogCategories = blogCategory;
                }
                _context.SaveChanges();
                _context.Blogs.Add(blog);
                _context.SaveChanges();
                var blogTranslation = new BlogTranslation()
                {
                    BlogId = blog.BlogId,
                    LangId = dto.LangId,
                    Title = dto.Title,
                    ShortDes = dto.ShortDes,
                    Description = dto.Description
                };
                _context.BlogTranslations.Add(blogTranslation);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }
        //GET:Admin/AdminBlogs/AddLanguages
        public IActionResult AddLanguages(int? id, int? langId)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", langId);
            var result = (from b in _context.Blogs
                          join bt in _context.BlogTranslations on b.BlogId equals bt.BlogId
                          join l in _context.Languages on bt.LangId equals l.LangId
                          where b.BlogId == id && bt.LangId == langId
                          select new BlogTranslation()
                          {
                              BlogId = b.BlogId,
                              LangId = l.LangId,
                          }).FirstOrDefault();
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //POST:Admin/AdminBlogs/AddLanguages
        public IActionResult AddLanguages(int id, int langId, BlogTranslation data)
        {
            if (id != data.BlogId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", data.LangId);
                var exist = _context.BlogTranslations.Where(x => x.BlogId == data.BlogId && x.LangId == data.LangId).FirstOrDefault();
                if (exist != null)
                {
                    _context.BlogTranslations.Remove(exist);
                    var result = new BlogTranslation()
                    {
                        BlogId = data.BlogId,
                        LangId = data.LangId,
                        Title = data.Title,
                        ShortDes = data.ShortDes,
                        Description = data.Description
                    };
                    _context.BlogTranslations.Add(result);
                    _context.SaveChanges();
                }
                else
                {
                    var result = new BlogTranslation()
                    {
                        BlogId = data.BlogId,
                        LangId = data.LangId,
                        Title = data.Title,
                        ShortDes = data.ShortDes,
                        Description = data.Description
                    };
                    _context.BlogTranslations.Add(result);
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(data);
        }
        // GET: Admin/AdminBlogs/Edit/5
        public IActionResult Edit(int? id, int? langId)
        {
            if (id == null || langId == null)
            {
                return NotFound();
            }
            try
            {
                ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", langId);
                var blog = _context.Blogs.Include(x => x.BlogCategories).Where(x => x.BlogId == id).FirstOrDefault();
                var blogTranslation = _context.BlogTranslations.Where(x => x.BlogId == id && x.LangId == langId).FirstOrDefault();
                var categoryNewIds = new List<int>();
                var dto = new BlogDto();
                foreach (var item in blog.BlogCategories.ToList())
                {
                    categoryNewIds.Add(item.CategoryNewId);
                }
                dto.BlogId = blog.BlogId;
                dto.ImageName = blog.BlogImg;
                dto.LangId = blogTranslation.LangId;
                dto.Title = blogTranslation.Title;
                dto.ShortDes = blogTranslation.ShortDes;
                dto.Description = blogTranslation.Description;
                dto.CategoryNewIds = categoryNewIds.ToArray();
                dto.ListCategoryNews = _context.CategoryNewTranslations
                    .Include(x => x.CategoryNew)
                    .Where(x => x.LangId == 1)
                    .Select(x => new SelectListItem { Text = x.CategoryNewName, Value = x.CategoryNewId.ToString() }).ToList();
                return View(dto);
            }
            catch
            {
                return NotFound();
            }
        }

        // POST: Admin/AdminBlogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, int langId, BlogDto dto)
        {
            if (id != dto.BlogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", dto.LangId);
                    //find blog
                    var blog = _context.Blogs.Include(x => x.BlogCategories).Where(x => x.BlogId == dto.BlogId).FirstOrDefault();
                    //find BlogTranslation and update
                    var blogTranslation = _context.BlogTranslations.Where(x => x.BlogId == dto.BlogId && x.LangId == dto.LangId).FirstOrDefault();
                    if (blogTranslation != null)
                    {
                        blogTranslation.BlogId = dto.BlogId;
                        blogTranslation.LangId = dto.LangId;
                        blogTranslation.Title = dto.Title;
                        blogTranslation.ShortDes = dto.ShortDes;
                        blogTranslation.Description = dto.Description;
                        _context.BlogTranslations.Update(blogTranslation);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var blogTran = new BlogTranslation()
                        {
                            BlogId = dto.BlogId,
                            LangId = dto.LangId,
                            Title = dto.Title,
                            ShortDes = dto.ShortDes,
                            Description = dto.Description
                        };
                        _context.BlogTranslations.Add(blogTran);
                        _context.SaveChanges();
                    }
                    //Find list BlogCategory and remove
                    var listBlogCategory = new List<BlogCategory>();
                    blog.BlogCategories.ToList().ForEach(result => listBlogCategory.Add(result));
                    _context.BlogCategories.RemoveRange(listBlogCategory);
                    _context.SaveChanges();
                    //now update blog
                    blog.BlogId = dto.BlogId;
                    if (dto.ImageFile != null)
                    {
                        string filePath = Path.Combine(_hostEnvironment.WebRootPath, "image\\blog", blog.BlogImg);
                        System.IO.File.Delete(filePath);
                        blog.BlogImg = ProcessUploadedFile(dto);
                    }
                    //update blogcategory
                    if (dto.CategoryNewIds.Length > 0)
                    {
                        listBlogCategory = new List<BlogCategory>();
                        foreach (var item in dto.CategoryNewIds)
                        {
                            listBlogCategory.Add(new BlogCategory { BlogId = blog.BlogId, CategoryNewId = item });
                        }
                        blog.BlogCategories = listBlogCategory;
                    }
                    _context.SaveChanges();

                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        // GET: Admin/AdminBlogs/Delete/5
        public IActionResult Delete(int? id, int? langId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = _context.BlogTranslations
                .Include(x => x.Blog)
                .Include(x => x.Lang)
                .Where(x => x.BlogId == id && x.LangId == langId)
                .FirstOrDefault();
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // POST: Admin/AdminBlogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int langId)
        {
            var blog = await _context.Blogs.Where(x => x.BlogId == id).FirstOrDefaultAsync();
            var blogTranslation = _context.BlogTranslations.Where(x => x.BlogId == id).ToList();
            var blogCategory = _context.BlogCategories.Where(x => x.BlogId == id).ToList();
            _context.BlogCategories.RemoveRange(blogCategory);
            _context.SaveChanges();
            _context.BlogTranslations.RemoveRange(blogTranslation);
            _context.SaveChanges();
            //delete from wwwroot
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image\\blog", blog.BlogImg);
            _context.Blogs.Remove(blog);
            if (await _context.SaveChangesAsync() > 0)
            {
                if (System.IO.File.Exists(CurrentImage))
                {
                    System.IO.File.Delete(CurrentImage);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.BlogId == id);
        }
        private string ProcessUploadedFile(BlogDto dto)
        {
            if (dto.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "image\\blog");
                dto.ImageName = Guid.NewGuid().ToString() + "_" + dto.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, dto.ImageName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                dto.ImageFile.CopyTo(fileStream);
            }

            return dto.ImageName;
        }
    }
}
