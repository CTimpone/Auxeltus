namespace Serilog.Enrichers.Sensitive
{
    public static class LogMaskingConstants
    {
        private const string creditCardMatch = "(\\d{11,12})(\\d{4})"; //obviously way looser than any actual credit card Regex
        private const string creditCardReplacement = "****$2";

        public static readonly MaskingItem CreditCardMasking = new MaskingItem
        {
            MatchingRegex = creditCardMatch,
            ReplacementText = creditCardReplacement
        };
    }
}
