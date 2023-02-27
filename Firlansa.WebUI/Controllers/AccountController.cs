using Firlansa.WebUI.AppCode.Extensions;
using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities.Membership;
using Firlansa.WebUI.Models.FormModels;
using Firlansa.WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly FirlansaDbContext db;
        private readonly SignInManager<FirlansaUser> signInManager;
        private readonly UserManager<FirlansaUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IActionContextAccessor ctx;

        public AccountController(FirlansaDbContext db, SignInManager<FirlansaUser> signInManager, UserManager<FirlansaUser> userManager,
            IConfiguration configuration,
            IActionContextAccessor ctx)
        {
            this.db = db;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;
            this.ctx = ctx;
        }
        [Route("/signin.html")]
        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }

        [Route("/accessdenied.html")]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [Route("/logout.html")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        public async Task<IActionResult> Profile()
        {
            var viewModel = new ProfileViewModel();
            viewModel.Order = db.Orders.Where(o => o.FirlansaUserId.ToString() == User.GetUserId() && o.OrderStatus == "APPROVED" && o.DeletedById == null).OrderByDescending(o => o.CreatedDated).Include(o => o.OrderItems).ToList();
            viewModel.FirlansaUser = await userManager.FindByIdAsync(User.GetUserId());
            viewModel.ProductStatuses = await db.ProductStatuses
                .Where(ps => ps.DeletedById != null && ps.FirlansaUserId == Convert.ToInt32(User.GetUserId()))
                .Include(ps => ps.Product)
                .Include(ps => ps.Color)
                .Include(ps => ps.Size)
                .ToListAsync();
            return View(viewModel);
        }
        public async Task<IActionResult> Order(int id)
        {
            var viewModel = new OrderViewModel();
            viewModel.Order = db.Orders
                .Include(o => o.Adress)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.Id == id);
            viewModel.Orders = db.Orders
                .Where(o => o.Id == id)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToList()
                .OrderByDescending(o => o.CreatedDated);
            viewModel.Products = db.Products
                .Where(p => p.DeletedById == null)
                .Include(p => p.Category)
                .Include(p => p.Images.Where(i => i.IsMain && i.DeletedById == null))
                .Include(p => p.Specifications.Where(s => s.DeletedById == null))
                .ThenInclude(p => p.Color)
                .Include(p => p.Specifications.Where(s => s.DeletedById == null))
                .ThenInclude(p => p.Size)
                .ToList();
            viewModel.Users = db.Users.ToList();
            double totalAmount = 0;
            foreach (var orderItem in db.OrderItems)
            {
                if (orderItem.OrderId == id)
                {
                    totalAmount += orderItem.Product.Price * orderItem.Quantity;
                }
            }
            ViewBag.TotalAmount = totalAmount;
            return View(viewModel);
        }
        [HttpPost]
        [Route("/signin.html")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(LoginFormModel user)
        {
            if (ModelState.IsValid)
            {
                FirlansaUser foundedUser = null;

                foundedUser = await userManager.FindByEmailAsync(user.Email);

                if (foundedUser == null)
                {
                    ViewBag.Message = "E-Poçtunuz və ya şifrəniz yanlışdır!";
                    goto end;
                }
                var signInResult = await signInManager.PasswordSignInAsync(foundedUser, user.Password, true, true);

                if (!signInResult.Succeeded)
                {
                    ViewBag.Message = "E-Poçtunuz və ya şifrəniz yanlışdır!";
                    goto end;
                }

                var callBackUrl = Request.Query["ReturnUrl"];

                if (!string.IsNullOrWhiteSpace(callBackUrl))
                {
                    return Redirect(callBackUrl);
                }
                return RedirectToAction("Index", "Home");
            }

        end:
            return View(user);
        }
        [AllowAnonymous]
        [Route("/register.html")]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [Route("/register.html")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new FirlansaUser();
                user.Email = model.Email;
                if (db.Users.Any(u => u.Email == model.Email))
                {
                    ViewBag.Message = "Bu email ilə artıq qeydiyyatdan keçmisiniz";
                    goto end;
                }
                user.UserName = model.Email;
                user.PhoneNumberConfirmed = true;
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    string path = $"{ctx.GetAppLink()}/registration-confirm.html?email={user.Email}&token={token}";
                    var emailResponse = configuration.SendEmail(user.Email, "Firlansa istifadəçi qeydiyyatı", $"Zəhmət olmasa <a href=\"{path}\">link</a> vasitəsilə qeydiyyatı tamamlayasınız");
                    if (emailResponse)
                    {
                        ViewBag.Message = "Təbriklər qeydiyyat tamamlandı, zəhmət olmasa elektron poçtunuza gələn mail'i təsdiqləyəsiniz. ";
                    }
                    else
                    {
                        ViewBag.Message = "E-mailə göndərərkən səhv aşkar olundu, yenidən cəhd edin";
                        goto end;
                    }

                    return View();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
        end:
            return View(model);
        }
        [HttpGet]
        [Route("/registration-confirm.html")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterConfirm(string email, string token)
        {
            var foundedUser = await userManager.FindByEmailAsync(email);
            if (foundedUser == null)
            {
                ViewBag.Message = "Xətalı Token göndərilib";
                goto end;
            }
            token = token.Replace(" ", "+");
            var result = await userManager.ConfirmEmailAsync(foundedUser, token);
            if (!result.Succeeded)
            {
                ViewBag.Message = "Xətalı Token göndərilib";
                goto end;
            }
            ViewBag.Message = "Hesabınız təsdiqləndi!";
        end:
            return RedirectToAction(nameof(SignIn));
        }
    }
}
