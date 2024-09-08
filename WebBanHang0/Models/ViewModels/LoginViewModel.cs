using System.ComponentModel.DataAnnotations;

namespace WebBanHang0.Models.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
