namespace Serilog.Enrichers.Sensitive
{
    public class MaskingItem
    {
        public string MatchingRegex { get; set; }
        public string ReplacementText { get; set; }
    }
}
