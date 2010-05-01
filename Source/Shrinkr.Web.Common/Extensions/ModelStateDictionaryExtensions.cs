namespace Shrinkr.Web
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Extensions;
    using Services;

    public static class ModelStateDictionaryExtensions
    {
        public static void Merge(this ModelStateDictionary instance, IEnumerable<RuleViolation> ruleViolations)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotNull(ruleViolations, "ruleViolations");

            ruleViolations.Each(violation => instance.AddModelError(violation.ParameterName, violation.ErrorMessage));
        }
    }
}