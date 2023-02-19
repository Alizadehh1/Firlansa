using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities;
using Firlansa.WebUI.Models.Entities.Membership;
using Firlansa.WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly FirlansaDbContext db;
        private readonly UserManager<FirlansaUser> userManager;

        public OrdersController(FirlansaDbContext db, UserManager<FirlansaUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        [Authorize(Policy = "admin.orders.index")]
        public IActionResult Index()
        {
            var viewModel = new OrderViewModel();
            viewModel.Orders = db.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Adress)
                .ToList()
                .OrderByDescending(o => o.CreatedDated);
            viewModel.Users = db.Users.ToList();
            return View(viewModel);
        }
        [Authorize(Policy = "admin.orders.details")]
        public IActionResult Details(int id)
        {
            var viewModel = new OrderViewModel();
            viewModel.Order = db.Orders
                .Include(o=>o.Adress)
                .Include(o => o.OrderItems)
                .ThenInclude(oi=>oi.Product)
                .FirstOrDefault(o => o.Id == id);
            viewModel.Orders = db.Orders
                .Where(o => o.Id == id)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToList()
                .OrderByDescending(o => o.CreatedDated);
            viewModel.Products = db.Products
                .Where(p => p.DeletedById == null)
                .Include(p=>p.Specifications)
                .ThenInclude(s=>s.Size)
                .Include(p => p.Specifications)
                .ThenInclude(s=>s.Color)
                .Include(p=>p.Images.Where(i=>i.IsMain==true && i.DeletedById==null))
                .Include(p=>p.Category)
                .ToList();
            viewModel.Users = db.Users.ToList();
            double totalAmount = 0;
            foreach (var orderItem in db.OrderItems)
            {
                if (orderItem.OrderId==id)
                {
                    totalAmount += orderItem.Product.Price * orderItem.Quantity;
                }
            }
            ViewBag.TotalAmount = totalAmount;
            return View(viewModel);
        }
    }
}
