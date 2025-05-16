using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GP.Data;
using GP.Models;
using System.Security.Claims;
using System;

namespace GP.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // عرض السلة
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            return View(cartItems);
        }

        // إضافة منتج للسلة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart([FromBody] CartRequestModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == model.ProductId && c.UserId == userId);

            if (existingItem != null)
            {
                // دمج اللون إذا مش موجود
                var colors = (existingItem.SelectedColors ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim()).ToList();
                if (!colors.Contains(model.SelectedColor))
                {
                    colors.Add(model.SelectedColor);
                    existingItem.SelectedColors = string.Join(", ", colors);
                }

                existingItem.Quantity += model.Quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductId = model.ProductId,
                    UserId = userId,
                    Quantity = model.Quantity,
                    SelectedColors = model.SelectedColor
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }


        // حذف منتج من السلة
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}


