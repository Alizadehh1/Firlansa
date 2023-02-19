using System;
using Firlansa.WebUI.AppCode.Infrastructure;

namespace Firlansa.WebUI.Models.Entities
{
    public class Subscribe : BaseEntity
    {
        public string Email { get; set; }
        public bool EmailSended { get; set; } = false;
        public DateTime? AppliedDate { get; set; }
    }
}
