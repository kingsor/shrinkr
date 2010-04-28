namespace Shrinkr.Services
{
    using System;
    using DataTransferObjects;
    using DomainObjects;
    using Extensions;
    using Infrastructure;
    using Repositories;

    public class UserService : IUserService
    {
        private readonly Settings settings;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IEventAggregator eventAggregator;

        public UserService(Settings settings, IUserRepository userRepository, IUnitOfWork unitOfWork, IEventAggregator eventAggregator)
        {
            Check.Argument.IsNotNull(settings, "settings");
            Check.Argument.IsNotNull(userRepository, "userRepository");
            Check.Argument.IsNotNull(unitOfWork, "unitOfWork");
            Check.Argument.IsNotNull(eventAggregator, "eventAggregator");

            this.settings = settings;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.eventAggregator = eventAggregator;
        }

        public UserDTO GetByName(string name)
        {
            User user = userRepository.GetByName(name);

            return (user != null) ? new UserDTO(user) : null;
        }

        public UserResult Save(string userName, string email)
        {
            UserResult result = Validation.Validate<UserResult>(() => string.IsNullOrWhiteSpace(userName), "userName", TextMessages.UserNameCannotBeBlank)
                                          .And(() => !string.IsNullOrWhiteSpace(email) && !email.IsEmail(), "email", TextMessages.EmailAddressIsNotInCorrectFormat)
                                          .Result();

            if (result.RuleViolations.IsEmpty())
            {
                User user = userRepository.GetByName(userName);
                User tempUser = user;

                result = Validation.Validate<UserResult>(() => (tempUser != null) && tempUser.IsLockedOut, "userName", TextMessages.UserIsCurrentlyLockedOut.FormatWith(userName)).Result();

                if (result.RuleViolations.IsEmpty())
                {
                    bool isNewUser = false;

                    if (user == null)
                    {
                        user = new User { Name = userName };

                        if (settings.Api.Allowed)
                        {
                            user.AllowApiAccess(settings.Api.DailyLimit);
                        }

                        userRepository.Add(user);

                        isNewUser = true;
                    }

                    if (!string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(user.Email))
                    {
                        user.Email = email.ToLower(Culture.Invariant);
                    }

                    user.LastActivityAt = SystemTime.Now();

                    unitOfWork.Commit();

                    if (isNewUser)
                    {
                        eventAggregator.GetEvent<UserCreatedEvent>().Publish(new EventArgs<User>(user));
                    }

                    result = new UserResult(new UserDTO(user));
                }
            }

            return result;
        }

        public UserResult UpdateLastActivity(string userName)
        {
            UserResult result = Validation.Validate<UserResult>(() => string.IsNullOrWhiteSpace(userName), "userName", TextMessages.UserNameCannotBeBlank).Result();

            if (result.RuleViolations.IsEmpty())
            {
                User user = userRepository.GetByName(userName);

                result = Validation.Validate<UserResult>(() => user == null, "userName", TextMessages.UserDoesNotExist.FormatWith(userName))
                                   .Or(() => user.IsLockedOut, "userName", TextMessages.UserIsCurrentlyLockedOut.FormatWith(userName))
                                   .Result();

                if (result.RuleViolations.IsEmpty())
                {
                    user.LastActivityAt = SystemTime.Now();
                    unitOfWork.Commit();

                    result = new UserResult(new UserDTO(user));
                }
            }

            return result;
        }

        public UserResult RegenerateApiKey(string userName)
        {
            UserResult result = Validation.Validate<UserResult>(() => string.IsNullOrWhiteSpace(userName), "userName", TextMessages.UserNameCannotBeBlank)
                                          .Result();

            if (result.RuleViolations.IsEmpty())
            {
                User user = userRepository.GetByName(userName);

                result = Validation.Validate<UserResult>(() => user == null, "apiKey", TextMessages.UserDoesNotExist.FormatWith(userName))
                                   .Or(() => user.IsLockedOut, "userName", TextMessages.UserIsCurrentlyLockedOut.FormatWith(userName))
                                   .Result();

                if (result.RuleViolations.IsEmpty())
                {
                    try
                    {
                        user.GenerateApiKey();
                        unitOfWork.Commit();

                        result = new UserResult(new UserDTO(user));
                    }
                    catch (InvalidOperationException e)
                    {
                        result.RuleViolations.Add(new RuleViolation("userName", e.Message));
                    }
                }
            }

            return result;
        }
    }
}