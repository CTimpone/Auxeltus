using Auxeltus.AccessLayer.Sql;
using Auxeltus.Api.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Enrichers.Sensitive;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System;

namespace Auxeltus.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json");
            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.Async(a => a.Console(new RenderedCompactJsonFormatter()))
                .Enrich.FromLogContext()
                .Enrich.WithSensitiveDataMasking()
                .CreateLogger();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AuxeltusSqlContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("Auxeltus_SQLConnectionString")));

            services.AddTransient<IRoleCommand, RoleCommand>();
            services.AddTransient<IRoleQuery, RoleQuery>();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Auxeltus API",
                    Version = "v1",
                    Description = "The API that powers the Auxeltus HR platform.",
                    Contact = new OpenApiContact
                    {
                        Name = "Chris Timpone",
                        Email = string.Empty,
                        Url = new Uri("https://github.com/CTimpone/Auxeltus/"),
                    },
                });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<SerilogMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auxeltus API V1");
            });
        }
    }
}
