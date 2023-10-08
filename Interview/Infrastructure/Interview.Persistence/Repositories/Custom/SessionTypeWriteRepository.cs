using Interview.Application.Repositories.Custom;
using Interview.Domain.Entities.Models;
using Interview.Persistence.Contexts.InterviewDbContext;
using Interview.Persistence.Repositories.Concrete;

namespace Interview.Persistence.Repositories.Custom
{
    public class SessionTypeWriteRepository : WriteRepository<SessionType>, ISessionTypeWriteRepository
    {
        public SessionTypeWriteRepository(InterviewContext context) : base(context)
        {
        }
    }
}
