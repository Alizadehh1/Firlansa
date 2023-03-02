using Firlansa.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Models.ViewModels
{
    public class ShopViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Color> Colors { get; set; }
        public List<Size> Sizes { get; set; }
        public List<Product> Products { get; set; }
        public List<ProductSpecification> ProductSpecifications { get; set; }
        public Product Product { get; set; }
        public PagedViewModel<Product> PagedViewModel { get; set; }
    }
}
