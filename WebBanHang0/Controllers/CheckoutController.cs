using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebBanHang0.Models;
using WebBanHang0.Repository;

namespace WebBanHang0.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        public CheckoutController(DatabaseContext context)
        {
            _databaseContext = context;
        }
        public async Task<IActionResult> Checkout()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var ordercode= Guid.NewGuid().ToString();
                var orderItem = new OrderModel();
                orderItem.OrderCode = ordercode;
                orderItem.UserName = userEmail;
                orderItem.Status = 1;
                orderItem.CreatedDate = DateTime.Now;
                _databaseContext.Add(orderItem);
                _databaseContext.SaveChanges();
                List<CartModel> cartItems = HttpContext.Session.GetJason<List<CartModel>>("Cart") ?? new List<CartModel>();
                foreach (var cart in cartItems)
                {
                    var odDetails = new OrderDetails();
                    odDetails.UserName= userEmail;
                    odDetails.OrderCode = ordercode;
                    odDetails.ProductId= cart.ProductId;
                    odDetails.Price = cart.Price;
                    odDetails.Quantity = cart.Quantity;
                    _databaseContext.Add(odDetails);
                    _databaseContext.SaveChanges();
                }
                HttpContext.Session.Remove("Cart");
                TempData["success"] = "Created";
                return RedirectToAction("Index", "Cart");
            }
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
