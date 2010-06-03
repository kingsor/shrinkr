namespace Shrinkr.Infrastructure
{
    public interface IBackgroundService
    {
        string Name
        {
            get;
        }

        bool IsRunning
        {
            get;
        }

        void Start();

        void Stop();
    }
}