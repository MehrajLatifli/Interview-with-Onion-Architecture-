using Interview.Domain.Validations;
using Microsoft.AspNetCore.Http;

namespace Interview.Domain.AuthModels
{
    public class UploadFile
    {
        [FileSize(5, 10)]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" })]
        public IFormFile ImagePath { get; set; }
    }
}
