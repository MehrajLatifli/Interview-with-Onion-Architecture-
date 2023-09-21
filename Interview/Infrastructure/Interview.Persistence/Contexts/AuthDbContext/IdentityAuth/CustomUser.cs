using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview.Persistence.Contexts.AuthDbContext.IdentityAuth
{
    public class CustomUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public string? ImagePath { get; set; }

        public string? Roles { get; set; }
    }
}
