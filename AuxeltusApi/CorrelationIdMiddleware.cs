using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Enrichers.Sensitive;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Microsoft.Extensions.Primitives;

namespace Auxeltus.Api.Middleware
{
    public class CorrelationIdMiddleware
    {
        private const string correlationIdKey = "X-Correlation-ID";
        private const string responseCorrelationIdKey = "CorrelationId";


        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new NullReferenceException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new NullReferenceException(nameof(httpContext));
            }

            string correlationId = null;

            if (httpContext.Request.Headers.TryGetValue(correlationIdKey, out StringValues correlationIdHeader))
            {
                correlationId = correlationIdHeader;
            } 
            else
            {
                correlationId = Guid.NewGuid().ToString("N");
            }
            
            httpContext.Response.Headers[responseCorrelationIdKey] = correlationId;

            await _next(httpContext);
        }

    }
}
