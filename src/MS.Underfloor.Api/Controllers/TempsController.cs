using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MS.Underfloor.Api.Data;

namespace MS.Underfloor.Api.Controllers
{
    [ApiController]
    [Route("api/temps")]
    public class TempsController : ControllerBase
    {
        public static Buffer<TempReport> Reports { get; set; } = new Buffer<TempReport>(60 * 24);

        private readonly ITempsRepository _tempsRepository;
        private readonly ILogger<TempsController> _logger;

        public TempsController(ITempsRepository tempsRepository, ILogger<TempsController> logger)
        {
            _tempsRepository = tempsRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Add(TempReport report)
        {
            if (report == null)
                return BadRequest();

            report.Timestamp = DateTime.Now;
            Reports.Add(report);

            await _tempsRepository.InsertTemp(report);

            return Accepted();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            return Ok(await _tempsRepository.GetTemps(DateTime.Now));
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
