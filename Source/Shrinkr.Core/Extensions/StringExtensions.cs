namespace Shrinkr.Extensions
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        private static readonly Regex emailExpression = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex webUrlExpression = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex stripHTMLExpression = new Regex("<\\S[^><]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        [DebuggerStepThrough]
        public static string FormatWith(this string instance, params object[] args)
        {
            Check.Argument.IsNotNullOrEmpty(instance, "instance");

            return string.Format(Culture.Current, instance, args);
        }

        [DebuggerStepThrough]
        public static string Hash(this string instance)
        {
            Check.Argument.IsNotNullOrEmpty(instance, "instance");

            using (MD5 md5 = MD5.Create())
            {
                byte[] data = Encoding.Unicode.GetBytes(instance);
                byte[] hash = md5.ComputeHash(data);

                return Convert.ToBase64String(hash);
            }
        }

        [DebuggerStepThrough]
        public static T ToEnum<T>(this string instance, T defaultValue) where T : struct, IComparable, IFormattable
        {
            T convertedValue = defaultValue;

            if (!string.IsNullOrWhiteSpace(instance) && !Enum.TryParse(instance.Trim(), true, out convertedValue))
            {
                convertedValue = defaultValue;
            }

            return convertedValue;
        }

        [DebuggerStepThrough]
        public static string StripHtml(this string instance)
        {
            Check.Argument.IsNotNullOrEmpty(instance, "instance");

            return stripHTMLExpression.Replace(instance, string.Empty);
        }

        [DebuggerStepThrough]
        public static bool IsEmail(this string instance)
        {
            return !string.IsNullOrWhiteSpace(instance) && emailExpression.IsMatch(instance);
        }

        [DebuggerStepThrough]
        public static bool IsWebUrl(this string instance)
        {
            return !string.IsNullOrWhiteSpace(instance) && webUrlExpression.IsMatch(instance);
        }

        [DebuggerStepThrough]
        public static bool IsIPAddress(this string instance)
        {
            IPAddress ip;

            return !string.IsNullOrWhiteSpace(instance) && IPAddress.TryParse(instance, out ip);
        }
    }
}