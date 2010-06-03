namespace Shrinkr.UnitTests
{
    using System;

    using DomainObjects;
    using Extensions;
    using Infrastructure;

    using Xunit;

    public class UserTests
    {
        private readonly User user;

        public UserTests()
        {
            user = new User();
        }

        [Fact]
        public void Aliases_should_be_empty_when_new_instance_is_created()
        {
            Assert.Empty(user.Aliases);
        }

        [Fact]
        public void CreatedAt_should_be_set_when_new_instance_is_created()
        {
            Assert.NotEqual(DateTime.MinValue, user.CreatedAt);
        }

        [Fact]
        public void LastActivityAt_should_be_set_when_new_instance_is_created()
        {
            Assert.NotEqual(DateTime.MinValue, user.LastActivityAt);
        }

        [Fact]
        public void ApiSetting_should_not_be_null()
        {
            Assert.NotNull(user.ApiSetting);
        }

        [Fact]
        public void Should_be_able_to_set_role()
        {
            user.Role = Role.Administrator;

            Assert.Equal(Role.Administrator, user.Role);
        }

        [Fact]
        public void Should_be_able_to_check_whether_api_is_accessible()
        {
            Assert.False(user.CanAccessApi);
        }

        [Fact]
        public void GenerateApiKey_should_throw_exception_api_access_is_not_allowed()
        {
            user.ApiSetting.Allowed = false;

            Assert.Throws<InvalidOperationException>(() => user.GenerateApiKey());
        }

        [Fact]
        public void GenerateApiKey_should_generate_new_api_key()
        {
            string oldKey = user.ApiSetting.Key;
            user.ApiSetting.Allowed = true;
            user.ApiSetting.DailyLimit = ApiSetting.InfiniteLimit;
            
            user.GenerateApiKey();

            Assert.NotEqual(oldKey, user.ApiSetting.Key);
        }

        [Fact]
        public void Should_be_able_to_allow_api_access()
        {
            user.AllowApiAccess(100);

            Assert.True(user.CanAccessApi);
        }

        [Fact]
        public void Should_be_able_to_check_whether_daily_limit_has_exceeded()
        {
            user.ApiSetting.Key = Guid.NewGuid().ToString().ToUpperInvariant();
            user.ApiSetting.Allowed = true;
            user.ApiSetting.DailyLimit = 3;

            var sixHoursBack = SystemTime.Now().AddHours(-6);

            user.Aliases.AddRange(new[] { new Alias { CreatedAt = sixHoursBack, CreatedByApi = true, User = user }, new Alias { CreatedAt = sixHoursBack, CreatedByApi = true, User = user }, new Alias { CreatedAt = sixHoursBack, CreatedByApi = true, User = user } });

            Assert.True(user.HasExceededDailyLimit());
        }

        [Fact]
        public void Daliylimit_should_never_exceed_when_set_to_infinite()
        {
            user.AllowApiAccess(ApiSetting.InfiniteLimit);

            Assert.True(user.CanAccessApi);
            Assert.False(user.HasExceededDailyLimit());
        }
    }
}