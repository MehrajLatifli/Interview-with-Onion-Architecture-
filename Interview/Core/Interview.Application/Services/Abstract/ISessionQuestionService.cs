

using Azure.Core;
using Interview.Application.Mapper.DTO;
using Interview.Domain.Entities.Models.Requests;

namespace Interview.Application.Services.Abstract
{
    public interface ISessionQuestionService
    {
        #region SessionQuestion service

        public Task SessionQuestionCreate(SessionQuestionDTO_forCreate model);

        public Task<List<SessionQuestionDTO_forGetandGetAll>> GetSessionQuestion();

        public Task<List<SessionQuestionDTO_forGetandGetAll>> GetSessionQuestionBySessionId(int sessionId);

        public Task<SessionQuestionDTO_forGetandGetAll> GetSessionQuestionById(int id);

        public Task SessionQuestionUpdate(SessionQuestionDTO_forUpdate model);

        public Task<SessionQuestionDTO_forGetandGetAll> DeleteSessionQuestionById(int id);

        public Task<List<QuestionDTO_forGetandGetAll>> GetRandomQuestion(RandomQuestionRequestModel model);
        public Task<List<QuestionDTO_forGetandGetAll>> GetRandomQuestion2(int QuestionCount, int VacantionId, int SessionId);

        public Task<List<QuestionDTO_forGetandGetAll>> GetAllQuestionByPage(QuestionByPageRequestModel questionByPageRequestModel);



        #endregion
    }


}
