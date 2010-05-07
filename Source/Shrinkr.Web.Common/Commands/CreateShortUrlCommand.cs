namespace Shrinkr.Web
{
    public class CreateShortUrlCommand : UserCommand
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