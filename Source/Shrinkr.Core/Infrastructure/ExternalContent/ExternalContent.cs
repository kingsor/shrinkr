namespace Shrinkr.Infrastructure
{
    public class ExternalContent
    {
        public ExternalContent(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public string Title
        {
            get;
            private set;
        }

        public string Content
        {
            get;
            private set;
        }
    }
}