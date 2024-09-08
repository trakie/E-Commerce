using Microsoft.AspNetCore.Identity;

namespace WebBanHang0.Models
{
	public class AppUserModel : IdentityUser
	{
		public string Occupation { get; set; }
		public string RoleId { get; set; }
	}
}
