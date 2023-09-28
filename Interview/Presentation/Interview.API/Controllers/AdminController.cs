using AutoMapper;
using Interview.Application.Exception;
using Interview.Application.Mapper.DTO;
using Interview.Application.Repositories.Custom;
using Interview.Application.Services.Admins.Abstract;
using Interview.Domain.AuthModels;
using Interview.Domain.Entities.Models;
using Interview.Persistence.Contexts.AuthDbContext.IdentityAuth;
using Interview.Persistence.Repositories.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Interview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;
        List<IdentityError> errorList = new List<IdentityError>();
        private readonly IMapper _mapper;

        private readonly IBranchWriteRepository _branchWriteRepository;
        private readonly IBranchReadRepository _branchReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateQuestionWriteRepository _candidateQuestionWriteRepository;
        private readonly ICandidateQuestionReadRepository _candidateQuestionReadRepository;
        private readonly ICandidateVacancyWriteRepository _candidateVacancyWriteRepository;
        private readonly ICandidateVacancyReadRepository _candidateVacancyReadRepository;
        private readonly IDepartmentWriteRepository _departmentWriteRepository;
        private readonly IDepartmentReadRepository _departmentReadRepository;
        private readonly IJobDegreeWriteRepository _jobDegreeWriteRepository;
        private readonly IJobDegreeReadRepository _jobDegreeReadRepository;
        private readonly IOpenQuestionWriteRepository _openQuestionWriteRepository;
        private readonly IOpenQuestionReadRepository _openQuestionReadRepository;
        private readonly IQuestionWriteRepository _questionWriteRepository;
        private readonly IQuestionReadRepository _questionReadRepository;
        private readonly IQuestionCategoryWriteRepository _questionCategoryWriteRepository;
        private readonly IQuestionCategoryReadRepository _questionCategoryReadRepository;
        private readonly IQuestionLevelWriteRepository _questionLevelWriteRepository;
        private readonly IQuestionLevelReadRepository _questionLevelReadRepository;
        private readonly IQuestionValueWriteRepository _questionValueWriteRepository;
        private readonly IQuestionValueReadRepository _questionValueReadRepository;
        private readonly ISectorWriteRepository _sectorWriteRepository;
        private readonly ISectorReadRepository _sectorReadRepository;
        private readonly IVacancyWriteRepository _vacancyWriteRepository;
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly ISectorService _sectorService;

        public AdminController(

            UserManager<CustomUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            SignInManager<CustomUser> signInManager,

            IBranchWriteRepository branchWriteRepository,
            IBranchReadRepository branchReadRepository,
            ICandidateWriteRepository candidateWriteRepository,
            ICandidateReadRepository candidateReadRepository,
            ICandidateQuestionWriteRepository candidateQuestionWriteRepository,
            ICandidateQuestionReadRepository candidateQuestionReadRepository,
            ICandidateVacancyWriteRepository candidateVacancyWriteRepository,
            ICandidateVacancyReadRepository candidateVacancyReadRepository,
            IDepartmentWriteRepository departmentWriteRepository,
            IDepartmentReadRepository departmentReadRepository,
            IJobDegreeWriteRepository jobDegreeWriteRepository,
            IJobDegreeReadRepository jobDegreeReadRepository,
            IOpenQuestionWriteRepository openQuestionWriteRepository,
            IOpenQuestionReadRepository openQuestionReadRepository,
            IQuestionWriteRepository questionWriteRepository,
            IQuestionReadRepository questionReadRepository,
            IQuestionCategoryWriteRepository questionCategoryWriteRepository,
            IQuestionCategoryReadRepository questionCategoryReadRepository,
            IQuestionLevelWriteRepository questionLevelWriteRepository,
            IQuestionLevelReadRepository questionLevelReadRepository,
            IQuestionValueWriteRepository questionValueWriteRepository,
            IQuestionValueReadRepository questionValueReadRepository,
            ISectorWriteRepository sectorWriteRepository,
            ISectorReadRepository sectorReadRepository,
            IVacancyWriteRepository vacancyWriteRepository,
            IVacancyReadRepository vacancyReadRepository,
            IMapper mapper,
            ISectorService sectorService)
        {
            _sectorService = sectorService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;

            _branchWriteRepository = branchWriteRepository;
            _branchReadRepository = branchReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _candidateQuestionWriteRepository = candidateQuestionWriteRepository;
            _candidateQuestionReadRepository = candidateQuestionReadRepository;
            _candidateVacancyWriteRepository = candidateVacancyWriteRepository;
            _candidateVacancyReadRepository = candidateVacancyReadRepository;
            _departmentWriteRepository = departmentWriteRepository;
            _departmentReadRepository = departmentReadRepository;
            _jobDegreeWriteRepository = jobDegreeWriteRepository;
            _jobDegreeReadRepository = jobDegreeReadRepository;
            _openQuestionWriteRepository = openQuestionWriteRepository;
            _openQuestionReadRepository = openQuestionReadRepository;
            _questionWriteRepository = questionWriteRepository;
            _questionReadRepository = questionReadRepository;
            _questionCategoryWriteRepository = questionCategoryWriteRepository;
            _questionCategoryReadRepository = questionCategoryReadRepository;
            _questionLevelWriteRepository = questionLevelWriteRepository;
            _questionLevelReadRepository = questionLevelReadRepository;
            _questionValueWriteRepository = questionValueWriteRepository;
            _questionValueReadRepository = questionValueReadRepository;
            _sectorWriteRepository = sectorWriteRepository;
            _sectorReadRepository = sectorReadRepository;
            _vacancyWriteRepository = vacancyWriteRepository;
            _vacancyReadRepository = vacancyReadRepository;
            _mapper = mapper;
        }



        [HttpGet("getAdmins")]
        public async Task<IActionResult> GetAdmins()
        {

            var list = new List<GetAuthModel>();


            foreach (var user in _userManager.Users.ToList().Where(i => i.Roles == UserRoles.Admin))
            {

                if (user != null)
                {


                    list.Add(new GetAuthModel()
                    {
                        Username = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        ImagePath = user.ImagePath,
                        Roles = user.Roles,



                    });
                }

                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found" });
                }

            }
            if (list.Any())
            {

                return Ok(list);
            }

            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Nothing found" });
            }

        }


        [HttpGet("getHR")]
        public async Task<IActionResult> GetHR()
        {

            var list = new List<GetAuthModel>();


            foreach (var user in _userManager.Users.ToList().Where(i => i.Roles == UserRoles.HR))
            {

                if (user != null)
                {


                    list.Add(new GetAuthModel()
                    {
                        Username = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        ImagePath = user.ImagePath,
                        Roles = user.Roles,



                    });
                }

                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found" });
                }

            }
            if (list.Any())
            {

                return Ok(list);
            }

            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Nothing found" });
            }

        }

        #region Sector


        [HttpGet("sector/{id}")]
        public async Task<IActionResult> GetSectorById(string id)
        {


            try
            {


                var item = _mapper.Map<SectorDTO_forGetandGetAll>(await _sectorReadRepository.GetByIdAsync(id, false));

                if (item == null)
                {
                    return NotFound(new Response { Status = "Error", Message = "Not Found!" });
                }

                return Ok(item);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }

        }


        [HttpGet("sector")]
        public async Task<IActionResult> GetSector()
        {

            List<SectorDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {


                datas = _mapper.Map<List<SectorDTO_forGetandGetAll>>(_sectorReadRepository.GetAll(false));
            });



            if (datas.Count() <= 0)
            {
                return NotFound(new Response { Status = "Error", Message = "Not Found!" });
            }


            return Ok(datas);

        }


        [HttpPost]
        [Route("sector")]
        public async Task<IActionResult> SectorCreate([FromBody] SectorDTO_forCreate model)
        {

            await _sectorService.SectorCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Sector created successfully!" });

        }


        [HttpPut]
        [Route("sector")]
        public async Task<IActionResult> SectorUpdate([FromBody] SectorDTO_forUpdate model)
        {


            var existing = await _sectorReadRepository.GetByIdAsync(model.Id.ToString(), false);


            if (existing is null)
                throw new NotFoundException("eXCEPTION");




            var update = new Sector
            {
                Id = model.Id,
                SectorName = model.SectorName,

            };

            _sectorWriteRepository.Update(update);
            await _sectorWriteRepository.SaveAsync();


            return Ok(new Response { Status = "Success", Message = "The Sector updated successfully!" });




        }


        [HttpDelete("sector/{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            if (_sectorReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _sectorWriteRepository.RemoveByIdAsync(id.ToString());
                await _sectorWriteRepository.SaveAsync();

                return Ok(new Response { Status = "Success", Message = "The Sector deleted successfully!" });
            }

            else
            {
                return BadRequest(new Response { Status = "Error", Message = "The sector could not be deleted !" });
            }

        }

        #endregion

    }
}


