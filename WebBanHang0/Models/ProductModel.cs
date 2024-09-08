using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebBanHang0.Repository.Validation;

namespace WebBanHang0.Models
{
    public class ProductModel
    {
		[Key]
		public int Id { get; set; }

		[Required, MinLength(4)]
		public string Name { get; set; }

		[Required, MinLength(4)]
		public string Description { get; set; }

		public string Slug { get; set; }

		public int Status { get; set; }

		[Required]
		[Range(0.01, double.MaxValue)]
		[Column(TypeName ="decimal(8, 2)")]
		public int Price { get; set; }

        [Required(ErrorMessage = "The Brand field is required."), Range(1, int.MaxValue)]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "The Category field is required."), Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

		public CategoryModel Category { get; set; }

        public BrandModel Brand { get; set; }

		public string Image { get; set; } 
			
		[NotMapped]
		[FileExtension]
		public IFormFile? ImgUpload { get; set; }


	}
}
