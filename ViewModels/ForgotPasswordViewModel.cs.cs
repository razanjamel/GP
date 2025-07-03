using System.ComponentModel.DataAnnotations;

namespace GP.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
