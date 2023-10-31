﻿using Interview.API.API_Routes;
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
    public class LevelController : ControllerBase
    {



        private readonly ILevelService _levelService;

        public LevelController(ILevelService levelService)
        {
            _levelService = levelService;
        }





        #region Level


        [HttpGet(Routes.LevelById)]
        public async Task<IActionResult> GetLevelById(int id)
        {

            var data = await _levelService.GetLevelById(id);

            return Ok(data);

        }


        [HttpGet(Routes.Level)]
        public async Task<IActionResult> GetLevel()
        {

            var data = await _levelService.GetLevel();


            return Ok(data);

        }


        [HttpPost(Routes.Level)]

        public async Task<IActionResult> LevelCreate([FromBody] LevelDTO_forCreate model)
        {

            await _levelService.LevelCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Level created successfully!" });

        }


        [HttpPut(Routes.Level)]
        public async Task<IActionResult> LevelUpdate([FromBody] LevelDTO_forUpdate model)
        {


            await _levelService.LevelUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Level updated successfully!" });




        }


        [HttpDelete(Routes.LevelById)]
        public async Task<IActionResult> LevelDelete(int id)
        {

            await _levelService.DeleteLevelById(id);

            return Ok(new Response { Status = "Success", Message = "The Level deleted successfully!" });
        }


        #endregion
    }


}
