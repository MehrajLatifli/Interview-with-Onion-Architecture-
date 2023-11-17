

using Interview.Application.Mapper.DTO;

namespace Interview.Application.Services.Abstract
{
    public interface ICandidateService
    {
        #region Candidate service

        public Task CandidateCreate(CandidateDTO_forCreate model);

        public Task<List<CandidateDTO_forGetandGetAll>> GetCandidate();

        public Task<CandidateDTO_forGetandGetAll> GetCandidateById(int id);

        public Task CandidateUpdate(CandidateDTO_forUpdate model);

        public Task<CandidateDTO_forGetandGetAll> DeleteCandidateById(int id);

        #endregion
    }


}
