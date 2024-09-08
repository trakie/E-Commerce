using Microsoft.AspNetCore.Mvc;
using WebBanHang0.Repository;

namespace WebBanHang0.Controllers
{
    public class ProductController : Controller
    {
		private readonly DatabaseContext _databaseContext;
		public ProductController(DatabaseContext context)
		{
			_databaseContext = context;
		}


		public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int Id)
        {
            if (Id == null) return RedirectToAction("Index");
			var productById = _databaseContext.Products.Where(p => p.Id == Id).FirstOrDefault();
			return View(productById);
        }

    }
}
