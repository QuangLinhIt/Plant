using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Plant.Models;

namespace Plant.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminAspNetUsersController : Controller
    {
        private readonly plantContext _context;

        public AdminAspNetUsersController(plantContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminAspNetUsers
        public IActionResult Index(int page=1)
        {
            var pageNumber = page;
            var pageSize = 10;
            var result = _context.AspNetUsers.ToList();
            PagedList<AspNetUser> models = new PagedList<AspNetUser>(result.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        private bool AspNetUserExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }
    }
}
