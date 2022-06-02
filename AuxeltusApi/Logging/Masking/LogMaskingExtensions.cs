using Serilog.Configuration;

namespace Serilog.Enrichers.Sensitive
{
    public static class ExtensionMethods
    {
        public static SensitiveArea EnterSensitiveArea(this Microsoft.Extensions.Logging.ILogger logger, params MaskingItem[] maskingItems)
        {
            var sensitiveArea = new SensitiveArea(maskingItems);

            SensitiveArea.Instance = sensitiveArea;

            return sensitiveArea;
        }

        public static LoggerConfiguration WithSensitiveDataMasking(this LoggerEnrichmentConfiguration loggerConfiguration)
        {
            return loggerConfiguration
                .With(new SensitiveDataEnricher());
        }
    }
}
