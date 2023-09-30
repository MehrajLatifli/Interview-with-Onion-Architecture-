using Interview.Application.Mapper.DTO;

namespace Interview.Application.Services.Admins.Abstract
{
    public interface IAdminService
    {
        public Task SectorCreate(SectorDTO_forCreate model);

        public Task <List<SectorDTO_forGetandGetAll>> GetSector();

        public Task<SectorDTO_forGetandGetAll> GetSectorById(int id);

        public Task SectorUpdate(SectorDTO_forUpdate model);

        public Task<SectorDTO_forGetandGetAll> DeleteSectorById(int id);
    }
}
