using JOIEnergy.Domain;
using System.Collections.Generic;

namespace JOIEnergy.Interface
{
    public interface IDataSeeder
    {
        IReadOnlyCollection<PricePlan> GetPricePlans();
        IReadOnlyDictionary<string, string> GetSmartMeterAccounts();
        IReadOnlyDictionary<string, List<ElectricityReading>> GetElectricityReadings();
        // New method to mutate data
        void AddReading(string smartMeterId, ElectricityReading electricityReading);
        void AddReadings(string smartMeterId, IEnumerable<ElectricityReading> electricityReadings);
    }
}
