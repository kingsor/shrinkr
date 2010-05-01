namespace Shrinkr.Infrastructure
{
    public interface IBaseX
    {
        string Encode(long value);

        long Decode(string value);

        bool IsValid(string value);
    }
}