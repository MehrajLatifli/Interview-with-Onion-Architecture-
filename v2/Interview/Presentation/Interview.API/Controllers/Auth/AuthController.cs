﻿using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
using Interview.Application.Mapper.DTO.AuthDTO;

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
        [Route(Routes.addUser)]
        public async Task<IActionResult> AddUser([FromForm] RegisterDTO model)
        {

            await _authservice.AddUser(model, ServiceExtension.ConnectionStringAzure);


            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
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

        [HttpGet]
        [Route(Routes.GetMehtods)]
        public async Task<IActionResult> GetMehtods()
        {
            return Ok(await _authservice.GetMehtods(User));



        }

        [HttpGet]
        [Route(Routes.GetUserAccess)]
        public async Task<IActionResult> GetUserAccess()
        {
            return Ok(await _authservice.GetUserAccess(User));


        }

        [HttpPost]
        [Route(Routes.addRole)]
        public async Task<IActionResult> AddRole([FromBody] CreateRoleRequestModel model)
        {
            await _authservice.AddRole(model.RoleName, User);


            return Ok(new Response { Status = "Success", Message = "Role successfully created!" });
        }

        [HttpPost]
        [Route(Routes.addRoleClaim)]
        public async Task<IActionResult> AddRoleClaim([FromBody] CreateRoleClaimRequestModel model)
        {
            await _authservice.AddRoleClaim(model.RoleId,model.RoleAccessMethodId, User);


            return Ok(new Response { Status = "Success", Message = "RoleClaim successfully created!" });
        }

        [HttpPost]
        [Route(Routes.addUserClaim)]
        public async Task<IActionResult> AddUserClaim([FromBody] CreateUserClaimRequestModel model)
        {
            await _authservice.AddUserClaim(model.UserId, model.UserAccessId, User);


            return Ok(new Response { Status = "Success", Message = "UserClaim successfully created!" });
        }

        [HttpPost]
        [Route(Routes.addUserRole)]
        public async Task<IActionResult> AddUserRole([FromBody] CreateUserRoleRequestModel model)
        {
            await _authservice.AddUserRole(model.UserId, model.RoleId, User);


            return Ok(new Response { Status = "Success", Message = "UserRole successfully created!" });
        }

    }
}
