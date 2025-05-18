using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GP.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public Product? Product { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        public string SelectedColors { get; set; } = "";

        public decimal TotalPrice => Product != null ? Quantity * Product.Price : 0;
        public int? OrderId { get; set; }
        public Order? Order { get; set; }


    }
}
