using Interview.API.API_Routes;
using Interview.Application.Mapper.DTO;
using Interview.Application.Services.Abstract;
using Interview.Domain.Entities.AuthModels;
using Interview.Persistence.Contexts.AuthDbContext.IdentityAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Interview.API.Controllers.Operations
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class VacancyController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;

        private readonly IService _service;

        public VacancyController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<CustomUser> signInManager, IService service)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _service = service;
        }


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
    }


}
