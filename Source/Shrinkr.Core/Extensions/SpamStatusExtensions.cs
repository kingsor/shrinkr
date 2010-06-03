namespace Shrinkr.DomainObjects
{
    public static class SpamStatusExtensions
    {
        public static bool IsBadWord(this SpamStatus instance)
        {
            return (instance & SpamStatus.BadWord) == SpamStatus.BadWord;
        }

        public static bool IsPhishing(this SpamStatus instance)
        {
            return (instance & SpamStatus.Phishing) == SpamStatus.Phishing;
        }

        public static bool IsMalware(this SpamStatus instance)
        {
            return (instance & SpamStatus.Malware) == SpamStatus.Malware;
        }

        public static bool IsSpam(this SpamStatus instance)
        {
            return IsBadWord(instance) || IsPhishing(instance) || IsMalware(instance);
        }

        public static bool IsClean(this SpamStatus instance)
        {
            return (instance & SpamStatus.Clean) == SpamStatus.Clean;
        }
    }
}