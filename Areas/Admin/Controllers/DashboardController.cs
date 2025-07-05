using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GP.Data;
using GP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var orderCount = _context.Orders.Count();

            var user = await _userManager.GetUserAsync(User);

            string fullName = user != null ? $"{user.FirstName} {user.LastName}" : "Admin";

            ViewBag.FullName = fullName;
            ViewBag.OrderCount = orderCount;

            return View();
        }

    }

}