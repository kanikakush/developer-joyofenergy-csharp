using JOIEnergy.Domain;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Mvc;

namespace JOIEnergy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeterReadingController : Controller
    {
        private readonly IMeterReadingService _meterReadingService;
        public MeterReadingController(IMeterReadingService meterReadingService)
        {
            _meterReadingService = meterReadingService;
        }
        // POST api/values
        [HttpPost("store")]
        public IActionResult Post([FromBody] MeterReadings meterReadings)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult("Internal Server Error" + ModelState);
            }
            var result = _meterReadingService.StoreReadings(meterReadings.SmartMeterId, meterReadings.ElectricityReadings);
            return new OkObjectResult(result);
        }

        [HttpGet("read/{smartMeterId}")]
        public ObjectResult GetReading(string smartMeterId)
        {
            return new OkObjectResult(_meterReadingService.GetReadings(smartMeterId));
        }
    }
}
