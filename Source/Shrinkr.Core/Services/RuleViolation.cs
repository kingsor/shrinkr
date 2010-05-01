namespace Shrinkr.Services
{
    using System.Diagnostics;

    public class RuleViolation
    {
        [DebuggerStepThrough]
        public RuleViolation(string parameterName, string errorMessage)
        {
            Check.Argument.IsNotNullOrEmpty(parameterName, "parameterName");
            Check.Argument.IsNotNullOrEmpty(errorMessage, "errorMessage");

            ParameterName = parameterName;
            ErrorMessage = errorMessage;
        }

        public string ParameterName
        {
            get;
            private set;
        }

        public string ErrorMessage
        {
            get;
            private set;
        }
    }
}