using Firlansa.WebUI.AppCode.Extensions;
using Firlansa.WebUI.AppCode.Infrastructure;
using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities;
using Firlansa.WebUI.Models.FormModels;
using Firlansa.WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Recources;
namespace Firlansa.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private readonly FirlansaDbContext db;

        public ShopController(FirlansaDbContext db)
        {
            this.db = db;
        }
        [AllowAnonymous]
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
                .Include(p => p.Category)
                .Where(p => p.DeletedById == null)
                .ToListAsync();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Filter(ShopFilterFormModel model)
        {
            var query = db.Products
                .Include(p => p.Images.Where(i => i.DeletedById == null && i.IsMain == true))
                .Include(p => p.Category)
                .Include(p => p.Specifications.Where(s => s.DeletedById == null))
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
        [AllowAnonymous]
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
                .Include(ps => ps.Size)
                .Include(ps=>ps.Color)
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> Wishlist()
        {
            if (Request.Cookies.TryGetValue("cardsForWishlist", out string cardsForCookie))
            {
                int[] idsFromCookie = cardsForCookie.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();

                var products = from p in db.Products.Where(p => p.DeletedById == null)
                               where idsFromCookie.Contains(p.Id) && p.DeletedById == null
                               select p;

                return View(products.Include(p => p.Category).Include(i => i.Images.Where(i => i.IsMain && i.DeletedById == null)).ToList());
            }

            return View(new List<Product>());
        }
        [AllowAnonymous]
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

                var data = await db.Adresses
                 .Where(c => c.DeletedById == null)
                 .Select(c => new SelectListItem
                 {
                     Value = c.Id.ToString(),
                     Text = c.ParentId == null ? c.Name : $"- {c.Name}",
                     Disabled = c.ParentId == null
                 })
                 .ToArrayAsync();
                var districts = await db.Adresses
                    .Where(a => a.DeletedById == null)
                    .ToListAsync();


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
                ViewBag.SecretKey = "894D14D6CBE44397AC28DFF6C5BE2A43";
                ViewBag.Merchant = "ES1091685";

                var productss = Tuple.Create(products, colorsFromCookie, sizesFromCookie, quantitiesFromCookie, data, districts);

                return View(productss);
            }
            return View(new Tuple<List<Product>, int[], int[], int[], SelectListItem[], Order>(null, null, null, null, null, null));

        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Notification([FromBody] int[] items)
        {

            var product = await db.Products
                .FirstOrDefaultAsync(m => m.Id == items[0] && m.DeletedById == null);
            var currentProductStatus = await db.ProductStatuses
                .FirstOrDefaultAsync(ps => ps.FirlansaUserId == items[3] && ps.ColorId == items[1] && ps.SizeId == items[2] && ps.ProductId == items[0] && ps.DeletedById == null);
            if (product == null)
                return Json(new CommandJsonResponse(true, $"Xəta baş verdi, belə bir məhsul tapılmadı, zəhmət olmasa yenidən yoxlayın!"));
            else if (currentProductStatus != null)
                return Json(new CommandJsonResponse(true, $"Bu məhsulu artıq bildirişlərə əlavə etmisiniz!"));
            var productStatus = new ProductStatus();
            var num = db.Products.Where(p => p.Id > 2);
            productStatus.ProductId = items[0];
            productStatus.ColorId = items[1];
            productStatus.SizeId = items[2];
            productStatus.FirlansaUserId = Convert.ToInt32(User.GetUserId());
            await db.ProductStatuses.AddAsync(productStatus);
            await db.SaveChangesAsync();
            return Json(new CommandJsonResponse(false, $"Siz '{product.Name}' adlı məhsul yenidən stokda olduqda xəbərdar olacaqsız!"));
        }
        [Authorize]
        public async Task<IActionResult> CompleteOrder(int id)
        {
            var entity = await db.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Adress)
                .Include(o => o.FirlansaUser)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            var client = new RestClient("https://api.payriff.com/api/v2/getStatusOrder");
            var request = new RestRequest();
            request.Timeout = -1;
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "C7DF29708CC0410192D48AAC06B3A1C0");
            var body = @"{" + "\n" +
            @"    ""body"": {" + "\n" +
            @"        ""language"": ""Az""," + "\n" +
            @$"        ""orderId"": ""{entity.OrderId}""," + "\n" +
            @$"        ""sessionId"": ""{entity.SessionId}""" + "\n" +
            @"    }," + "\n" +
            @"    ""merchant"": ""ES1091628""" + "\n" +
            @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = client.Post(request);
            var responseJson = JsonConvert.DeserializeObject<dynamic>(response.Content);
            var statusOrder = responseJson.payload.orderStatus;
            if (!response.IsSuccessful)
                return NotFound();

            if (statusOrder == "APPROVED")
            {
                foreach (var item in entity.OrderItems)
                {
                    db.ProductSpecifications.FirstOrDefault(ps => ps.ProductId == item.ProductId && ps.SizeId == item.SizeId && ps.ColorId == item.ColorId).Quantity -= item.Quantity;
                }
            }
            else
            {
                return NotFound();
            }

            entity.OrderStatus = statusOrder;
            await db.SaveChangesAsync();
            return View(statusOrder);
        }
        [Authorize]
        public async Task<IActionResult> PlaceOrder(string productIds, string quantities, string colorIds, string sizeIds, string location, string phoneNumber, int addressId, string postCode)
        {
            if (location == null || phoneNumber == null || addressId == 0 || postCode == null)
            {
                return Json(new CommandJsonResponse(true, "Zəhmət olmasa çatdırılma məlumatlarını düzgün daxil edin"));
            }
          

            int[] productId = productIds.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();
            int[] quantity = quantities.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();
            int[] colorId = colorIds.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();
            int[] sizeId = sizeIds.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();


            var newOrder = new Order();
            newOrder.FirlansaUserId = Convert.ToInt32(User.GetUserId());
            newOrder.Location = location;
            newOrder.PhoneNumber = phoneNumber;
            newOrder.AdressId = addressId;
            newOrder.OrderStatus = "CREATED";
            newOrder.PostCode = postCode;
            double? totalPrice = 0;
            if (productId != null)
            {
                newOrder.OrderItems = new List<OrderItem>();
                int i = 0;
                foreach (var prid in productId)
                {
                    if (quantity[i]<=0)
                    {
                        return Json(new CommandJsonResponse(true, $"Məhsulun sayını düzgün daxil edin!"));
                    }
                    if (db.ProductSpecifications.FirstOrDefault(ps => ps.ColorId == colorId[i] && ps.SizeId == sizeId[i] && ps.ProductId == prid).Quantity < quantity[i])
                    {
                        return Json(new CommandJsonResponse(true, $"Almaq istədiyiniz məhsul təəssüf ki stoklarımızda {db.ProductSpecifications.FirstOrDefault(ps => ps.ColorId == colorId[i] && ps.SizeId == sizeId[i] && ps.ProductId == prid).Quantity} ədəd qalıb"));
                    }
                    totalPrice += db.Products.FirstOrDefault(p => p.Id == prid).Price * quantity[i];
                    newOrder.OrderItems.Add(new OrderItem
                    {
                        ProductId = prid,
                        ColorId = colorId[i],
                        SizeId = sizeId[i],
                        OrderId = newOrder.Id,
                        Quantity = quantity[i]
                    });
                    i++;
                }
            }

            totalPrice += db.Adresses.FirstOrDefault(a => a.Id == addressId).ShippingPrice;
            await db.Orders.AddAsync(newOrder);
            await db.SaveChangesAsync();

            string routeUrl = $"{Url.Action("CompleteOrder")}/{newOrder.Id}";
            string approveUrl = $"https://localhost:44379/{routeUrl}";


            var client = new RestClient("https://api.payriff.com/api/v2/createOrder");
            var request = new RestRequest();
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "C7DF29708CC0410192D48AAC06B3A1C0");
            var body = @"{" + "\n" +
            @"    ""body"": {" + "\n" +
            @$"        ""amount"": {totalPrice}," + "\n" +
            @"        ""currencyType"": ""AZN""," + "\n" +
            @"        ""description"": ""Example""," + "\n" +
            @"        ""language"": ""AZ""," + "\n" +
            @$"        ""approveURL"": ""{approveUrl}""," + "\n" +
            @"        ""cancelURL"": ""https://localhost:44379/shop/basket""," + "\n" +
            @"        ""declineURL"": ""https://decline,com""" + "\n" +
            @"    }," + "\n" +
            @"    ""merchant"": ""ES1091628""" + "\n" +
            @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = client.Post(request);
            var responseJson = JsonConvert.DeserializeObject<dynamic>(response.Content);
            var paymentUrl = responseJson.payload.paymentUrl;
            newOrder.OrderId = responseJson.payload.orderId;
            newOrder.SessionId = responseJson.payload.sessionId;
            await db.SaveChangesAsync();
            return Json(new CommandJsonResponse(false, $"{paymentUrl}"));

        }
        [AllowAnonymous]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index", "Shop");
            }
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
                .Include(p => p.Category)
                .Where(p => p.DeletedById == null && p.Name.ToLower().Contains(query.ToLower())
                || p.Description.ToLower().Contains(query.ToLower())
                || p.ShortDescription.ToLower().Contains(query.ToLower())
                || p.Price.ToString().ToLower().Contains(query.ToLower())
                || p.Category.Name.ToLower().Contains(query.ToLower()))
                .ToListAsync();
            return View(model);
        }

        private bool CheckIsNumber(string value)
        {
            return int.TryParse(value, out int v);
        }
        private bool CheckIsDouble(string value)
        {
            return double.TryParse(value, out double v);
        }
    }
}
