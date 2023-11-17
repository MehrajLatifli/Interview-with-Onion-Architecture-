﻿using Interview.API.API_Routes;
using Interview.Application.Mapper.DTO;
using Interview.Application.Services.Abstract;
using Interview.Domain.Entities.AuthModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Interview.API.Controllers.Operations
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class QuestionController : ControllerBase
    {


        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }





        #region Question


        [HttpGet(Routes.QuestionById)]
        public async Task<IActionResult> GetQuestionById(int id)
        {

            var data = await _questionService.GetQuestionById(id);

            return Ok(data);

        }


        [HttpGet(Routes.Question)]
        public async Task<IActionResult> GetQuestion()
        {

            var data = await _questionService.GetQuestion();


            return Ok(data);

        }



        [HttpPost(Routes.Question)]
        public async Task<IActionResult> QuestionCreate([FromBody] QuestionDTO_forCreate model)
        {

            await _questionService.QuestionCreate(model);

            return Ok(new Response { Status = "Success", Message = "The Question created successfully!" });

        }


        [HttpPut(Routes.Question)]
        public async Task<IActionResult> QuestionUpdate([FromBody] QuestionDTO_forUpdate model)
        {


            await _questionService.QuestionUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The Question updated successfully!" });




        }


        [HttpDelete(Routes.QuestionById)]
        public async Task<IActionResult> QuestionDelete(int id)
        {

            await _questionService.DeleteQuestionById(id);

            return Ok(new Response { Status = "Success", Message = "The Question deleted successfully!" });
        }


        #endregion        
    }


}
