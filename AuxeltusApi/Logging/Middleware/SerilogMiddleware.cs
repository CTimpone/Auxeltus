using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Enrichers.Sensitive;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace AuxeltusApi.Logging.Middleware
{
    public class SerilogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SerilogMiddleware> _logger;

        public SerilogMiddleware(RequestDelegate next, ILogger<SerilogMiddleware> logger)
        {
            _next = next ?? throw new NullReferenceException(nameof(next));
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new NullReferenceException(nameof(httpContext));
            }

            using (LogContext.PushProperty("test", "different"))
            {
                Stopwatch sw = Stopwatch.StartNew();
                try
                {
                    await _next(httpContext);
                    sw.Stop();

                    //Control request/response log masking based on path
                    switch (httpContext.Request.Path)
                    {
                        //Restrictive log masking by default for unconfigured paths
                        default:
                            using (_logger.EnterSensitiveArea(LogMaskingConstants.CreditCardMasking))
                            {
                                _logger.LogInformation($"Request {httpContext.Request.Path}");
                            }
                            break;
                    }
                    //var statusCode = httpContext.Response?.StatusCode;
                    //var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                    //var log = level == LogEventLevel.Error ? LogForErrorContext(httpContext) : Log;
                    //log.Write(level, MessageTemplate, httpContext.Request.Method, GetPath(httpContext), statusCode, elapsedMs);
                }
                // Never caught, because `LogException()` returns false.
                catch (Exception ex) { }
                //when (LogException(httpContext, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()), ex)) { }
            }

        }

    }
}
