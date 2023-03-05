using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities;

namespace Firlansa.WebUI.AppCode.Modules.SliderModule
{
    public class SliderAllQuery : IRequest<IEnumerable<Slider>>
    {
        public class SliderAllQueryHandler : IRequestHandler<SliderAllQuery, IEnumerable<Slider>>
        {
            readonly FirlansaDbContext db;
            public SliderAllQueryHandler(FirlansaDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<Slider>> Handle(SliderAllQuery request, CancellationToken cancellationToken)
            {
                var data = db.Sliders
                    .Where(s => s.DeletedById == null);

                return data;
            }
        }
    }
}
