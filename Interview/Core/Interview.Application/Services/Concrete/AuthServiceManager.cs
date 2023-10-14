using Interview.Application.Mapper.AuthDTO;
using Interview.Application.Mapper.DTO;
using Interview.Application.Services.Abstract;
using Interview.Domain.Entities.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview.Application.Services.Concrete
{
    public class AuthServiceManager : IAuthService
    {
        public Task CandidateDocumentUpdate(CandidateDocumentDTO_forUpdate model, string connection)
        {
            throw new NotImplementedException();
        }

        public Task<List<GetAuthModel>> GetAdmins()
        {
            throw new NotImplementedException();
        }

        public Task<List<GetAuthModel>> GetHR()
        {
            throw new NotImplementedException();
        }

        public Task Login(LoginDTO model)
        {
            throw new NotImplementedException();
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public Task RefreshToken(TokenModel model)
        {
            throw new NotImplementedException();
        }

        public Task RegisterAdmin(RegisterDTO model)
        {
            throw new NotImplementedException();
        }

        public Task RegisterHR(RegisterDTO model)
        {
            throw new NotImplementedException();
        }

        public Task Revoke(string username)
        {
            throw new NotImplementedException();
        }

        public Task RevokeAll()
        {
            throw new NotImplementedException();
        }

        public Task UpdatePassword(UpdatePasswordDTO model)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProfile(UpdateProfileDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
