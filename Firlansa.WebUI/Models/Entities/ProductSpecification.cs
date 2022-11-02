using Firlansa.WebUI.AppCode.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Models.Entities
{
    public class ProductSpecification : HistoryEntity
    {
        public int ColorId { get; set; }
        public virtual Color Color { get; set; }
        public int SizeId { get; set; }
        public virtual Size Size { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
