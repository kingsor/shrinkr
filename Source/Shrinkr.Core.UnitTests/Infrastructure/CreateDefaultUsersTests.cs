namespace Shrinkr.UnitTests
{
    using Moq;
    using Xunit;

    public class CreateDefaultUsersTests
    {
        private readonly User user;
        private readonly Mock<IUserRepository> userRepository;
        private readonly Mock<IUnitOfWork> unitOfWork;

        private CreateDefaultUsers task;

        public CreateDefaultUsersTests()
        {
            user = new User { Name = "http://foobar.myopenid.com/" };
            userRepository = new Mock<IUserRepository>();
            unitOfWork = new Mock<IUnitOfWork>();

            task = new CreateDefaultUsers(new[] { user }, userRepository.Object, unitOfWork.Object);
        }

        [Fact]
        public void Execute_should_create_users_when_not_exists()
        {
            userRepository.Setup(r => r.GetByName(It.IsAny<string>())).Returns((User) null);
            userRepository.Setup(r => r.Add(It.IsAny<User>())).Verifiable();
            unitOfWork.Setup(uow => uow.Commit()).Verifiable();

            task.Execute();

            userRepository.Verify();
            unitOfWork.Verify();
        }
    }
}