using System.Collections.Generic;
using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    public interface IMeterReadingService
    {
        List<ElectricityReading> GetReadings(string smartMeterId);
        IReadOnlyDictionary<string, List<ElectricityReading>> StoreReadings(string smartMeterId, List<ElectricityReading> electricityReadings);
    }
}