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
    public class StructureController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;

        private readonly IService _service;

        public StructureController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<CustomUser> signInManager, IService service)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _service = service;
        }


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
    }


}
