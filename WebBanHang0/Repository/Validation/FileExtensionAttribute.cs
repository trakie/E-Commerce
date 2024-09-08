using System.ComponentModel.DataAnnotations;

namespace WebBanHang0.Repository.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                string[] extensions = { "jpeg", "jpg", "png" };
                bool result = extension.Any(x => extension.EndsWith(x));
                if (!result)
                {
                    return new ValidationResult("Error");
                }

            }
            return ValidationResult.Success;
        }
    }
}
