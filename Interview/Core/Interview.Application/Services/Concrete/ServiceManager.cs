using AutoMapper;
using Interview.Application.Exception;
using Interview.Application.Mapper.DTO;
using Interview.Application.Repositories.Custom;
using Interview.Application.Services.Abstract;
using Interview.Domain.Entities.Models;

namespace Interview.Application.Services.Concrete
{
    public class ServiceManager : IService
    {

        public readonly IMapper _mapper;

        private readonly ISectorWriteRepository _sectorWriteRepository;
        private readonly ISectorReadRepository _sectorReadRepository;
        private readonly IBranchWriteRepository _branchWriteRepository;
        private readonly IBranchReadRepository  _branchReadRepository;
        private readonly IDepartmentWriteRepository _departmentWriteRepository;
        private readonly IDepartmentReadRepository _departmentReadRepository;
        private readonly IVacancyWriteRepository _vacancyWriteRepository;
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IJobDegreeWriteRepository _jobDegreeWriteRepository;
        private readonly IJobDegreeReadRepository _jobDegreeReadRepository;

        public ServiceManager(IMapper mapper, ISectorWriteRepository sectorWriteRepository, ISectorReadRepository sectorReadRepository, IBranchWriteRepository branchWriteRepository, IBranchReadRepository branchReadRepository, IDepartmentWriteRepository departmentWriteRepository, IDepartmentReadRepository departmentReadRepository, IVacancyWriteRepository vacancyWriteRepository, IVacancyReadRepository vacancyReadRepository, IJobDegreeWriteRepository jobDegreeWriteRepository, IJobDegreeReadRepository jobDegreeReadRepository)
        {
            _mapper = mapper;
            _sectorWriteRepository = sectorWriteRepository;
            _sectorReadRepository = sectorReadRepository;
            _branchWriteRepository = branchWriteRepository;
            _branchReadRepository = branchReadRepository;
            _departmentWriteRepository = departmentWriteRepository;
            _departmentReadRepository = departmentReadRepository;
            _vacancyWriteRepository = vacancyWriteRepository;
            _vacancyReadRepository = vacancyReadRepository;
            _jobDegreeWriteRepository = jobDegreeWriteRepository;
            _jobDegreeReadRepository = jobDegreeReadRepository;
        }








        #region Sector service manager

        public async Task SectorCreate(SectorDTO_forCreate model)
        {

            var entity = _mapper.Map<Sector>(model);

            await _sectorWriteRepository.AddAsync(entity);

            await _sectorWriteRepository.SaveAsync();
        }

        public async Task<List<SectorDTO_forGetandGetAll>> GetSector()
        {
            List<SectorDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<SectorDTO_forGetandGetAll>>(_sectorReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("Sector not found");
            }

            return datas;
        }

        public async Task<SectorDTO_forGetandGetAll> GetSectorById(int id)
        {
            SectorDTO_forGetandGetAll item = null;


            item = _mapper.Map<SectorDTO_forGetandGetAll>(await _sectorReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("Sector not found");
            }

            return item;
        }

        public async Task SectorUpdate(SectorDTO_forUpdate model)
        {

            var existing = await _sectorReadRepository.GetByIdAsync(model.Id.ToString(), false);


            if (existing is null)
            {
                throw new NotFoundException("Sector not found");

            }


            var update = new Sector
            {
                Id = model.Id,
                SectorName = model.SectorName,

            };

            _sectorWriteRepository.Update(update);
            await _sectorWriteRepository.SaveAsync();

        }

        public async Task<SectorDTO_forGetandGetAll> DeleteSectorById(int id)
        {

            if (_sectorReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _sectorWriteRepository.RemoveByIdAsync(id.ToString());
                await _sectorWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("Sector not found");
            }
        }

        #endregion


        #region Branch service manager

        public async Task BranchCreate(BranchDTO_forCreate model)
        {

            var existing = _mapper.Map<SectorDTO_forGetandGetAll>(await _sectorReadRepository.GetByIdAsync(model.SectorId.ToString(), false));

            if (existing is null)
            {
                throw new NotFoundException("Sector not found");
            }

            else
            {
                var entity = _mapper.Map<Branch>(model);


                await _branchWriteRepository.AddAsync(entity);

                await _branchWriteRepository.SaveAsync();
            }
        }

        public async Task<List<BranchDTO_forGetandGetAll>> GetBranch()
        {
            List<BranchDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<BranchDTO_forGetandGetAll>>(_branchReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("Branch not found");
            }

            return datas;
        }

        public async Task<BranchDTO_forGetandGetAll> GetBranchById(int id)
        {
            BranchDTO_forGetandGetAll item = null;


            item = _mapper.Map<BranchDTO_forGetandGetAll>(await _branchReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("Branch not found");
            }

            return item;
        }

        public async Task BranchUpdate(BranchDTO_forUpdate model)
        {
            var existing = await _branchReadRepository.GetByIdAsync(model.Id.ToString(), false);

            var existing2 = _mapper.Map<SectorDTO_forGetandGetAll>(await _sectorReadRepository.GetByIdAsync(model.SectorId.ToString(), false));


            if (existing is null)
            {
                throw new NotFoundException("Branch not found");

            }

            if (existing2 is null)
            {
                throw new NotFoundException("Sector not found");
            }

            var update = new Branch
            {
                Id = model.Id,
                BranchName = model.BranchName,
                SectorId=model.SectorId,

            };

            _branchWriteRepository.Update(update);
            await _branchWriteRepository.SaveAsync();
        }

        public async Task<BranchDTO_forGetandGetAll> DeleteBranchById(int id)
        {
            if (_branchReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _branchWriteRepository.RemoveByIdAsync(id.ToString());
                await _branchWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("Sector not found");
            }
        }

        #endregion


        #region Department service manager

        public async Task DepartmentCreate(DepartmentDTO_forCreate model)
        {

            var existing = _mapper.Map<BranchDTO_forGetandGetAll>(await _branchReadRepository.GetByIdAsync(model.BranchId.ToString(), false));

            if (existing is null)
            {
                throw new NotFoundException("Branch not found");
            }

            else
            {
                var entity = _mapper.Map<Department>(model);


                await _departmentWriteRepository.AddAsync(entity);

                await _departmentWriteRepository.SaveAsync();
            }
        }

        public async Task<List<DepartmentDTO_forGetandGetAll>> GetDepartment()
        {
            List<DepartmentDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<DepartmentDTO_forGetandGetAll>>(_departmentReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("Department not found");
            }

            return datas;
        }

        public async Task<DepartmentDTO_forGetandGetAll> GetDepartmentById(int id)
        {
            DepartmentDTO_forGetandGetAll item = null;


            item = _mapper.Map<DepartmentDTO_forGetandGetAll>(await _departmentReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("Department not found");
            }

            return item;
        }

        public async Task DepartmentUpdate(DepartmentDTO_forUpdate model)
        {
            var existing = await _departmentReadRepository.GetByIdAsync(model.Id.ToString(), false);

            var existing2 = _mapper.Map<BranchDTO_forGetandGetAll>(await _branchReadRepository.GetByIdAsync(model.BranchId.ToString(), false));


            if (existing is null)
            {
                throw new NotFoundException("Department not found");

            }

            if (existing2 is null)
            {
                throw new NotFoundException("Branch not found");
            }

            var update = new Department
            {
                Id = model.Id,
                DepartmentName = model.DepartmentName,
                BranchId=model.BranchId,

            };

            _departmentWriteRepository.Update(update);
            await _departmentWriteRepository.SaveAsync();
        }

        public async Task<DepartmentDTO_forGetandGetAll> DeleteDepartmentById(int id)
        {
            if (_departmentReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _departmentWriteRepository.RemoveByIdAsync(id.ToString());
                await _departmentWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("Sector not found");
            }
        }

        #endregion


        #region JobDegree service manager

        public async Task JobDegreeCreate(JobDegreeDTO_forCreate model)
        {

            var entity = _mapper.Map<JobDegree>(model);

            await _jobDegreeWriteRepository.AddAsync(entity);

            await _jobDegreeWriteRepository.SaveAsync();
        }

        public async Task<List<JobDegreeDTO_forGetandGetAll>> GetJobDegree()
        {
            List<JobDegreeDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<JobDegreeDTO_forGetandGetAll>>(_jobDegreeReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("JobDegree not found");
            }

            return datas;
        }

        public async Task<JobDegreeDTO_forGetandGetAll> GetJobDegreeById(int id)
        {
            JobDegreeDTO_forGetandGetAll item = null;


            item = _mapper.Map<JobDegreeDTO_forGetandGetAll>(await _jobDegreeReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("JobDegree not found");
            }

            return item;
        }

        public async Task JobDegreeUpdate(JobDegreeDTO_forUpdate model)
        {

            var existing = await _jobDegreeReadRepository.GetByIdAsync(model.Id.ToString(), false);


            if (existing is null)
            {
                throw new NotFoundException("JobDegree not found");

            }


            var update = new JobDegree
            {
                Id = model.Id,
                Degree = model.Degree,

            };

            _jobDegreeWriteRepository.Update(update);
            await _jobDegreeWriteRepository.SaveAsync();

        }

        public async Task<JobDegreeDTO_forGetandGetAll> DeleteJobDegreeById(int id)
        {

            if (_jobDegreeReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _jobDegreeWriteRepository.RemoveByIdAsync(id.ToString());
                await _jobDegreeWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("JobDegree not found");
            }
        }

        #endregion


        #region Vacancy service manager

        public async Task VacancyCreate(VacancyDTO_forCreate model)
        {

            var existing = _mapper.Map<JobDegreeDTO_forGetandGetAll>(await _jobDegreeReadRepository.GetByIdAsync(model.JobDegreeId.ToString(), false));

            var existing2 = _mapper.Map<SectorDTO_forGetandGetAll>(await _sectorReadRepository.GetByIdAsync(model.SectorId.ToString(), false));

            if (existing is null)
            {
                throw new NotFoundException("JobDegree not found");
            }

            if (existing2 is null)
            {
                throw new NotFoundException("Sector not found");
            }

            else
            {
                var entity = _mapper.Map<Vacancy>(model);


                await _vacancyWriteRepository.AddAsync(entity);

                await _vacancyWriteRepository.SaveAsync();
            }
        }

        public async Task<List<VacancyDTO_forGetandGetAll>> GetVacancy()
        {
            List<VacancyDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<VacancyDTO_forGetandGetAll>>(_vacancyReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("Vacancy not found");
            }

            return datas;
        }

        public async Task<VacancyDTO_forGetandGetAll> GetVacancyById(int id)
        {
            VacancyDTO_forGetandGetAll item = null;


            item = _mapper.Map<VacancyDTO_forGetandGetAll>(await _vacancyReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("Vacancy not found");
            }

            return item;
        }

        public async Task VacancyUpdate(VacancyDTO_forUpdate model)
        {
            var existing = await _vacancyReadRepository.GetByIdAsync(model.Id.ToString(), false);

            var existing2 = _mapper.Map<JobDegreeDTO_forGetandGetAll>(await _jobDegreeReadRepository.GetByIdAsync(model.JobDegreeId.ToString(), false));

            var existing3 = _mapper.Map<SectorDTO_forGetandGetAll>(await _sectorReadRepository.GetByIdAsync(model.SectorId.ToString(), false));


            if (existing is null)
            {
                throw new NotFoundException("Vacancy not found");

            }

            if (existing2 is null)
            {
                throw new NotFoundException("Job Degree not found");
            }

            if (existing3 is null)
            {
                throw new NotFoundException("Sector not found");
            }

            var update = new Vacancy
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                JobDegreeId=model.JobDegreeId,
                SectorId=model.SectorId,

            };

            _vacancyWriteRepository.Update(update);
            await _vacancyWriteRepository.SaveAsync();
        }

        public async Task<VacancyDTO_forGetandGetAll> DeleteVacancyById(int id)
        {
            if (_vacancyReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _vacancyWriteRepository.RemoveByIdAsync(id.ToString());
                await _vacancyWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("Sector not found");
            }
        }

        #endregion
    }
}
