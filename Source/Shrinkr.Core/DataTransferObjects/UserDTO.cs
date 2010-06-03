namespace Shrinkr.DataTransferObjects
{
    using System;

    using DomainObjects;

    public class UserDTO
    {
        public UserDTO(User user)
        {
            Check.Argument.IsNotNull(user, "user");

            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            IsLockedOut = user.IsLockedOut;
            CreatedAt = user.CreatedAt;
            LastActivityAt = user.LastActivityAt;
            Role = user.Role;

            ApiAccessAllowed = user.ApiSetting.Allowed.GetValueOrDefault();
            ApiKey = user.ApiSetting.Key;
            ApiDailyLimit = user.ApiSetting.DailyLimit;
            CanAccessApi = user.CanAccessApi;

            if (CanAccessApi)
            {
                HasExceededApiDailyLimit = user.HasExceededDailyLimit();
            }
        }

        public long Id
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Email
        {
            get;
            private set;
        }

        public bool IsLockedOut
        {
            get;
            private set;
        }

        public DateTime CreatedAt
        {
            get;
            private set;
        }

        public DateTime LastActivityAt
        {
            get;
            private set;
        }

        public Role Role
        {
            get;
            private set;
        }

        public bool ApiAccessAllowed
        {
            get;
            private set;
        }

        public string ApiKey
        {
            get;
            private set;
        }

        public int? ApiDailyLimit
        {
            get;
            private set;
        }

        public bool CanAccessApi
        {
            get;
            private set;
        }

        public bool HasExceededApiDailyLimit
        {
            get;
            private set;
        }
    }
}