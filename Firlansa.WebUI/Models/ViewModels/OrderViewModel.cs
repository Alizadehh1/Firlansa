using Firlansa.WebUI.Models.Entities;
using Firlansa.WebUI.Models.Entities.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Models.ViewModels
{
    public class OrderViewModel
    {
        public List<FirlansaUser> Users { get; set; }
        public IOrderedEnumerable<Order> Orders { get; set; }
        public List<Product> Products { get; set; }
        public Order Order { get; set; }
    }
}
