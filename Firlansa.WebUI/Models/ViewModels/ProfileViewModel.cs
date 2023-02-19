using Firlansa.WebUI.Models.Entities;
using Firlansa.WebUI.Models.Entities.Membership;
using System.Collections.Generic;

namespace Firlansa.WebUI.Models.ViewModels
{
    public class ProfileViewModel
    {
        public FirlansaUser FirlansaUser { get; set; }
        public List<Order> Order { get; set; }
        public List<ProductStatus> ProductStatuses { get; set; }
    }
}
