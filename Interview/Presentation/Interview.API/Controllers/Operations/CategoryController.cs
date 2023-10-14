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
    public class CategoryController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;

        private readonly IService _service;

        public CategoryController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<CustomUser> signInManager, IService service)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _service = service;
        }

        #region Category


        [HttpGet(Routes.CategoryById)]
        public async Task<IActionResult> GetCategoryById(int id)
        {

            var data = await _service.GetCategoryById(id);

            return Ok(data);

        }


        [HttpGet(Routes.Category)]
        public async Task<IActionResult> GetCategory()
        {

            var data = await _service.GetCategory();


            return Ok(data);

        }


        [HttpPost(Routes.Category)]
        public async Task<IActionResult> CategoryCreate([FromForm] CategoryDTO_forCreate model)
        {

            await _service.CategoryCreate(model);

            return Ok(new Response { Status = "Success", Message = "The SessionType created successfully!" });

        }


        [HttpPut(Routes.Category)]
        public async Task<IActionResult> CategoryUpdate([FromForm] CategoryDTO_forUpdate model)
        {


            await _service.CategoryUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The SessionType updated successfully!" });




        }


        [HttpDelete(Routes.CategoryById)]
        public async Task<IActionResult> DeleteCategoryById(int id)
        {

            await _service.DeleteCategoryById(id);

            return Ok(new Response { Status = "Success", Message = "The SessionType deleted successfully!" });
        }


        #endregion
    }


}
