using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHang0.Models;
using WebBanHang0.Repository;

namespace WebBanHang0.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin, Seller")]
    public class BrandController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public BrandController(DatabaseContext context)
        {
            _databaseContext = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _databaseContext.Brands.OrderByDescending(p => p.Id).ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {
            if (ModelState.IsValid)
            {
                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _databaseContext.Brands.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Exsisted");
                    return View(brand);
                }

                _databaseContext.Add(brand);
                await _databaseContext.SaveChangesAsync();
                TempData["success"] = "Added";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["fail"] = "Model Error";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {
            BrandModel brand = await _databaseContext.Brands.FindAsync(Id);

            if (brand == null)
            {
                return NotFound();
            }

            _databaseContext.Brands.Remove(brand);
            await _databaseContext.SaveChangesAsync();
            TempData["success"] = "Deleted";
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            BrandModel brand = await _databaseContext.Brands.FindAsync(Id);
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandModel brand)
        {
            if (ModelState.IsValid)
            {
                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _databaseContext.Brands.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Exsisted");
                    return View(brand);
                }

                _databaseContext.Update(brand);
                await _databaseContext.SaveChangesAsync();
                TempData["success"] = "Added";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["fail"] = "Model Error";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
        }




    }
}
