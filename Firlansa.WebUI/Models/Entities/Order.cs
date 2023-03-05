using Firlansa.WebUI.AppCode.Infrastructure;
using Firlansa.WebUI.Models.Entities.Membership;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Firlansa.WebUI.Models.Entities
{
    public class Order : BaseEntity
    {
        public int FirlansaUserId { get; set; }
        public virtual FirlansaUser FirlansaUser { get; set; }
        [Required(ErrorMessage = "İnzibati ərazini daxil edin zəhmət olmasa!")]
        public int AdressId { get; set; }
        public virtual Adress Adress { get; set; }
        [Required(ErrorMessage = "Çatdırılma Ünvanını daxil edin zəhmət olmasa!")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Əlaqə nömrəsini daxil edin zəhmət olmasa!")]
        public string PhoneNumber { get; set; }
        public string PostCode { get; set; }
        public string OrderStatus { get; set; }
        public int? OrderId { get; set; }
        public string SessionId { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
