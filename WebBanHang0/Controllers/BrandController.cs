using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang0.Models;
using WebBanHang0.Repository;

namespace WebBanHang0.Controllers
{
	public class BrandController : Controller
	{
		private readonly DatabaseContext _databaseContext;

		public BrandController(DatabaseContext context)
		{
			_databaseContext = context;
		}

		public async Task<IActionResult> Index(string Slug = "")
		{
			BrandModel brand = _databaseContext.Brands.Where(c => c.Slug == Slug).FirstOrDefault();
			if (brand == null) return RedirectToAction("Index");
			var productByBrand = _databaseContext.Products.Where(p => p.BrandId == brand.Id);
			return View(await productByBrand.OrderByDescending(p => p.Id).ToListAsync());
		}
	}
}
