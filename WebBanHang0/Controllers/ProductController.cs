using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Search(string searchTerm)
        {
            var products = await _databaseContext.Products.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)).ToListAsync();
            ViewBag.Keyword = searchTerm;
            return View(products);
        }
        public async Task<IActionResult> Details(int Id)
        {
            if (Id == null) return RedirectToAction("Index");
            var productById = _databaseContext.Products.Where(p => p.Id == Id).FirstOrDefault();
            var relatedProducts = await _databaseContext.Products
                .Where(p => p.CategoryId == productById.CategoryId && p.Id != productById.Id)
                .Take(4)
                .ToListAsync();
            ViewBag.RelatedProducts = relatedProducts;
            return View(productById);
        }

    }
}
