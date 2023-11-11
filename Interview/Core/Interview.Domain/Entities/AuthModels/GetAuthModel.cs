using Interview.Domain.Entities.IdentityAuth;
using Microsoft.AspNetCore.Http;

namespace Interview.Domain.Entities.AuthModels
{
    public class GetAuthModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string ImagePath { get; set; }

        public List <UserClaimModel> UserClaims { get; set; }

    }

    public class GetRoleModel
    {
        public string Id { get; set; }
        public string Rolename { get; set; }

    }
}
