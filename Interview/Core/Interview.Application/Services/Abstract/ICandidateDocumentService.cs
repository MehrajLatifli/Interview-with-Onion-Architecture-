

using Interview.Application.Mapper.DTO;

namespace Interview.Application.Services.Abstract
{
    public interface ICandidateDocumentService
    {
        #region CandidateDocument service

        public Task CandidateDocumentCreate(CandidateDocumentDTO_forCreate model, string connection);

        public Task<List<CandidateDocumentDTO_forGetandGetAll>> GetCandidateDocument();

        public Task<CandidateDocumentDTO_forGetandGetAll> GetCandidateDocumentById(int id);

        public Task CandidateDocumentUpdate(CandidateDocumentDTO_forUpdate model, string connection);

        public Task<CandidateDocumentDTO_forGetandGetAll> DeleteCandidateDocumentById(int id);

        #endregion



    }


}
