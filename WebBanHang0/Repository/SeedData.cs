using Microsoft.EntityFrameworkCore;
using WebBanHang0.Models;

namespace WebBanHang0.Repository
{
	public class SeedData
	{
		public static void SeedingData(DatabaseContext _context)
		{
			_context.Database.Migrate();
			if (!_context.Products.Any())
			{
				CategoryModel electronics = new CategoryModel { Name = "Electronics", Slug = "electronics", Description = "electronics" };
				CategoryModel clothes = new CategoryModel { Name = "Clothes", Slug = "clothes", Description = "clothes" };
				BrandModel apple = new BrandModel { Name = "Apple", Slug = "apple", Description = "Apple" };
				BrandModel uniqlo = new BrandModel { Name = "Uniqlo", Slug = "uniqlo", Description = "Uniqlo" };
				_context.Products.AddRange
				(
					new ProductModel { Name = "Macbook", Slug = "macbook", Description = "Macbook", Image = "1.jpg", Category = electronics, Brand = apple, Price = 1999 },
					new ProductModel { Name = "Shirt", Slug = "shirt", Description = "Shirt", Image = "1.jpg", Category = clothes, Brand = uniqlo, Price = 199 }
				);
				_context.SaveChanges();
			}
		}
	}
}
