using Microsoft.AspNetCore.Mvc;

namespace External.Api.Controllers
{
    [Route("api/very-secret")]
    public class VerySecretController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetVerySecret()
        {
            return Ok("Very secret");
        }
    }
}