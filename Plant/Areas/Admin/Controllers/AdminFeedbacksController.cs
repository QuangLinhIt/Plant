using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Plant.ModelDto;
using Plant.Models;

namespace Plant.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class AdminFeedbacksController : Controller
    {
        private readonly plantContext _context;

        public AdminFeedbacksController(plantContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminFeedbacks
        public IActionResult Index(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;
            var result = (from f in _context.Feedbacks
                          join po in _context.ProductOrders on f.FeedbackId equals po.FeedbackId
                          join pt in _context.ProductTranslations on po.ProductId equals pt.ProductId
                          join o in _context.Orders on po.OrderId equals o.OrderId
                          join c in _context.Customers on o.CustomerId equals c.CustomerId
                          where pt.LangId == 1
                          select new FeedbackDto()
                          {
                              Email = c.Email,
                              ProductName = pt.ProductName,
                              FeedbackId = f.FeedbackId,
                              Star = f.Star,
                              FeedBackContent = f.FeedbackContent,
                              ShopFeedbackId = f.ShopFeedbackId
                          }).ToList();
            var models = new PagedList<FeedbackDto>(result.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminFeedbacks/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var result = new FeedbackDto();
            var feedback = _context.Feedbacks.Where(x => x.FeedbackId == id).FirstOrDefault();
            result.FeedbackId = feedback.FeedbackId;
            result.Star = feedback.Star;
            result.FeedBackContent = feedback.FeedbackContent;
            result.ShopFeedbackId = feedback.ShopFeedbackId;
            result.CreateDay = feedback.CreateDay;
            var productOrder = _context.ProductOrders.Where(x => x.FeedbackId == id).FirstOrDefault();
            result.Color = productOrder.Color;
            var product = _context.Products.Where(x => x.ProductId == productOrder.ProductId).FirstOrDefault();
            var productTranslation = _context.ProductTranslations.Where(x => x.ProductId == productOrder.ProductId && x.LangId == 1).FirstOrDefault();
            result.ProductName = productTranslation.ProductName;
            result.ProductImage = product.Image;
            var order = _context.Orders.Where(x => x.OrderId == productOrder.OrderId).FirstOrDefault();
            var customer = _context.Customers.Where(x => x.CustomerId == order.CustomerId).FirstOrDefault();
            result.FirstName = customer.FirstName;
            result.LastName = customer.LastName;
            result.Email = customer.Email;
            result.Phone = customer.Phone;
            var listImg = new List<String>();
            var feedbackImgs = _context.FeedbackImages.Where(x => x.FeedbackId == id).ToList();
            foreach(var item in feedbackImgs)
            {
                listImg.Add(item.FeedbackImg);
            }
            result.ListImageName = listImg.ToArray();
            if (result.ShopFeedbackId != null)
            {
                var shopFeedback = _context.ShopFeedbacks.Where(x => x.ShopFeedbackId == result.ShopFeedbackId).FirstOrDefault();
                result.ShopFeedbackContent = shopFeedback.ShopFeedbackContent;
            }
            return View(result);
        }

        // POST: Admin/AdminFeedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FeedbackDto dto)
        {
            if (id != dto.FeedbackId)
            {
                return NotFound();
            }
            var feedback = _context.Feedbacks.Where(x => x.FeedbackId == id).FirstOrDefault();
            if (feedback.ShopFeedbackId == null)
            {
                var shopFeedback = new ShopFeedback();
                shopFeedback.ShopFeedbackContent = dto.ShopFeedbackContent;
                _context.ShopFeedbacks.Add(shopFeedback);
                _context.SaveChanges();
                feedback.ShopFeedbackId = shopFeedback.ShopFeedbackId;
                _context.Feedbacks.Update(feedback);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        private bool FeedbackExists(int id)
        {
            return _context.Feedbacks.Any(e => e.FeedbackId == id);
        }
    }
}
