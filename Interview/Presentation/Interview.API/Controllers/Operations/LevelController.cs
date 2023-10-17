using Interview.API.API_Routes;
using Interview.Application.Mapper.DTO;
using Interview.Application.Services.Abstract;
using Interview.Domain.Entities.AuthModels;
using Interview.Domain.Entities.IdentityAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Interview.API.Controllers.Operations
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class LevelController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;

        private readonly IService _service;

        public LevelController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<CustomUser> signInManager, IService service)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _service = service;
        }


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
    }


}
