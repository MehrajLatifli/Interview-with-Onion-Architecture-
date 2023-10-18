

using Interview.Application.Mapper.DTO;

namespace Interview.Application.Services.Abstract
{
    public interface ISessionQuestionService
    {
        #region SessionQuestion service

        public Task SessionQuestionCreate(SessionQuestionDTO_forCreate model);

        public Task<List<SessionQuestionDTO_forGetandGetAll>> GetSessionQuestion();

        public Task<SessionQuestionDTO_forGetandGetAll> GetSessionQuestionById(int id);

        public Task SessionQuestionUpdate(SessionQuestionDTO_forUpdate model);

        public Task<SessionQuestionDTO_forGetandGetAll> DeleteSessionQuestionById(int id);

        public Task<List<QuestionDTO_forGetandGetAll>> GetRandomQuestion(int questionCount, int structureId, int positionId, int vacantionId, int sessionId);

        #endregion
    }


}
