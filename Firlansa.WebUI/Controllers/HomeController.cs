using Firlansa.WebUI.Models;
using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly FirlansaDbContext db;

        public HomeController(FirlansaDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contact(Contact model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    error = true,
                    message = ModelState.SelectMany(ms => ms.Value.Errors).First().ErrorMessage
                });
            }
            await db.Contacts.AddAsync(model);
            await db.SaveChangesAsync();
            return Json(new
            {
                error = false,
                message = "Müraciyyətiniz qeydə alındı!"
            });
        }
        public async Task<IActionResult> Faq()
        {
            var faqs = await db.Faqs.Where(f => f.DeletedById == null).ToListAsync();
            return View(faqs);
        }
    }
}
