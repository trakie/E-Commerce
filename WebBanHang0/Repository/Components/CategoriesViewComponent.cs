using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebBanHang0.Repository.Components
{
	public class CategoriesViewComponent : ViewComponent
	{
		private readonly DatabaseContext _databaseContext;
		public CategoriesViewComponent(DatabaseContext context)
		{
			_databaseContext = context;
		}
		public async Task<IViewComponentResult> InvokeAsync() => View( await _databaseContext.Categories.ToListAsync());
	}
}
