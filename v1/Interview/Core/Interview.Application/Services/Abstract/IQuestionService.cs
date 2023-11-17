

using Interview.Application.Mapper.DTO;

namespace Interview.Application.Services.Abstract
{
    public interface IQuestionService
    {

        #region Question service

        public Task QuestionCreate(QuestionDTO_forCreate model);

        public Task<List<QuestionDTO_forGetandGetAll>> GetQuestion();

        public Task<QuestionDTO_forGetandGetAll> GetQuestionById(int id);

        public Task QuestionUpdate(QuestionDTO_forUpdate model);

        public Task<QuestionDTO_forGetandGetAll> DeleteQuestionById(int id);

        #endregion
    }


}
