using AutoMapper;
using Interview.Application.Exception;
using Interview.Application.Mapper.DTO;
using Interview.Application.Repositories.Custom;
using Interview.Application.Services.Abstract;
using Interview.Domain.Entities.Models;

namespace Interview.Application.Services.Concrete
{
    public class SessionServiceManager : ISessionService
    {

        public readonly IMapper _mapper;

      

        private readonly ISessionWriteRepository _sessionWriteRepository;
        private readonly ISessionReadRepository _sessionReadRepository;

        private readonly IVacancyWriteRepository _vacancyWriteRepository;
        private readonly IVacancyReadRepository _vacancyReadRepository;

        private readonly ICandidateWriteRepository  _candidateWriteRepository;
        private readonly ICandidateReadRepository  _candidateReadRepository;

        public SessionServiceManager(IMapper mapper, ISessionWriteRepository sessionWriteRepository, ISessionReadRepository sessionReadRepository, IVacancyWriteRepository vacancyWriteRepository, IVacancyReadRepository vacancyReadRepository, ICandidateWriteRepository candidateWriteRepository, ICandidateReadRepository candidateReadRepository)
        {
            _mapper = mapper;
            _sessionWriteRepository = sessionWriteRepository;
            _sessionReadRepository = sessionReadRepository;
            _vacancyWriteRepository = vacancyWriteRepository;
            _vacancyReadRepository = vacancyReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _candidateReadRepository = candidateReadRepository;
        }



        #region Session service manager

        public async Task SessionCreate(SessionDTO_forCreate model)
        {



            var entity = _mapper.Map<Session>(model);


            var existing1 = await _vacancyReadRepository.GetByIdAsync(model.VacancyId.ToString(), false);
            var existing2 = await _candidateReadRepository.GetByIdAsync(model.CandidateId.ToString(), false);

            if (existing1 is null)
            {
                throw new NotFoundException("Vacancy not found");

            }

            if (existing2 is null)
            {
                throw new NotFoundException("Candidate not found");

            }

            entity = new Session
            {
                EndValue = entity.EndValue,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                VacancyId = model.VacancyId,
                CandidateId = model.CandidateId,


            };


            await _sessionWriteRepository.AddAsync(entity);

            await _sessionWriteRepository.SaveAsync();
        }

        public async Task<List<SessionDTO_forGetandGetAll>> GetSession()
        {
            List<SessionDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<SessionDTO_forGetandGetAll>>(_sessionReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("Session not found");
            }

            return datas;
        }

        public async Task<SessionDTO_forGetandGetAll> GetSessionById(int id)
        {
            SessionDTO_forGetandGetAll item = null;


            item = _mapper.Map<SessionDTO_forGetandGetAll>(await _sessionReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("Session not found");
            }

            return item;
        }

        public async Task SessionUpdate(SessionDTO_forUpdate model)
        {


            var existing = await _sessionReadRepository.GetByIdAsync(model.Id.ToString(), false);
            var existing2 = await _vacancyReadRepository.GetByIdAsync(model.VacancyId.ToString(), false);
            var existing3 = await _candidateReadRepository.GetByIdAsync(model.CandidateId.ToString(), false);




            if (existing is null)
            {
                throw new NotFoundException("Session not found");

            }

            if (existing2 is null)
            {
                throw new NotFoundException("Vacancy not found");

            }

            if (existing3 is null)
            {
                throw new NotFoundException("Candidate not found");

            }




            var entity = _mapper.Map<Session>(model);



            entity = new Session
            {
                Id = model.Id,
                EndValue = entity.EndValue,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                VacancyId = model.VacancyId,
                CandidateId = model.CandidateId,


            };

            _sessionWriteRepository.Update(entity);
            await _sessionWriteRepository.SaveAsync();

        }

        public async Task<SessionDTO_forGetandGetAll> DeleteSessionById(int id)
        {

            if (_sessionReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _sessionWriteRepository.RemoveByIdAsync(id.ToString());
                await _sessionWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("Session not found");
            }
        }



        #endregion


    }


}
