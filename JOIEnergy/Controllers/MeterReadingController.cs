using JOIEnergy.Domain;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Mvc;
using System;

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
            try
            {
                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult("Invalid input" + ModelState);
                }
                var result = _meterReadingService.StoreReadings(meterReadings.SmartMeterId, meterReadings.ElectricityReadings);
                return new OkObjectResult(result);
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult("Internal Server Error" + ex.Message);
            }
        }

        [HttpGet("read/{smartMeterId}")]
        public IActionResult GetReading(string smartMeterId)
        {
            try
            {
                var readings = _meterReadingService.GetReadings(smartMeterId);
                if(readings == null || readings.Count == 0)
                {
                    return new NotFoundObjectResult("No Readings found for the Smart Meter Id: " + smartMeterId);
                }
                return new OkObjectResult(readings);
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult("Internal Server Error" + ex.Message);
            }
        }
    }
}
