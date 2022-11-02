using System.ComponentModel.DataAnnotations;

namespace Firlansa.WebUI.Models.FormModels
{
    public class RegisterFormModel
    {
        [Required(ErrorMessage = "'Email' Xanasını boş saxlamayın!")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "'Şifrə' Xanasını boş saxlamayın!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "'Şifrənın təkrarı' Xanasını boş saxlamayın!")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Şifrələr Uyğun Deyil!")]
        public string ConfirmPassword { get; set; }

    }
}
