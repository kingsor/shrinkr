namespace Shrinkr.UnitTests
{
    using System;

    using Xunit;

    public class CheckArgumentTests
    {
        [Fact]
        public void IsNotNull_should_throw_exception_when_passing_null_value()
        {
            Assert.Throws<ArgumentNullException>(() => Check.Argument.IsNotNull(null, "x"));
        }

        [Fact]
        public void IsNotNull_should_not_throw_exception_when_passing_non_null_value()
        {
            Assert.DoesNotThrow(() => Check.Argument.IsNotNull(new object(), "x"));
        }

        [Fact]
        public void IsNotNullOrEmpty_should_throw_exception_when_passing_null_string()
        {
            Assert.Throws<ArgumentException>(() => Check.Argument.IsNotNullOrEmpty(null, "x"));
        }

        [Fact]
        public void IsNotNullOrEmpty_should_throw_exception_when_passing_empty_string()
        {
            Assert.Throws<ArgumentException>(() => Check.Argument.IsNotNullOrEmpty(string.Empty, "x"));
        }

        [Fact]
        public void IsNotNullOrEmpty_should_not_throw_exception_when_passing_non_empty_string()
        {
            Assert.DoesNotThrow(() => Check.Argument.IsNotNullOrEmpty("xxx", "x"));
        }

        [Fact]
        public void IsNotZeroOrNegative_should_throw_exception_when_passing_negative_integer_value()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Check.Argument.IsNotZeroOrNegative(-1, "x"));
        }

        [Fact]
        public void IsNotZeroOrNegative_should_throw_exception_when_passing_zero_as_integer_value()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Check.Argument.IsNotZeroOrNegative(0, "x"));
        }

        [Fact]
        public void IsNotZeroOrNegative_should_not_throw_exception_when_passing_positive_integer_value()
        {
            Assert.DoesNotThrow(() => Check.Argument.IsNotZeroOrNegative(1, "x"));
        }

        [Fact]
        public void IsNotNegative_should_throw_exception_when_passing_negative_integer_value()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Check.Argument.IsNotNegative(-1, "x"));
        }

        [Fact]
        public void IsNotNegative_with_should_not_throw_exception_when_passing_positive_integer_value()
        {
            Assert.DoesNotThrow(() => Check.Argument.IsNotNegative(1, "x"));
        }

        [Fact]
        public void IsNotZeroOrNegative_should_throw_exception_when_passing_negative_long_value()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Check.Argument.IsNotZeroOrNegative(-1L, "x"));
        }

        [Fact]
        public void IsNotZeroOrNegative_should_throw_exception_when_passing_zero_as_long_value()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Check.Argument.IsNotZeroOrNegative(0L, "x"));
        }

        [Fact]
        public void IsNotZeroOrNegative_should_not_throw_exception_when_passing_positive_long_value()
        {
            Assert.DoesNotThrow(() => Check.Argument.IsNotZeroOrNegative(1L, "x"));
        }

        [Fact]
        public void IsNotNegative_should_throw_exception_when_passing_negative_long_value()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Check.Argument.IsNotNegative(-1L, "x"));
        }

        [Fact]
        public void IsNotNegative_with_should_not_throw_exception_when_passing_positive_long_value()
        {
            Assert.DoesNotThrow(() => Check.Argument.IsNotNegative(1L, "x"));
        }

        [Fact]
        public void IsNotZeroOrNegative_should_throw_exception_when_passing_float_value()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Check.Argument.IsNotZeroOrNegative(-1f, "x"));
        }

        [Fact]
        public void IsNotZeroOrNegative_should_throw_exception_when_passing_zero_as_float_value()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Check.Argument.IsNotZeroOrNegative(0f, "x"));
        }

        [Fact]
        public void IsNotZeroOrNegative_should_not_throw_exception_when_passing_positive_float_value()
        {
            Assert.DoesNotThrow(() => Check.Argument.IsNotZeroOrNegative(1f, "x"));
        }

        [Fact]
        public void IsNotNegative_should_throw_exception_when_passing_negative_float_value()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Check.Argument.IsNotNegative(-1f, "x"));
        }

        [Fact]
        public void IsNotNegative_with_should_not_throw_exception_when_passing_positive_float_value()
        {
            Assert.DoesNotThrow(() => Check.Argument.IsNotNegative(1f, "x"));
        }
    }
}