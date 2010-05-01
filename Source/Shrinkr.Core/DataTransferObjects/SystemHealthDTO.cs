namespace Shrinkr.DataTransferObjects
{
    public class SystemHealthDTO
    {
        public SystemHealthDTO(int urlCreated, int urlVisited, int userCreated, int userVisited)
        {
            UrlCreated = urlCreated;
            UrlVisited = urlVisited;
            UserCreated = userCreated;
            UserVisited = userVisited;
        }

        public int UrlCreated
        {
            get;
            private set;
        }

        public int UrlVisited
        {
            get;
            private set;
        }

        public int UserCreated
        {
            get;
            private set;
        }

        public int UserVisited
        {
            get;
            private set;
        }
    }
}