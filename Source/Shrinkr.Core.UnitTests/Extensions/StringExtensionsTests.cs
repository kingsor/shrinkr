namespace Shrinkr.UnitTests
{
    using DomainObjects;
    using Extensions;

    using Xunit;

    public class StringExtensionsTests
    {
        [Fact]
        public void FormatWith_should_replace_place_holder_tokens_with_provided_value()
        {
            Assert.Equal("A-B-C-D", "{0}-{1}-{2}-{3}".FormatWith("A", "B", "C", "D"));
        }

        [Fact]
        public void Hash_should_always_return_twenty_four_characters_length_string()
        {
            Assert.Equal(24, "abcd".Hash().Length);
            Assert.Equal(24, "a dummy string".Hash().Length);
            Assert.Equal(24, "x".Hash().Length);
            Assert.Equal(24, "http://weblogs.asp.net/rashid/".Hash().Length);
            Assert.Equal(24, "http://shrinkr.com".Hash().Length);
        }

        [Fact]
        public void ToEnum_should_return_correct_value_when_converted_successfully()
        {
            Assert.Equal(Role.Administrator, "Administrator".ToEnum(Role.User));
        }

        [Fact]
        public void ToEnum_should_return_default_value_when_unable_to_convert()
        {
            Assert.Equal(Role.User, "foobar".ToEnum(Role.User));
        }

        [Fact]
        public void StripHtml_should_return_plain_Text()
        {
            const string Html = "<div style=\"border:1px #000\">This is a div</div>";

            Assert.Equal("This is a div", Html.StripHtml());
        }

        [Fact]
        public void IsEmail_should_return_true_for_valid_email()
        {
            Assert.True("kazimanzurrashid@gmail.com".IsEmail());
        }

        [Fact]
        public void IsEmail_should_return_false_for_invalid_email()
        {
            Assert.False("foobar".IsEmail());
        }

        [Fact]
        public void IsWebUrl_should_return_true_for_valid_url()
        {
            Assert.True("http://www.shrinkr.com".IsWebUrl());
            Assert.True("http://shrinkr.com".IsWebUrl());
        }

        [Fact]
        public void IsWebUrl_should_return_false_for_invalid_url()
        {
            Assert.False("www.shrinkr.com".IsWebUrl());
            Assert.False("shrinkr.com".IsWebUrl());
        }

        [Fact]
        public void IsIPAddress_should_return_true_for_valid_url()
        {
            Assert.True("192.168.0.1".IsIPAddress());
        }

        [Fact]
        public void IsIPAddress_should_return_false_for_invalid_ip_address()
        {
            Assert.False("257.0.0.123".IsIPAddress());
        }
    }
}