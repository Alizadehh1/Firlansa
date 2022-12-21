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
    public class ProductAllQuery : IRequest<IEnumerable<Product>>
    {
        public class ProductAllQueryHandler : IRequestHandler<ProductAllQuery, IEnumerable<Product>>
        {
            private readonly FirlansaDbContext db;

            public ProductAllQueryHandler(FirlansaDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<Product>> Handle(ProductAllQuery request, CancellationToken cancellationToken)
            {
                var entity = db.Products
                    .Include(c => c.Category)
                    .Include(i => i.Images.Where(i => i.IsMain == true && i.DeletedById==null))
                    .Where(p => p.DeletedById == null)
                    .OrderByDescending(p => p.CreatedDated);
                return entity;
            }
        }
    }
}
