using GP.Models;
using GP.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }


        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }


        public IActionResult ViewProductsByCategory(int id)
        {
            var products = _context.Products
            .Where(p => p.CategoryId == id)
            .ToList();
            return View(products);
        }

        public IActionResult ProductDetails(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpGet]
        public IActionResult DesignRequest()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitDesignRequest(DesignRequest model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You must be logged in to submit a design request.";
                return RedirectToAction("DesignRequest");
            }

            // «· Õﬁﬁ «·ÌœÊÌ „‰ «·’Ê—…
            if (model.DesignImage == null || model.DesignImage.Length == 0)
            {
                ModelState.AddModelError("DesignImage", "Please upload a design image.");
            }

            if (!ModelState.IsValid)
            {
                // «Œ Ì«—Ì: ⁄—÷ ﬂ· «·√Œÿ«¡ ›Ì «·ﬂÊ‰”Ê·
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Model Error: " + error.ErrorMessage);
                }

                TempData["ErrorMessage"] = "There was an error. Please check your inputs.";
                return View("DesignRequest", model);
            }

            // Õ›Ÿ «·’Ê—…
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.DesignImage.FileName);
            string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            string filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.DesignImage.CopyToAsync(stream);
            }

            model.ImagePath = "/uploads/" + fileName;
            model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.SubmittedAt = DateTime.Now;

            _context.DesignRequests.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your design request has been submitted successfully!";
            return RedirectToAction("DesignRequest");
        }






        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
