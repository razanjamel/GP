using GP.Data; 
using GP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GP.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId && c.OrderId == null)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "You cannot complete the order because the cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            var order = new Order
            {
                FullName = "",
                Address = "",
                PhoneNumber = "",
                UserId = userId,
                CartItems = new List<CartItem>()
            };


            return View(order);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitOrder(Order order)
        {
            if (!ModelState.IsValid)
                return View("Index", order);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            order.UserId = userId;
            order.OrderDate = DateTime.Now;
            order.Status = "Pending";

            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId && c.OrderId == null)
                .Include(c => c.Product)
                .ToListAsync();

            if (!cartItems.Any())
            {
                ModelState.AddModelError("", "Your cart is empty.");
                return View("Index", order);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderDetails = cartItems.Select(ci => new OrderDetail
            {
                OrderId = order.Id,
                ProductId = ci.ProductId,
                ProductName = ci.Product?.Name ?? "",
                ProductPrice = ci.Product?.Price ?? 0,
                Quantity = ci.Quantity,
                SelectedColor = ci.SelectedColors
            }).ToList();

            _context.OrderDetails.AddRange(orderDetails);

            foreach (var item in cartItems)
            {
                item.OrderId = order.Id;
            }
            _context.CartItems.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your order has been placed successfully!";
            return RedirectToAction("MyOrders", "Order");
        }


        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }



    }
}
