using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Firlansa.WebUI.AppCode.Extensions;
using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities;

namespace Firlansa.WebUI.AppCode.Modules.ProductModule
{
    public class ProductCreateCommand : IRequest<Product>
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public double Price { get; set; }
        public double? OldPrice { get; set; }
        public string ForSearch { get; set; }
        public ProductSpecification[] Specification { get; set; }
        public ImageItem[] Images { get; set; }
        [Obsolete]
        public class ProductCreateCommandHandler : IRequestHandler<ProductCreateCommand, Product>
        {
            readonly FirlansaDbContext db;
            readonly IActionContextAccessor ctx;
            readonly IHostingEnvironment env;

            public ProductCreateCommandHandler(FirlansaDbContext db,
                IActionContextAccessor ctx,
                IHostingEnvironment env)
            {
                this.db = db;
                this.ctx = ctx;
                this.env = env;
            }

            public async Task<Product> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
            {


                var product = new Product();

                product.Name = request.Name;
                product.CategoryId = request.CategoryId;
                product.Description = request.Description;
                product.ShortDescription = request.ShortDescription;
                product.Price = request.Price;
                product.OldPrice = request.OldPrice;
                product.ForSearch = request.Name;



                if (request.Images != null && request.Images.Any(i => i.File != null))
                {
                    product.Images = new List<ProductImage>();

                    foreach (var productFile in request.Images.Where(i => i.File != null))
                    {
                        string name = await env.SaveFile(productFile.File, cancellationToken, "product");

                        product.Images.Add(new ProductImage
                        {
                            ImagePath = name,
                            IsMain = productFile.IsMain
                        });
                    }
                }
                else
                {
                    ctx.AddModelError("Images", "Sekil qeyd edilmeyib");
                    goto l1;
                }

                if (request.Specification != null && request.Specification.Length > 0)
                {
                    product.Specifications = new List<Firlansa.WebUI.Models.Entities.ProductSpecification>();

                    foreach (var specification in request.Specification)
                    {
                        product.Specifications.Add(new Firlansa.WebUI.Models.Entities.ProductSpecification
                        {
                            ColorId = specification.ColorId,
                            SizeId = specification.SizeId
                        });
                    }
                }

                await db.Products.AddAsync(product, cancellationToken);
                try
                {
                    await db.SaveChangesAsync(cancellationToken);
                    return product;
                }
                catch (Exception ex)
                {

                    ctx.AddModelError("Name", "Xeta bash verdi,Biraz sonra yeniden yoxlayin");

                    return product;
                }

            l1:
                return null;
            }
        }
    }

}
