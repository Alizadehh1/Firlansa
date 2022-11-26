using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities;
using Firlansa.WebUI.Models.FormModels;
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
        public async Task<IActionResult> Index()
        {
            var model = new ShopViewModel();
            model.Categories = await db.Categories
                .Where(c => c.DeletedById == null)
                .Include(c => c.Children.Where(c => c.DeletedById == null))
                .ToListAsync();
            model.Colors = await db.Colors
                .Where(c => c.DeletedById == null)
                .ToListAsync();
            model.Sizes = await db.Sizes
                .Where(s => s.DeletedById == null)
                .ToListAsync();
            model.Products = await db.Products
                .Include(p => p.Images.Where(i => i.DeletedById == null && i.IsMain == true))
                .Include(p=>p.Category)
                .Where(p => p.DeletedById == null)
                .ToListAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Filter(ShopFilterFormModel model)
        {
            var query = db.Products
                .Include(p => p.Images.Where(i => i.DeletedById == null && i.IsMain == true))
                .Include(p => p.Category)
                .Include(p=>p.Specifications.Where(s=>s.DeletedById==null))
                .Where(p => p.DeletedById == null)
                .AsQueryable();

            if (model?.Colors?.Count() > 0)
            {
                query = query.Where(p => p.Specifications.Any(ps => model.Colors.Contains(ps.ColorId)));
            }
            if (model?.Sizes?.Count() > 0)
            {
                query = query.Where(p => p.Specifications.Any(ps => model.Sizes.Contains(ps.SizeId)));
            }
            if (model?.Categories?.Count() > 0)
            {
                //query = query.Where(p => p.Specifications.Any(ps => model.Sizes.Contains(ps.SizeId)));
                query = query.Where(p => model.Categories.Contains(p.CategoryId));
            }

            return PartialView("_ProductContainer", await query.ToListAsync());

            //return Json(new
            //{
            //    error = false,
            //    data = await query.ToListAsync()
            //});
        }
        public async Task<IActionResult> Details(int id)
        {
            var model = new ShopViewModel();
            model.Product = await db.Products
                .Include(p => p.Images.Where(i => i.DeletedById == null))
                .Include(p => p.Category)
                .Include(p => p.Specifications.Where(s => s.DeletedById == null))
                .ThenInclude(s => s.Color)
                .Include(p => p.Specifications.Where(s => s.DeletedById == null))
                .ThenInclude(s => s.Size)
                .FirstOrDefaultAsync(p => p.DeletedById == null && p.Id == id);
            model.Colors = db.ProductSpecifications
                .Where(ps => ps.DeletedById == null && ps.ProductId == id)
                .Select(ps => ps.Color)
                .Distinct()
                .ToList();
            model.Sizes = db.ProductSpecifications
                .Where(ps => ps.DeletedById == null && ps.ProductId == id)
                .Select(ps => ps.Size)
                .Distinct()
                .ToList();
            model.ProductSpecifications = db.ProductSpecifications
                .Where(ps => ps.DeletedById == null && ps.ProductId == id)
                .Include(ps=>ps.Size)
                .Distinct()
                .ToList();
            model.Products = await db.Products
                .Include(p => p.Images.Where(i => i.DeletedById == null && i.IsMain == true))
                .Where(p => p.DeletedById == null)
                .ToListAsync();
            if (model.Product == null || id == null)
            {
                return NotFound();
            }
            return View(model);
        }
        public async Task<IActionResult> Quickview(int id)
        {
            var model = new ShopViewModel();
            model.Product = await db.Products
                .Include(p => p.Images.Where(i => i.DeletedById == null))
                .Include(p => p.Category)
                .Include(p => p.Specifications.Where(s => s.DeletedById == null))
                .ThenInclude(s => s.Color)
                .Include(p => p.Specifications.Where(s => s.DeletedById == null))
                .ThenInclude(s => s.Size)
                .FirstOrDefaultAsync(p => p.DeletedById == null && p.Id == id);
            model.Colors = db.ProductSpecifications
                .Where(ps => ps.DeletedById == null && ps.ProductId == id)
                .Select(ps => ps.Color)
                .Distinct()
                .ToList();
            model.Sizes = db.ProductSpecifications
                .Where(ps => ps.DeletedById == null && ps.ProductId == id)
                .Select(ps => ps.Size)
                .Distinct()
                .ToList();
            model.ProductSpecifications = db.ProductSpecifications
                .Where(ps => ps.DeletedById == null && ps.ProductId == id)
                .Include(ps => ps.Size)
                .Distinct()
                .ToList();
            model.Products = await db.Products
                .Include(p => p.Images.Where(i => i.DeletedById == null && i.IsMain == true))
                .Where(p => p.DeletedById == null)
                .ToListAsync();
            if (model.Product == null || id == null)
            {
                return NotFound();
            }
            return View(model);
        }
        public async Task<IActionResult> Basket()
        {
            if (Request.Cookies.TryGetValue("cards", out string cards) && Request.Cookies.TryGetValue("colors", out string colors) && Request.Cookies.TryGetValue("sizes", out string sizes) && Request.Cookies.TryGetValue("qtys", out string qtys))
            {
                int[] productIdsFromCookie = cards.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();

                int[] colorsFromCookie = colors.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();

                int[] sizesFromCookie = sizes.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();

                int[] quantitiesFromCookie = qtys.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();



                var allProducts = db.Products
                    .Include(p => p.Images.Where(i => i.IsMain == true && i.DeletedById == null))
                    .Include(p => p.Specifications.Where(s => s.DeletedById == null))
                    .ThenInclude(p => p.Color)
                    .Include(p => p.Specifications.Where(s => s.DeletedById == null))
                    .ThenInclude(p => p.Size)
                    .Where(p => p.DeletedById == null)
                    .ToList();


                List<Product> products = new List<Product>();

                for (int j = 0; j < productIdsFromCookie.Length; j++)
                {
                    for (int i = 0; i < allProducts.Count; i++)
                    {
                        if (productIdsFromCookie[j] == allProducts[i].Id)
                        {
                            products.Add(allProducts[i]);
                        }

                    }
                }

                ViewBag.Colors = colorsFromCookie;
                ViewBag.ProductIds = productIdsFromCookie;


                var productss = Tuple.Create(products, colorsFromCookie, sizesFromCookie, quantitiesFromCookie);

                return View(productss);
            }
            return View(new Tuple<List<Product>, int[], int[], int[]>(null, null, null, null));

        }

        private bool CheckIsNumber(string value)
        {
            return int.TryParse(value, out int v);
        }
    }
}
