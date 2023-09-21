using Interview.Persistence.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Interview.Domain.AuthModels
{
    public class UpdateModel
    {
        [Required(ErrorMessage = "User Id is required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "User Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "User Password is required")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "New Password is required")]
        public string? NewPassword { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Image Path is required")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" })]
        [FileSize(5, 10)]
        public IFormFile ImagePath { get; set; }

    }
}
