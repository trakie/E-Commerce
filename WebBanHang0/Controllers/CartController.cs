using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using WebBanHang0.Models;
using WebBanHang0.Models.ViewModels;
using WebBanHang0.Repository;

namespace WebBanHang0.Controllers
{
    public class CartController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        public CartController(DatabaseContext _context)
        {
            _databaseContext = _context;
        }
        public IActionResult Index()
        {
            List<CartModel> cartItems = HttpContext.Session.GetJason<List<CartModel>>("Cart") ?? new List<CartModel>();
            CartViewModel cartVM = new()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
            };
            return View(cartVM);
        }
        public ActionResult Checkout()
        {
            return View("~/Views/Checkout/Index.cshtml");
        }
        public async Task<IActionResult> Add(int Id)
        {
            ProductModel product = await _databaseContext.Products.FindAsync(Id);
            List<CartModel> cart = HttpContext.Session.GetJason<List<CartModel>>("Cart") ?? new List<CartModel>();
            CartModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
            if (cartItems == null)
            {
                cart.Add(new CartModel(product));
            }
            else
            {
                cartItems.Quantity += 1;
            }
            HttpContext.Session.SetJson("Cart", cart);
            TempData["success"] = "Add Item";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> Decrease(int Id)
        {
            List<CartModel> cart = HttpContext.Session.GetJason<List<CartModel>>("Cart");
            CartModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
            if (cartItems.Quantity > 1)
            {
                --cartItems.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.ProductId == Id);
            }
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Increase(int Id)
        {
            List<CartModel> cart = HttpContext.Session.GetJason<List<CartModel>>("Cart");
            CartModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
            if (cartItems.Quantity >= 1)
            {
                ++cartItems.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.ProductId == Id);
            }
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int Id)
        {
            List<CartModel> cart = HttpContext.Session.GetJason<List<CartModel>>("Cart");
            cart.RemoveAll(p => p.ProductId == Id);
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
		public async Task<IActionResult> Clear()
        {
			HttpContext.Session.Remove("Cart");
			return RedirectToAction("Index");
		}

	}
}
