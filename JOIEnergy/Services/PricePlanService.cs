using JOIEnergy.Domain;
using JOIEnergy.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JOIEnergy.Services
{
    public class PricePlanService : IPricePlanService
    {
        public interface Debug { void Log(string s); };

        private IMeterReadingService _meterReadingService;
        private readonly IDataSeeder _dataSeeder;
        public PricePlanService(IMeterReadingService meterReadingService, IDataSeeder dataSeeder)
        {
            _meterReadingService = meterReadingService;
            _dataSeeder = dataSeeder;
        }
        public Dictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterId)
        {
            List<ElectricityReading> electricityReadings = _meterReadingService.GetReadings(smartMeterId);

            if (!electricityReadings.Any())
            {
                return new Dictionary<string, decimal>();
            }
            return _dataSeeder.GetPricePlans().ToDictionary(plan => plan.PlanName, plan => calculateCost(electricityReadings, plan));
        }
        private decimal calculateCost(List<ElectricityReading> electricityReadings, PricePlan pricePlan)
        {
            var average = calculateAverageReading(electricityReadings);
            var timeElapsed = calculateTimeElapsed(electricityReadings);
            var averagedCost = average / timeElapsed;
            return Math.Round(averagedCost * pricePlan.UnitRate, 3);
        }
        private decimal calculateAverageReading(List<ElectricityReading> electricityReadings)
        {
            var newSummedReadings = electricityReadings.Select(readings => readings.Reading).Aggregate((reading, accumulator) => reading + accumulator);

            return newSummedReadings / electricityReadings.Count();
        }
        private decimal calculateTimeElapsed(List<ElectricityReading> electricityReadings)
        {
            var first = electricityReadings.Min(reading => reading.Time);
            var last = electricityReadings.Max(reading => reading.Time);

            return (decimal)(last - first).TotalHours;
        }

    }
}
