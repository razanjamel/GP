using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GP.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public required string FullName { get; set; }

        [Required]
        public required string Address { get; set; }

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        public string? Notes { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";


        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string PaymentMethod { get; set; } = "Cash on Delivery";

        public required string UserId { get; set; }

        public required List<CartItem> CartItems { get; set; } = new List<CartItem>();

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    }
}
