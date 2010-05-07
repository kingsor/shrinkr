namespace Shrinkr.UnitTests
{
    using DomainObjects;

    using Xunit;

    public class ApiSettingTests
    {
        private readonly ApiSetting apiSetting;

        public ApiSettingTests()
        {
            apiSetting = new ApiSetting();
        }

        [Fact]
        public void Should_be_able_to_set_daily_limit()
        {
            apiSetting.DailyLimit = 1000;

            Assert.Equal(1000, apiSetting.DailyLimit);
        }
    }
}