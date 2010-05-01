namespace Shrinkr.Web
{
    using DomainObjects;
    using Extensions;

    public static class SpamStatusExtensions
    {
        public static string AsHtml(this SpamStatus instance)
        {
            return AsHtml(instance, null);
        }

        public static string AsHtml(this SpamStatus instance, string id)
        {
            const string Template = "<span{0}{1}>{2}</span>";

            string idAttribute = string.IsNullOrWhiteSpace(id) ? string.Empty : " id=\"{0}\"".FormatWith(id);

            string html;

            switch (instance)
            {
                case SpamStatus.BadWord:
                case SpamStatus.Phishing:
                case SpamStatus.Malware:
                    {
                        html = Template.FormatWith(idAttribute, " class=\"warningText\"", "Spam");
                        break;
                    }

                default:
                    {
                        html = Template.FormatWith(idAttribute, string.Empty, instance.AsDescriptiveText());
                        break;
                    }
            }

            return html;
        }
    }
}