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
    public class SessionQuestionController : ControllerBase
    {


        private readonly ISessionQuestionService _sessionQuestionService;

        public SessionQuestionController(ISessionQuestionService sessionQuestionService)
        {
            _sessionQuestionService = sessionQuestionService;
        }



        #region SessionQuestion


        [HttpGet(Routes.SessionQuestionById)]
        public async Task<IActionResult> GetSessionQuestionById(int id)
        {

            var data = await _sessionQuestionService.GetSessionQuestionById(id);

            return Ok(data);

        }


        [HttpGet(Routes.SessionQuestion)]
        public async Task<IActionResult> GetSessionQuestion()
        {

            var data = await _sessionQuestionService.GetSessionQuestion();


            return Ok(data);

        }


        [HttpPost(Routes.SessionQuestion)]
        public async Task<IActionResult> SessionQuestionCreate([FromForm] SessionQuestionDTO_forCreate model)
        {

            await _sessionQuestionService.SessionQuestionCreate(model);

            return Ok(new Response { Status = "Success", Message = "The SessionQuestion created successfully!" });

        }


        [HttpPut(Routes.SessionQuestion)]
        public async Task<IActionResult> SessionQuestionUpdate([FromForm] SessionQuestionDTO_forUpdate model)
        {


            await _sessionQuestionService.SessionQuestionUpdate(model);


            return Ok(new Response { Status = "Success", Message = "The SessionQuestion updated successfully!" });




        }


        [HttpDelete(Routes.SessionQuestionById)]
        public async Task<IActionResult> SessionQuestionDelete(int id)
        {

            await _sessionQuestionService.DeleteSessionQuestionById(id);

            return Ok(new Response { Status = "Success", Message = "The SessionQuestion deleted successfully!" });
        }


        [HttpGet(Routes.RandomQuestionById)]
        public async Task<IActionResult> GetRandomQuestion(int questionCount, int structureId, int positionId, int vacantionId, int sessionId)
        {

            var data = await _sessionQuestionService.GetRandomQuestion( questionCount,  structureId,  positionId,  vacantionId,  sessionId);


            return Ok(data);

        }

        #endregion
    }


}
