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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", order);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            order.UserId = userId;
            order.OrderDate = DateTime.Now;
            order.Status = "Pending";

            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId && c.OrderId == null)
                .Include(c => c.Product)
                .ToListAsync();

            if (cartItems.Any())
            {
                order.CartItems = cartItems;

                foreach (var item in cartItems)
                {
                    item.Order = order;
                }
            }
            else
            {
                ModelState.AddModelError("", "Your cart is empty.");
                return View("Index", order);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "✔️ Your order has been placed successfully!";
            return RedirectToAction("MyOrders", "Order");
        }
        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }


    }
    }
