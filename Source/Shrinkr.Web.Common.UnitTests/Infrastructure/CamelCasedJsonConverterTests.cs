namespace Shrinkr.Web.UnitTests
{
    using System.Web.Script.Serialization;
    using Xunit;

    public class CamelCasedJsonConverterTests
    {
        [Fact]
        public void Should_be_able_to_serialize()
        {
            var converter = new CamelCasedJsonConverter();

            var result = converter.Serialize(new Dummy(), new JavaScriptSerializer());

            Assert.Equal("foo", result["Field1"]);
            Assert.Equal("bar", result["Field2"]);
            Assert.Equal("foo", result["Property1"]);
            Assert.Equal("bar", result["Property2"]);

            Assert.DoesNotContain("fieled3", result.Keys);
            Assert.DoesNotContain("Fieled4", result.Keys);
            Assert.DoesNotContain("fieled5", result.Keys);
            Assert.DoesNotContain("Property3", result.Keys);
            Assert.DoesNotContain("Property4", result.Keys);
            Assert.DoesNotContain("Property5", result.Keys);
            Assert.DoesNotContain("Property6", result.Keys);
        }

        public class Dummy
        {
            #pragma warning disable 169

            public readonly string Field1 = "foo";
            public string Field2 = "bar";

            private string fieled3;
            public static string Field4;

            [ScriptIgnore]
            public string Field5;

            #pragma warning restore 169

            public Dummy()
            {
                Property1 = "foo";
                Property2 = "bar";
            }

            // ReSharper disable MemberCanBePrivate.Local
            // ReSharper disable UnusedAutoPropertyAccessor.Local

            public string Property1 { get; set; }

            public string Property2 { get; set; }

            // ReSharper restore UnusedAutoPropertyAccessor.Local
            // ReSharper restore MemberCanBePrivate.Local

            // ReSharper disable UnusedMember.Local

            private string Property3 { get; set; }

            public static string Property4 { get; set; }

            // ReSharper disable UnusedAutoPropertyAccessor.Local
            public string Property5 { private get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local

            [ScriptIgnore]
            public string Property6 { get; set; }

            // ReSharper restore UnusedMember.Local
        }
    }
}