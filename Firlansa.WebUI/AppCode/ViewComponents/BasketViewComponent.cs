using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.AppCode.ViewComponents
{
    public class BasketViewComponent : ViewComponent
    {
        private readonly FirlansaDbContext db;

        public BasketViewComponent(FirlansaDbContext db)
        {
            this.db = db;
        }
        public IViewComponentResult Invoke()
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
                    .Include(p => p.Specifications)
                    .ThenInclude(p => p.Color)
                    .Include(p => p.Specifications)
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
                double subTotal = 0;
                for (int i = 0; i < products.Count; i++)
                {
                int index = 0;
                    for (int j = 0; j < quantitiesFromCookie.Length; j++)
                    {
                        if (index > 0)
                        {
                            continue;
                        }
                        subTotal = subTotal + products[i].Price * quantitiesFromCookie[i];
                        index++;
                    }
                }
                ViewBag.SubTotal = subTotal;
                ViewBag.Products = products;
                ViewBag.Quantities = quantitiesFromCookie;
                ViewBag.Colors = colorsFromCookie;
                ViewBag.Sizes = sizesFromCookie;
                


                return View();
            }

            return View();
        }
        private bool CheckIsNumber(string value)
        {
            return int.TryParse(value, out int v);
        }
    }
}
