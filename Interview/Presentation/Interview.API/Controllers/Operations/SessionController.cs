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
    public class SessionController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;

        private readonly IService _service;

        public SessionController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<CustomUser> signInManager, IService service)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _service = service;
        }



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
    }


}
