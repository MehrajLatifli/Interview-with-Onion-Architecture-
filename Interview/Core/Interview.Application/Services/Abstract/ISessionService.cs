

using Interview.Application.Mapper.DTO;
using System.Security.Claims;

namespace Interview.Application.Services.Abstract
{
    public interface ISessionService
    {
        #region Session service

        public Task SessionCreate(SessionDTO_forCreate model, ClaimsPrincipal User);

        public Task<List<SessionDTO_forGetandGetAll>> GetSession();

        public Task<SessionDTO_forGetandGetAll> GetSessionById(int id);

        public Task SessionUpdate(SessionDTO_forUpdate model);

        public Task<SessionDTO_forGetandGetAll> DeleteSessionById(int id);



        #endregion
    }


}
