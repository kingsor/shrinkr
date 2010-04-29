namespace Shrinkr.UnitTests
{
    using System.Collections.Generic;

    using Extensions;

    using Xunit;

    public class CollectionExtensionsTests
    {
        [Fact]
        public void IsNullOrEmpty_should_return_true_for_null_collection()
        {
            List<int> x = null;

            Assert.True(x.IsNullOrEmpty());
        }

        [Fact]
        public void IsNullOrEmpty_should_return_true_for_empty_collection()
        {
            List<int> x = new List<int>();

            Assert.True(x.IsNullOrEmpty());
        }

        [Fact]
        public void IsNull_Should_return_true_for_empty_collection()
        {
            Assert.True(new List<int>().IsEmpty());
        }

        [Fact]
        public void AddRange_should_add_specified_items()
        {
            var collection = new List<int> { 1, 2, 3 };

            collection.AddRange(new[] { 4, 5 });

            Assert.Contains(4, collection);
            Assert.Contains(5, collection);
        }
    }
}