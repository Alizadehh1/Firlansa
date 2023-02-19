using Firlansa.WebUI.AppCode.Infrastructure;
using Firlansa.WebUI.Models.Entities.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Models.Entities
{
    public class ProductStatus : BaseEntity
    {
        public int ColorId { get; set; }
        public virtual Color Color { get; set; }
        public int SizeId { get; set; }
        public virtual Size Size { get; set; }
        public int FirlansaUserId { get; set; }
        public virtual FirlansaUser FirlansaUser { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
