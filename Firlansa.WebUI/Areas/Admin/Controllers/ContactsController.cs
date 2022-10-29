using Firlansa.WebUI.AppCode.Modules.ContactModule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactsController : Controller
    {
        readonly IMediator mediator;
        public ContactsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index(ContactAllQuery query)
        {
            var data = await mediator.Send(query);
            return View(data);
        }
        public async Task<IActionResult> Details(ContactSingleQuery query)
        {
            var data = await mediator.Send(query);
            return View(data);
        }
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
        public async Task<IActionResult> Answer(ContactAnswerCommand command)
        {
            var data = await mediator.Send(command);
            if (!ModelState.IsValid)
            {
                return View(data);
            }
            return RedirectToAction(nameof(Details), routeValues: new
            {
                id = command.Id
            });
        }
    }
}
