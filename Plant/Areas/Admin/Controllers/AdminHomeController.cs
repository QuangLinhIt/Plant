using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        [Area("Admin")]
        [Authorize(Roles = "Admin")]

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
