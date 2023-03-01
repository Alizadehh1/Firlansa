using Firlansa.WebUI.AppCode.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firlansa.WebUI.Models.Entities
{
    public class Adress : BaseEntity
    {
        [Required(ErrorMessage = "Ünvanın adını daxil edin!")]
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Required(ErrorMessage = "Çatdırılma qiymətini daxil edin!")]
        [DataType("decimal(38,2)")]
        public double ShippingPrice { get; set; }
        public int? ParentId { get; set; }
        public virtual Adress Parent { get; set; }
        public virtual ICollection<Adress> Children { get; set; }
    }
}

