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
    public class ContactsController : Controller
    {
        private readonly plantContext _context;

        public ContactsController(plantContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Contact data)
        {
            if (ModelState.IsValid)
            {
                var contact = new Contact()
                {
                    Name = data.Name,
                    Phone = data.Phone,
                    Email = data.Email,
                    Title = data.Title,
                    Description = data.Description,
                    CreateDay = DateTime.Today
                };
                _context.Contacts.Add(contact);
                _context.SaveChanges();
                return View();
            }
            return View(data);
        }
    }
}
