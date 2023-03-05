using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities;

namespace Firlansa.WebUI.AppCode.Modules.SliderModule
{
    public class SliderSingleQuery : IRequest<Slider>
    {
        public int Id { get; set; }

        public class SliderSingleQueryHandler : IRequestHandler<SliderSingleQuery, Slider>
        {
            readonly FirlansaDbContext db;
            public SliderSingleQueryHandler(FirlansaDbContext db)
            {
                this.db = db;
            }
            public async Task<Slider> Handle(SliderSingleQuery request, CancellationToken cancellationToken)
            {
                var data = await db.Sliders
                    .FirstOrDefaultAsync(s => s.DeletedById == null && s.Id == request.Id, cancellationToken);

                return data;
            }
        }
    }
}
