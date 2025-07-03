using System.ComponentModel.DataAnnotations;

namespace GP.ViewModels
{
    public class MyAccountViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? ProfileImageUrl { get; set; }

    }
}
