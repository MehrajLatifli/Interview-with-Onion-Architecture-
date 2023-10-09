

using Interview.Application.Mapper.DTO;

namespace Interview.Application.Services.Abstract
{
    public interface IService
    {
        #region CandidateDocument service

        public Task CandidateDocumentCreate(CandidateDocumentDTO_forCreate model, string connection);

        public Task<List<CandidateDocumentDTO_forGetandGetAll>> GetCandidateDocument();

        public Task<CandidateDocumentDTO_forGetandGetAll> GetCandidateDocumentById(int id);

        public Task CandidateDocumentUpdate(CandidateDocumentDTO_forUpdate model, string connection);

        public Task<CandidateDocumentDTO_forGetandGetAll> DeleteCandidateDocumentById(int id);

        #endregion


        #region Candidate service

        public Task  CandidateCreate(CandidateDTO_forCreate model);

        public Task<List<CandidateDTO_forGetandGetAll>> GetCandidate();

        public Task<CandidateDTO_forGetandGetAll> GetCandidateById(int id);

        public Task CandidateUpdate(CandidateDTO_forUpdate model);

        public Task<CandidateDTO_forGetandGetAll> DeleteCandidateById(int id);

        #endregion


        #region Level service

        public Task LevelCreate(LevelDTO_forCreate model);

        public Task<List<LevelDTO_forGetandGetAll>> GetLevel();

        public Task<LevelDTO_forGetandGetAll> GetLevelById(int id);

        public Task LevelUpdate(LevelDTO_forUpdate model);

        public Task<LevelDTO_forGetandGetAll> DeleteLevelById(int id);

        #endregion


        #region SessionType service

        public Task SessionTypeCreate(SessionTypeDTO_forCreate model);

        public Task<List<SessionTypeDTO_forGetandGetAll>> GetSessionType();

        public Task<SessionTypeDTO_forGetandGetAll> GetSessionTypeById(int id);

        public Task SessionTypeUpdate(SessionTypeDTO_forUpdate model);

        public Task<SessionTypeDTO_forGetandGetAll> DeleteSessionTypeById(int id);

        #endregion


        #region Question service

        public Task QuestionCreate(QuestionDTO_forCreate model);

        public Task<List<QuestionDTO_forGetandGetAll>> GetQuestion();

        public Task<List<QuestionDTO_forGetandGetAll>> GetRandomQuestion();

        public Task<QuestionDTO_forGetandGetAll> GetQuestionById(int id);

        public Task QuestionUpdate(QuestionDTO_forUpdate model);

        public Task<QuestionDTO_forGetandGetAll> DeleteQuestionById(int id);

        #endregion


        #region SessionQuestion service

        public Task SessionQuestionCreate(SessionQuestionDTO_forCreate model);

        public Task<List<SessionQuestionDTO_forGetandGetAll>> GetSessionQuestion();

        public Task<SessionQuestionDTO_forGetandGetAll> GetSessionQuestionById(int id);

        public Task SessionQuestionUpdate(SessionQuestionDTO_forUpdate model);

        public Task<SessionQuestionDTO_forGetandGetAll> DeleteSessionQuestionById(int id);

        #endregion


        #region Session service

        public Task SessionCreate(SessionDTO_forCreate model);

        public Task<List<SessionDTO_forGetandGetAll>> GetSession();

        public Task<SessionDTO_forGetandGetAll> GetSessionById(int id);

        public Task SessionUpdate(SessionDTO_forUpdate model);

        public Task<SessionDTO_forGetandGetAll> DeleteSessionById(int id);

        #endregion


        #region Structure service

        public Task StructureCreate(StructureDTO_forCreate model);

        public Task<List<StructureDTO_forGetandGetAll>> GetStructure();

        public Task<StructureDTO_forGetandGetAll> GetStructureById(int id);

        public Task StructureUpdate(StructureDTO_forUpdate model);

        public Task<StructureDTO_forGetandGetAll> DeleteStructureById(int id);

        #endregion


        #region StructureType service

        public Task StructureTypeCreate(StructureTypeDTO_forCreate model);

        public Task<List<StructureTypeDTO_forGetandGetAll>> GetStructureType();

        public Task<StructureTypeDTO_forGetandGetAll> GetStructureTypeById(int id);

        public Task StructureTypeUpdate(StructureTypeDTO_forUpdate model);

        public Task<StructureTypeDTO_forGetandGetAll> DeleteStructureTypeById(int id);

        #endregion


        #region Vacancy service

        public Task VacancyCreate(VacancyDTO_forCreate model);

        public Task<List<VacancyDTO_forGetandGetAll>> GetVacancy();

        public Task<VacancyDTO_forGetandGetAll> GetVacancyById(int id);

        public Task VacancyUpdate(VacancyDTO_forUpdate model);

        public Task<VacancyDTO_forGetandGetAll> DeleteVacancyById(int id);

        #endregion


        #region Position service

        public Task PositionCreate(PositionDTO_forCreate model);

        public Task<List<PositionDTO_forGetandGetAll>> GetPosition();

        public Task<PositionDTO_forGetandGetAll> GetPositionById(int id);

        public Task PositionUpdate(PositionDTO_forUpdate model);

        public Task<PositionDTO_forGetandGetAll> DeletePositionById(int id);

        #endregion
    }
}
