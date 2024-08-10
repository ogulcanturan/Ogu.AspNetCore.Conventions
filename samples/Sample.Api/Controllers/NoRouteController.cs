using Microsoft.AspNetCore.Mvc;

namespace Sample.Api.Controllers
{
    public class NoRouteController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Route isn't specified in the Controller's class");
        }
    }
}