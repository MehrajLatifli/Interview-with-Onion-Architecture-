using AutoMapper;
using Interview.Application.Exception;
using Interview.Application.Mapper.DTO;
using Interview.Application.Repositories.Custom;
using Interview.Application.Services.Abstract;
using Interview.Domain.AuthModels;
using Interview.Domain.Entities.Models;
using Interview.Persistence.Contexts.AuthDbContext.IdentityAuth;
using Interview.Persistence.Repositories.Custom;
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


        #region Sector


        [HttpGet("sector/{id}")]
        public async Task<IActionResult> GetSectorById(int id)
        {

            var data = await _service.GetSectorById(id);

            return Ok(data);

        }


        [HttpGet("sector")]
        public async Task<IActionResult> GetSector()
        {

          var data = await _service.GetSector();


            return Ok(data);

        }


        [HttpPost]
        [Route("sector")]
        public async Task<IActionResult> SectorCreate([FromBody] SectorDTO_forCreate model)
        {

            await _service.SectorCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Sector created successfully!" });

        }


        [HttpPut]
        [Route("sector")]
        public async Task<IActionResult> SectorUpdate([FromBody] SectorDTO_forUpdate model)
        {


            await _service.SectorUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Sector updated successfully!" });




        }


        [HttpDelete("sector/{id}")]
        public async Task<IActionResult> SectorDelete(int id)
        {

            await _service.DeleteSectorById(id);

            return Ok(new Response { Status = "Success", Message = "The Sector deleted successfully!" });
        }


        #endregion


        #region Branch


        [HttpGet("branch/{id}")]
        public async Task<IActionResult> GetBranchById(int id)
        {

            var data = await _service.GetBranchById(id);

            return Ok(data);

        }


        [HttpGet("branch")]
        public async Task<IActionResult> GetBranch()
        {

            var data = await _service.GetBranch();


            return Ok(data);

        }


        [HttpPost]
        [Route("branch")]
        public async Task<IActionResult> BranchCreate([FromBody] BranchDTO_forCreate model)
        {

            await _service.BranchCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Branch created successfully!" });

        }


        [HttpPut]
        [Route("branch")]
        public async Task<IActionResult> BranchUpdate([FromBody] BranchDTO_forUpdate model)
        {


            await _service.BranchUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Branch updated successfully!" });




        }


        [HttpDelete("branch/{id}")]
        public async Task<IActionResult> BranchDelete(int id)
        {

            await _service.DeleteBranchById(id);

            return Ok(new Response { Status = "Success", Message = "The Branch deleted successfully!" });
        }


        #endregion


        #region Department


        [HttpGet("department/{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {

            var data = await _service.GetDepartmentById(id);

            return Ok(data);

        }


        [HttpGet("department")]
        public async Task<IActionResult> GetDepartment()
        {

            var data = await _service.GetDepartment();


            return Ok(data);

        }


        [HttpPost]
        [Route("department")]
        public async Task<IActionResult> DepartmentCreate([FromBody] DepartmentDTO_forCreate model)
        {

            await _service.DepartmentCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Department created successfully!" });

        }


        [HttpPut]
        [Route("department")]
        public async Task<IActionResult> DepartmentUpdate([FromBody] DepartmentDTO_forUpdate model)
        {


            await _service.DepartmentUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Department updated successfully!" });




        }


        [HttpDelete("department/{id}")]
        public async Task<IActionResult> DepartmentDelete(int id)
        {

            await _service.DeleteDepartmentById(id);

            return Ok(new Response { Status = "Success", Message = "The Department deleted successfully!" });
        }


        #endregion


        #region JobDegree


        [HttpGet("jobdegree/{id}")]
        public async Task<IActionResult> GetJobDegreeById(int id)
        {

            var data = await _service.GetJobDegreeById(id);

            return Ok(data);

        }


        [HttpGet("jobdegree")]
        public async Task<IActionResult> GetJobDegree()
        {

            var data = await _service.GetJobDegree();


            return Ok(data);

        }


        [HttpPost]
        [Route("jobdegree")]
        public async Task<IActionResult> JobDegreeCreate([FromBody] JobDegreeDTO_forCreate model)
        {

            await _service.JobDegreeCreate(model);

            return Ok(new Response { Status = "Success", Message = "The JobDegree created successfully!" });

        }


        [HttpPut]
        [Route("jobdegree")]
        public async Task<IActionResult> JobDegreeUpdate([FromBody] JobDegreeDTO_forUpdate model)
        {


            await _service.JobDegreeUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The JobDegree updated successfully!" });




        }


        [HttpDelete("jobdegree/{id}")]
        public async Task<IActionResult> JobDegreeDelete(int id)
        {

            await _service.DeleteJobDegreeById(id);

            return Ok(new Response { Status = "Success", Message = "The JobDegree deleted successfully!" });
        }


        #endregion


        #region Vacancy


        [HttpGet("vacancy/{id}")]
        public async Task<IActionResult> GetVacancyById(int id)
        {

            var data = await _service.GetVacancyById(id);

            return Ok(data);

        }


        [HttpGet("vacancy")]
        public async Task<IActionResult> GetVacancy()
        {

            var data = await _service.GetVacancy();


            return Ok(data);

        }


        [HttpPost]
        [Route("vacancy")]
        public async Task<IActionResult> VacancyCreate([FromBody] VacancyDTO_forCreate model)
        {

            await _service.VacancyCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Vacancy created successfully!" });

        }


        [HttpPut]
        [Route("vacancy")]
        public async Task<IActionResult> VacancyUpdate([FromBody] VacancyDTO_forUpdate model)
        {


            await _service.VacancyUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Vacancy updated successfully!" });




        }


        [HttpDelete("vacancy/{id}")]
        public async Task<IActionResult> VacancyDelete(int id)
        {

            await _service.DeleteVacancyById(id);

            return Ok(new Response { Status = "Success", Message = "The Vacancy deleted successfully!" });
        }


        #endregion


        #region Candidate


        [HttpGet("candidate/{id}")]
        public async Task<IActionResult> GetCandidateById(int id)
        {

            var data = await _service.GetCandidateById(id);

            return Ok(data);

        }


        [HttpGet("candidate")]
        public async Task<IActionResult> GetCandidate()
        {

            var data = await _service.GetCandidate();


            return Ok(data);

        }


        [HttpPost]
        [Route("candidate")]
        public async Task<IActionResult> CandidateCreate([FromForm] CandidateDTO_forCreate model)
        {

            await _service.CandidateCreate(model, ServiceExtension.ConnectionStringAzure);

            return Ok(new Response { Status = "Success", Message = "The Candidate created successfully!" });

        }


        [HttpPut]
        [Route("candidate")]
        public async Task<IActionResult> CandidateUpdate([FromForm] CandidateDTO_forUpdate model)
        {


            await _service.CandidateUpdate(model, ServiceExtension.ConnectionStringAzure);


            return Ok(new Response { Status = "Success", Message = "The Candidate updated successfully!" });




        }


        [HttpDelete("candidate/{id}")]
        public async Task<IActionResult> CandidateDelete(int id)
        {

            await _service.DeleteCandidateById(id);

            return Ok(new Response { Status = "Success", Message = "The Candidate deleted successfully!" });
        }


        #endregion
    }
}


