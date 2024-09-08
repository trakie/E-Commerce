using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebBanHang0.Models;

namespace WebBanHang0.Repository
{
	public class DatabaseContext : IdentityDbContext<AppUserModel>
	{
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base (options)
		{
		
		}

		public DbSet<BrandModel> Brands { get; set; }

		public DbSet<ProductModel> Products { get; set; }

		public DbSet<CategoryModel> Categories { get; set; }
		public DbSet<OrderModel> Orders { get; set; }
		public DbSet<OrderDetails> OrdersDetails { get; set; }
	}
}
