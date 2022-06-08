using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Auxeltus.Api.Models
{
    public class Error
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorType Type { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public string Field { get; set; }

        public Error(ErrorType type, int code, string message, string field)
        {
            Type = type;
            Code = code;
            Message = message;
            Field = field;
        }
        public Error(ErrorType type, int code, string message)
        {
            Type = type;
            Code = code;
            Message = message;
        }
    }
}
