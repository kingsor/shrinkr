namespace Shrinkr.UnitTests
{
    using System;

    using DomainObjects;

    using Xunit;

    public class VisitTests
    {
        private readonly Visit visit;

        public VisitTests()
        {
            visit = new Visit();
        }

        [Fact]
        public void CreatedAt_should_be_set_when_new_instance_is_created()
        {
            Assert.NotEqual(DateTime.MinValue, visit.CreatedAt);
        }
    }
}