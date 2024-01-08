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
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Plant.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]

    public class AdminContactsController : Controller
    {
        private readonly plantContext _context;
        public INotyfService _notyfService { get; }
        public AdminContactsController(plantContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
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
