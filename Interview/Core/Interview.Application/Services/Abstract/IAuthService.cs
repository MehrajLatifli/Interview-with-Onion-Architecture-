using Interview.Application.Mapper.AuthDTO;
using Interview.Application.Mapper.DTO;
using Interview.Domain.Entities.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Interview.Application.Services.Abstract
{
    public interface IAuthService
    {

        #region Auth service

        public Task CreateAndAssignCustomRole(string userId, string roleName, ClaimsPrincipal User);
        public Task RegisterAdmin(RegisterDTO model, string ConnectionStringAzure);

        public Task RegisterHR(RegisterDTO model, string ConnectionStringAzure);

        public Task<LoginResponse> Login(LoginDTO model);

        public Task Logout();

        public Task<TokenModel> RefreshToken(TokenModel model);

        public Task Revoke(string username);

        public Task RevokeAll();


        public Task<List<GetAuthModel>> GetAdmins(ClaimsPrincipal User);

        public Task<List<GetAuthModel>> GetHR(ClaimsPrincipal User);

        public Task UpdateProfile(UpdateProfileDTO model, System.Security.Claims.ClaimsPrincipal claims, string ConnectionStringAzure);

        public Task UpdatePassword(UpdatePasswordDTO model,ClaimsPrincipal User);


        #endregion

    }
}
