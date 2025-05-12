using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GP.Data;
using GP.Models;
using System;

namespace GP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        
        // عرض كل المنتجات
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return View(products);
        }

        // عرض صفحة إضافة
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // إضافة منتج جديد

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Product product, IFormFile Image)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                ModelState.AddModelError("Name", "Product name is required.");
            if (Image == null || Image.Length == 0)
                ModelState.AddModelError("Image", "Product image is required.");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(product);
            }

            // رفع الصورة (مرة واحدة فقط)
            var uploadsFolder = Path.Combine(_env.WebRootPath, "image");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Image!.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Image.CopyToAsync(stream);
            }

            product.ImageUrl = "/image/" + uniqueFileName;

            // حفظ المنتج
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Product added successfully!";
            return RedirectToAction(nameof(Index));
        }




        // عرض صفحة التعديل
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        // تعديل المنتج
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product, IFormFile? Image)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(product);
            }

            var existing = await _context.Products.FindAsync(product.Id);
            if (existing == null) return NotFound();

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.AvailableColors = product.AvailableColors;
            existing.CategoryId = product.CategoryId;

            if (Image != null && Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "image");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                existing.ImageUrl = "/image/" + fileName;
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Product updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // حذف المنتج
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Product deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
