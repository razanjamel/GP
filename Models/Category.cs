using System.ComponentModel.DataAnnotations;

namespace GP.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
