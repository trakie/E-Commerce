using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebBanHang0.Repository.Components
{
	public class BrandsViewComponent : ViewComponent
	{
		private readonly DatabaseContext _databaseContext;
		public BrandsViewComponent(DatabaseContext context)
		{
			_databaseContext = context;
		}
		public async Task<IViewComponentResult> InvokeAsync() => View( await _databaseContext.Brands.ToListAsync());
	}
}
