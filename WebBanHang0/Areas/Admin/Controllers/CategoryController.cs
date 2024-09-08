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
	[Authorize(Roles = "Admin, Seller")]
	public class CategoryController : Controller
	{
		private readonly DatabaseContext _databaseContext;

		public CategoryController(DatabaseContext context)
		{
			_databaseContext = context;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _databaseContext.Categories.OrderByDescending(p => p.Id).ToListAsync());
		}

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _databaseContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Exsisted");
                    return View(category);
                }

                _databaseContext.Add(category);
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
            CategoryModel category = await _databaseContext.Categories.FindAsync(Id);

            if (category == null)
            {
                return NotFound();
            }

            _databaseContext.Categories.Remove(category);
            await _databaseContext.SaveChangesAsync();
            TempData["fail"] = "Deleted";
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            CategoryModel category = await _databaseContext.Categories.FindAsync(Id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _databaseContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Exsisted");
                    return View(category);
                }

                _databaseContext.Update(category);
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
