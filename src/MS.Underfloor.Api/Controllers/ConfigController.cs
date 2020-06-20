using Microsoft.AspNetCore.Mvc;
using MS.Underfloor.Api.Data;

namespace MS.Underfloor.Api.Controllers
{
    [ApiController]
    [Route("api/config")]
    public class ConfigController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new AgentConfig());
        }
    }
}
