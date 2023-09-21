using System.ComponentModel.DataAnnotations;

namespace Interview.Domain.AuthModels
{
    public class UpdatePassword
    {

        [Required(ErrorMessage = "User Id is required")]
        public int Id { get; set; }


        [Required(ErrorMessage = "User Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        public string? NewPassword { get; set; }


    }
}
