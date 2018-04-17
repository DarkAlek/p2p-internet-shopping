using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace web_server.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(100, ErrorMessage = "Hasło musi mieć długość co najmniej 6 znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Podane hasła do siebie nie pasują.")]
        public string ConfirmPassword { get; set; }

        public string UserType { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([1-9]+)([0-9]{7})([0-9]*)$", ErrorMessage = "Podany numer telefonu jest niepoprawny.")]
        [StringLength(12, ErrorMessage = "Podany numer telefonu jest nieprawidłowy.", MinimumLength = 5)]
        public string PhoneNumber { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(100, ErrorMessage = "Hasło musi mieć długość co najmniej 6 znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Podane hasła do siebie nie pasują.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
