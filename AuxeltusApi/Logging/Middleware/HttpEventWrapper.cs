using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Auxeltus.Api.Middleware
{
    public class HttpEventWrapper
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public HttpEvent Type { get; set; }
        public string Content { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string QueryParameters { get; set; }
        public int? StatusCode { get; set; }

        public HttpEventWrapper(HttpContext context, string content, HttpEvent type)
        {
            Type = type;
            Content = content;
            Method = context.Request.Method;
            Path = context.Request.Path;
            QueryParameters = context.Request.QueryString.Value;

            if (type == HttpEvent.Response)
            {
                StatusCode = context.Response.StatusCode;
            }
        }
    }
}
