namespace Shrinkr.UnitTests
{
    using System;

    using DomainObjects;

    using Xunit;

    public class AliasTests
    {
        private readonly Alias alias;

        public AliasTests()
        {
            alias = new Alias();
        }

        [Fact]
        public void Visits_should_be_empty_when_new_instance_is_created()
        {
            Assert.Empty(alias.Visits);
        }

        [Fact]
        public void CreatedAt_should_be_set_when_new_instance_is_created()
        {
            Assert.NotEqual(DateTime.MinValue, alias.CreatedAt);
        }
    }
}