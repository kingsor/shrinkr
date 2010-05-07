namespace Shrinkr.Web
{
    public class CreateShortUrlApiCommand : ApiCommand
    {
        public string Url
        {
            get;
            set;
        }

        public string Alias
        {
            get;
            set;
        }
    }
}