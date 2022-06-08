using System;

namespace Auxeltus.Api.Extensions
{
    public static class ExceptionExtensions
    {
        public static bool Contains(this Exception ex, string sub, bool caseSensitive = false)
        {
            return ex.Message.Contains(sub, !caseSensitive ?
                StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) ||
                (ex.InnerException != null && ex.InnerException.Contains(sub, caseSensitive));
        }
    }
}
