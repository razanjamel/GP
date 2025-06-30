using GP.Data;
using GP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string status = "")
        {
            var query = _context.Orders
                .Include(o => o.OrderDetails) 
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.Status != null && o.Status.ToLower() == status.ToLower());
            }

            var orders = await query
                .AsNoTracking()
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();


            return View(orders);
        }

        // Details
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderDetails)  
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // Update Status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            order.Status = status;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Order status updated successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
