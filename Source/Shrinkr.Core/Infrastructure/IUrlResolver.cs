namespace Shrinkr.Infrastructure
{
    public interface IUrlResolver
    {
        string ApplicationRoot
        {
            get;
        }

        string Preview(string aliasName);

        string Visit(string aliasName);

        string Absolute(string relativeUrl);
    }
}