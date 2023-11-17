using AutoMapper;
using Interview.Application.Exception;
using Interview.Application.Mapper.DTO;
using Interview.Application.Repositories.Custom;
using Interview.Application.Services.Abstract;
using Interview.Domain.Entities.IdentityAuth;
using Interview.Domain.Entities.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Interview.Application.Services.Concrete
{
    public class SessionServiceManager : ISessionService
    {

        public readonly IMapper _mapper;

        private readonly UserManager<CustomUser> _userManager;

        private readonly ISessionWriteRepository _sessionWriteRepository;
        private readonly ISessionReadRepository _sessionReadRepository;

        private readonly IVacancyWriteRepository _vacancyWriteRepository;
        private readonly IVacancyReadRepository _vacancyReadRepository;

        private readonly ICandidateWriteRepository  _candidateWriteRepository;
        private readonly ICandidateReadRepository  _candidateReadRepository;

        private readonly ISessionQuestionWriteRepository _sessionQuestionWriteRepository;
        private readonly ISessionQuestionReadRepository _sessionQuestionReadRepository;


        private readonly IQuestionWriteRepository _questionWriteRepository;
        private readonly IQuestionReadRepository _questionReadRepository;

        private readonly ILevelWriteRepository _levelWriteRepository;
        private readonly ILevelReadRepository _levelReadRepository;

        public SessionServiceManager(IMapper mapper, ISessionWriteRepository sessionWriteRepository, ISessionReadRepository sessionReadRepository, IVacancyWriteRepository vacancyWriteRepository, IVacancyReadRepository vacancyReadRepository, ICandidateWriteRepository candidateWriteRepository, ICandidateReadRepository candidateReadRepository, ISessionQuestionWriteRepository sessionQuestionWriteRepository, ISessionQuestionReadRepository sessionQuestionReadRepository, IQuestionWriteRepository questionWriteRepository, IQuestionReadRepository questionReadRepository, ILevelWriteRepository levelWriteRepository, ILevelReadRepository levelReadRepository, UserManager<CustomUser> userManager)
        {
            _mapper = mapper;
            _sessionWriteRepository = sessionWriteRepository;
            _sessionReadRepository = sessionReadRepository;
            _vacancyWriteRepository = vacancyWriteRepository;
            _vacancyReadRepository = vacancyReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _sessionQuestionWriteRepository = sessionQuestionWriteRepository;
            _sessionQuestionReadRepository = sessionQuestionReadRepository;
            _questionWriteRepository = questionWriteRepository;
            _questionReadRepository = questionReadRepository;
            _levelWriteRepository = levelWriteRepository;
            _levelReadRepository = levelReadRepository;
            _userManager = userManager;
        }







        #region Session service manager

        public async Task SessionCreate(SessionDTO_forCreate model, ClaimsPrincipal User)
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

            var username = User.Identity.Name;

            var currentUser = await _userManager.FindByNameAsync(username);

            if (currentUser is null)
            {
                throw new NotFoundException("CurrentUser not found");
            }

            entity = new Session
            {

                EndValue = entity.EndValue,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                VacancyId = model.VacancyId,
                CandidateId = model.CandidateId,
                UserAccountId= currentUser.Id,


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




            await Task.Run(() =>
            {
                if (!_mapper.Map<List<SessionDTO_forGetandGetAll>>(_sessionReadRepository.GetAll(false)).Any(i => i.Id == model.Id))
                {
                    throw new NotFoundException("Session not found");
                }
            });


  


            var  sessionQuery = from sq in _mapper.Map<List<SessionQuestionDTO_forGetandGetAll>>(_sessionQuestionReadRepository.GetAll(false))
                            join q in _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false)) on sq.QuestionId equals q.Id
                            join s in _mapper.Map<List<SessionDTO_forGetandGetAll>>(_sessionReadRepository.GetAll(false)) on sq.SessionId equals s.Id
                            join l in _mapper.Map<List<LevelDTO_forGetandGetAll>>(_levelReadRepository.GetAll(false)) on q.LevelId equals l.Id
                            where s.Id == model.Id
                            select new 
                            {
                                SessionId = model.Id,
                                EndValue = l.Coefficient * sq.Value,
                                SessionEndDate = model.EndDate,
                            
                            };



          

            var totalEndValue = sessionQuery.ToList().Sum(session => session.EndValue);

            var entity = _mapper.Map<Session>(model);



            entity = new Session
            {
                Id = model.Id,
                EndValue = totalEndValue,
                StartDate = _mapper.Map<SessionDTO_forGetandGetAll>(await _sessionReadRepository.GetByIdAsync(model.Id.ToString(), false)).StartDate,
                EndDate = model.EndDate,
                VacancyId = _sessionReadRepository.GetAll(false).Where(i => i.Id == model.Id).FirstOrDefault().VacancyId,
                CandidateId = _sessionReadRepository.GetAll(false).Where(i => i.Id == model.Id).FirstOrDefault().CandidateId,
                UserAccountId= _sessionReadRepository.GetAll(false).Where(i => i.Id == model.Id).FirstOrDefault().UserAccountId,


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
