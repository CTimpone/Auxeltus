using Auxeltus.AccessLayer.Sql;
using Auxeltus.Api.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers.Sensitive;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            services.AddControllers().AddNewtonsoftJson();
            ;
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
        }
    }
}
