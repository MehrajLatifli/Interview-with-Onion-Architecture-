using AutoMapper;
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
    [Authorize(Policy = "AdminOnly")]
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


        [HttpGet("candidatedocument/{id}")]
        public async Task<IActionResult> GetCandidateDocumentById(int id)
        {

            var data = await _service.GetCandidateDocumentById(id);

            return Ok(data);

        }


        [HttpGet("candidatedocument")]
        public async Task<IActionResult> GetCandidateDocument()
        {

            var data = await _service.GetCandidateDocument();


            return Ok(data);

        }


        [HttpPost]
        [Route("candidatedocument")]
        public async Task<IActionResult> CandidateDocumentCreate([FromForm] CandidateDocumentDTO_forCreate model)
        {

            await _service.CandidateDocumentCreate(model, ServiceExtension.ConnectionStringAzure);

            return Ok(new Response { Status = "Success", Message = "The CandidateDocument created successfully!" });

        }


        [HttpPut]
        [Route("candidatedocument")]
        public async Task<IActionResult> CandidateDocumentUpdate([FromForm] CandidateDocumentDTO_forUpdate model)
        {


            await _service.CandidateDocumentUpdate(model, ServiceExtension.ConnectionStringAzure);


            return Ok(new Response { Status = "Success", Message = "The CandidateDocument updated successfully!" });




        }


        [HttpDelete("candidatedocument/{id}")]
        public async Task<IActionResult> CandidateDocumentDelete(int id)
        {

            await _service.DeleteCandidateDocumentById(id);

            return Ok(new Response { Status = "Success", Message = "The CandidateDocument deleted successfully!" });
        }


        #endregion


        #region Candidate


        [HttpGet("Candidate/{id}")]
        public async Task<IActionResult> GetCandidateById(int id)
        {

            var data = await _service.GetCandidateById(id);

            return Ok(data);

        }


        [HttpGet("Candidate")]
        public async Task<IActionResult> GetCandidate()
        {

            var data = await _service.GetCandidate();


            return Ok(data);

        }


        [HttpPost]
        [Route("Candidate")]
        public async Task<IActionResult> CandidateCreate([FromForm] CandidateDTO_forCreate model)
        {

            await _service.CandidateCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Candidate created successfully!" });

        }


        [HttpPut]
        [Route("Candidate")]
        public async Task<IActionResult> CandidateUpdate([FromForm] CandidateDTO_forUpdate model)
        {


            await _service.CandidateUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Candidate updated successfully!" });




        }


        [HttpDelete("Candidate/{id}")]
        public async Task<IActionResult> CandidateDelete(int id)
        {

            await _service.DeleteCandidateById(id);

            return Ok(new Response { Status = "Success", Message = "The Candidate deleted successfully!" });
        }


        #endregion


        #region Level


        [HttpGet("Level/{id}")]
        public async Task<IActionResult> GetLevelById(int id)
        {

            var data = await _service.GetLevelById(id);

            return Ok(data);

        }


        [HttpGet("Level")]
        public async Task<IActionResult> GetLevel()
        {

            var data = await _service.GetLevel();


            return Ok(data);

        }


        [HttpPost]
        [Route("Level")]
        public async Task<IActionResult> LevelCreate([FromForm] LevelDTO_forCreate model)
        {

            await _service.LevelCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Level created successfully!" });

        }


        [HttpPut]
        [Route("Level")]
        public async Task<IActionResult> LevelUpdate([FromForm] LevelDTO_forUpdate model)
        {


            await _service.LevelUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Level updated successfully!" });




        }


        [HttpDelete("Level/{id}")]
        public async Task<IActionResult> LevelDelete(int id)
        {

            await _service.DeleteLevelById(id);

            return Ok(new Response { Status = "Success", Message = "The Level deleted successfully!" });
        }


        #endregion


        #region SessionType


        [HttpGet("SessionType/{id}")]
        public async Task<IActionResult> GetSessionTypeById(int id)
        {

            var data = await _service.GetSessionTypeById(id);

            return Ok(data);

        }


        [HttpGet("SessionType")]
        public async Task<IActionResult> GetSessionType()
        {

            var data = await _service.GetSessionType();


            return Ok(data);

        }


        [HttpPost]
        [Route("SessionType")]
        public async Task<IActionResult> SessionTypeCreate([FromForm] SessionTypeDTO_forCreate model)
        {

            await _service.SessionTypeCreate(model);

            return Ok(new Response { Status = "Success", Message = "The SessionType created successfully!" });

        }


        [HttpPut]
        [Route("SessionType")]
        public async Task<IActionResult> SessionTypeUpdate([FromForm] SessionTypeDTO_forUpdate model)
        {


            await _service.SessionTypeUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The SessionType updated successfully!" });




        }


        [HttpDelete("SessionType/{id}")]
        public async Task<IActionResult> SessionTypeDelete(int id)
        {

            await _service.DeleteSessionTypeById(id);

            return Ok(new Response { Status = "Success", Message = "The SessionType deleted successfully!" });
        }


        #endregion


        #region StructureType


        [HttpGet("StructureType/{id}")]
        public async Task<IActionResult> GetStructureTypeById(int id)
        {

            var data = await _service.GetStructureTypeById(id);

            return Ok(data);

        }


        [HttpGet("StructureType")]
        public async Task<IActionResult> GetStructureType()
        {

            var data = await _service.GetStructureType();


            return Ok(data);

        }


        [HttpPost]
        [Route("StructureType")]
        public async Task<IActionResult> StructureTypeCreate([FromForm] StructureTypeDTO_forCreate model)
        {

            await _service.StructureTypeCreate(model);

            return Ok(new Response { Status = "Success", Message = "The StructureType created successfully!" });

        }


        [HttpPut]
        [Route("StructureType")]
        public async Task<IActionResult> StructureTypeUpdate([FromForm] StructureTypeDTO_forUpdate model)
        {


            await _service.StructureTypeUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The StructureType updated successfully!" });




        }


        [HttpDelete("StructureType/{id}")]
        public async Task<IActionResult> StructureTypeDelete(int id)
        {

            await _service.DeleteStructureTypeById(id);

            return Ok(new Response { Status = "Success", Message = "The StructureType deleted successfully!" });
        }


        #endregion


        #region Structure


        [HttpGet("Structure/{id}")]
        public async Task<IActionResult> GetStructureById(int id)
        {

            var data = await _service.GetStructureById(id);

            return Ok(data);

        }


        [HttpGet("Structure")]
        public async Task<IActionResult> GetStructure()
        {

            var data = await _service.GetStructure();


            return Ok(data);

        }


        [HttpPost]
        [Route("Structure")]
        public async Task<IActionResult> StructureCreate([FromForm] StructureDTO_forCreate model)
        {

            await _service.StructureCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Structure created successfully!" });

        }


        [HttpPut]
        [Route("Structure")]
        public async Task<IActionResult> StructureUpdate([FromForm] StructureDTO_forUpdate model)
        {


            await _service.StructureUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Structure updated successfully!" });




        }


        [HttpDelete("Structure/{id}")]
        public async Task<IActionResult> StructureDelete(int id)
        {

            await _service.DeleteStructureById(id);

            return Ok(new Response { Status = "Success", Message = "The Structure deleted successfully!" });
        }


        #endregion


        #region Position

        [HttpGet("Position/{id}")]
        public async Task<IActionResult> GetPositionById(int id)
        {

            var data = await _service.GetPositionById(id);

            return Ok(data);

        }


        [HttpGet("Position")]
        public async Task<IActionResult> GetPosition()
        {

            var data = await _service.GetPosition();


            return Ok(data);

        }


        [HttpPost]
        [Route("Position")]
        public async Task<IActionResult> PositionCreate([FromForm] PositionDTO_forCreate model)
        {

            await _service.PositionCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Position created successfully!" });

        }


        [HttpPut]
        [Route("Position")]
        public async Task<IActionResult> PositionUpdate([FromForm] PositionDTO_forUpdate model)
        {


            await _service.PositionUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Position updated successfully!" });




        }


        [HttpDelete("Position/{id}")]
        public async Task<IActionResult> PositionDelete(int id)
        {

            await _service.DeletePositionById(id);

            return Ok(new Response { Status = "Success", Message = "The Position deleted successfully!" });
        }


        #endregion        


        #region Vacancy


        [HttpGet("Vacancy/{id}")]
        public async Task<IActionResult> GetVacancyById(int id)
        {

            var data = await _service.GetVacancyById(id);

            return Ok(data);

        }


        [HttpGet("Vacancy")]
        public async Task<IActionResult> GetVacancy()
        {

            var data = await _service.GetVacancy();


            return Ok(data);

        }


        [HttpPost]
        [Route("Vacancy")]
        public async Task<IActionResult> VacancyCreate([FromForm] VacancyDTO_forCreate model)
        {

            await _service.VacancyCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Vacancy created successfully!" });

        }


        [HttpPut]
        [Route("Vacancy")]
        public async Task<IActionResult> VacancyUpdate([FromForm] VacancyDTO_forUpdate model)
        {


            await _service.VacancyUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Vacancy updated successfully!" });




        }


        [HttpDelete("Vacancy/{id}")]
        public async Task<IActionResult> VacancyDelete(int id)
        {

            await _service.DeleteVacancyById(id);

            return Ok(new Response { Status = "Success", Message = "The Vacancy deleted successfully!" });
        }


        #endregion


        #region Session


        [HttpGet("Session/{id}")]
        public async Task<IActionResult> GetSessionById(int id)
        {

            var data = await _service.GetSessionById(id);

            return Ok(data);

        }


        [HttpGet("Session")]
        public async Task<IActionResult> GetSession()
        {

            var data = await _service.GetSession();


            return Ok(data);

        }


        [HttpPost]
        [Route("Session")]
        public async Task<IActionResult> SessionCreate([FromForm] SessionDTO_forCreate model)
        {

            await _service.SessionCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Session created successfully!" });

        }


        [HttpPut]
        [Route("Session")]
        public async Task<IActionResult> SessionUpdate([FromForm] SessionDTO_forUpdate model)
        {


            await _service.SessionUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Session updated successfully!" });




        }


        [HttpDelete("Session/{id}")]
        public async Task<IActionResult> SessionDelete(int id)
        {

            await _service.DeleteSessionById(id);

            return Ok(new Response { Status = "Success", Message = "The Session deleted successfully!" });
        }


        #endregion


        #region Question


        [HttpGet("Question/{id}")]
        public async Task<IActionResult> GetQuestionById(int id)
        {

            var data = await _service.GetQuestionById(id);

            return Ok(data);

        }


        [HttpGet("Question")]
        public async Task<IActionResult> GetQuestion()
        {

            var data = await _service.GetQuestion();


            return Ok(data);

        }


        [HttpGet("RandomQuestion")]
        public async Task<IActionResult> GetRandomQuestion()
        {

            var data = await _service.GetRandomQuestion();


            return Ok(data);

        }


        [HttpPost]
        [Route("Question")]
        public async Task<IActionResult> QuestionCreate([FromForm] QuestionDTO_forCreate model)
        {

            await _service.QuestionCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Question created successfully!" });

        }


        [HttpPut]
        [Route("Question")]
        public async Task<IActionResult> QuestionUpdate([FromForm] QuestionDTO_forUpdate model)
        {


            await _service.QuestionUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Question updated successfully!" });




        }


        [HttpDelete("Question/{id}")]
        public async Task<IActionResult> QuestionDelete(int id)
        {

            await _service.DeleteQuestionById(id);

            return Ok(new Response { Status = "Success", Message = "The Question deleted successfully!" });
        }


        #endregion        
        
        
        #region SessionQuestion


        [HttpGet("SessionQuestion/{id}")]
        public async Task<IActionResult> GetSessionQuestionById(int id)
        {

            var data = await _service.GetSessionQuestionById(id);

            return Ok(data);

        }


        [HttpGet("SessionQuestion")]
        public async Task<IActionResult> GetSessionQuestion()
        {

            var data = await _service.GetSessionQuestion();


            return Ok(data);

        }


        [HttpPost]
        [Route("SessionQuestion")]
        public async Task<IActionResult> SessionQuestionCreate([FromForm] SessionQuestionDTO_forCreate model)
        {

            await _service.SessionQuestionCreate(model);

            return Ok(new Response { Status = "Success", Message = "The SessionQuestion created successfully!" });

        }


        [HttpPut]
        [Route("SessionQuestion")]
        public async Task<IActionResult> SessionQuestionUpdate([FromForm] SessionQuestionDTO_forUpdate model)
        {


            await _service.SessionQuestionUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The SessionQuestion updated successfully!" });




        }


        [HttpDelete("SessionQuestion/{id}")]
        public async Task<IActionResult> SessionQuestionDelete(int id)
        {

            await _service.DeleteSessionQuestionById(id);

            return Ok(new Response { Status = "Success", Message = "The SessionQuestion deleted successfully!" });
        }


#endregion        
        
        
    
        



        

        
        
        


    }
}


