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

    public class AdminOrdersController : Controller
    {
        private readonly plantContext _context;

        public AdminOrdersController(plantContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminOrders
        public IActionResult Index(int page = 1, string SearchEmail = "")
        {
            var pageNumber = page;
            var pageSize = 10;
            if (!String.IsNullOrEmpty(SearchEmail))
            {
                var result = (from o in _context.Orders
                              join c in _context.Customers on o.CustomerId equals c.CustomerId
                              orderby o.OrderId descending
                              where c.Email == SearchEmail
                              select new OrderDto()
                              {
                                  OrderId = o.OrderId,
                                  CustomerId = o.CustomerId,
                                  FirstName = c.FirstName,
                                  LastName = c.LastName,
                                  Email = c.Email,
                                  Total = o.Total,
                                  CreateDate = o.CreateDate,
                                  OrderStatus = o.OrderStatus
                              }).ToList();
                PagedList<OrderDto> models = new PagedList<OrderDto>(result.AsQueryable(), pageNumber, pageSize);
                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            else
            {
                var result = (from o in _context.Orders
                              join c in _context.Customers on o.CustomerId equals c.CustomerId
                              orderby o.OrderId descending
                              select new OrderDto()
                              {
                                  OrderId = o.OrderId,
                                  CustomerId = o.CustomerId,
                                  FirstName = c.FirstName,
                                  LastName = c.LastName,
                                  Email = c.Email,
                                  Total = o.Total,
                                  CreateDate = o.CreateDate,
                                  OrderStatus = o.OrderStatus
                              }).ToList();
                PagedList<OrderDto> models = new PagedList<OrderDto>(result.AsQueryable(), pageNumber, pageSize);
                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
        }
        // GET: Admin/AdminOrders/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _context.Orders.Where(x => x.OrderId == id).FirstOrDefault();
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                var customer = _context.Customers.Where(x => x.CustomerId == order.CustomerId).FirstOrDefault();
                var result = new OrderDto();
                result.OrderId = order.OrderId;
                result.PaymentId = order.PaymentId;
                result.PaymentStatus = order.PaymentStatus;
                result.CreateDate = order.CreateDate;
                result.Money = order.Money;
                result.ShipFee = order.ShipFee;
                result.Total = order.Total;
                result.OrderStatus = order.OrderStatus;
                result.Deleted = order.Deleted;
                result.CustomerId = customer.CustomerId;
                result.Email = customer.Email;
                result.FirstName = customer.FirstName;
                result.LastName = customer.LastName;
                result.Phone = customer.Phone;
                result.City = customer.City;
                result.District = customer.District;
                result.Ward = customer.Ward;
                result.Road = customer.Road;
                var listCart = new List<ListCartDto>();
                var productOrder = _context.ProductOrders.Where(x => x.OrderId == order.OrderId).ToList();
                foreach (var item in productOrder)
                {
                    var cartItem = new ListCartDto();
                    cartItem.ProductId = item.ProductId;
                    cartItem.Price = item.Price;
                    cartItem.OriginalPrice = item.OriginalPrice;
                    cartItem.Quantity = item.Quantity;
                    cartItem.Color = item.Color;
                    var product = _context.Products.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                    var productTranslation = _context.ProductTranslations.Where(x => x.ProductId == item.ProductId && x.LangId == 1).FirstOrDefault();
                    cartItem.ProductName = productTranslation.ProductName;
                    cartItem.ProductImg = product.Image;

                    listCart.Add(cartItem);
                }
                result.ListCart = listCart;
                var payment = _context.Payments.Where(x => x.PaymentId == order.PaymentId).FirstOrDefault();
                result.PaymentName = payment.PaymentName;
                var paymentStatus = new List<SelectListItem>()
                    {
                        new SelectListItem { Value = "Chưa thanh toán", Text = "Chưa thanh toán" },
                        new SelectListItem { Value = "Đã thanh toán", Text = "Đã thanh toán" },
                    };
                ViewData["PaymentStatus"] = new SelectList(paymentStatus, "Value", "Text", result.PaymentStatus);

                var orderStatus = new List<SelectListItem>()
                    {
                        new SelectListItem { Value = "Chờ xác nhận", Text = "Chờ xác nhận" },
                        new SelectListItem { Value = "Đang giao hàng", Text = "Đang giao hàng" },
                        new SelectListItem { Value = "Giao hàng thành công", Text = "Giao hàng thành công" },
                        new SelectListItem { Value = "Đã hủy", Text = "Đã hủy" },
                    };
                ViewData["OrderStatus"] = new SelectList(orderStatus, "Value", "Text", result.OrderStatus);
                return View(result);
            }
        }

        // POST: Admin/AdminOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderDto dto)
        {
            if (id != dto.OrderId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var order = _context.Orders.Where(x => x.OrderId == dto.OrderId).FirstOrDefault();
                    //xét sự thay đổi trạng thái đơn hàng lúc trước và sau cập nhật
                    if (order.OrderStatus == "Chờ xác nhận" && dto.OrderStatus == "Đang giao hàng")
                    {
                        foreach (var item in dto.ListCart)
                        {
                            var productColor = _context.ProductColors
                                .Where(x => x.ProductId == item.ProductId && x.Color == item.Color)
                                .FirstOrDefault();
                            productColor.Stock -= item.Quantity;
                            _context.ProductColors.Update(productColor);
                            _context.SaveChanges();
                        }
                    }
                    order.PaymentStatus = dto.PaymentStatus;
                    order.OrderStatus = dto.OrderStatus;
                    _context.Orders.Update(order);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(dto.OrderId))
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
            return View(dto);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
