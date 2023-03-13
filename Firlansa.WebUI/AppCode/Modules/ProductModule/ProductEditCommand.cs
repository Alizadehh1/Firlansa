using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
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
    public class ProductEditCommand : IRequest<ProductEditCommandResponse>
    {
        public int Id { get; set; }
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
        public class ProductEditCommandHandler : IRequestHandler<ProductEditCommand, ProductEditCommandResponse>
        {
            readonly FirlansaDbContext db;
            readonly IHostingEnvironment env;
            readonly IActionContextAccessor ctx;

            public ProductEditCommandHandler(FirlansaDbContext db, IHostingEnvironment env, IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<ProductEditCommandResponse> Handle(ProductEditCommand request, CancellationToken cancellationToken)
            {

                var response = new ProductEditCommandResponse
                {
                    Product = null,
                };

                if (!ctx.ModelIsValid())
                {

                    return response;
                }

                var product = await db.Products
                    .Include(p=>p.Category)
                    .Include(p => p.Images.Where(i=>i.DeletedById==null))
                    .Include(p=>p.Specifications.Where(s => s.DeletedById == null))
                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
                if (product == null)
                {
                    ctx.AddModelError("Name", "Product tapilmadi");
                    return response;
                }


                product.Name = request.Name;
                product.CategoryId = request.CategoryId;
                product.ShortDescription = request.ShortDescription;
                product.Description = request.Description;
                product.Price = request.Price;
                product.OldPrice = request.OldPrice;
                product.ForSearch = request.Name;
                product.Slug = request.Name.ToSlug();

                if (request.Images != null)
                {
                    if (product.Images == null)
                    {
                        product.Images = new List<ProductImage>();
                    }

                    foreach (var productFile in request.Images)
                    {
                        if (productFile.File == null && string.IsNullOrWhiteSpace(productFile.TempPath))
                        {
                            var dbProductImage = product.Images.FirstOrDefault(p => p.Id == productFile.Id);

                            if (dbProductImage != null)
                            {
                                dbProductImage.DeletedById = 1;
                                dbProductImage.DeletedDate = DateTime.UtcNow.AddHours(4);
                                dbProductImage.IsMain = false;
                            }
                        }
                        else if (productFile.File != null)
                        {
                            string name = await env.SaveFile(productFile.File, cancellationToken, "product");

                            product.Images.Add(new ProductImage
                            {
                                ImagePath = name,
                                IsMain = productFile.IsMain
                            });
                        }
                        else if (!string.IsNullOrWhiteSpace(productFile.TempPath))
                        {
                            var dbProductImage = product.Images.FirstOrDefault(p => p.Id == productFile.Id);

                            if (dbProductImage != null)
                            {
                                dbProductImage.IsMain = productFile.IsMain;
                            }
                        }
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
                            SizeId = specification.SizeId,
                            Quantity = specification.Quantity
                        });
                    }
                }


                try
                {
                    await db.SaveChangesAsync(cancellationToken);
                    response.Product = product;
                    return response;
                }
                catch (Exception ex)
                {
                    response.Product = product;

                    ctx.AddModelError("Name", "Xeta bash verdi,Biraz sonra yeniden yoxlayin");

                    return response;
                }

            l1:
                return null;
            }
        
        }
    }
}
public class ProductEditCommandResponse
{
    public Product Product { get; set; }
}
