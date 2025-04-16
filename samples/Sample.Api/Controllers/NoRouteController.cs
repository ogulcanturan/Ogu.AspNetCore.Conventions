using Microsoft.AspNetCore.Mvc;

namespace Sample.Api.Controllers
{
    [Route("no-route")]
    [Route("api/no-route/v2")]

    public class NoRouteController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Route isn't specified in the Controller's class");
        }
    }
}