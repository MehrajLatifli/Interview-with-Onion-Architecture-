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
    public class QuestionController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;

        private readonly IService _service;

        public QuestionController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<CustomUser> signInManager, IService service)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _service = service;
        }


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
    }


}
