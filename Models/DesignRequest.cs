using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace GP.Models
{
    public enum DesignStatus
    {
        New,
        InProgress,
        Shipped,
        Delivered,
        Cancelled
    }
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

        [NotMapped] 
        [Required]
        public IFormFile DesignImage { get; set; }

        public string? ImagePath { get; set; }

        [Required]
        public string Width { get; set; }

        [Required]
        public string Height { get; set; }

        public string Color { get; set; }

        public string Fabric { get; set; }

        public int Id { get; set; }  
        public string? UserId { get; set; }
        public DesignStatus Status { get; set; } = DesignStatus.New;

        [ScaffoldColumn(false)]
        public DateTime SubmittedAt { get; set; }
    }
}
