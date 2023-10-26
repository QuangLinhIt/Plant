using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Plant.Models;

namespace Plant.Controllers
{
    public class OrdersController : Controller
    {
        private readonly plantContext _context;

        public OrdersController(plantContext context)
        {
            _context = context;
        }

        // GET: Orders
        public IActionResult Cart()
        {
            return View();
        }

    }
}
