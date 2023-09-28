using Interview.Application.Mapper.DTO;

namespace Interview.Application.Services.Admins.Abstract
{
    public interface ISectorService
    {
        public Task SectorCreate(SectorDTO_forCreate model);
    }
}
