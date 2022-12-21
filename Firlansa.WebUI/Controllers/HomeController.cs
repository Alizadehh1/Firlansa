using Firlansa.WebUI.Models;
using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities;
using Firlansa.WebUI.Models.ViewModels;
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

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel();
            model.LastestProducts = (from data in db.Products
                                     orderby data.CreatedDated descending
                                     select data).Take(3).Include(p => p.Images.Where(i => i.IsMain == true)).Include(p=>p.Category).ToList();
            model.SaleProducts = db.Products
                .Where(p => p.OldPrice != null)
                .Take(3)
                .Include(p => p.Images.Where(i => i.IsMain == true))
                .Include(p => p.Category)
                .ToList();
            return View(model);
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
