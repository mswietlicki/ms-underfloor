using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MS.Underfloor.Api.Controllers
{
    [ApiController]
    [Route("api/temps")]
    public class TempsController : ControllerBase
    {
        public static Buffer<TempReport> Reports { get; set; } = new Buffer<TempReport>(60 * 24);
        private readonly ILogger<TempsController> _logger;

        public TempsController(ILogger<TempsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Add(TempReport report)
        {
            if (report == null)
                return BadRequest();

            report.Timestamp = DateTime.Now;
            Reports.Add(report);

            return Accepted();
        }

        [HttpGet]
        public IActionResult List()
        {
            return Ok(Reports.ToList());
        }
    }

    public class Buffer<T> : Queue<T>
    {
        private int? maxCapacity { get; set; }

        public Buffer() { maxCapacity = null; }
        public Buffer(int capacity) { maxCapacity = capacity; }

        public void Add(T newElement)
        {
            if (this.Count == (maxCapacity ?? -1)) this.Dequeue(); // no limit if maxCapacity = null
            this.Enqueue(newElement);
        }
    }
}
