using Firlansa.WebUI.AppCode.Infrastructure;
using Recources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Firlansa.WebUI.Models.Entities
{
    public class Contact : BaseEntity
    {
        [Required(ErrorMessageResourceType = typeof(ContactForm), ErrorMessageResourceName = "CantBeEmpty")]
        public string Comment { get; set; }
        [Required(ErrorMessageResourceType = typeof(ContactForm), ErrorMessageResourceName = "CantBeEmpty")]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(ContactForm), ErrorMessageResourceName = "CantBeEmpty")]
        [EmailAddress(ErrorMessageResourceType = typeof(ContactForm), ErrorMessageResourceName = "InvalidEmailAddress")]
        public string Email { get; set; }
        public int? AnsweredById { get; set; }
        public DateTime? AnswerDate { get; set; }
        public string AnswerMessage { get; set; }
    }
}
