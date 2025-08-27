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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddControllers();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IMeterReadingService, MeterReadingService>();
            services.AddScoped<IPricePlanService, PricePlanService>();
           
            // Seed initial data (move seeding logic out of Startup)
            services.AddSingleton<IDataSeeder, DataSeeder>();
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
    }
}
