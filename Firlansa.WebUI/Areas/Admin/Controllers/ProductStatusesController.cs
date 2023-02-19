using Firlansa.WebUI.AppCode.Infrastructure;
using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities;
using Firlansa.WebUI.Models.Entities.Membership;
using Firlansa.WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductStatusesController : Controller
    {
        private readonly FirlansaDbContext db;
        private readonly IConfiguration configuration;
        private readonly UserManager<FirlansaUser> userManager;

        public ProductStatusesController(FirlansaDbContext db, IConfiguration configuration, UserManager<FirlansaUser> userManager)
        {
            this.db = db;
            this.configuration = configuration;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = new ProductStatusViewModel();
            viewModel.ProductStatusesDistinct = await db.ProductStatuses
                .Where(ps => ps.DeletedById == null)
                .Include(ps => ps.Product)
                .ThenInclude(ps => ps.Images.Where(i => i.IsMain))
                .Include(ps => ps.Size)
                .Include(ps => ps.Color)
                .Select(ps => new ProductStatus
                {
                    ProductId = ps.ProductId,
                    ColorId = ps.ColorId,
                    SizeId = ps.SizeId,
                    Product = ps.Product,
                    Color = ps.Color,
                    Size = ps.Size
                })
                .Distinct()
                .ToListAsync();
            viewModel.ProductStatuses = await db.ProductStatuses
                .Where(ps => ps.DeletedById == null)
                .Include(ps => ps.Product)
                .ThenInclude(ps => ps.Images.Where(i => i.IsMain))
                .Include(ps => ps.Size)
                .Include(ps => ps.Color)
                .ToListAsync();
            viewModel.Products = await db.Products
                .Where(p => p.DeletedById == null)
                .Include(p => p.Images.Where(i => i.IsMain))
                .ToListAsync();
            return View(viewModel);
        }
        [Authorize(Policy = "admin.productstatuses.notify")]
        public async Task<IActionResult> Notify([FromBody] int[] items)
        {
            var productStatuses = await db.ProductStatuses
                .Where(m => m.ProductId == items[0] && m.ColorId == items[1] && m.SizeId == items[2] && m.DeletedById == null)
                .Include(ps=>ps.FirlansaUser)
                .Include(ps=>ps.Color)
                .Include(ps=>ps.Size)
                .Include(ps=>ps.Product)
                .ToListAsync();
            if (productStatuses == null)
            {
                return Json(new CommandJsonResponse(true, $"Xəta baş verdi, zəhmət olmasa yenidən yoxlayın"));
            }
            var displayName = configuration["emailAccount:displayName"];
            var smtpServer = configuration["emailAccount:smtpServer"];
            var smtpPort = Convert.ToInt32(configuration["emailAccount:smtpPort"]);
            var userName = configuration["emailAccount:userName"];
            var password = configuration["emailAccount:password"];
            foreach (var productStatus in productStatuses)
            {
                SmtpClient client = new SmtpClient(smtpServer, smtpPort);
                client.Credentials = new NetworkCredential(userName, password);
                client.EnableSsl = true;
                var from = new MailAddress(userName, displayName);
                MailMessage message = new MailMessage(from, new MailAddress(productStatus.FirlansaUser.Email));
                message.Subject = "Firlansa";
                message.Body = $"Maraqlandığınız məhsul yenidən stokda. Belə ki, '{productStatus.Product.Name}' adlı '{productStatus.Color.Name}' rəngli '{productStatus.Size.Name}' ölçülü məhsul yenidən stoklarımızda. Almağa tələsin ";
                message.IsBodyHtml = false;
                client.Send(message);
                var user = await userManager.GetUserAsync(User);
                productStatus.DeletedById = user.Id;
                productStatus.DeletedDate = DateTime.UtcNow.AddHours(4);
            }
            await db.SaveChangesAsync();
            return Json(new CommandJsonResponse(false, $"İstifadəçilərə bildiriş çatdırıldı!"));
        }
    }
}
