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
    public class PositionController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;

        private readonly IService _service;

        public PositionController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<CustomUser> signInManager, IService service)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _service = service;
        }


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
    }


}
