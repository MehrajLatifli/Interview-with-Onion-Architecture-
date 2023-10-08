namespace Interview.Domain.Entities.AuthModels
{
    public class GetAuthModel
    {

        public string Username { get; set; }

        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string ImagePath { get; set; }

        public string Roles { get; set; }
    }
}
