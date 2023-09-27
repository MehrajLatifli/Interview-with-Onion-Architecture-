using Interview.Domain.AuthModels;
using Interview.Persistence.Contexts.AuthDbContext.IdentityAuth;
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
        List<IdentityError> errorList = new List<IdentityError>();

        public AdminController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<CustomUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
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

    }
}
