using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Firlansa.WebUI.Models.Entities.Membership;
using Microsoft.AspNetCore.Authorization;

namespace Firlansa.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorsController : Controller
    {
        private readonly FirlansaDbContext db;
        private readonly UserManager<FirlansaUser> userManager;

        public ColorsController(FirlansaDbContext db, UserManager<FirlansaUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        [Authorize(Policy = "admin.colors.index")]
        public async Task<IActionResult> Index()
        {
            return View(await db.Colors.Where(c=>c.DeletedById==null).ToListAsync());
        }
        [Authorize(Policy = "admin.colors.details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color = await db.Colors
                .FirstOrDefaultAsync(m => m.DeletedById == null && m.Id == id);
            if (color == null)
            {
                return NotFound();
            }

            return View(color);
        }
        [Authorize(Policy = "admin.colors.create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.colors.create")]
        public async Task<IActionResult> Create(Color color)
        {
            if (ModelState.IsValid)
            {
                db.Add(color);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(color);
        }
        [Authorize(Policy = "admin.colors.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color = await db.Colors.FirstOrDefaultAsync(c=>c.DeletedById==null && c.Id==id);
            if (color == null)
            {
                return NotFound();
            }
            return View(color);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.colors.edit")]
        public async Task<IActionResult> Edit(int id, Color color)
        {
            if (id != color.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(color);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColorExists(color.Id))
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
            return View(color);
        }
        [Authorize(Policy = "admin.colors.delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var color = await db.Colors
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedById==null);
            if (color == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Mövcud deyil"
                });
            }
            var user = await userManager.GetUserAsync(User);
            color.DeletedById = user.Id;
            color.DeletedDate = DateTime.UtcNow.AddHours(4);
            await db.SaveChangesAsync();
            return Json(new
            {
                error = false,
                message = "Uğurla silindi"
            });
        }

        private bool ColorExists(int id)
        {
            return db.Colors.Any(e => e.Id == id);
        }
    }
}
