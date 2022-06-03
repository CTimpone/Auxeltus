using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Enrichers.Sensitive;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Auxeltus.Api.Middleware
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

            using (LogContext.PushProperty("CorrelationId", httpContext.Response.Headers["CorrelationId"]))
            {
                httpContext.Request.EnableBuffering();
                string requestContent = await new StreamReader(httpContext.Request.Body, Encoding.UTF8).ReadToEndAsync().ConfigureAwait(false);
                httpContext.Request.Body.Position = 0;

                MaskingItem[] ContentMaskingRules = GenerateMaskingRulesForMethod(httpContext.Request.Path.Value);

                if (ContentMaskingRules.Length > 0)
                {
                    using (_logger.EnterSensitiveArea(ContentMaskingRules))
                    {
                        _logger.LogInformation(JsonConvert.SerializeObject(new HttpEventWrapper(httpContext, requestContent, HttpEvent.Request)));
                    }
                }
                else
                {
                    _logger.LogInformation(JsonConvert.SerializeObject(new HttpEventWrapper(httpContext, requestContent, HttpEvent.Request)));
                }

                Stopwatch sw = Stopwatch.StartNew();
                try
                {
                    using (var swapStream = new MemoryStream())
                    {
                        var originalResponseBody = httpContext.Response.Body;

                        httpContext.Response.Body = swapStream;

                        await _next(httpContext);

                        sw.Stop();

                        swapStream.Seek(0, SeekOrigin.Begin);
                        string responseBody = new StreamReader(swapStream).ReadToEnd();
                        swapStream.Seek(0, SeekOrigin.Begin);

                        await swapStream.CopyToAsync(originalResponseBody);
                        httpContext.Response.Body = originalResponseBody;

                        using (LogContext.PushProperty("ElapsedMs", sw.Elapsed))
                        {
                            if (ContentMaskingRules.Length > 0)
                            {
                                using (_logger.EnterSensitiveArea(ContentMaskingRules))
                                {
                                    LogResponse(httpContext, responseBody);
                                }
                            }
                            else
                            {
                                LogResponse(httpContext, responseBody);
                            }
                        }
                    }
                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex, $"Exception thrown in {nameof(SerilogMiddleware)}");
                }
            }

        }

        private MaskingItem[] GenerateMaskingRulesForMethod(string path)
        {
            //Control request/response log masking based on path, so that everything that needs masked is, but nothing else
            switch (path)
            {
                //Restrictive log masking by default for unconfigured paths
                default:
                    return new MaskingItem[] { LogMaskingConstants.CreditCardMasking };
            }
        }

        private void LogResponse(HttpContext context, string body)
        {
            if (context.Response.StatusCode < 400)
            {
                _logger.LogInformation(JsonConvert.SerializeObject(new HttpEventWrapper(context, body,
                    HttpEvent.Response)));
            }
            else
            {
                _logger.LogError(JsonConvert.SerializeObject(new HttpEventWrapper(context, body,
                    HttpEvent.Response)));
            }
        }
    }
}