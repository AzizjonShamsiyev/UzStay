using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using UzStay.Api.Brokers.Storages;

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
            services.AddDbContext<StorageBroker>();
            services.AddControllers();


            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc(
                    name: "v1", 
                    info : apiInfo); 
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
    }
}
