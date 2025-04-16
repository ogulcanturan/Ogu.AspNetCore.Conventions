using Microsoft.AspNetCore.Mvc;

namespace Sample.Api.Controllers
{
    [Route("api/[controller]")]
    public class SamplesController : ControllerBase
    {
        private static readonly string[] Weekdays = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];

        [HttpGet]
        public IActionResult GetSample()
        {
            return Ok(Weekdays);
        }
    }
}