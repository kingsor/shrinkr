namespace Shrinkr.Web.UnitTests
{
    using System;
    using System.Web;
    using System.Net;
    using System.Collections.Generic;
    using System.Reflection;

    using MvcExtensions;

    using Repositories;

    using Moq;

    using Xunit;
    using Xunit.Extensions;

    public class BlockRestrictedIPAddressTaskTests
    {
        private readonly Mock<HttpContextBase> httpContext;
        private readonly Mock<IBannedIPAddressRepository> bannedIpAddressRepository;
        private readonly BlockRestrictedIPAddress blockRestrictedIpAddress;

        public BlockRestrictedIPAddressTaskTests()
        {
            httpContext = MvcTestHelper.CreateHttpContext();
            bannedIpAddressRepository = new Mock<IBannedIPAddressRepository>();

            blockRestrictedIpAddress = new BlockRestrictedIPAddress(httpContext.Object, bannedIpAddressRepository.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("192.168.0.1")]
        [InlineData("192.168.0.2")]
        public void Execute_should_block_ip_address(string ipAddress)
        {
            httpContext.SetupGet(c => c.Request.UserHostAddress).Returns(ipAddress);
            CacheBlockedIpAddress("192.168.0.1");
            bannedIpAddressRepository.Setup(r => r.IsMatching("192.168.0.2")).Returns(true);

            blockRestrictedIpAddress.Execute();

            httpContext.VerifySet(c => c.Response.StatusCode = (int)HttpStatusCode.Forbidden);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("192.168.0.1")]
        [InlineData("192.168.0.2")]
        public void Execute_should_end_response_when_ip_address_is_blocked(string ipAddress)
        {
            httpContext.SetupGet(c => c.Request.UserHostAddress).Returns(ipAddress);
            CacheBlockedIpAddress("192.168.0.1");
            bannedIpAddressRepository.Setup(r => r.IsMatching("192.168.0.2")).Returns(true);

            blockRestrictedIpAddress.Execute();

            httpContext.Verify(c => c.Response.End());
        }

        [Theory]
        [InlineData("", TaskContinuation.Break)]
        [InlineData(" ", TaskContinuation.Break)]
        [InlineData(null, TaskContinuation.Break)]
        [InlineData("192.168.0.1", TaskContinuation.Break)]
        [InlineData("192.168.0.2", TaskContinuation.Break)]
        [InlineData("192.168.0.3", TaskContinuation.Continue)]
        public void Execute_should_continue_task_chain(string ipAddress, TaskContinuation expectedValue)
        {
            httpContext.SetupGet(c => c.Request.UserHostAddress).Returns(ipAddress);
            CacheBlockedIpAddress("192.168.0.1");
            bannedIpAddressRepository.Setup(r => r.IsMatching("192.168.0.2")).Returns(true);

            TaskContinuation actualValue = blockRestrictedIpAddress.Execute();
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void Execute_should_add_matched_blocked_ip_to_cache()
        {
            const string IPAddress = "192.168.0.2";

            httpContext.SetupGet(c => c.Request.UserHostAddress).Returns(IPAddress);
            bannedIpAddressRepository.Setup(r => r.IsMatching(IPAddress)).Returns(true);

            blockRestrictedIpAddress.Execute();

            HashSet<string> blockedIps = GetBlockedIpAddressesCache();

            Assert.True(blockedIps.Contains(IPAddress));
        }

        private static void CacheBlockedIpAddress(string ipAddress)
        {
            var localIpCache = GetBlockedIpAddressesCache();

            localIpCache.Add(ipAddress);
        }

        private static HashSet<string> GetBlockedIpAddressesCache()
        {
            Type type = typeof(BlockRestrictedIPAddress);

            FieldInfo localIpCacheFieldInfo = type.GetField("localIpCache", BindingFlags.NonPublic | BindingFlags.Static);

            return (HashSet<string>)localIpCacheFieldInfo.GetValue(null);
        }
    }
}
