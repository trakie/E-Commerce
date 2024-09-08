using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang0.Models;
using WebBanHang0.Repository;

namespace WebBanHang0.Controllers
{
    public class CategoryController : Controller
    {
		private readonly DatabaseContext _databaseContext;
		public CategoryController (DatabaseContext context)
        {
            _databaseContext = context;
        }

		public async Task<IActionResult> Index(string Slug = "")
        {
            CategoryModel category = _databaseContext.Categories.Where(c =>  c.Slug == Slug).FirstOrDefault();
            if (category == null) return RedirectToAction("Index");
            var productByCate = _databaseContext.Products.Where(p => p.CategoryId == category.Id);
            return View(await productByCate.OrderByDescending(p => p.Id).ToListAsync());
        }
    }
}
