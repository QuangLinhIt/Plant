using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Plant.ModelDto;
using Plant.Models;

namespace Plant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductsController : Controller
    {
        private readonly plantContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdminProductsController(plantContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Admin/AdminProducts
        public IActionResult Index(int page = 1, int LangId = 0)
        {
            ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", LangId);
            var pageNumber = page;
            var pageSize = 10;
            var listProduct = new List<ProductDto>();
            if (LangId != 0)
            {
                listProduct = (from p in _context.Products
                               join pt in _context.ProductTranslations on p.ProductId equals pt.ProductId
                               join pc in _context.ProductColors on p.ProductId equals pc.ProductId
                               join l in _context.Languages on pt.LangId equals l.LangId
                               where pt.LangId == LangId
                               select new ProductDto()
                               {
                                   ProductId = p.ProductId,
                                   LangId = l.LangId,
                                   LangName = l.LangName,
                                   Voucher = p.Voucher,
                                   ProductName = pt.ProductName,
                                   Price = pt.Price,
                                   OriginalPrice = pt.OriginalPrice,
                                   ShortDes = pt.ShortDes,
                                   Description = pt.Description,
                                   
                                   ImageName = p.Image
                               }).AsNoTracking().ToList();
            }
            else
            {
                listProduct = (from p in _context.Products
                               join pt in _context.ProductTranslations on p.ProductId equals pt.ProductId
                               join pc in _context.ProductColors on p.ProductId equals pc.ProductId
                               join l in _context.Languages on pt.LangId equals l.LangId
                               where pt.LangId == LangId
                               select new ProductDto()
                               {
                                   ProductId = p.ProductId,
                                   LangId = l.LangId,
                                   LangName = l.LangName,
                                   Voucher = p.Voucher,
                                   ProductName = pt.ProductName,
                                   Price = pt.Price,
                                   OriginalPrice = pt.OriginalPrice,
                                   ShortDes = pt.ShortDes,
                                   Description = pt.Description,
                                  
                                   ImageName = p.Image
                               }).AsNoTracking().ToList();
            }
            var models = new PagedList<ProductDto>(listProduct.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        public IActionResult Filtter(int LangId = 0)
        {
            var url = $"/Admin/AdminProducts/Index?LangId={LangId}";
            if (LangId == 0)
                url = $"/Admin/AdminProducts/Index";
            return Json(new { status = "success", redirectUrl = url });
        }


        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName");
            var productDto = new ProductDto();
            var categoryIds = new List<int>();

            productDto.ListCategory = _context.CategoryTranslations.Where(x => x.LangId == 1).Select(x => new SelectListItem { Text = x.CategoryName, Value = x.CategoryId.ToString() }).ToList();
            return View(productDto);
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDto dto)
        {
            if (ModelState.IsValid)
            {
                ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", dto.LangId);
                //Add Product
                var product = new Product()
                {
                    Voucher = dto.Voucher,
                    Image = ProcessUploadedFile(dto)
                };
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                //Add ProductCategory
                var productCategory = new List<ProductCategory>();
                if (dto.CategoryIds.Length > 0)
                {
                    foreach (var categoryid in dto.CategoryIds)
                    {
                        productCategory.Add(new ProductCategory { ProductId = product.ProductId, CategoryId = categoryid });
                    }
                    product.ProductCategories = productCategory;
                }
                await _context.SaveChangesAsync();
             
                //Add ProductTranslation
                var productTranslation = new ProductTranslation()
                {
                    ProductId = product.ProductId,
                    LangId = dto.LangId,
                    ProductName = dto.ProductName,
                    ShortDes = dto.ShortDes,
                    Description = dto.Description,
                    Price = dto.Price,
                    OriginalPrice = dto.OriginalPrice,
                };
                _context.ProductTranslations.Add(productTranslation);
                await _context.SaveChangesAsync();
                //Add list ProductImgs
                var listProductImg = new List<ProductImg>();
                if (dto.ListImageFile.Count > 0)
                {
                    foreach (var item in dto.ListImageFile)
                    {
                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "image\\listProducts");
                        var name = Guid.NewGuid().ToString() + "_" + item.FileName;
                        string filePath = Path.Combine(uploadsFolder, name);
                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        item.CopyTo(fileStream);
                        //add productImg
                        var productImg = new ProductImg()
                        {
                            ProductId = product.ProductId,
                            Img = name
                        };
                        listProductImg.Add(productImg);
                    }
                    _context.ProductImgs.AddRange(listProductImg);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        // GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Voucher,ProductImg")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            return View(product);
        }

        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
        private string ProcessUploadedFile(ProductDto dto)
        {
            if (dto.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "image\\product");
                dto.ImageName = Guid.NewGuid().ToString() + "_" + dto.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, dto.ImageName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                dto.ImageFile.CopyTo(fileStream);
            }

            return dto.ImageName;
        }
    }
}
