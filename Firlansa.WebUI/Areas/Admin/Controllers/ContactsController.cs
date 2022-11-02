using Firlansa.WebUI.AppCode.Modules.ContactModule;
using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities.Membership;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactsController : Controller
    {
        readonly IMediator mediator;
        private readonly UserManager<FirlansaUser> userManager;
        private readonly FirlansaDbContext db;

        public ContactsController(IMediator mediator, UserManager<FirlansaUser> userManager,FirlansaDbContext db)
        {
            this.mediator = mediator;
            this.userManager = userManager;
            this.db = db;
        }
        [Authorize(Policy = "admin.contacts.index")]
        public async Task<IActionResult> Index(ContactAllQuery query)
        {
            var data = await mediator.Send(query);
            return View(data);
        }
        [Authorize(Policy = "admin.contacts.details")]
        public async Task<IActionResult> Details(ContactSingleQuery query)
        {
            var data = await mediator.Send(query);
            return View(data);
        }
        [Authorize(Policy = "admin.contacts.answer")]
        public async Task<IActionResult> Answer(ContactSingleQuery query)
        {
            var data = await mediator.Send(query);
            if (data.AnsweredById!=null)
            {
                return RedirectToAction(nameof(Details),routeValues:new
                {
                    id = query.Id
                });
            }
            return View(data);
        }
        [HttpPost]
        [Authorize(Policy = "admin.contacts.answer")]
        public async Task<IActionResult> Answer(ContactAnswerCommand command)
        {
            var data = await mediator.Send(command);
            if (!ModelState.IsValid)
            {
                return View(data);
            }
            var user = await userManager.GetUserAsync(User);
            data.AnsweredById = user.Id;
            data.AnswerDate = DateTime.UtcNow.AddHours(4);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), routeValues: new
            {
                id = command.Id
            });
        }
    }
}
