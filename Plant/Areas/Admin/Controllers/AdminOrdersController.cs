using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Plant.ModelDto;
using Plant.Models;

namespace Plant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminOrdersController : Controller
    {
        private readonly plantContext _context;

        public AdminOrdersController(plantContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminOrders
        public IActionResult Index(int page=1,int day=0,int month=0,int year=0)
        {
            var pageNumber = page;
            var pageSize = 10;
            var result = (from o in _context.Orders
                         join c in _context.Customers on o.CustomerId equals c.CustomerId
                         orderby o.OrderId descending
                         select new OrderDto()
                         {
                             OrderId = o.OrderId,
                             CustomerId = o.CustomerId,
                             FirstName=c.FirstName,
                             LastName=c.LastName,
                             Email = c.Email,
                             Total = o.Total,
                             CreateDate = o.CreateDate,
                             OrderStatus=o.OrderStatus
                         }).ToList();
            PagedList<OrderDto> models = new PagedList<OrderDto>(result.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        public IActionResult FiltterCalendar(int day=0,int month=0,int year=0)
        {
            var url = $"/Admin/AdminOrders/Index?day={day}&month={month}&year={year}";
            if (day==0 || month ==0 || year==0)
                url = $"/Admin/AdminOrders/Index";
            return Json(new { status = "success", redirectUrl = url });
        }
        // GET: Admin/AdminOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", order.CustomerId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "PaymentName", order.PaymentId);
            return View(order);
        }

        // POST: Admin/AdminOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,PaymentId,PaymentStatus,CreateDate,Money,ShipFee,Total,OrderStatus,Deleted")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", order.CustomerId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "PaymentName", order.PaymentId);
            return View(order);
        }

        // GET: Admin/AdminOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Payment)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/AdminOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
