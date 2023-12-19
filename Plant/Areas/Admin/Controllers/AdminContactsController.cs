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

    public class AdminContactsController : Controller
    {
        private readonly plantContext _context;

        public AdminContactsController(plantContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminContacts
        public IActionResult Index(int page=1)
        {
            var pageNumber = page;
            var pageSize = 10;
            var ListContact = new List<Contact>();
            ListContact = _context.Contacts
            .AsNoTracking()
            .OrderByDescending(x => x.ContactId)
            .ToList();

            PagedList<Contact> models = new PagedList<Contact>(ListContact.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        public IActionResult Detail (int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var contact = _context.Contacts.Where(x => x.ContactId == id).FirstOrDefault();
            return View(contact);

        }
    }
}
