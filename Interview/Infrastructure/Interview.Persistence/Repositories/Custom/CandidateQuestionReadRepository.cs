
using Interview.Application.Repositories.Custom;
using Interview.Domain.Entities.Models;
using Interview.Persistence.Contexts.InterviewDbContext;
using Interview.Persistence.Repositories.Concrete;


namespace Interview.Persistence.Repositories.Custom
{
    public class CandidateQuestionReadRepository : ReadRepository<CandidateQuestion>, ICandidateQuestionReadRepository
    {
        public CandidateQuestionReadRepository(InterviewContext context) : base(context)
        {
        }
    }
}
