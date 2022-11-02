using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities;
using Firlansa.WebUI.Models.Entities.Membership;
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
    public class SizesController : Controller
    {
        private readonly FirlansaDbContext db;
        private readonly UserManager<FirlansaUser> userManager;

        public SizesController(FirlansaDbContext db, UserManager<FirlansaUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        [Authorize(Policy = "admin.sizes.index")]
        public async Task<IActionResult> Index()
        {
            return View(await db.Sizes.Where(s=>s.DeletedById==null).ToListAsync());
        }
        [Authorize(Policy = "admin.sizes.details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await db.Sizes
                .FirstOrDefaultAsync(m => m.DeletedById == null && m.Id == id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }
        [Authorize(Policy = "admin.sizes.create")]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.sizes.create")]
        public async Task<IActionResult> Create(Size size)
        {
            if (ModelState.IsValid)
            {
                db.Add(size);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(size);
        }
        [Authorize(Policy = "admin.sizes.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await db.Sizes.FirstOrDefaultAsync(s => s.DeletedById == null && s.Id == id);

            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.sizes.edit")]
        public async Task<IActionResult> Edit(int id, Size size)
        {
            if (id != size.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(size);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SizeExists(size.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(size);
        }

        [Authorize(Policy = "admin.sizes.delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var size = await db.Sizes
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedById==null);

            if (size == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Mövcud deyil"
                });
            }
            var user = await userManager.GetUserAsync(User);
            size.DeletedById = user.Id;
            size.DeletedDate = DateTime.UtcNow.AddHours(4);
            await db.SaveChangesAsync();
            return Json(new
            {
                error = false,
                message = "Uğurla silindi"
            });
        }

        private bool SizeExists(int id)
        {
            return db.Sizes.Any(e => e.Id == id);
        }
    }
}
