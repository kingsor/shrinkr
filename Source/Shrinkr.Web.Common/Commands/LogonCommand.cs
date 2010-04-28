namespace Shrinkr.Web
{
    public class LogOnCommand
    {
        public string UserName
        {
            get; set;
        }

        public bool? RememberMe
        {
            get;
            set;
        }

        public string ReturnUrl
        {
            get;
            set;
        }
    }
}