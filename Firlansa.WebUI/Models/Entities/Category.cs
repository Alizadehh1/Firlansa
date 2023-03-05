using Firlansa.WebUI.AppCode.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Firlansa.WebUI.Models.Entities
{
    public class Category : BaseEntity
    {
        [Required(ErrorMessage ="Kateqoriyanın adını daxil edin")]
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }
    }
}
