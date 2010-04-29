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
            public static string Field4;
            public string Field2 = "bar";

            [ScriptIgnore]
            public string Field5;

            private string fieled3;

            #pragma warning restore 169

            public Dummy()
            {
                Property1 = "foo";
                Property2 = "bar";
            }

            public static string Property4 { get; set; }

            public string Property1 { get; set; }

            public string Property2 { get; set; }

            public string Property5 { private get; set; }

            [ScriptIgnore]
            public string Property6 { get; set; }

            private string Property3 { get; set; }
        }
    }
}