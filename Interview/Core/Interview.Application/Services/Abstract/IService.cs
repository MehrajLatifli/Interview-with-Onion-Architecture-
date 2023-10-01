using Interview.Application.Mapper.DTO;

namespace Interview.Application.Services.Abstract
{
    public interface IService
    {
        #region Sector service

        public Task SectorCreate(SectorDTO_forCreate model);

        public Task<List<SectorDTO_forGetandGetAll>> GetSector();

        public Task<SectorDTO_forGetandGetAll> GetSectorById(int id);

        public Task SectorUpdate(SectorDTO_forUpdate model);

        public Task<SectorDTO_forGetandGetAll> DeleteSectorById(int id);

        #endregion

        #region Branch service

        public Task BranchCreate(BranchDTO_forCreate model);

        public Task<List<BranchDTO_forGetandGetAll>> GetBranch();

        public Task<BranchDTO_forGetandGetAll> GetBranchById(int id);

        public Task BranchUpdate(BranchDTO_forUpdate model);

        public Task<BranchDTO_forGetandGetAll> DeleteBranchById(int id);

        #endregion

        #region Department service

        public Task DepartmentCreate(DepartmentDTO_forCreate model);

        public Task<List<DepartmentDTO_forGetandGetAll>> GetDepartment();

        public Task<DepartmentDTO_forGetandGetAll> GetDepartmentById(int id);

        public Task DepartmentUpdate(DepartmentDTO_forUpdate model);

        public Task<DepartmentDTO_forGetandGetAll> DeleteDepartmentById(int id);

        #endregion
    }
}
