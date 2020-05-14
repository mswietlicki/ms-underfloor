using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MS.Underfloor.Api.Controllers
{
    [ApiController]
    [Route("api/temps")]
    public class TempsController : ControllerBase
    {
        public static List<TempReport> Reports { get; set; } = new List<TempReport>();
        private readonly ILogger<TempsController> _logger;

        public TempsController(ILogger<TempsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Add(TempReport report)
        {
            if(report == null)
                return BadRequest();

            report.Timestamp = DateTime.Now;
            Reports.Add(report);

            return Accepted();
        }

        [HttpGet]
        public IActionResult List()
        {
            return Ok(Reports);
        }
    }
}
