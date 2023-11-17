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
using Interview.API.API_Routes;
using Interview.Application.Validations;
using Interview.Application.Validations.Interview.Application.Validations;
using Interview.Domain.Entities.Requests;
using Interview.Persistence.Contexts.InterviewDbContext;
using Microsoft.EntityFrameworkCore;
using Interview.Persistence.SqlQueries;

namespace Interview.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        public readonly IMapper _mapper;
        private readonly IAuthService _authservice;
        private readonly InterviewContext _interviewContext;


        public AuthController(IMapper mapper, IAuthService authservice, InterviewContext interviewContext)
        {
            _mapper = mapper;
            _authservice = authservice;
            _interviewContext = interviewContext;
        }



        [HttpPost]
        [Route(Routes.RegisterUser)]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterDTO model)
        {

            await _authservice.RegisterUser(model, ServiceExtension.ConnectionStringAzure);


            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }


        [HttpPost]
        [Route(Routes.Login)]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {


            return Ok(await _authservice.Login(model));

        }







    }
}
