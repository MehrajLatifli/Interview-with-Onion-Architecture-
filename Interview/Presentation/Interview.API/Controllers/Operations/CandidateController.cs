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
    public class CandidateController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;

        private readonly IService _service;

        public CandidateController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<CustomUser> signInManager, IService service)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _service = service;
        }

        #region Candidate


        [HttpGet(Routes.CandidateById)]
        public async Task<IActionResult> GetCandidateById(int id)
        {

            var data = await _service.GetCandidateById(id);

            return Ok(data);

        }


        [HttpGet(Routes.Candidate)]
        public async Task<IActionResult> GetCandidate()
        {

            var data = await _service.GetCandidate();


            return Ok(data);

        }


        [HttpPost(Routes.Candidate)]
        public async Task<IActionResult> CandidateCreate([FromForm] CandidateDTO_forCreate model)
        {

            await _service.CandidateCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Candidate created successfully!" });

        }


        [HttpPut(Routes.Candidate)]

        public async Task<IActionResult> CandidateUpdate([FromForm] CandidateDTO_forUpdate model)
        {


            await _service.CandidateUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Candidate updated successfully!" });




        }


        [HttpDelete(Routes.CandidateById)]
        public async Task<IActionResult> CandidateDelete(int id)
        {

            await _service.DeleteCandidateById(id);

            return Ok(new Response { Status = "Success", Message = "The Candidate deleted successfully!" });
        }


        #endregion
    }


}
