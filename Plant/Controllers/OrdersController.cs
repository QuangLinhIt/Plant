using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Plant.Areas.Identity.Data;
using Plant.Models;
using Plant.ViewModels;

namespace Plant.Controllers
{
    public class OrdersController : Controller
    {
        private readonly plantContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public OrdersController(plantContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Orders
        public IActionResult Cart()
        {
            return View();
        }
        public JsonResult GetProductById(int productId, string color)
        {
            //get selected languages
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            var culture = cultureInfo.Name;

            var product = (from p in _context.Products
                           join pc in _context.ProductColors on p.ProductId equals pc.ProductId
                           join pt in _context.ProductTranslations on p.ProductId equals pt.ProductId
                           join l in _context.Languages on pt.LangId equals l.LangId
                           where p.ProductId == productId && l.SignLanguages == culture && pc.Color == color
                           select new ProductVm()
                           {
                               ProductId = p.ProductId,
                               Image = p.Image,
                               ProductName = pt.ProductName,
                               Price = p.Price,
                               OriginalPrice = p.OriginalPrice,
                               Color = pc.Color,
                               Stock = pc.Stock
                           }).FirstOrDefault();
            return Json(product);
        }

        public async Task<IActionResult> Order()
        {
            var orderVm = new OrderVm();
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var email = await _userManager.GetEmailAsync(user);
                orderVm.FirstName = user.FirstName;
                orderVm.LastName = user.LastName;
                orderVm.Email = user.Email;
                if (user.PhoneNumber != null)
                {
                    orderVm.Phone = user.PhoneNumber;
                }
                var customer = _context.Customers.Where(x => x.Email == email).FirstOrDefault();
                if (customer != null)
                {
                    orderVm.City = customer.City;
                    orderVm.District = customer.District;
                    orderVm.Ward = customer.Ward;
                    orderVm.Road = customer.Road;
                }
                return View(orderVm);
            }
            else
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Order(OrderVm data)
        {
            if (data.FirstName != null && data.LastName != null && data.Email != null
                && data.Phone != null && data.City != 0 && data.District != 0 
                && data.Ward != 0 && data.Ward != 0 && data.Road != null && data.PaymentId != 0)
            {
                //add table order
                var newOrder = new Order();
                newOrder.PaymentId = data.PaymentId;
                newOrder.PaymentStatus = "Chờ xác nhận";
                newOrder.CreateDate = DateTime.Now;
                newOrder.ShipFee = 50000;
                newOrder.OrderStatus = "Chờ xác nhận";
                newOrder.Deleted = false;
                decimal money = 0;
                decimal total = 50000;
                foreach (var item in data.ListCart)
                {
                    money += item.Price * item.Quantity;
                    total += item.Price * item.Quantity;
                }
                newOrder.Money = money;
                newOrder.Total = total;
                //add table customer
                var customer = _context.Customers
                    .Where(x => x.Email == data.Email && x.Phone == data.Phone
                    && x.City == data.City && x.District == data.District
                    && x.Ward == data.Ward && x.Road == data.Road)
                    .FirstOrDefault();
                if (customer == null)
                {
                    var newCustomer = new Customer()
                    {
                        Email = data.Email,
                        FirstName = data.FirstName,
                        LastName = data.LastName,
                        Phone = data.Phone,
                        City = data.City,
                        District = data.District,
                        Ward = data.Ward,
                        Road = data.Road
                    };
                    _context.Customers.Add(newCustomer);
                    _context.SaveChanges();
                    newOrder.CustomerId = newCustomer.CustomerId;
                }
                else
                {
                    newOrder.CustomerId = customer.CustomerId;
                }
                _context.Orders.Add(newOrder);
                _context.SaveChanges();

                //add table ProductOrder
                var listCart = new List<ProductOrder>();
                foreach (var item in data.ListCart)
                {
                    listCart.Add(new ProductOrder()
                    {
                        OrderId = newOrder.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        OriginalPrice = item.OriginalPrice,
                        Color = item.Color,
                        FeedbackId = null
                    });
                }
                newOrder.ProductOrders = listCart;
                _context.SaveChanges();
                return RedirectToAction("CartDone", "Orders", new { id = newOrder.OrderId });
            }
            else
            {
                return View(data);
            }
        }
        public IActionResult CartDone(int id)
        {
            var order = _context.Orders.Where(x => x.OrderId == id).FirstOrDefault();
            if (order != null)
            {
                var customer = _context.Customers.Where(x => x.CustomerId == order.CustomerId).FirstOrDefault();
                var orderVm = new OrderVm()
                {
                    OrderId=order.OrderId,
                    CustomerId = customer.CustomerId,
                    Total = order.Total,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Phone = customer.Phone,
                    City = customer.City,
                    District = customer.District,
                    Ward = customer.Ward,
                    Road = customer.Road
                };
                return View(orderVm);
            }
            else
            {
                return NotFound();
            }

        }
    }
}
