using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Interview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "HROnly")]
    public class HRController : ControllerBase
    {
    }
}
