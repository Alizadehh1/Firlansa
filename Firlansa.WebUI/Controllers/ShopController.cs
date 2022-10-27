using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private readonly FirlansaDbContext db;

        public ShopController(FirlansaDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var model = new ShopViewModel();
            model.Categories = db.Categories
                .Where(c => c.DeletedById == null)
                .Include(c => c.Children.Where(c => c.DeletedById == null))
                .ToList();
                
            return View(model);
        }
    }
}
