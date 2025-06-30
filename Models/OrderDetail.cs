using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GP.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        // العلاقة مع Order
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        // بيانات المنتج عند الطلب
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; } = string.Empty;  

        [Column(TypeName = "decimal(18,2)")]
        public decimal ProductPrice { get; set; }              

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public string SelectedColor { get; set; } = string.Empty;
    }
}
