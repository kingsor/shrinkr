namespace Shrinkr.UnitTests
{
    using DomainObjects;
    using Infrastructure;
    using Repositories;
    using Services;

    using Moq;
    using Xunit;

    public class UserServiceTests
    {
        private const string UserName = "http://kazimanzurrashid.myopenid.com";
        private const string UserEmail = "kazimanzurrashid@gmail.com";

        private readonly Mock<IUserRepository> userRepository;
        private readonly Mock<IUnitOfWork> unitOfWork;

        private readonly Mock<UserCreatedEvent> userCreatedEvent;

        private readonly UserService userService;

        public UserServiceTests()
        {
            userRepository = new Mock<IUserRepository>();
            unitOfWork = new Mock<IUnitOfWork>();

            userCreatedEvent = new Mock<UserCreatedEvent>();

            var eventAggregator = new Mock<IEventAggregator>();
            eventAggregator.Setup(ea => ea.GetEvent<UserCreatedEvent>()).Returns(userCreatedEvent.Object);

            var settings = new Settings { Api = new ApiSettings(true, 5) };

            userService = new UserService(settings, userRepository.Object, unitOfWork.Object, eventAggregator.Object);
        }

        [Fact]
        public void Save_should_add_user_if_user_is_new()
        {
            userRepository.Setup(repository => repository.Add(It.IsAny<User>())).Verifiable();

            userService.Save(UserName, UserEmail);

            userRepository.Verify();
        }

        [Fact]
        public void Seve_should_set_api_settings_when_user_is_new()
        {
            var result = userService.Save(UserName, UserEmail);

            Assert.True(result.User.ApiAccessAllowed);
            Assert.Equal(5, result.User.ApiDailyLimit);
            Assert.NotEqual(string.Empty, result.User.ApiKey);
        }

        [Fact]
        public void Save_should_update_email_when_email_is_blank()
        {
            var user = CreateUser();

            userRepository.Setup(repository => repository.GetByName(It.IsAny<string>())).Returns(user.Object);
            user.SetupSet(u => u.Email).Verifiable();

            userService.Save(UserName, UserEmail);

            user.Verify();
        }

        [Fact]
        public void Save_should_update_user_last_activity_at()
        {
            var user = CreateUser();

            userRepository.Setup(repository => repository.GetByName(It.IsAny<string>())).Returns(user.Object);
            user.SetupSet(u => u.Email).Verifiable();

            userService.Save(UserName, UserEmail);

            user.Verify();
        }

        [Fact]
        public void Save_should_publish_new_user_created_event_when_user_is_new()
        {
            userCreatedEvent.Setup(evt => evt.Publish(It.IsAny<EventArgs<User>>())).Verifiable();

            userService.Save(UserName, UserEmail);

            userCreatedEvent.Verify();
        }

        [Fact]
        public void Save_should_persist_changes()
        {
            userRepository.Setup(repository => repository.GetByName(It.IsAny<string>())).Returns(CreateUser().Object);
            unitOfWork.Setup(uow => uow.Commit()).Verifiable();

            userService.Save(UserName, UserEmail);

            unitOfWork.Verify();
        }

        [Fact]
        public void UpdateLastActivity_should_update_user_last_activity_at()
        {
            var user = CreateUser();

            userRepository.Setup(repository => repository.GetByName(It.IsAny<string>())).Returns(user.Object);

            user.SetupSet(u => u.LastActivityAt).Verifiable();

            userService.UpdateLastActivity(UserName);

            user.Verify();
        }

        [Fact]
        public void UpdateLastActivity_should_persist_changes()
        {
            userRepository.Setup(repository => repository.GetByName(It.IsAny<string>())).Returns(CreateUser().Object);
            
            userService.UpdateLastActivity(UserName);

            unitOfWork.Verify(uow => uow.Commit());
        }

        [Fact]
        public void RegenerateApiKey_should_generate_api_key_for_user()
        {
            Mock<User> user = CreateUser();

            userRepository.Setup(repository => repository.GetByName(It.IsAny<string>())).Returns(user.Object);

            userService.RegenerateApiKey(UserName);

            user.Verify(u => u.GenerateApiKey(), Times.Once());
        }

        [Fact]
        public void RegenerateApiKey_should_persist_changes()
        {
            Mock<User> user = CreateUser();

            userRepository.Setup(repository => repository.GetByName(It.IsAny<string>())).Returns(user.Object);

            userService.RegenerateApiKey(UserName);

            unitOfWork.Verify(uow => uow.Commit());
        }

        private static Mock<User> CreateUser()
        {
            var user = new Mock<User>();

            user.SetupGet(u => u.ApiSetting).Returns(new ApiSetting());

            return user;
        }
    }
}