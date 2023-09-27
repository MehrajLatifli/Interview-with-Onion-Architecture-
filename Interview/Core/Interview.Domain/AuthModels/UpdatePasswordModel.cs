using System.ComponentModel.DataAnnotations;

namespace Interview.Domain.AuthModels
{
    public class UpdatePasswordModel
    {

        [Required(ErrorMessage = "Old password is required")]
        public string? OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        public string? NewPassword { get; set; }


    }
}
