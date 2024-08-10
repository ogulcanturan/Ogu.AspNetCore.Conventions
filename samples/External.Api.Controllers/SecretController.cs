using Microsoft.AspNetCore.Mvc;

namespace External.Api.Controllers
{
    [Route("api/secret")]
    public class SecretController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetSecret()
        {
            return Ok("Secret");
        }
    }
}