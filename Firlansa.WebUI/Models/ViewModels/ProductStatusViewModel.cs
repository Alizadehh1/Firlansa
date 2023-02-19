using Firlansa.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Models.ViewModels
{
    public class ProductStatusViewModel
    {
        public List<Product> Products { get; set; }
        public List<ProductStatus> ProductStatusesDistinct { get; set; }
        public List<ProductStatus> ProductStatuses { get; set; }
    }
}
