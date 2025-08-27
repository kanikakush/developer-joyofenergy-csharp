using JOIEnergy.Interface;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JOIEnergy
{
    public class Startup
    {
        //private const string MOST_EVIL_PRICE_PLAN_ID = "price-plan-0";
        //private const string RENEWABLES_PRICE_PLAN_ID = "price-plan-1";
        //private const string STANDARD_PRICE_PLAN_ID = "price-plan-2";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var readings =
            //    GenerateMeterElectricityReadings();

            //var pricePlans = new List<PricePlan> {
            //    new PricePlan{
            //        PlanName = MOST_EVIL_PRICE_PLAN_ID,
            //        EnergySupplier = Enums.Supplier.DrEvilsDarkEnergy,
            //        UnitRate = 10m,
            //        PeakTimeMultiplier = new List<PeakTimeMultiplier>()
            //    },
            //    new PricePlan{
            //        PlanName = RENEWABLES_PRICE_PLAN_ID,
            //        EnergySupplier = Enums.Supplier.TheGreenEco,
            //        UnitRate = 2m,
            //        PeakTimeMultiplier = new List<PeakTimeMultiplier>()
            //    },
            //    new PricePlan{
            //        PlanName = STANDARD_PRICE_PLAN_ID,
            //        EnergySupplier = Enums.Supplier.PowerForEveryone,
            //        UnitRate = 1m,
            //        PeakTimeMultiplier = new List<PeakTimeMultiplier>()
            //    }
            //};

            // services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddControllers();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IMeterReadingService, MeterReadingService>();
            services.AddScoped<IPricePlanService, PricePlanService>();

            // Register repositories (in-memory for now, replaceable later)
            // services.AddSingleton<IPricePlanRepository, InMemoryPricePlanRepository>();
            //services.AddSingleton<IMeterReadingRepository, InMemoryMeterReadingRepository>();
            //services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();

            // Seed initial data (move seeding logic out of Startup)
            services.AddSingleton<IDataSeeder, DataSeeder>();

            //services.AddTransient<IAccountService, AccountService>();
            //services.AddTransient<IMeterReadingService, MeterReadingService>();
            //services.AddSingleton(DataSeeder.GetPricePlans());
            //services.AddSingleton(DataSeeder.GetSmartMeterAccounts());
            //services.AddTransient<IPricePlanService, PricePlanService>();
            //services.AddSingleton((IServiceProvider arg) => readings);
            //services.AddSingleton((IServiceProvider arg) => pricePlans);
            //services.AddSingleton((IServiceProvider arg) => DataSeeder.GetSmartMeterAccounts());
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseMvc();
        }
        //public interface IPricePlanRepository
        //{
        //    IEnumerable<PricePlan> GetPricePlans();
        //}

        //public class InMemoryPricePlanRepository : IPricePlanRepository
        //{
        //    private readonly List<PricePlan> _plans = new()
        //{
        //    new PricePlan { PlanName = "price-plan-0", EnergySupplier = Enums.Supplier.DrEvilsDarkEnergy, UnitRate = 10m },
        //    new PricePlan { PlanName = "price-plan-1", EnergySupplier = Enums.Supplier.TheGreenEco, UnitRate = 2m },
        //    new PricePlan { PlanName = "price-plan-2", EnergySupplier = Enums.Supplier.PowerForEveryone, UnitRate = 1m }
        //};

        //    public IEnumerable<PricePlan> GetPricePlans() => _plans;
        //}

    }
}
