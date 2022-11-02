using Firlansa.WebUI.AppCode.Infrastructure;
using Firlansa.WebUI.AppCode.Modules.ProductModule;
using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities.Membership;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly FirlansaDbContext db;
        private readonly IMediator mediator;
        private readonly UserManager<FirlansaUser> userManager;

        public ProductsController(FirlansaDbContext db, IMediator mediator, UserManager<FirlansaUser> userManager)
        {
            this.db = db;
            this.mediator = mediator;
            this.userManager = userManager;
        }
        [Authorize(Policy = "admin.products.index")]
        public async Task<IActionResult> Index(ProductAllQuery query)
        {
            var data = await mediator.Send(query);

            return View(data);
        }
        [Authorize(Policy = "admin.products.details")]
        public async Task<IActionResult> Details(ProductSingleQuery query)
        {
            var entity = await mediator.Send(query);

            if (entity == null)
            {
                return NotFound();
            }


            return View(entity);
        }
        [Authorize(Policy = "admin.products.create")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(db.Categories.Where(i => i.DeletedById == null), "Id", "Name");
            ViewData["Colors"] = new SelectList(db.Colors.Where(i => i.DeletedById == null), "Id", "Name");
            ViewData["Sizes"] = new SelectList(db.Sizes.Where(i => i.DeletedById == null), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.products.create")]
        public async Task<IActionResult> Create(ProductCreateCommand command)
        {
            var product = await mediator.Send(command);

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(command);
        }
        [Authorize(Policy = "admin.products.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Products
                .Include(p=>p.Category)
                .Include(p => p.Images.Where(i => i.DeletedById == null))
                .Include(p => p.Specifications.Where(i => i.DeletedById == null))
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(db.Categories.Where(i => i.DeletedById == null), "Id", "Name", product.CategoryId);
            ViewData["Colors"] = new SelectList(db.Colors.Where(i => i.DeletedById == null), "Id", "Name");
            ViewData["Sizes"] = new SelectList(db.Sizes.Where(i => i.DeletedById == null), "Id", "Name");
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.products.edit")]
        public async Task<IActionResult> Edit(ProductEditCommand model)
        {

            var product = await mediator.Send(model);

            if (product == null)
            {
                return NotFound();
            }

            return Json(new CommandJsonResponse(false, $"Ugurlu emeliyyat, yeni mehsulun kodu:{product.Product.Id}"));
        }

        [Authorize(Policy = "admin.products.delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var product = await db.Products
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedById == null);
            if (product == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Mövcud deyil"
                });
            }
            var user = await userManager.GetUserAsync(User);
            product.DeletedById = user.Id;
            product.DeletedDate = DateTime.UtcNow.AddHours(4);
            await db.SaveChangesAsync();
            return Json(new
            {
                error = false,
                message = "Uğurla silindi"
            });
        }


        private bool ProductExists(int id)
        {
            return db.Products.Any(e => e.Id == id);
        }
    }
}
