using Interview.API.API_Routes;
using Interview.Application.Mapper.DTO;
using Interview.Application.Services.Abstract;
using Interview.Domain.Entities.AuthModels;
using Interview.Persistence.Contexts.AuthDbContext.IdentityAuth;
using Interview.Persistence.ServiceExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Interview.API.Controllers.Operations
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class CandidateDocumentController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;

        private readonly IService _service;

        public CandidateDocumentController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<CustomUser> signInManager, IService service)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _service = service;
        }

        #region CandidateDocument


        [HttpGet(Routes.CandidateDocumentById)]
        public async Task<IActionResult> GetCandidateDocumentById(int id)
        {

            var data = await _service.GetCandidateDocumentById(id);

            return Ok(data);

        }


        [HttpGet(Routes.CandidateDocument)]
        public async Task<IActionResult> GetCandidateDocument()
        {

            var data = await _service.GetCandidateDocument();


            return Ok(data);

        }


        [HttpPost]
        [Route(Routes.CandidateDocument)]
        public async Task<IActionResult> CandidateDocumentCreate([FromForm] CandidateDocumentDTO_forCreate model)
        {

            await _service.CandidateDocumentCreate(model, ServiceExtension.ConnectionStringAzure);

            return Ok(new Response { Status = "Success", Message = "The CandidateDocument created successfully!" });

        }


        [HttpPut]
        [Route(Routes.CandidateDocument)]
        public async Task<IActionResult> CandidateDocumentUpdate([FromForm] CandidateDocumentDTO_forUpdate model)
        {


            await _service.CandidateDocumentUpdate(model, ServiceExtension.ConnectionStringAzure);


            return Ok(new Response { Status = "Success", Message = "The CandidateDocument updated successfully!" });




        }


        [HttpDelete(Routes.CandidateDocumentById)]
        public async Task<IActionResult> CandidateDocumentDelete(int id)
        {

            await _service.DeleteCandidateDocumentById(id);

            return Ok(new Response { Status = "Success", Message = "The CandidateDocument deleted successfully!" });
        }


        #endregion
    }


}
