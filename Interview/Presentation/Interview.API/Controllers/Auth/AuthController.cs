using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Interview.Application.Mapper.AuthDTO;
using Interview.Domain.Entities.AuthModels;
using Interview.Domain.Entities.Models;
using Interview.Persistence.ServiceExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Interview.Application.Services.Abstract;
using Microsoft.IdentityModel.Claims;
using Claim = System.Security.Claims.Claim;
using ClaimsPrincipal = System.Security.Claims.ClaimsPrincipal;
using Interview.API.API_Routes;
using Interview.Application.Validations;
using Interview.Application.Validations.Interview.Application.Validations;
using Interview.Domain.Entities.IdentityAuth;
using Interview.Domain.Entities.Requests;

namespace Interview.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        public readonly IMapper _mapper;
        private readonly IAuthService _authservice;


        public AuthController(IMapper mapper, IAuthService authservice)
        {
            _mapper = mapper;
            _authservice = authservice;
        }

        [HttpGet(Routes.GetRoleAccessType)]
        public async Task<IActionResult> GetRoleAccessType()
        {


            return Ok(await _authservice.GetRoleAccessType(User));

        }

        //[CustomAuthorize("Admin", CustomRole = "Admin")]
        //[CustomAuthorize(CustomRoles = new[] { "Admin" })]
        //[Authorize(Roles = UserRoles.Admin)]
        [HttpGet(Routes.GetAdmins)]
        public async Task<IActionResult> GetAdmins()
        {


            return Ok(await _authservice.GetAdmins(User));

        }


        //[CustomAuthorize(CustomRoles = new[] { "Admin", "HR" })]
        //[Authorize(Roles = "HR")]
        //[Authorize(Roles = UserRoles.HR)]
        [HttpGet(Routes.GetHR)]
        public async Task<IActionResult> GetHR()
        {

            return Ok(await _authservice.GetHR(User));

        }


        [Authorize]
        [HttpPost]
        [Route(Routes.createRole)]
        public async Task<IActionResult> CreateRole(string userId, string roleName, int roleAccesstype)
        {
            await _authservice.CreateRoleForUser( userId,  roleName,  roleAccesstype, User);


            return Ok(new Response { Status = "Success", Message = "Role successfully created!" });
        }


        [HttpPost]
        [Route(Routes.RegisterAdmin)]
        public async Task<IActionResult> RegisterAdmin([FromForm] RegisterDTO model)
        {

           await _authservice.RegisterAdmin(model, ServiceExtension.ConnectionStringAzure);


            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }


        [HttpPost]
        [Route(Routes.RegisterHR)]
        public async Task<IActionResult> RegisterHR([FromForm] RegisterDTO model)
        {

            await _authservice.RegisterHR(model, ServiceExtension.ConnectionStringAzure);


            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route(Routes.RegisterUser)]
        public async Task<IActionResult> RegisterUser([FromQuery] RegisterUserRequestModel registerUserRequestModel, [FromForm] RegisterDTO model)
        {

            await _authservice.RegisterUser( model, ServiceExtension.ConnectionStringAzure, registerUserRequestModel.customRoles, registerUserRequestModel.roleAccesstype);


            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }


        [HttpPost]
        [Route(Routes.Login)]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {


          return Ok (await _authservice.Login(model));

        }


        [HttpPost]
        [Route(Routes.Logout)]
        public async Task<IActionResult> Logout()
        {
            await _authservice.Logout();

            return Ok(new Response { Status = "Success", Message = "User logout!" });
        }







        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [Route(Routes.RefreshToken)]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
           return Ok(await _authservice.RefreshToken(tokenModel));

        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [Route(Routes.RevokeUsername)]
        public async Task<IActionResult> Revoke(string username)
        {
            await _authservice.Revoke(username);

            return NoContent();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [Route(Routes.RevokeAll)]
        public async Task<IActionResult> RevokeAll()
        {
            await _authservice.RevokeAll();

            return NoContent();
        }

        [Authorize(Policy = "AllRoles")]
        [HttpPut]
        [Route(Routes.UpdateProfile)]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDTO model)
        {
        

            await _authservice.UpdateProfile(model, User, ServiceExtension.ConnectionStringAzure);


            return Ok(new Response { Status = "Success", Message = "User updated successfully!" });
        }



        [Authorize(Policy = "AllRoles")]
        [HttpPut]
        [Route(Routes.UpdatePassword)]
        public async Task<IActionResult> UpdatePassword([FromForm] UpdatePasswordDTO model)
        {


            await _authservice.UpdatePassword(model, User);



            return Ok(new Response { Status = "Success", Message = "User updated successfully!" });
        }


  



    }
}
