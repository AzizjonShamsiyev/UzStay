using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using UzStay.Api.Brokers.Logging;
using UzStay.Api.Brokers.Storages;
using UzStay.Api.Services.Foundations.Guests;

namespace UzStay.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        OpenApiInfo apiInfo = new OpenApiInfo { Title = "UzStay.Api", Version = "v1" };

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<StorageBroker>();
            AddBrokers(services);
            AddFoundationServices(services);

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc(
                    name: "v1",
                    info: apiInfo);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(option => option.SwaggerEndpoint(
                    url: "/swagger/v1/swagger.json",
                    name: "UzStay.Api v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                endpoints.MapControllers());
        }

        private void AddBrokers(IServiceCollection services)
        {
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
        }

        private void AddFoundationServices(IServiceCollection services) =>
            services.AddTransient<IGuestService, GuestService>();
    }
}
