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
    public class SessionQuestionController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;

        private readonly IService _service;

        public SessionQuestionController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<CustomUser> signInManager, IService service)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _service = service;
        }


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

            var data = await _service.GetRandomQuestion(questionCount, positionId, vacantionId, sessionId);


            return Ok(data);

        }

        #endregion
    }


}
