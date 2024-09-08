using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang0.Repository;

namespace WebBanHang0.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin, Seller")]
	public class OrderController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public OrderController(DatabaseContext context)
        {
            _databaseContext = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _databaseContext.Orders.OrderByDescending(p => p.Id).ToListAsync());
        }
        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var DetailsOrder = await _databaseContext.OrdersDetails.Include(o=>o.Product).Where(o=>o.OrderCode==ordercode).ToListAsync();
            return View(DetailsOrder);
        }

    }
}
