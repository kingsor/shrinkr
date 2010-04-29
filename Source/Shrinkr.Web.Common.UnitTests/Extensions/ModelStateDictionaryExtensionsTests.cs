namespace Shrinkr.Web.UnitTests
{
    using System.Web.Mvc;
    using System.Collections.Generic;

    using Services;

    using Xunit;

    public class ModelStateDictionaryExtensionsTests
    {
        private readonly ModelStateDictionary modelState;

        public ModelStateDictionaryExtensionsTests()
        {
            modelState = new ModelStateDictionary();
        }

        [Fact]
        public void Merge_should_add_all_rule_violations_to_current_model_state_dictionary_instance()
        {
            var violations = new List<RuleViolation> { new RuleViolation("param1", "error1"), new RuleViolation("param2", "error2") };

            modelState.Merge(violations);
            
            Assert.Equal(violations.Count, modelState.Count);
        }
    }
}