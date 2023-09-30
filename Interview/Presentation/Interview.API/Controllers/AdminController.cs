using AutoMapper;
using Interview.Application.Exception;
using Interview.Application.Mapper.DTO;
using Interview.Application.Repositories.Custom;
using Interview.Application.Services.Admins.Abstract;
using Interview.Domain.AuthModels;
using Interview.Domain.Entities.Models;
using Interview.Persistence.Contexts.AuthDbContext.IdentityAuth;
using Interview.Persistence.Repositories.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Interview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly IMapper _mapper;

        private readonly IAdminService _adminService;

        public AdminController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<CustomUser> signInManager, IMapper mapper, IAdminService adminService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _mapper = mapper;
            _adminService = adminService;
        }

        [HttpGet("getAdmins")]
        public async Task<IActionResult> GetAdmins()
        {

            var list = new List<GetAuthModel>();


            foreach (var user in _userManager.Users.ToList().Where(i => i.Roles == UserRoles.Admin))
            {

                if (user != null)
                {


                    list.Add(new GetAuthModel()
                    {
                        Username = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        ImagePath = user.ImagePath,
                        Roles = user.Roles,



                    });
                }

                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found" });
                }

            }
            if (list.Any())
            {

                return Ok(list);
            }

            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Nothing found" });
            }

        }


        [HttpGet("getHR")]
        public async Task<IActionResult> GetHR()
        {

            var list = new List<GetAuthModel>();


            foreach (var user in _userManager.Users.ToList().Where(i => i.Roles == UserRoles.HR))
            {

                if (user != null)
                {


                    list.Add(new GetAuthModel()
                    {
                        Username = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        ImagePath = user.ImagePath,
                        Roles = user.Roles,



                    });
                }

                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found" });
                }

            }
            if (list.Any())
            {

                return Ok(list);
            }

            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Nothing found" });
            }

        }

        #region Sector


        [HttpGet("sector/{id}")]
        public async Task<IActionResult> GetSectorById(int id)
        {


            var data = await _adminService.GetSectorById(id);

            return Ok(data);

        }


        [HttpGet("sector")]
        public async Task<IActionResult> GetSector()
        {

          var data = await _adminService.GetSector();


            return Ok(data);

        }


        [HttpPost]
        [Route("sector")]
        public async Task<IActionResult> SectorCreate([FromBody] SectorDTO_forCreate model)
        {

            await _adminService.SectorCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Sector created successfully!" });

        }


        [HttpPut]
        [Route("sector")]
        public async Task<IActionResult> SectorUpdate([FromBody] SectorDTO_forUpdate model)
        {


            await _adminService.SectorUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Sector updated successfully!" });




        }


        [HttpDelete("sector/{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            await _adminService.DeleteSectorById(id);

            return Ok(new Response { Status = "Success", Message = "The Sector deleted successfully!" });
        }

        #endregion

    }
}


