namespace Shrinkr.Infrastructure
{
    public interface IExternalContentService
    {
        ExternalContent Retrieve(string url);
    }
}