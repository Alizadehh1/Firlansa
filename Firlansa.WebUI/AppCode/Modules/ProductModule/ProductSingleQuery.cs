using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities;

namespace Firlansa.WebUI.AppCode.Modules.ProductModule
{
    public class ProductSingleQuery : IRequest<Product>
    {
        public int Id { get; set; }
        public class ProductSingleQueryHandler : IRequestHandler<ProductSingleQuery, Product>
        {
            private readonly FirlansaDbContext db;

            public ProductSingleQueryHandler(FirlansaDbContext db)
            {
                this.db = db;
            }
            public async Task<Product> Handle(ProductSingleQuery request, CancellationToken cancellationToken)
            {
                var entity = await db.Products
                    .Include(p=>p.Images.Where(i => i.DeletedById == null))
                    .Include(p=>p.Category)
                    .Include(p=>p.Specifications.Where(s => s.DeletedById == null))
                    .ThenInclude(s=>s.Color)
                    .Include(s=>s.Specifications.Where(i => i.DeletedById == null))
                    .ThenInclude(s=>s.Size)
                    .FirstOrDefaultAsync(p => p.Id == request.Id && p.DeletedById == null, cancellationToken);
                return entity;
            }
        }
    }
}
