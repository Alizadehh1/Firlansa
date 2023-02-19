using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities.Membership;
using Firlansa.WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly FirlansaDbContext db;
        private readonly UserManager<FirlansaUser> userManager;

        public UsersController(FirlansaDbContext db, UserManager<FirlansaUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        [Authorize("admin.users.index")]
        public async Task<IActionResult> Index()
        {
            var query = await db.Users.ToListAsync();
           
            return View(query);
        }
        [Authorize("admin.users.details")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.Roles = await (from r in db.Roles
                                   join ur in db.UserRoles on new
                                   {
                                       RoleId = r.Id,
                                       UserId = user.Id
                                   } equals new
                                   {
                                       ur.RoleId,
                                       ur.UserId
                                   } into lJoin
                                   from lj in lJoin.DefaultIfEmpty()
                                   select Tuple.Create(r.Id, r.Name, lj != null)).ToListAsync();

            ViewBag.Principals = (from p in Program.principals
                                  join uc in db.UserClaims on new { ClaimValue = "1", ClaimType = p, UserId = user.Id } equals new { uc.ClaimValue, uc.ClaimType, uc.UserId } into lJoin
                                  from lj in lJoin.DefaultIfEmpty()
                                  select Tuple.Create(p, lj != null)).ToList();
            return View(user);
        }

        [HttpPost]
        [Authorize(Policy = "admin.users.delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Movcud deyil"
                });
            }
            IdentityResult result = await userManager.DeleteAsync(user);
            db.SaveChanges();
            return Json(new
            {
                error = false,
                message = "Ugurla silindi"
            });
        }

        [HttpPost]
        [Route("/user-set-role")]
        [Authorize("admin.users.setrole")]
        public async Task<IActionResult> SetRole(int userId, int roleId, bool selected)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            

            if (user == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Xətalı Sorğu"
                });
            }
            var role = await db.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            if (role == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Xətalı Sorğu"
                });
            }

            if (selected)
            {
                if (await db.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId))
                {
                    return Json(new
                    {
                        error = true,
                        message = $"'{user.Email}' adlı istifadəçi '{role.Name}' adlı roldadır!"
                    });
                }
                else
                {
                    await db.UserRoles.AddAsync(new FirlansaUserRole
                    {
                        UserId = userId,
                        RoleId = roleId
                    });

                    await db.SaveChangesAsync();

                    return Json(new
                    {
                        error = false,
                        message = $"'{user.Email}' adlı istifadəçi '{role.Name}' rola əlavə edildi!"
                    });
                }
            }
            else
            {
                var userRole = await db.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

                if (userRole==null)
                {
                    return Json(new
                    {
                        error = true,
                        message = $"'{user.Email}' adlı istifadəçi '{role.Name}' adlı rolda deyil!"
                    });
                }
                else
                {
                    db.UserRoles.Remove(userRole);

                    await db.SaveChangesAsync();

                    return Json(new
                    {
                        error = false,
                        message = $"'{user.Email}' adlı istifadəçi '{role.Name}' adlı roldan çıxarıldı!"
                    });
                }
            }
        }

        [HttpPost]
        [Route("/user-set-principal")]
        [Authorize("admin.users.setprincipal")]
        public async Task<IActionResult> SetPrincipal(int userId, string principalName, bool selected)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var hasPrincipal = Program.principals.Contains(principalName);

            if (user == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Xətalı Sorğu"
                });
            }

            if (!hasPrincipal)
            {
                return Json(new
                {
                    error = true,
                    message = "Xətalı Sorğu"
                });
            }


            if (selected)
            {
                if (await db.UserClaims.AnyAsync(ur => ur.UserId == userId && ur.ClaimType.Equals(principalName) && ur.ClaimValue=="1"))
                {
                    return Json(new
                    {
                        error = true,
                        message = $"'{user.Email}' adlı istifadəçi '{principalName}' adlı səlahiyyətə sahibdir!"
                    });
                }
                else
                {
                    await db.UserClaims.AddAsync(new FirlansaUserClaim
                    {
                        UserId = userId,
                        ClaimType = principalName,
                        ClaimValue = "1"
                    });

                    await db.SaveChangesAsync();

                    return Json(new
                    {
                        error = false,
                        message = $"'{user.Email}' adlı istifadəçiyə '{principalName}' adlı səlahiyyət əlavə edildi!"
                    });
                }
            }
            else
            {
                var userClaim = await db.UserClaims.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.ClaimType.Equals(principalName) && ur.ClaimValue == "1");

                if (userClaim == null)
                {
                    return Json(new
                    {
                        error = true,
                        message = $"'{user.Email}' adlı istifadəçi '{principalName}' adlı səlahiyyətə sahib deyil!"
                    });
                }
                else
                {
                    db.UserClaims.Remove(userClaim);

                    await db.SaveChangesAsync();

                    return Json(new
                    {
                        error = false,
                        message = $"'{user.Email}' adlı istifadəçi '{principalName}' adlı səlahiyyətdən azad edildi!"
                    });
                }
            }
        }
    }
}
