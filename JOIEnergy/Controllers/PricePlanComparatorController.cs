using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Mvc;

namespace JOIEnergy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PricePlanComparatorController : Controller
    {
        //private readonly string PRICE_PLAN_ID_KEY = Environment.GetEnvironmentVariable("PRICE_PLAN_ID_KEY");
        //private readonly string PRICE_PLAN_COMPARISONS_KEY = Environment.GetEnvironmentVariable("PRICE_PLAN_ID_KEY");
        private readonly IPricePlanService _pricePlanService;
        private readonly IAccountService _accountService;
        public PricePlanComparatorController(IPricePlanService pricePlanService, IAccountService accountService)
        {
            _pricePlanService = pricePlanService;
            _accountService = accountService;
        }

        [HttpGet("compare-all/{smartMeterId}")]
        public IActionResult CalculatedCostForEachPricePlan(string smartMeterId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(smartMeterId))
                    return BadRequest("Smart Meter Id must be provided.");

                string pricePlanId = _accountService.GetPricePlanIdForSmartMeterId(smartMeterId);
                if (pricePlanId == null)
                    return new NotFoundObjectResult("No price Plan found for the Smart Meter Id: " + smartMeterId);

                Dictionary<string, decimal> costPerPricePlan = _pricePlanService.GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);
                if (!costPerPricePlan.Any())
                    return new NotFoundObjectResult(string.Format("Smart Meter ID ({0}) not found", smartMeterId));

                var response = new
                {
                    PricePlanId = pricePlanId,
                    Comparisons = costPerPricePlan
                };
                return Ok(response);
            }catch(Exception ex)
            {
                return new BadRequestObjectResult("Internal Server Error" + ex.Message);
            }  
        }

        [HttpGet("recommend/{smartMeterId}")]
        public IActionResult RecommendCheapestPricePlans(string smartMeterId, [FromQuery] int? limit = null)
        {
            if (string.IsNullOrWhiteSpace(smartMeterId))
                return BadRequest("Smart Meter Id must be provided.");

            var consumptionForPricePlans = _pricePlanService
                .GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);

            if (consumptionForPricePlans == null || !consumptionForPricePlans.Any())
                return NotFound($"No readings found for Smart Meter Id: {smartMeterId}");

            var recommendations = consumptionForPricePlans
                .OrderBy(cp => cp.Value)
                .Select(cp => new { PricePlanId = cp.Key, Cost = cp.Value });

            if (limit.HasValue && limit.Value > 0)
                recommendations = recommendations.Take(limit.Value);

            return Ok(recommendations);
        }
    }
}
