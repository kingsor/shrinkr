namespace Shrinkr.Services
{
    using DataTransferObjects;

    public interface IUserService
    {
        UserDTO GetByName(string name);

        UserResult Save(string userName, string email);

        UserResult UpdateLastActivity(string userName);

        UserResult RegenerateApiKey(string userName);
    }
}