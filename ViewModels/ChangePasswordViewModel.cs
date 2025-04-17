using System.ComponentModel.DataAnnotations;

namespace GP.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} charachters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Newpassword { get; set; }

        [Required(ErrorMessage = "Confitm New password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("Newpassword", ErrorMessage = "The password and confirmation password do not match!")]
        public string ConfirmNewPassword { get; set; }
    }
}
