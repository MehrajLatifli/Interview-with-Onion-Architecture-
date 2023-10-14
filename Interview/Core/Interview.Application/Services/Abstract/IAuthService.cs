using Interview.Application.Mapper.AuthDTO;
using Interview.Application.Mapper.DTO;
using Interview.Domain.Entities.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview.Application.Services.Abstract
{
    public interface IAuthService
    {

        #region Auth service

        public Task RegisterAdmin(RegisterDTO model);

        public Task RegisterHR(RegisterDTO model);

        public Task Login(LoginDTO model);

        public Task Logout();

        public Task RefreshToken(TokenModel model);

        public Task Revoke(string username);

        public Task RevokeAll();

        public Task CandidateDocumentUpdate(CandidateDocumentDTO_forUpdate model, string connection);

        public Task<List<GetAuthModel>> GetAdmins();

        public Task<List<GetAuthModel>> GetHR();

        public Task UpdateProfile(UpdateProfileDTO model);

        public Task UpdatePassword(UpdatePasswordDTO model);


        #endregion

    }
}
