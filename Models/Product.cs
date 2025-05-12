using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GP.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(0.01, double.MaxValue)]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        public string? AvailableColors { get; set; }


        public string? ImageUrl { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
