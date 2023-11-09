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
            var listProduct = new List<ProductTranslation>();
            if (LangId != 0)
            {
                listProduct = _context.ProductTranslations
                     .Include(x => x.Product)
                     .Include(x => x.Lang)
                     .Where(x => x.LangId == LangId)
                     .OrderByDescending(x => x.ProductId)
                     .AsNoTracking()
                     .ToList();
            }
            else
            {
                listProduct = _context.ProductTranslations
                      .Include(x => x.Product)
                      .Include(x => x.Lang)
                      .OrderByDescending(x => x.ProductId)
                      .AsNoTracking()
                      .ToList();
            }
            var models = new PagedList<ProductTranslation>(listProduct.AsQueryable(), pageNumber, pageSize);
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
                var product = new Product();

                product.Voucher = dto.Voucher;
                product.Image = ProcessUploadedFile(dto);
                if (dto.Voucher == null)
                {
                    product.Price = dto.OriginalPrice;
                }
                else
                {
                    product.Price = dto.OriginalPrice * (100 - dto.Voucher.Value) / 100;
                }
                product.OriginalPrice = dto.OriginalPrice;

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
                var productTranslation = new ProductTranslation();
                productTranslation.ProductId = product.ProductId;
                productTranslation.LangId = dto.LangId;
                productTranslation.ProductName = dto.ProductName;
                productTranslation.ShortDes = dto.ShortDes;
                productTranslation.Description = dto.Description;
                productTranslation.TakeCare = dto.TakeCare;
                productTranslation.Application = dto.Application;

                _context.ProductTranslations.Add(productTranslation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        // GET: Admin/AdminProducts/Edit/5
        public IActionResult Edit(int? id, int? langId)
        {
            if (id == null || langId == null)
            {
                return NotFound();
            }
            ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", langId);
            var product = _context.Products.Include(x => x.ProductCategories).Where(x => x.ProductId == id).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                var productTranslation = _context.ProductTranslations.Include(x => x.Lang).Where(x => x.ProductId == id && x.LangId == langId).FirstOrDefault();
                var dto = new ProductDto();
                var categoryIds = new List<int>();
                foreach (var item in product.ProductCategories.ToList())
                {
                    categoryIds.Add(item.CategoryId);
                }
                dto.ProductId = product.ProductId;
                dto.LangId = productTranslation.LangId;
                dto.LangName = productTranslation.Lang.LangName;
                dto.Voucher = product.Voucher;
                dto.ProductName = productTranslation.ProductName;
                dto.Price = product.Price;
                dto.OriginalPrice = product.OriginalPrice;
                dto.ShortDes = productTranslation.ShortDes;
                dto.Description = productTranslation.Description;
                dto.TakeCare = productTranslation.TakeCare;
                dto.Application = productTranslation.Application;
                dto.ImageName = product.Image;
                dto.CategoryIds = categoryIds.ToArray();
                dto.ListCategory = _context.CategoryTranslations.Where(x => x.LangId == 1).Select(x => new SelectListItem { Text = x.CategoryName, Value = x.CategoryId.ToString() }).ToList();
                return View(dto);
            }
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, int langId, ProductDto dto)
        {
            if (id != dto.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", dto.LangId);
                    //find product
                    var product = _context.Products.Include(x => x.ProductCategories).Where(x => x.ProductId == dto.ProductId).FirstOrDefault();
                    product.ProductId = dto.ProductId;
                    product.Voucher = dto.Voucher;
                    product.OriginalPrice = dto.OriginalPrice;
                    if (dto.Voucher == null)
                    {
                        product.Price = product.OriginalPrice;
                    }
                    else
                    {
                        product.Price = dto.OriginalPrice * (100 - dto.Voucher.Value) / 100;
                    }
                    if (dto.ImageFile != null)
                    {
                        string filePath = Path.Combine(_hostEnvironment.WebRootPath, "image\\product", product.Image);
                        System.IO.File.Delete(filePath);
                        product.Image = ProcessUploadedFile(dto);
                    }
                    else
                    {
                        product.Image = dto.ImageName;
                    }
                    _context.Products.Update(product);
                    _context.SaveChanges();
                    //find list productCategory and remove
                    var listProductCategory = new List<ProductCategory>();
                    product.ProductCategories.ToList().ForEach(result => listProductCategory.Add(result));
                    _context.ProductCategories.RemoveRange(listProductCategory);
                    _context.SaveChanges();
                    //add productCategory 
                    if (dto.CategoryIds.Length > 0)
                    {
                        listProductCategory = new List<ProductCategory>();
                        foreach (var item in dto.CategoryIds)
                        {
                            listProductCategory.Add(new ProductCategory()
                            {
                                ProductId = product.ProductId,
                                CategoryId = item
                            });
                        }
                        product.ProductCategories = listProductCategory;
                    }
                    _context.SaveChanges();

                    //update ProductTranslation
                    var productTranslation = _context.ProductTranslations.Where(x => x.ProductId == dto.ProductId && x.LangId == dto.LangId).FirstOrDefault();
                    if (productTranslation != null)
                    {
                        productTranslation.ProductTranslationId = productTranslation.ProductTranslationId;
                        productTranslation.LangId = dto.LangId;
                        productTranslation.ProductName = dto.ProductName;
                        productTranslation.ShortDes = dto.ShortDes;
                        productTranslation.Description = dto.Description;
                        productTranslation.TakeCare = dto.TakeCare;
                        productTranslation.Application = dto.Application;
                       
                        _context.ProductTranslations.Update(productTranslation);
                        _context.SaveChanges();
                    }
                    else
                    {
                        productTranslation = new ProductTranslation();
                        productTranslation.LangId = dto.LangId;
                        productTranslation.ProductName = dto.ProductName;
                        productTranslation.ShortDes = dto.ShortDes;
                        productTranslation.Description = dto.Description;
                        productTranslation.TakeCare = dto.TakeCare;
                        productTranslation.Application = dto.Application;
                       
                        _context.ProductTranslations.Add(productTranslation);
                        _context.SaveChanges();
                    }
                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        //GET: Admin/AdminProducts/AddLanguages
        public IActionResult AddLanguages(int? id, int? langId)
        {
            if (id == null || langId == null)
            {
                return NotFound();
            }
            ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", langId);
            var result = (from pt in _context.ProductTranslations
                          where pt.ProductId == id && pt.LangId == langId
                          select new ProductTranslation()
                          {
                              ProductTranslationId = pt.ProductTranslationId,
                              ProductId = pt.ProductId,
                              LangId = pt.LangId,
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
        //POST:Admin/AdminProducts/AddLanguages
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddLanguages(int id, int langId, ProductTranslation data)
        {
            if (id != data.ProductId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    ViewData["Languages"] = new SelectList(_context.Languages, "LangId", "LangName", data.LangId);
                    var product = _context.Products.Where(x => x.ProductId == data.ProductId).FirstOrDefault();
                    if (product != null)
                    {
                        var productTranslation = _context.ProductTranslations.Where(x => x.ProductId == data.ProductId && x.LangId == data.LangId).FirstOrDefault();
                        if (productTranslation != null)
                        {
                            _context.ProductTranslations.Remove(productTranslation);
                            _context.SaveChanges();
                            var result = new ProductTranslation();
                            result.LangId = data.LangId;
                            result.ProductId = data.ProductId;
                            result.ProductName = data.ProductName;
                            result.ShortDes = data.ShortDes;
                            result.Description = data.Description;
                            result.TakeCare = data.TakeCare;
                            result.Application = data.Application;
                            _context.ProductTranslations.Add(result);
                            _context.SaveChanges();
                        }
                        else
                        {
                            var result = new ProductTranslation();
                            result.LangId = data.LangId;
                            result.ProductId = data.ProductId;
                            result.ProductName = data.ProductName;
                            result.ShortDes = data.ShortDes;
                            result.Description = data.Description;
                            result.TakeCare = data.TakeCare;
                            result.Application = data.Application;
                            _context.ProductTranslations.Add(result);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        return NotFound();
                    }

                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(data);
        }
        //GET:Admin/AdminProducts/Color
        public IActionResult Color(int? id, int? langId)
        {
            if (id == null || langId == null)
            {
                return NotFound();
            }
            var result = _context.ProductColors
                .Where(x => x.ProductId == id)
                .ToList();
            var productTranslation = (from pt in _context.ProductTranslations
                                      where pt.ProductId == id && pt.LangId == langId
                                      select new ProductTranslation()
                                      {
                                          ProductName = pt.ProductName,
                                          LangId = pt.LangId,
                                      }).FirstOrDefault();
            ViewBag.ProductTranslationData = productTranslation;
            var product = (from p in _context.Products
                           where p.ProductId == id
                           select new Product()
                           {
                               ProductId = p.ProductId,
                               Image = p.Image,
                               Voucher = p.Voucher,
                           }).FirstOrDefault();
            ViewBag.ProductData = product;
            var languages = _context.Languages.Where(x => x.LangId == langId).FirstOrDefault();
            ViewBag.Languages = languages;
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        //GET:Admin/AdminProducts/AddColor
        public IActionResult AddColor(int? id, int? langId)
        {
            if (id == null || langId == null)
            {
                return NotFound();
            }
            ViewBag.ProductId = id;
            ViewBag.Languages = langId;
            return View();
        }
        //POST:Admin/AdminProducts/AddColor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddColor(int id, int langId, ProductColorDto dto)
        {
            if (id != dto.ProductId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var data = new ProductColor()
                    {
                        ProductId = dto.ProductId,
                        Color = dto.Color,
                        Stock = dto.Stock
                    };
                    _context.ProductColors.Add(data);
                    _context.SaveChanges();
                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction("Color", "AdminProducts", new { id = dto.ProductId, langId = dto.LangId });
            }
            return View(dto);
        }
        //GET:Admin/AdminProducts
        public IActionResult EditColor(int? id, int? langId)
        {
            if (id == null || langId == null)
            {
                return NotFound();
            }
            var productColor = _context.ProductColors.Where(x => x.ProductColorId == id).FirstOrDefault();
            var productTranslation = _context.ProductTranslations
                .Where(x => x.ProductId == productColor.ProductId && x.LangId == langId)
                .FirstOrDefault();
            var dto = new ProductColorDto()
            {
                ProductColorId = id.Value,
                ProductId = productColor.ProductId,
                ProductName = productTranslation.ProductName,
                LangId = langId.Value,
                Color = productColor.Color,
                Stock = productColor.Stock,
            };
            return View(dto);
        }
        //POST:Admin/AdminProducts/EditColor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditColor(int id, int langId, ProductColorDto dto)
        {
            if (id != dto.ProductColorId || langId != dto.LangId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var productColor = _context.ProductColors.Where(x => x.ProductColorId == dto.ProductColorId).FirstOrDefault();
                if (productColor == null)
                {
                    return NotFound();
                }
                productColor.ProductColorId = dto.ProductColorId;
                productColor.ProductId = dto.ProductId;
                productColor.Color = dto.Color;
                productColor.Stock = dto.Stock;
                _context.ProductColors.Update(productColor);
                _context.SaveChanges();
                return RedirectToAction("Color", "AdminProducts", new { id = dto.ProductId, langId = dto.LangId });
            }
            return View(dto);
        }
        //GET:Admin/AdminProducts/DeleteColor
        public IActionResult DeleteColor(int? id, int? langId)
        {
            if (id == null || langId == null)
            {
                return NotFound();
            }
            var productColor = _context.ProductColors.Where(x => x.ProductColorId == id).FirstOrDefault();
            var productTranslation = _context.ProductTranslations.Where(x => x.ProductId == productColor.ProductId).FirstOrDefault();
            if (productTranslation != null)
            {
                var dto = new ProductColorDto()
                {
                    ProductColorId = id.Value,
                    ProductId = productColor.ProductId,
                    ProductName = productTranslation.ProductName,
                    LangId = langId.Value,
                    Color = productColor.Color,
                    Stock = productColor.Stock
                };
                return View(dto);
            }
            else
            {
                return NotFound();
            }
        }
        //Delete:Admin/AdminProducts/DeleteColor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteColor(int id, int langId, ProductColorDto dto)
        {
            if (id != dto.ProductColorId || langId != dto.LangId)
            {
                return NotFound();
            }
            var productColor = _context.ProductColors.Where(x => x.ProductColorId == id).FirstOrDefault();
            _context.ProductColors.Remove(productColor);
            _context.SaveChanges();
            return RedirectToAction("Color", "AdminProducts", new { id = dto.ProductId, langId = dto.LangId });
        }

        //GET: Admin/AdminProducts/AddPicture
        public IActionResult Picture(int? id, int? langId)
        {
            if (id == null || langId == null)
            {
                return NotFound();
            }
            var productTranslation = _context.ProductTranslations.Where(x => x.ProductId == id && x.LangId == langId).FirstOrDefault();
            var productImg = _context.ProductImgs.Where(x => x.ProductId == id).ToList();
            var dto = new ProductImageDto();
            if (productImg == null)
            {
                dto.ProductId = id.Value;
                dto.LangId = langId.Value;
                dto.ProductName = productTranslation.ProductName;
            }
            else
            {
                dto.ProductId = id.Value;
                dto.LangId = langId.Value;
                dto.ProductName = productTranslation.ProductName;
                var listProductName = new List<string>();
                foreach (var item in productImg)
                {
                    listProductName.Add(item.Img);
                }
                dto.ListImageName = listProductName.ToArray();
            }
            return View(dto);
        }
        //POST:Admin/AdminProducts/Picture
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Picture(int id, int langId, ProductImageDto dto)
        {
            if (id != dto.ProductId || langId != dto.LangId)
            {
                return NotFound();
            }
            if (dto.ListImageFile == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var exist = _context.ProductImgs.Where(x => x.ProductId == dto.ProductId).ToList();
                var listProductImg = new List<ProductImg>();
                if (exist == null)
                {
                    //add picture
                    foreach (var item in dto.ListImageFile)
                    {
                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "image\\listProduct");
                        string filePath = Path.Combine(uploadsFolder, item.FileName);
                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        item.CopyTo(fileStream);
                        listProductImg.Add(new ProductImg()
                        {
                            ProductId = dto.ProductId,
                            Img = item.FileName
                        });
                    }
                    _context.ProductImgs.AddRange(listProductImg);
                    _context.SaveChanges();
                }
                else
                {
                    //delete image in wwwroot
                    foreach (var item in exist)
                    {
                        string filePath = Path.Combine(_hostEnvironment.WebRootPath, "image\\listProduct", item.Img);
                        System.IO.File.Delete(filePath);
                    }
                    //delete exist
                    _context.ProductImgs.RemoveRange(exist);
                    _context.SaveChanges();
                    //add picture
                    foreach (var item in dto.ListImageFile)
                    {
                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "image\\listProduct");
                        string filePath = Path.Combine(uploadsFolder, item.FileName);
                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        item.CopyTo(fileStream);
                        listProductImg.Add(new ProductImg()
                        {
                            ProductId = dto.ProductId,
                            Img = item.FileName
                        });
                    }
                    _context.ProductImgs.AddRange(listProductImg);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }
        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id, int? langId)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productTranslation = _context.ProductTranslations.Where(x => x.ProductId == id && x.LangId == langId).FirstOrDefault();
            if (productTranslation == null)
            {
                return NotFound();
            }
            return View(productTranslation);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int langId)
        {
            var product = _context.Products.Where(x => x.ProductId == id).FirstOrDefault();
            var productCategory = _context.ProductCategories.Where(x => x.ProductId == id).ToList();
            var productColor = _context.ProductColors.Where(x => x.ProductId == id).ToList();
            var productTranslation = _context.ProductTranslations.Where(x => x.ProductId == id).ToList();
            var productImg = _context.ProductImgs.Where(x => x.ProductId == id).ToList();
            //delete productImages from wwwroot
            foreach (var item in productImg)
            {
                var CurrentProductImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image\\listProduct", item.Img);
                _context.ProductImgs.Remove(item);
                if (await _context.SaveChangesAsync() > 0)
                {
                    if (System.IO.File.Exists(CurrentProductImage))
                    {
                        System.IO.File.Delete(CurrentProductImage);
                    }
                }
            }
            //delete table 
            _context.ProductTranslations.RemoveRange(productTranslation);
            _context.SaveChanges();
            _context.ProductColors.RemoveRange(productColor);
            _context.SaveChanges();
            _context.ProductCategories.RemoveRange(productCategory);
            _context.SaveChanges();

            //delete product from wwwroot
            var CurrentProduct = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image\\product", product.Image);
            _context.Products.Remove(product);
            if (await _context.SaveChangesAsync() > 0)
            {
                if (System.IO.File.Exists(CurrentProduct))
                {
                    System.IO.File.Delete(CurrentProduct);
                }
            }
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
