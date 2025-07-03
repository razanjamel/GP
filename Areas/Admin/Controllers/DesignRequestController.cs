using Microsoft.AspNetCore.Mvc;
using GP.Data;
using GP.Models;
using Microsoft.EntityFrameworkCore;

namespace GP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DesignRequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DesignRequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? status)
        {
            var requests = _context.DesignRequests.AsQueryable();

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<DesignStatus>(status, out var parsedStatus))
            {
                requests = requests.Where(r => r.Status == parsedStatus);
            }

            var list = await requests
                .OrderByDescending(r => r.SubmittedAt)
                .ToListAsync();

            ViewBag.CurrentStatus = status;

            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, DesignStatus newStatus)
        {
            var request = await _context.DesignRequests.FindAsync(id);
            if (request == null)
                return NotFound();

            request.Status = newStatus;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
