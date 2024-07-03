using HNG.StageOneTask.BackendC_.Implementations;
using HNG.StageOneTask.BackendC_.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace HNG.StageOneTask.BackendC_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreetingController : ControllerBase
    {
        private readonly GreetingService _greetingService;

        public GreetingController(GreetingService greetingService)
        {
            _greetingService = greetingService;
        }

        [HttpGet("hello")]
        public async Task<ActionResult> Greetings([FromQuery]string visitor_name, CancellationToken token)
        {
            var greeting = await _greetingService.Greet(HttpContext, visitor_name, token);
            return Ok(greeting);
        }

    }
}
