

using Interview.Application.Mapper.AuthDTO;
using Interview.Application.Mapper.DTO;
using Interview.Domain.Entities.AuthModels;

namespace Interview.Application.Services.Abstract
{
    public interface IVacancyService
    {

        #region Vacancy service

        public Task VacancyCreate(VacancyDTO_forCreate model);

        public Task<List<VacancyDTO_forGetandGetAll>> GetVacancy();

        public Task<VacancyDTO_forGetandGetAll> GetVacancyById(int id);

        public Task VacancyUpdate(VacancyDTO_forUpdate model);

        public Task<VacancyDTO_forGetandGetAll> DeleteVacancyById(int id);

        #endregion
    }


}
