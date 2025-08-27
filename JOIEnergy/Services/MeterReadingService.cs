using JOIEnergy.Domain;
using JOIEnergy.Interface;
using System.Collections.Generic;

namespace JOIEnergy.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IDataSeeder _dataSeeder;
        public MeterReadingService(IDataSeeder dataSeeder)
        {
            _dataSeeder = dataSeeder;
        }
        public List<ElectricityReading> GetReadings(string smartMeterId)
        {
            var readings = _dataSeeder.GetElectricityReadings();
            return readings.ContainsKey(smartMeterId)
                ? readings[smartMeterId]
                : new List<ElectricityReading>();
        }
        public IReadOnlyDictionary<string, List<ElectricityReading>> StoreReadings(string smartMeterId, List<ElectricityReading> electricityReadings)
        {
            var readings = _dataSeeder.GetElectricityReadings();
            if (!readings.ContainsKey(smartMeterId))
            {
                _dataSeeder.AddReadings(smartMeterId, new List<ElectricityReading>());
            }
            electricityReadings.ForEach(electricityReading => readings[smartMeterId].Add(electricityReading));
            return readings;
        }
    }
}
