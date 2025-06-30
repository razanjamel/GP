using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace GP.Models
{
    public class DesignRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [NotMapped]
        public IFormFile DesignImage { get; set; }

        [Required]
        public string Width { get; set; }

        [Required]
        public string Height { get; set; }

        public string Color { get; set; }

        public string Fabric { get; set; }

        [Key]
        public string UserId { get; set; }
    }
}
