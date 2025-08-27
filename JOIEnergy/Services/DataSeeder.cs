using JOIEnergy.Const;
using JOIEnergy.Domain;
using JOIEnergy.Generator;
using JOIEnergy.Interface;
using System.Collections.Generic;
using System.Linq;

namespace JOIEnergy.Services
{
    public class DataSeeder : IDataSeeder
    {
        private readonly List<PricePlan> _pricePlans;
        private readonly Dictionary<string, string> _smartMeterAccounts;
        private readonly Dictionary<string, List<ElectricityReading>> _electricityReadings;
        public DataSeeder()
        {
            _pricePlans = SeedPricePlans();
            _smartMeterAccounts = SeedSmartMeterAccounts();
            _electricityReadings = SeedElectricityReadings(_smartMeterAccounts.Keys);
        }
        public IReadOnlyCollection<PricePlan> GetPricePlans() => _pricePlans;
        public IReadOnlyDictionary<string, string> GetSmartMeterAccounts() => _smartMeterAccounts;
        public IReadOnlyDictionary<string, List<ElectricityReading>> GetElectricityReadings() => _electricityReadings;
        public void AddReading(string smartMeterId, ElectricityReading electricityReading)
        {
            if (!_electricityReadings.ContainsKey(smartMeterId))
            {
                _electricityReadings[smartMeterId] = new List<ElectricityReading>();
            }
            _electricityReadings[smartMeterId].Add(electricityReading);
        }
        public void AddReadings(string smartMeterId, IEnumerable<ElectricityReading> electricityReadings)
        {
            if (!_electricityReadings.ContainsKey(smartMeterId))
            {
                _electricityReadings[smartMeterId] = new List<ElectricityReading>();
            }
            _electricityReadings[smartMeterId].AddRange(electricityReadings);
        }
        private static List<PricePlan> SeedPricePlans()
        {
            return new List<PricePlan>
            {
                new PricePlan
                {
                    PlanName = PricePlanName.plan0,
                    EnergySupplier = Enums.Supplier.DrEvilsDarkEnergy,
                    UnitRate = 10m,
                    PeakTimeMultiplier = new List<PeakTimeMultiplier>()
                },
                new PricePlan
                {
                    PlanName = PricePlanName.plan1,
                    EnergySupplier = Enums.Supplier.TheGreenEco,
                    UnitRate = 2m,
                    PeakTimeMultiplier = new List<PeakTimeMultiplier>()
                },
                new PricePlan
                {
                    PlanName = PricePlanName.plan2,
                    EnergySupplier = Enums.Supplier.PowerForEveryone,
                    UnitRate = 1m,
                    PeakTimeMultiplier = new List<PeakTimeMultiplier>()
                },
            };
        }
        private static Dictionary<string, string> SeedSmartMeterAccounts()
        {
            return new Dictionary<string, string>
            {
                { "smart-meter-0", "user-0" },
                { "smart-meter-1", "user-1" },
                { "smart-meter-2", "user-2" },
                { "smart-meter-3", "user-3" },
                { "smart-meter-4", "user-4" },
            };
        }
        private static Dictionary<string, List<ElectricityReading>> SeedElectricityReadings(IEnumerable<string> smartMeterIds)
        {
            var generator = new ElectricityReadingGenerator();
            return smartMeterIds.ToDictionary(
                id => id,
                id => generator.Generate(20).ToList()
            );
        }
    }
}
