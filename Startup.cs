using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace DNS_Site {
    public class Startup {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger) {
            if (env.IsDevelopment()) {
                logger.LogInformation("Starting development server...");
                app.UseDeveloperExceptionPage();
            }
            else {
                logger.LogInformation("Starting production server...");
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            // Routing configuration
            app.UseRouting();

            // Handling request
            app.Use(async (context, next) =>
            {
                logger.LogInformation("Start");
                await next.Invoke();
                logger.LogInformation("End");
            });

            // Using controllers
            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}"
                    //defaults: new { controller = "Default", action = "Index" }
                );
            });

            app.Use(async (context, next) =>
            {
                logger.LogInformation("Not found! " + context.Request.Path.ToString());
                // If request is API
                if (Regex.Matches(context.Request.Path.ToString(), "^/api/.*").Count != 0)
                {
                    context.Response.StatusCode = 400; // Bad Request
                    await context.Response.WriteAsync("API function not found");
                }
                // If request is not API
                else
                {
                    // To SPA middleware
                    await next.Invoke();
                }
            });

            // Sending SPA
            app.UseSpa(spa => {
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment()) {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
