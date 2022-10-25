using Firlansa.WebUI.Models;
using Firlansa.WebUI.Models.DataContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Controllers
{
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
    }
}
