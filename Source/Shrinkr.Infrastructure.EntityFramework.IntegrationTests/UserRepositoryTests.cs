namespace Shrinkr.Infrastructure.EntityFramework.IntegrationTests
{
    using System;

    using Xunit;
    using Xunit.Extensions;

    public class UserRepositoryTests : DatabaseTest
    {
        private readonly UserRepository repository;

        public UserRepositoryTests()
        {
            repository = new UserRepository(databasefactory, queryFactory);
        }

        [Fact, AutoRollback]
        public void Should_be_able_to_add_user()
        {
            var user = ObjectMother.CreateUser();

            repository.Add(user);
            unitOfWork.Commit();

            Assert.NotEqual(0, user.Id);
        }

        [Fact, AutoRollback]
        public void Should_be_able_to_delete()
        {
            var user = ObjectMother.CreateUser();
            repository.Add(user);
            unitOfWork.Commit();
            repository.Delete(user);
            unitOfWork.Commit();

            Assert.Null(repository.GetById(user.Id));
        }

        [Fact, AutoRollback]
        public void Should_be_able_to_get_by_id()
        {
            var user = ObjectMother.CreateUser();

            repository.Add(user);
            unitOfWork.Commit();

            Assert.NotNull(repository.GetById(user.Id));
        }

        [Fact, AutoRollback]
        public void Should_be_able_to_get_by_api_key()
        {
            var user = ObjectMother.CreateUser();
            user.ApiSetting.Key = Guid.NewGuid().ToString();

            repository.Add(user);
            unitOfWork.Commit();

            Assert.NotNull(repository.GetByApiKey(user.ApiSetting.Key));
        }

        [Fact, AutoRollback]
        public void Should_be_able_to_count_created()
        {
            var user1 = ObjectMother.CreateUser();
            var user2 = ObjectMother.CreateUser();
            var user3 = ObjectMother.CreateUser();

            user1.CreatedAt = SystemTime.Now().AddDays(-3);
            user2.CreatedAt = SystemTime.Now().AddDays(-2);
            user3.CreatedAt = SystemTime.Now().AddDays(-1);

            repository.Add(user1);
            repository.Add(user2);
            repository.Add(user3);

            unitOfWork.Commit();

            var count = repository.GetCreatedCount(SystemTime.Now().AddDays(-4), SystemTime.Now());

            Assert.Equal(3, count);
        }

        [Fact, AutoRollback]
        public void Should_be_able_to_count_visited()
        {
            var user1 = ObjectMother.CreateUser();
            var user2 = ObjectMother.CreateUser();
            var user3 = ObjectMother.CreateUser();

            user1.LastActivityAt = SystemTime.Now().AddDays(-3);
            user2.LastActivityAt = SystemTime.Now().AddDays(-2);
            user3.LastActivityAt = SystemTime.Now().AddDays(-1);

            repository.Add(user1);
            repository.Add(user2);
            repository.Add(user3);

            unitOfWork.Commit();

            var count = repository.GetVisitedCount(SystemTime.Now().AddDays(-4), SystemTime.Now());

            Assert.Equal(3, count);
        }
    }
}