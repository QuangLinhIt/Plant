using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Plant.Areas.Identity.Data;
using Plant.Models;
using Plant.Services;
using Plant.ViewModels;

namespace Plant.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class OrdersController : Controller
    {
        private readonly plantContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IVnPayService _vnPayservice;
        public INotyfService _notyfService { get; }

        public OrdersController(plantContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IVnPayService vnPayService, INotyfService notyfService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _vnPayservice = vnPayService;
            _notyfService = notyfService;
        }

        // GET: Orders
        [HttpGet]
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
            if (product != null)
            {
                return Json(product);
            }
            else
            {
                return Json(new { error = "Product not found" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Order(string returnUrl = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = "/Orders/Order" });
            }
            var orderVm = new OrderVm();
            var user = await _userManager.GetUserAsync(User);
            var email = await _userManager.GetEmailAsync(user);
            orderVm.FirstName = user.FirstName;
            orderVm.LastName = user.LastName;
            orderVm.Email = user.Email;
            var customer = _context.Customers.Where(x => x.Email == email).FirstOrDefault();
            if (customer != null)
            {
                orderVm.City = customer.City;
                orderVm.District = customer.District;
                orderVm.Ward = customer.Ward;
                orderVm.Road = customer.Road;
                if (user.PhoneNumber != null)
                {
                    orderVm.Phone = user.PhoneNumber;
                }
                else
                {
                    orderVm.Phone = customer.Phone;
                }
            }
            return View(orderVm);
        }

        [HttpPost]
        public IActionResult Order(OrderVm data, string payment = "")
        {

            if (data.FirstName != null && data.LastName != null && data.Email != null
                && data.Phone != null && data.City != 0 && data.District != 0
                && data.Ward != 0 && data.Ward != 0 && data.Road != null)
            {
                decimal money = 0;
                decimal total = 50000;
                foreach (var item in data.ListCart)
                {
                    money += item.Price * item.Quantity;
                    total += item.Price * item.Quantity;
                }
                //tạo đơn hàng trước

                //add table order
                var newOrder = new Order();
                newOrder.CreateDate = DateTime.Now;
                newOrder.ShipFee = 50000;
                newOrder.PaymentStatus = "";
                newOrder.OrderStatus = "Chờ xác nhận";
                newOrder.Deleted = false;
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
                    var productColor = _context.ProductColors.Where(x => x.ProductId == item.ProductId && x.Color == item.Color).FirstOrDefault();
                    productColor.Stock -= item.Quantity;
                    _context.ProductColors.Update(productColor);
                    _context.SaveChanges();
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
                if (payment == "Thanh toán VNPAY")
                {
                    newOrder.PaymentStatus = "Thanh toán VNPAY";
                    _context.Orders.Update(newOrder);
                    _context.SaveChanges();
                    //xử lí thanh toán
                    var vnPayModel = new VnPayRequestVM()
                    {
                        Amount = total,
                        CreatedDate = DateTime.Now,
                        Description = $"{data.LastName} {data.FirstName} {data.Phone}",
                        OrderId = newOrder.OrderId,
                        FullName = $"{data.LastName} {data.FirstName}"
                    };
                    return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
                }
                if (payment == "Đặt hàng (COD)")
                {
                    newOrder.PaymentStatus = "Thanh toán khi nhận hàng";
                    _context.Orders.Update(newOrder);
                    _context.SaveChanges();
                    return RedirectToAction("CartDone", "Orders", new { id = newOrder.OrderId });
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return View(data);
            }
        }

        [HttpGet]
        public IActionResult CartDone(int id)
        {
            var order = _context.Orders.Where(x => x.OrderId == id).FirstOrDefault();
            if (order != null)
            {
                var customer = _context.Customers.Where(x => x.CustomerId == order.CustomerId).FirstOrDefault();
                var orderVm = new OrderVm()
                {
                    OrderId = order.OrderId,
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

        public IActionResult PaymentCallBack()
        {
            var response = _vnPayservice.PaymentExecute(Request.Query);
            string newOrderDescription = new string(response.OrderDescription.Where(char.IsDigit).ToArray());
            int orderId = int.Parse(newOrderDescription);
            if (response == null || response.VnPayResponseCode != "00") //thanh toán thất bại
            {
                //xóa đơn hàng
                var deleteOrder = _context.Orders.Where(x => x.OrderId == orderId).FirstOrDefault();
                if (deleteOrder != null)
                {
                    var deleteProductOrder = _context.ProductOrders.Where(x => x.OrderId == orderId).ToList();
                    foreach (var item in deleteProductOrder)
                    {
                        var productColor = _context.ProductColors.Where(x => x.ProductId == item.ProductId && x.Color == item.Color).FirstOrDefault();
                        productColor.Stock += item.Quantity;
                        _context.ProductColors.Update(productColor);
                        _context.SaveChanges();
                    }
                    _context.ProductOrders.RemoveRange(deleteProductOrder);
                    _context.SaveChanges();
                    _context.Orders.Remove(deleteOrder);
                    _context.SaveChanges();
                }
                _notyfService.Error("Lỗi thanh toán VN Pay:" + response.VnPayResponseCode);
                return RedirectToAction("Index", "Products");
            }
            // cập nhật đơn hàng khi thanh toán thành công
            var updateOrder = _context.Orders.Where(x => x.OrderId == orderId).FirstOrDefault();
            updateOrder.PaymentStatus = "Đã thanh toán thành công";
            _context.Orders.Update(updateOrder);
            _context.SaveChanges();
            return RedirectToAction("CartDone", "Orders", new { id = updateOrder.OrderId });
        }
    }
}
