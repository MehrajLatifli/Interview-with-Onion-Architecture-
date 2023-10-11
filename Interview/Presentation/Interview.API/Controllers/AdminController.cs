using AutoMapper;
using Interview.API.API_Routes;
using Interview.Application.Exception;
using Interview.Application.Mapper.DTO;
using Interview.Application.Services.Abstract;
using Interview.Domain.Entities.AuthModels;
using Interview.Domain.Entities.Models;
using Interview.Persistence.Contexts.AuthDbContext.IdentityAuth;
using Interview.Persistence.ServiceExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Interview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "AdminOnly")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;

        private readonly IService _service;

        public AdminController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<CustomUser> signInManager, IService service)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _service = service;
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


        #region CandidateDocument


        [HttpGet(Routes.CandidateDocumentById)]
        public async Task<IActionResult> GetCandidateDocumentById(int id)
        {

            var data = await _service.GetCandidateDocumentById(id);

            return Ok(data);

        }


        [HttpGet(Routes.CandidateDocument)]
        public async Task<IActionResult> GetCandidateDocument()
        {

            var data = await _service.GetCandidateDocument();


            return Ok(data);

        }


        [HttpPost]
        [Route(Routes.CandidateDocument)]
        public async Task<IActionResult> CandidateDocumentCreate([FromForm] CandidateDocumentDTO_forCreate model)
        {

            await _service.CandidateDocumentCreate(model, ServiceExtension.ConnectionStringAzure);

            return Ok(new Response { Status = "Success", Message = "The CandidateDocument created successfully!" });

        }


        [HttpPut]
        [Route(Routes.CandidateDocument)]
        public async Task<IActionResult> CandidateDocumentUpdate([FromForm] CandidateDocumentDTO_forUpdate model)
        {


            await _service.CandidateDocumentUpdate(model, ServiceExtension.ConnectionStringAzure);


            return Ok(new Response { Status = "Success", Message = "The CandidateDocument updated successfully!" });




        }


        [HttpDelete(Routes.CandidateDocumentById)]
        public async Task<IActionResult> CandidateDocumentDelete(int id)
        {

            await _service.DeleteCandidateDocumentById(id);

            return Ok(new Response { Status = "Success", Message = "The CandidateDocument deleted successfully!" });
        }


        #endregion


        #region Candidate


        [HttpGet(Routes.CandidateById)]
        public async Task<IActionResult> GetCandidateById(int id)
        {

            var data = await _service.GetCandidateById(id);

            return Ok(data);

        }


        [HttpGet(Routes.Candidate)]
        public async Task<IActionResult> GetCandidate()
        {

            var data = await _service.GetCandidate();


            return Ok(data);

        }


        [HttpPost(Routes.Candidate)]
        public async Task<IActionResult> CandidateCreate([FromForm] CandidateDTO_forCreate model)
        {

            await _service.CandidateCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Candidate created successfully!" });

        }


        [HttpPut(Routes.Candidate)]
     
        public async Task<IActionResult> CandidateUpdate([FromForm] CandidateDTO_forUpdate model)
        {


            await _service.CandidateUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Candidate updated successfully!" });




        }


        [HttpDelete(Routes.CandidateById)]
        public async Task<IActionResult> CandidateDelete(int id)
        {

            await _service.DeleteCandidateById(id);

            return Ok(new Response { Status = "Success", Message = "The Candidate deleted successfully!" });
        }


        #endregion


        #region Level


        [HttpGet(Routes.LevelById)]
        public async Task<IActionResult> GetLevelById(int id)
        {

            var data = await _service.GetLevelById(id);

            return Ok(data);

        }


        [HttpGet(Routes.Level)]
        public async Task<IActionResult> GetLevel()
        {

            var data = await _service.GetLevel();


            return Ok(data);

        }


        [HttpPost(Routes.Level)]
  
        public async Task<IActionResult> LevelCreate([FromForm] LevelDTO_forCreate model)
        {

            await _service.LevelCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Level created successfully!" });

        }


        [HttpPut(Routes.Level)]
        public async Task<IActionResult> LevelUpdate([FromForm] LevelDTO_forUpdate model)
        {


            await _service.LevelUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Level updated successfully!" });




        }


        [HttpDelete(Routes.LevelById)]
        public async Task<IActionResult> LevelDelete(int id)
        {

            await _service.DeleteLevelById(id);

            return Ok(new Response { Status = "Success", Message = "The Level deleted successfully!" });
        }


        #endregion


        #region Category


        [HttpGet(Routes.CategoryById)]
        public async Task<IActionResult> GetCategoryById(int id)
        {

            var data = await _service.GetCategoryById(id);

            return Ok(data);

        }


        [HttpGet(Routes.Category)]
        public async Task<IActionResult> GetCategory()
        {

            var data = await _service.GetCategory();


            return Ok(data);

        }


        [HttpPost(Routes.Category)]
        public async Task<IActionResult> CategoryCreate([FromForm] CategoryDTO_forCreate model)
        {

            await _service.CategoryCreate(model);

            return Ok(new Response { Status = "Success", Message = "The SessionType created successfully!" });

        }


        [HttpPut(Routes.Category)]
        public async Task<IActionResult> CategoryUpdate([FromForm] CategoryDTO_forUpdate model)
        {


            await _service.CategoryUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The SessionType updated successfully!" });




        }


        [HttpDelete(Routes.CategoryById)]
        public async Task<IActionResult> DeleteCategoryById(int id)
        {

            await _service.DeleteCategoryById(id);

            return Ok(new Response { Status = "Success", Message = "The SessionType deleted successfully!" });
        }


        #endregion


        #region StructureType


        [HttpGet(Routes.StructureTypeById)]
        public async Task<IActionResult> GetStructureTypeById(int id)
        {

            var data = await _service.GetStructureTypeById(id);

            return Ok(data);

        }


        [HttpGet(Routes.StructureType)]
        public async Task<IActionResult> GetStructureType()
        {

            var data = await _service.GetStructureType();


            return Ok(data);

        }


        [HttpPost]
        [Route(Routes.StructureType)]
        public async Task<IActionResult> StructureTypeCreate([FromForm] StructureTypeDTO_forCreate model)
        {

            await _service.StructureTypeCreate(model);

            return Ok(new Response { Status = "Success", Message = "The StructureType created successfully!" });

        }


        [HttpPut]
        [Route(Routes.StructureType)]
        public async Task<IActionResult> StructureTypeUpdate([FromForm] StructureTypeDTO_forUpdate model)
        {


            await _service.StructureTypeUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The StructureType updated successfully!" });




        }


        [HttpDelete(Routes.StructureTypeById)]
        public async Task<IActionResult> StructureTypeDelete(int id)
        {

            await _service.DeleteStructureTypeById(id);

            return Ok(new Response { Status = "Success", Message = "The StructureType deleted successfully!" });
        }


        #endregion


        #region Structure


        [HttpGet(Routes.StructureById)]
        public async Task<IActionResult> GetStructureById(int id)
        {

            var data = await _service.GetStructureById(id);

            return Ok(data);

        }


        [HttpGet(Routes.Structure)]
        public async Task<IActionResult> GetStructure()
        {

            var data = await _service.GetStructure();


            return Ok(data);

        }


        [HttpPost(Routes.Structure)]
        public async Task<IActionResult> StructureCreate([FromForm] StructureDTO_forCreate model)
        {

            await _service.StructureCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Structure created successfully!" });

        }


        [HttpPut(Routes.Structure)]
        public async Task<IActionResult> StructureUpdate([FromForm] StructureDTO_forUpdate model)
        {


            await _service.StructureUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Structure updated successfully!" });




        }


        [HttpDelete(Routes.StructureById)]
        public async Task<IActionResult> StructureDelete(int id)
        {

            await _service.DeleteStructureById(id);

            return Ok(new Response { Status = "Success", Message = "The Structure deleted successfully!" });
        }


        #endregion


        #region Position

        [HttpGet(Routes.PositionById)]
        public async Task<IActionResult> GetPositionById(int id)
        {

            var data = await _service.GetPositionById(id);

            return Ok(data);

        }


        [HttpGet(Routes.Position)]
        public async Task<IActionResult> GetPosition()
        {

            var data = await _service.GetPosition();


            return Ok(data);

        }


        [HttpPost(Routes.Position)]
        public async Task<IActionResult> PositionCreate([FromForm] PositionDTO_forCreate model)
        {

            await _service.PositionCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Position created successfully!" });

        }


        [HttpPut(Routes.Position)]
        public async Task<IActionResult> PositionUpdate([FromForm] PositionDTO_forUpdate model)
        {


            await _service.PositionUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Position updated successfully!" });




        }


        [HttpDelete(Routes.PositionById)]
        public async Task<IActionResult> PositionDelete(int id)
        {

            await _service.DeletePositionById(id);

            return Ok(new Response { Status = "Success", Message = "The Position deleted successfully!" });
        }


        #endregion        


        #region Vacancy


        [HttpGet(Routes.VacancyById)]
        public async Task<IActionResult> GetVacancyById(int id)
        {

            var data = await _service.GetVacancyById(id);

            return Ok(data);

        }


        [HttpGet(Routes.Vacancy)]
        public async Task<IActionResult> GetVacancy()
        {

            var data = await _service.GetVacancy();


            return Ok(data);

        }


        [HttpPost(Routes.Vacancy)]
        public async Task<IActionResult> VacancyCreate([FromForm] VacancyDTO_forCreate model)
        {

            await _service.VacancyCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Vacancy created successfully!" });

        }


        [HttpPut(Routes.Vacancy)]
        public async Task<IActionResult> VacancyUpdate([FromForm] VacancyDTO_forUpdate model)
        {


            await _service.VacancyUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Vacancy updated successfully!" });




        }


        [HttpDelete(Routes.VacancyById)]
        public async Task<IActionResult> VacancyDelete(int id)
        {

            await _service.DeleteVacancyById(id);

            return Ok(new Response { Status = "Success", Message = "The Vacancy deleted successfully!" });
        }


        #endregion


        #region Session


        [HttpGet(Routes.SessionById)]
        public async Task<IActionResult> GetSessionById(int id)
        {

            var data = await _service.GetSessionById(id);

            return Ok(data);

        }


        [HttpGet(Routes.Session)]
        public async Task<IActionResult> GetSession()
        {

            var data = await _service.GetSession();


            return Ok(data);

        }


        [HttpPost(Routes.Session)]
        public async Task<IActionResult> SessionCreate([FromForm] SessionDTO_forCreate model)
        {

            await _service.SessionCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Session created successfully!" });

        }


        [HttpPut(Routes.Session)]
        public async Task<IActionResult> SessionUpdate([FromForm] SessionDTO_forUpdate model)
        {


            await _service.SessionUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Session updated successfully!" });




        }


        [HttpDelete(Routes.SessionById)]
        public async Task<IActionResult> SessionDelete(int id)
        {

            await _service.DeleteSessionById(id);

            return Ok(new Response { Status = "Success", Message = "The Session deleted successfully!" });
        }


        #endregion


        #region Question


        [HttpGet(Routes.QuestionById)]
        public async Task<IActionResult> GetQuestionById(int id)
        {

            var data = await _service.GetQuestionById(id);

            return Ok(data);

        }


        [HttpGet(Routes.Question)]
        public async Task<IActionResult> GetQuestion()
        {

            var data = await _service.GetQuestion();


            return Ok(data);

        }



        [HttpPost(Routes.Question)]
        public async Task<IActionResult> QuestionCreate([FromForm] QuestionDTO_forCreate model)
        {

            await _service.QuestionCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Question created successfully!" });

        }


        [HttpPut(Routes.Question)]
        public async Task<IActionResult> QuestionUpdate([FromForm] QuestionDTO_forUpdate model)
        {


            await _service.QuestionUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Question updated successfully!" });




        }


        [HttpDelete(Routes.QuestionById)]
        public async Task<IActionResult> QuestionDelete(int id)
        {

            await _service.DeleteQuestionById(id);

            return Ok(new Response { Status = "Success", Message = "The Question deleted successfully!" });
        }


        #endregion        
        
        
        #region SessionQuestion


        [HttpGet(Routes.SessionQuestionById)]
        public async Task<IActionResult> GetSessionQuestionById(int id)
        {

            var data = await _service.GetSessionQuestionById(id);

            return Ok(data);

        }


        [HttpGet(Routes.SessionQuestion)]
        public async Task<IActionResult> GetSessionQuestion()
        {

            var data = await _service.GetSessionQuestion();


            return Ok(data);

        }


        [HttpPost(Routes.SessionQuestion)]
        public async Task<IActionResult> SessionQuestionCreate([FromForm] SessionQuestionDTO_forCreate model)
        {

            await _service.SessionQuestionCreate(model);

            return Ok(new Response { Status = "Success", Message = "The SessionQuestion created successfully!" });

        }


        [HttpPut(Routes.SessionQuestion)]
        public async Task<IActionResult> SessionQuestionUpdate([FromForm] SessionQuestionDTO_forUpdate model)
        {


            await _service.SessionQuestionUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The SessionQuestion updated successfully!" });




        }


        [HttpDelete(Routes.SessionQuestionById)]
        public async Task<IActionResult> SessionQuestionDelete(int id)
        {

            await _service.DeleteSessionQuestionById(id);

            return Ok(new Response { Status = "Success", Message = "The SessionQuestion deleted successfully!" });
        }


        [HttpGet(Routes.RandomQuestionById)]
        public async Task<IActionResult> GetRandomQuestion(int questionCount, int positionId, int vacantionId, int sessionId)
        {

            var data = await _service.GetRandomQuestion( questionCount,  positionId,  vacantionId,  sessionId);


            return Ok(data);

        }

        #endregion





    }
}


