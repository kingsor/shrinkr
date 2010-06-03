namespace Shrinkr.UnitTests
{
    using System.Collections.Generic;

    using DataTransferObjects;
    using DomainObjects;
    using Services;

    using Xunit;

    public class VisitResultTests
    {
        private readonly VisitDTO dto;
        private readonly VisitResult result;

        public VisitResultTests()
        {
            var visit = new Visit
                        {
                            IPAddress = "192.168.0.2",
                            Referrer = new Referrer { Domain = "twitter.com", Url = "http://twitter.com/manzurrashid" },
                            Alias = new Alias
                            {
                                Name = "shrnkr",
                                IPAddress = "192.168.0.1",
                                ShortUrl = new ShortUrl
                                {
                                    Url = "http://shrinkr.com",
                                    Title = "Kool Url Shrinking Service"
                                }
                            }
                        };

            dto = new VisitDTO(visit);
            result = new VisitResult(dto);
        }

        [Fact]
        public void Rule_violations_constructor_should_not_throw_exception()
        {
            Assert.DoesNotThrow(() => new VisitResult(new List<RuleViolation> { new RuleViolation("foo", "bar") }));
        }

        [Fact]
        public void Default_constructor_should_not_throw_exception()
        {
            Assert.DoesNotThrow(() => new VisitResult());
        }

        [Fact]
        public void Visit_should_be_same_which_is_passed_in_constructor()
        {
            Assert.Same(dto, result.Visit);
        }
    }
}