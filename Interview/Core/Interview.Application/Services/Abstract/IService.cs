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


        #region JobDegree service

        public Task JobDegreeCreate(JobDegreeDTO_forCreate model);

        public Task<List<JobDegreeDTO_forGetandGetAll>> GetJobDegree();

        public Task<JobDegreeDTO_forGetandGetAll> GetJobDegreeById(int id);

        public Task JobDegreeUpdate(JobDegreeDTO_forUpdate model);

        public Task<JobDegreeDTO_forGetandGetAll> DeleteJobDegreeById(int id);

        #endregion


        #region Vacancy service

        public Task VacancyCreate(VacancyDTO_forCreate model);

        public Task<List<VacancyDTO_forGetandGetAll>> GetVacancy();

        public Task<VacancyDTO_forGetandGetAll> GetVacancyById(int id);

        public Task VacancyUpdate(VacancyDTO_forUpdate model);

        public Task<VacancyDTO_forGetandGetAll> DeleteVacancyById(int id);

        #endregion
    }
}
