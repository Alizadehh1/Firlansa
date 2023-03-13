using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Firlansa.WebUI.AppCode.Modules.SliderModule;
using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Firlansa.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly FirlansaDbContext db;
        private readonly IMediator mediator;

        public SlidersController(FirlansaDbContext db,IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }
        [Authorize(Policy = "admin.sliders.index")]
        public async Task<IActionResult> Index(SliderAllQuery query)
        {
            var entity = await mediator.Send(query);
            return View(entity);
        }
        [Authorize(Policy = "admin.sliders.details")]
        public async Task<IActionResult> Details(SliderSingleQuery query)
        {
            var slider = await mediator.Send(query);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }
        [Authorize(Policy = "admin.sliders.create")]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.sliders.create")]
        public async Task<IActionResult> Create(SliderCreateCommand command)
        {
            var response = await mediator.Send(command);
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }
        [Authorize(Policy = "admin.sliders.edit")]
        public async Task<IActionResult> Edit(SliderSingleQuery query)
        {
            var slider = await mediator.Send(query);
            if (slider == null)
            {
                return NotFound();
            }
            var command = new SliderEditCommand();
            command.Id = slider.Id;
            command.ImagePath = slider.ImagePath;
            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.sliders.edit")]
        public async Task<IActionResult> Edit([FromRoute] int id, SliderEditCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            await mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Policy = "admin.sliders.delete")]
        public async Task<IActionResult> Delete(SliderRemoveCommand command)
        {
            var response = await mediator.Send(command);
            return Json(response);
        }
    }
}
