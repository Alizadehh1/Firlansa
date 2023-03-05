using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Firlansa.WebUI.AppCode.Extensions;
using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities;

namespace Firlansa.WebUI.AppCode.Modules.SliderModule
{
    public class SliderCreateCommand : IRequest<Slider>
    {
        public string ImagePath { get; set; }
        public IFormFile File { get; set; }
        public class SliderCreateCommandHandler : IRequestHandler<SliderCreateCommand, Slider>
        {
            private readonly FirlansaDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;

            public SliderCreateCommandHandler(FirlansaDbContext db, IWebHostEnvironment env, IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<Slider> Handle(SliderCreateCommand request, CancellationToken cancellationToken)
            {
                if (request?.File == null)
                {
                    ctx.AddModelError("ImagePath", "Image Cannot be empty");
                }

                if (ctx.ModelIsValid())
                {
                    string fileExtension = Path.GetExtension(request.File.FileName);
                    string name = $"slider-{Guid.NewGuid()}{fileExtension}";
                    string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", name);
                    using (var fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                    {
                        await request.File.CopyToAsync(fs, cancellationToken);
                    }
                    var slider = new Slider
                    {
                        ImagePath = name
                    };
                    db.Add(slider);
                    await db.SaveChangesAsync(cancellationToken);


                    return slider;
                }

                return null;
            }
        }
    }
}
