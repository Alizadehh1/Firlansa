using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly FirlansaDbContext db;
        //private readonly UserManager<FirlansaUser> userManager;

        public CategoriesController(FirlansaDbContext db/*, UserManager<FirlansaUser> userManager*/)
        {
            this.db = db;
            //this.userManager = userManager;
        }

        //[Authorize(Policy ="admin.categories.index")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
        {
            var query = await db.Categories
                .Where(c => c.DeletedById == null)
                .ToListAsync();
            return View(query);
        }

        //[Authorize(Policy = "admin.categories.details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await db.Categories
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedById == null);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //[Authorize(Policy = "admin.categories.create")]
        public IActionResult Create()
        {
            var data = db.Categories
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
        //[Authorize(Policy = "admin.categories.create")]
        public async Task<IActionResult> Create([Bind("Name,BrandId,ParentId,Id,CreatedById,CreatedDate,DeletedById,DeletedDate")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var data = db.Categories
                .Where(c => c.DeletedById == null)
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.ParentId == null ? c.Name : $"- {c.Name}"
                })
                .ToList();
            ViewData["ParentId"] = new SelectList(data, "Id", "Name", category.ParentId);
            return View(category);
        }
        //[Authorize(Policy = "admin.categories.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var data = db.Categories
                .Where(c => c.DeletedById == null && !c.Id.Equals(id))
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.ParentId == null ? c.Name : $"- {c.Name}"
                })
                .ToList();

            var category = await db.Categories
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedById == null);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(data, "Id", "Name", category.ParentId);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Policy = "admin.categories.edit")]
        public async Task<IActionResult> Edit(int id, [Bind("Name,BrandId,ParentId,Id,CreatedById,CreatedDate,DeletedById,DeletedDate")] Category category)
        {
            if (id != category.Id || category.DeletedById != null)
            {
                return NotFound();
            }

            var data = db.Categories
                .Where(c => c.DeletedById == null && !c.Id.Equals(id))
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.ParentId == null ? c.Name : $"- {c.Name}"
                })
                .ToList();

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(category);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            ViewData["ParentId"] = new SelectList(data, "Id", "Name", category.ParentId);
            return View(category);
        }

        [HttpPost]
        //[Authorize(Policy = "admin.categories.delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var entity = await db.Categories.Include(c=>c.Children.Where(c=>c.DeletedById==null)).FirstOrDefaultAsync(b => b.Id == id && b.DeletedById==null);
            if (entity == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Mövcud deyil"
                });
            }
            if (entity.ParentId == null && entity.Children.Count>0)
            {
                return Json(new
                {
                    error = true,
                    message = "Ana Kateqoriyanı Silmək Olmaz!"
                });
            }
            //var user = await userManager.GetUserAsync(User);
            //entity.DeletedById = user.Id;
            entity.DeletedById = 1;
            entity.DeletedDate = DateTime.UtcNow.AddHours(4);
            await db.SaveChangesAsync();
            return Json(new
            {
                error = false,
                message = "Uğurla silindi"
            });
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Any(e => e.Id == id);
        }
    }
}
