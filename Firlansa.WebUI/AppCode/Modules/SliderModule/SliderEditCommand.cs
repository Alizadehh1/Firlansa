using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
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
    public class SliderEditCommand : IRequest<Slider>
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public IFormFile File { get; set; }

        public class SliderEditCommandHandler : IRequestHandler<SliderEditCommand, Slider>
        {
            readonly FirlansaDbContext db;
            readonly IWebHostEnvironment env;
            readonly IActionContextAccessor ctx;
            public SliderEditCommandHandler(FirlansaDbContext db, IWebHostEnvironment env, IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<Slider> Handle(SliderEditCommand request, CancellationToken cancellationToken)
            {
                if (ctx.ModelIsValid())
                {
                    if (request.File == null && string.IsNullOrEmpty(request.ImagePath))
                    {
                        ctx.AddModelError("ImagePath", "Image Cannot be empty");
                    }
                    var currentEntity = await db.Sliders
                    .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);
                    if (currentEntity == null)
                    {
                        return null;
                    }
                    string oldFileName = currentEntity.ImagePath;
                    if (request.File != null)
                    {
                        string fileExtension = Path.GetExtension(request.File.FileName);
                        string name = $"slider-{Guid.NewGuid()}{fileExtension}";
                        string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", name);
                        using (var fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                        {
                            request.File.CopyTo(fs);
                        }
                        currentEntity.ImagePath = name;
                        string physicalPathOld = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", oldFileName);
                        if (System.IO.File.Exists(physicalPathOld))
                        {
                            System.IO.File.Delete(physicalPathOld);
                        }
                    }
                    await db.SaveChangesAsync(cancellationToken);
                    return currentEntity;
                }
                return null;
            }
        }
    }
}
