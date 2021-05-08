using CovidonusApiV2.Repositories;
using CovidonusApiV2.Repositories.Abstraction;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CovidonusApiV2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            CoreRepository.NewsApiKey = Configuration.GetValue<string>("newsKey");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(
            //        builder =>
            //        {
            //            builder.WithOrigins("http://localhost:56773/");
            //        });
            //});
            services.AddControllers();
            services.AddHangfire(c => c.UseMemoryStorage());
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "CovidonusApiV2", Version = "v1" }));
            services.AddSingleton<ISeedDataRepository, SeedDataRepository>();
            services.AddSingleton<ICovidRepository, CovidRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISeedDataRepository repository)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
            builder.WithOrigins("http://localhost:56773")
            .AllowAnyMethod()
            .AllowAnyHeader());
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Covidonus Api"));
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            RecurringJob.AddOrUpdate("Covid", () => repository.RefreshCovidDataAsync(true), Cron.Hourly);
        }
    }
}
