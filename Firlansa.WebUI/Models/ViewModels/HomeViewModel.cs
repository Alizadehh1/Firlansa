using Firlansa.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Product> LastestProducts { get; set; }
        public List<Product> SaleProducts { get; set; }
    }
}
