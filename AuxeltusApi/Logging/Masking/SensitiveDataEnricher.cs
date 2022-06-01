using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Serilog.Core;
using Serilog.Events;
using Serilog.Parsing;

namespace Serilog.Enrichers.Sensitive
{
    internal class SensitiveDataEnricher : ILogEventEnricher
    {
        private static readonly MessageTemplateParser Parser = new MessageTemplateParser();
        private readonly FieldInfo _messageTemplateBackingField;

        public SensitiveDataEnricher(params MaskingItem[] maskingItems)
        {
            var fields = typeof(LogEvent).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            _messageTemplateBackingField = fields.SingleOrDefault(f => f.Name.Contains("<MessageTemplate>"));
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (SensitiveArea.Instance != null)
            {
                MaskingItem[] maskingItems = SensitiveArea.InstanceMaskingRules;

                var messageTemplateText = ReplaceSensitiveDataFromString(logEvent.MessageTemplate.Text,
                    maskingItems);

                _messageTemplateBackingField.SetValue(logEvent, Parser.Parse(messageTemplateText));

                foreach (var property in logEvent.Properties.ToList())
                {
                    if (property.Value is ScalarValue scalar && scalar.Value is string stringValue)
                    {
                        logEvent.AddOrUpdateProperty(
                            new LogEventProperty(
                                property.Key,
                                new ScalarValue(ReplaceSensitiveDataFromString(stringValue,
                                    maskingItems))));
                    }
                }
            }
        }

        private string ReplaceSensitiveDataFromString(string input, MaskingItem[] maskingItems)
        {
            foreach (var mask in maskingItems)
            {
                input = Regex.Replace(input, mask.MatchingRegex, mask.ReplacementText);
            }

            return input;
        }
    }
}
