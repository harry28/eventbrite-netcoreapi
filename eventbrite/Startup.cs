using System.Reflection;
using AutoMapper;
using eventbrite.Extensions;
using eventbrite.Middleware;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace eventbrite
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public ILoggerFactory LoggerFactory { get; set; }

        public Startup(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            //Configure App Settings
            services.ConfigureAppSettings(Configuration);

            //Configure Dependencies
            services.ConfigureDependencies();

            //Configure CORS
            services.ConfigureCors();

            //Configure Fluent Services
            services.ConfigureFluentService();

            // Note .AddMiniProfiler() returns a IMiniProfilerBuilder for easy intellisense
            // Initiate before swagger
            services.ConfigureProfilerService();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.ConfigureSwaggerService();

            //Add AutoMapper
            services.ConfigureAutoMapper();

            //Add MediatR
            services.AddMediatR(typeof(Startup));

            services.AddMvc(options =>
                {
                    //Avoid Cross-Site Request Forgery added on 2/25/2018
                    //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseCors("CorsPolicy");

            //Use StaticFiles
            app.UseStaticFiles();

            // Showing Profiler when development
            //if (env.IsDevelopment())
            {
                // profiling, url to see last profile check: http://localhost:xxxxx/profiler/results
                // Initiate before swagger
                app.UseMiniProfiler();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "eventbrite V1");
                c.RoutePrefix = string.Empty;
                // this custom html has miniprofiler integration
                c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("eventbrite.wwwroot.SwaggerIndex.html");
            });

            
            //Keep UseMvc at the bottom
            app.UseMvc();
        }
    }
}
