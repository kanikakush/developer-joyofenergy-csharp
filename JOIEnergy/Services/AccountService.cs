using JOIEnergy.Interface;

namespace JOIEnergy.Services
{
    public class AccountService : IAccountService
    {
        private readonly IDataSeeder _dataSeeder;
        public AccountService(IDataSeeder dataSeeder) {
            _dataSeeder = dataSeeder;
        }
        public string GetPricePlanIdForSmartMeterId(string smartMeterId)
        { 
            var smartMeter = _dataSeeder.GetSmartMeterAccounts();
            if (!smartMeter.ContainsKey(smartMeterId))
            {
                return null;
            }
            return smartMeter[smartMeterId];
        }
    }
}
