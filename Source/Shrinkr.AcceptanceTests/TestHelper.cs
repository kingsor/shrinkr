namespace Shrinkr.AcceptanceTests
{
    using WatiN.Core;

    public static class TestHelper
    {
        public const string ApplicationUrl = "http://localhost/Shrinkr";

        public static Browser CreateBrowser()
        {
            return new IE();
        }
    }
}