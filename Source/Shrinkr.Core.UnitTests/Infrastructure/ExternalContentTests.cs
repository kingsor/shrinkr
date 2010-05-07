namespace Shrinkr.UnitTests
{
    using Infrastructure;

    using Xunit;

    public class ExternalContentTests
    {
        private readonly ExternalContent externalContent;

        public ExternalContentTests()
        {
            externalContent = new ExternalContent("This is a dummy title", "This is a dummy content");
        }

        [Fact]
        public void Title_should_be_same_which_is_passed_in_constructor()
        {
            Assert.Equal("This is a dummy title", externalContent.Title);
        }

        [Fact]
        public void Content_should_be_same_which_is_passed_in_constructor()
        {
            Assert.Equal("This is a dummy content", externalContent.Content);
        }
    }
}