using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Firlansa.WebUI.AppCode.Infrastructure;
using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities.Membership;

namespace Firlansa.WebUI.AppCode.Modules.SliderModule
{
    public class SliderRemoveCommand : IRequest<CommandJsonResponse>
    {
        public int Id { get; set; }
        public class BlogRemoveCommandHandler : IRequestHandler<SliderRemoveCommand, CommandJsonResponse>
        {
            readonly FirlansaDbContext db;
            readonly UserManager<FirlansaUser> userManager;

            public BlogRemoveCommandHandler(FirlansaDbContext db, UserManager<FirlansaUser> userManager)
            {
                this.db = db;
                this.userManager = userManager;
            }
            public async Task<CommandJsonResponse> Handle(SliderRemoveCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.Sliders
                    .FirstOrDefaultAsync(b => b.DeletedById == null && b.Id == request.Id, cancellationToken);

                if (entity == null)
                {
                    return new CommandJsonResponse(true, "Qeyd Movcud Deyil!");
                }
                //var user = await userManager.GetUserAsync(User);
                entity.DeletedById = 1;
                entity.DeletedDate = DateTime.UtcNow.AddHours(4);
                await db.SaveChangesAsync(cancellationToken);
                return new CommandJsonResponse(false, "Deleted Successfully");
            }
        }
    }
}
