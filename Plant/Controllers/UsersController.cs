using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Plant.Areas.Identity.Data;
using Plant.Models;
using Plant.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Plant.Controllers
{
    public class UsersController : Controller
    {
        private readonly plantContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        public UsersController(plantContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ManageOrder(string orderStatus="")
        {
            //set language
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            var culture = cultureInfo.Name;
            var language = _context.Languages.Where(x => x.SignLanguages == culture).FirstOrDefault();
            //user did login
            var user = await _userManager.GetUserAsync(User);
            var listCustomer = _context.Customers.Where(x => x.Email == user.Email).ToList();
            var listOrderVm = new List<OrderVm>();

            foreach (var customer in listCustomer)
            {
                if (string.IsNullOrEmpty(orderStatus))
                {
                    var listOrder = _context.Orders.Where(x => x.CustomerId == customer.CustomerId).OrderByDescending(x=>x.OrderId).ToList();
                    foreach (var order in listOrder)
                    {
                        var orderVm = new OrderVm();
                        orderVm.OrderId = order.OrderId;
                        orderVm.CustomerId = order.CustomerId;
                        orderVm.PaymentId = order.PaymentId;
                        var payment = _context.Payments.Where(x => x.PaymentId == order.PaymentId).FirstOrDefault();
                        orderVm.PaymentName = payment.PaymentName;
                        orderVm.CreateDate = order.CreateDate;
                        orderVm.Money = order.Money;
                        orderVm.ShipFee = order.ShipFee;
                        orderVm.Total = order.Total;
                        orderVm.Email = customer.Email;
                        orderVm.FirstName = customer.FirstName;
                        orderVm.LastName = customer.LastName;
                        orderVm.Phone = customer.Phone;
                        orderVm.City = customer.City;
                        orderVm.District = customer.District;
                        orderVm.Ward = customer.Ward;
                        orderVm.Road = customer.Road;
                        orderVm.PaymentStatus = order.PaymentStatus;
                        orderVm.OrderStatus = order.OrderStatus;
                        orderVm.Deleted = order.Deleted;
                        var listProductOrder = _context.ProductOrders.Where(x => x.OrderId == order.OrderId).ToList();
                        var listCartVm = new List<ListCartVm>();
                        foreach (var productOrder in listProductOrder)
                        {
                            var cartVm = new ListCartVm();
                            cartVm.Id = productOrder.Id;
                            cartVm.ProductId = productOrder.ProductId;
                            var product = _context.ProductTranslations
                                .Where(x => x.ProductId == productOrder.ProductId && x.LangId == language.LangId)
                                .FirstOrDefault();
                            cartVm.ProductName = product.ProductName;
                            var productImg = _context.Products.Where(x => x.ProductId == productOrder.ProductId).FirstOrDefault();
                            cartVm.ProductImg = productImg.Image;
                            cartVm.Price = productOrder.Price;
                            cartVm.OriginalPrice = productOrder.OriginalPrice;
                            cartVm.Quantity = productOrder.Quantity;
                            cartVm.Color = productOrder.Color;
                            cartVm.FeedbackId = productOrder.FeedbackId;
                            listCartVm.Add(cartVm);
                        }
                        orderVm.ListCart = listCartVm;
                        listOrderVm.Add(orderVm);
                    }

                }
                else
                {
                    var listOrder = _context.Orders.Where(x => x.CustomerId == customer.CustomerId && x.OrderStatus==orderStatus).ToList();
                    foreach (var order in listOrder)
                    {
                        var orderVm = new OrderVm();
                        orderVm.OrderId = order.OrderId;
                        orderVm.CustomerId = order.CustomerId;
                        orderVm.PaymentId = order.PaymentId;
                        var payment = _context.Payments.Where(x => x.PaymentId == order.PaymentId).FirstOrDefault();
                        orderVm.PaymentName = payment.PaymentName;
                        orderVm.CreateDate = order.CreateDate;
                        orderVm.Money = order.Money;
                        orderVm.ShipFee = order.ShipFee;
                        orderVm.Total = order.Total;
                        orderVm.Email = customer.Email;
                        orderVm.FirstName = customer.FirstName;
                        orderVm.LastName = customer.LastName;
                        orderVm.Phone = customer.Phone;
                        orderVm.City = customer.City;
                        orderVm.District = customer.District;
                        orderVm.Ward = customer.Ward;
                        orderVm.Road = customer.Road;
                        orderVm.PaymentStatus = order.PaymentStatus;
                        orderVm.OrderStatus = order.OrderStatus;
                        orderVm.Deleted = order.Deleted;
                        var listProductOrder = _context.ProductOrders.Where(x => x.OrderId == order.OrderId).ToList();
                        var listCartVm = new List<ListCartVm>();
                        foreach (var productOrder in listProductOrder)
                        {
                            var cartVm = new ListCartVm();
                            cartVm.ProductId = productOrder.ProductId;
                            var product = _context.ProductTranslations
                                .Where(x => x.ProductId == productOrder.ProductId && x.LangId == language.LangId)
                                .FirstOrDefault();
                            cartVm.ProductName = product.ProductName;
                            var productImg = _context.Products.Where(x => x.ProductId == productOrder.ProductId).FirstOrDefault();
                            cartVm.ProductImg = productImg.Image;
                            cartVm.Price = productOrder.Price;
                            cartVm.OriginalPrice = productOrder.OriginalPrice;
                            cartVm.Quantity = productOrder.Quantity;
                            cartVm.Color = productOrder.Color;
                            cartVm.FeedbackId = productOrder.FeedbackId;
                            listCartVm.Add(cartVm);
                        }
                        orderVm.ListCart = listCartVm;
                        listOrderVm.Add(orderVm);
                    }

                }
            }
            return View(listOrderVm);
        }

        public IActionResult AddFeedback(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                //set language
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                var culture = cultureInfo.Name;
                var language = _context.Languages.Where(x => x.SignLanguages == culture).FirstOrDefault();

                var productOrder = _context.ProductOrders.Where(x => x.Id == id).FirstOrDefault();
                var feedbackVm = new AddFeedbackVm();
                feedbackVm.ProductOrderId = productOrder.Id;
                feedbackVm.ProductId = productOrder.ProductId;
                var productTranslation = _context.ProductTranslations.Where(x => x.ProductId == productOrder.ProductId && x.LangId == language.LangId).FirstOrDefault();
                feedbackVm.ProductName = productTranslation.ProductName;
                feedbackVm.Color = productOrder.Color;
                var product = _context.Products.Where(x => x.ProductId == productOrder.ProductId).FirstOrDefault();
                feedbackVm.ProductImage = product.Image;
                return View(feedbackVm);
            }
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddFeedback(int id,AddFeedbackVm data)
        {
            if (id != data.ProductOrderId)
            {
                return NotFound();
            }
            else
            {
                if(data.ListImageFile ==null || data.FeedBackContent==null)
                {
                    return View(data);
                }
                var feedback = new Feedback();
                feedback.FeedbackContent = data.FeedBackContent;
                feedback.Star = data.Star;
                feedback.CreateDay = DateTime.Now;
                _context.Feedbacks.Add(feedback);
                _context.SaveChanges();
                var listFeedbackImage = new List<FeedbackImage>();
                //add picture
                foreach (var item in data.ListImageFile)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "image\\feedback");
                    string filePath = Path.Combine(uploadsFolder, item.FileName);
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    item.CopyTo(fileStream);
                    listFeedbackImage.Add(new FeedbackImage()
                    {
                        FeedbackId=feedback.FeedbackId,
                        FeedbackImg=item.FileName
                    });
                }
                _context.FeedbackImages.AddRange(listFeedbackImage);
                _context.SaveChanges();
                var productOrder = _context.ProductOrders.Where(x => x.Id == data.ProductOrderId).FirstOrDefault();
                productOrder.FeedbackId = feedback.FeedbackId;
                _context.ProductOrders.Update(productOrder);
                _context.SaveChanges();
                return RedirectToAction("ManageOrder", "Users", new { orderStatus = "" });
            }
        }
        
        public IActionResult ReviewFeedback(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //set language
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            var culture = cultureInfo.Name;
            var language = _context.Languages.Where(x => x.SignLanguages == culture).FirstOrDefault();

            var result = new ReviewFeedbackVm();
            var productOrder = _context.ProductOrders.Where(x => x.Id == id).FirstOrDefault();
            result.ProductOrderId = productOrder.Id;
            var order = _context.Orders.Where(x => x.OrderId == productOrder.OrderId).FirstOrDefault();
            var customer = _context.Customers.Where(x => x.CustomerId == order.CustomerId).FirstOrDefault();
            result.LastName = customer.LastName;
            result.FirstName = customer.FirstName;
            result.ProductId = productOrder.ProductId;
            var product = _context.Products.Where(x => x.ProductId == productOrder.ProductId).FirstOrDefault();
            result.ProductImage = product.Image;
            var productTranslation = _context.ProductTranslations.Where(x => x.ProductId == productOrder.ProductId && x.LangId == language.LangId).FirstOrDefault();
            result.ProductName = productTranslation.ProductName;
            result.Color = productOrder.Color;
            if (productOrder.FeedbackId != null)
            {
                var feedback = _context.Feedbacks.Where(x => x.FeedbackId == productOrder.FeedbackId).FirstOrDefault();
                result.FeedbackId = feedback.FeedbackId;
                result.Star = feedback.Star;
                result.CreateDay = feedback.CreateDay;
                var feedbackImgs = _context.FeedbackImages.Where(x => x.FeedbackId == productOrder.FeedbackId).ToList();
                var listImg = new List<string>();
                foreach (var item in feedbackImgs)
                {
                    listImg.Add(item.FeedbackImg);
                }
                result.ListImageName = listImg.ToArray();
                result.FeedBackContent = feedback.FeedbackContent;
                if (feedback.ShopFeedbackId != null)
                {
                    var shopFeedback = _context.ShopFeedbacks.Where(x => x.ShopFeedbackId == feedback.ShopFeedbackId).FirstOrDefault();
                    result.ShopFeedbackId = shopFeedback.ShopFeedbackId;
                    result.ShopFeedbackContent = shopFeedback.ShopFeedbackContent;
                    return View(result);
                }
                else{
                    return View(result);
                }
            }
            else
            {
                return NotFound();
            }
            
        }
    }
}
