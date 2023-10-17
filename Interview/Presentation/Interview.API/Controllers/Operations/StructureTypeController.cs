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
    public class StructureTypeController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;

        private readonly IService _service;

        public StructureTypeController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<CustomUser> signInManager, IService service)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _service = service;
        }

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
    }


}
