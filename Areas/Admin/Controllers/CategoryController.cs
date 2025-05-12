using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GP.Data;
using GP.Models;

namespace GP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Category
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        // GET: Admin/Category/Add
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // POST: Admin/Category/Add

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Category category, IFormFile Image)
        {
            if (Image == null || Image.Length == 0)
            {
                ModelState.AddModelError("Image", "Category image is required.");
            }
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Image!.FileName);

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Image.CopyToAsync(stream);
            }

            category.ImageUrl = "/image/" + uniqueFileName;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Category added successfully!";
            return RedirectToAction(nameof(Index));
        }




        // GET: Admin/Category/Edit/id
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: Admin/Category/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category, IFormFile? Image)
        {
            var categoryInDb = await _context.Categories.FindAsync(id);
            if (categoryInDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                categoryInDb.Name = category.Name;

                if (Image != null && Image.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    categoryInDb.ImageUrl = "/uploads/" + uniqueFileName;
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Category updated successfully";
                return RedirectToAction(nameof(Index));
            }

            return View(categoryInDb);
        }

        // GET: Admin/Category/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Category deleted successfully!";
            return RedirectToAction(nameof(Index));
        }




    }
}
