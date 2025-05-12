using System.ComponentModel.DataAnnotations;


namespace IdentityDemo.Areas.Admin.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Roles")]
        public List<string> Roles { get; set; }

        [Display(Name = "Is Locked Out")]
        public bool IsLockedOut { get; set; }
    }
}