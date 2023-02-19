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
    public class AddressesController : Controller
    {
        private readonly FirlansaDbContext db;
        private readonly UserManager<FirlansaUser> userManager;

        public AddressesController(FirlansaDbContext db, UserManager<FirlansaUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        [Authorize(Policy = "admin.adress.index")]
        public async Task<IActionResult> Index()
        {
            return View(await db.Adresses.Where(c=>c.DeletedById==null).ToListAsync());
        }
        [Authorize(Policy = "admin.adress.details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await db.Adresses
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(m => m.DeletedById == null && m.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }
        [Authorize(Policy = "admin.adress.create")]
        public IActionResult Create()
        {
            var data = db.Adresses
                 .Where(c => c.DeletedById == null)
                 .Select(c => new
                 {
                     Id = c.Id,
                     Name = c.ParentId == null ? c.Name : $"- {c.Name}"
                 })
                 .ToList();
            ViewData["ParentId"] = new SelectList(data, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.adress.create")]
        public async Task<IActionResult> Create(Adress address)
        {
            if (ModelState.IsValid)
            {
                db.Add(address);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var data = db.Adresses
                 .Where(c => c.DeletedById == null)
                 .Select(c => new
                 {
                     Id = c.Id,
                     Name = c.ParentId == null ? c.Name : $"- {c.Name}"
                 })
                 .ToList();
            ViewData["ParentId"] = new SelectList(data, "Id", "Name");
            return View(address);
        }
        [Authorize(Policy = "admin.adress.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var data = db.Adresses
                 .Where(c => c.DeletedById == null)
                 .Select(c => new
                 {
                     Id = c.Id,
                     Name = c.ParentId == null ? c.Name : $"- {c.Name}"
                 })
                 .ToList();
            ViewData["ParentId"] = new SelectList(data, "Id", "Name");

            var address = await db.Adresses.FirstOrDefaultAsync(c=>c.DeletedById==null && c.Id==id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.adress.edit")]
        public async Task<IActionResult> Edit(int id, Adress address)
        {
            if (id != address.Id)
            {
                return NotFound();
            }

            var data = db.Adresses
                 .Where(c => c.DeletedById == null)
                 .Select(c => new
                 {
                     Id = c.Id,
                     Name = c.ParentId == null ? c.Name : $"- {c.Name}"
                 })
                 .ToList();
            ViewData["ParentId"] = new SelectList(data, "Id", "Name");

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(address);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.Id))
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
            return View(address);
        }
        [Authorize(Policy = "admin.adress.delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var address = await db.Adresses.Include(c => c.Children.Where(c => c.DeletedById == null)).FirstOrDefaultAsync(b => b.Id == id && b.DeletedById == null);
            if (address == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Mövcud deyil"
                });
            }
            if (address.ParentId == null && address.Children.Count > 0)
            {
                return Json(new
                {
                    error = true,
                    message = "Ana Kateqoriyanı Silmək Olmaz!"
                });
            }
            var user = await userManager.GetUserAsync(User);
            address.DeletedById = user.Id;
            address.DeletedDate = DateTime.UtcNow.AddHours(4);
            await db.SaveChangesAsync();
            return Json(new
            {
                error = false,
                message = "Uğurla silindi"
            });
        }

        private bool AddressExists(int id)
        {
            return db.Adresses.Any(e => e.Id == id);
        }
    }
}
