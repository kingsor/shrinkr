// This class is copied from Subkismet.

// Canonicalization code is provided by RFC2396 URL project done by myself (Keyvan):
// http://www.codeplex.com/RFC2396

namespace Shrinkr.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// This class is responsible to provide some methods to convert a 
    /// normal URL to RFC2396 valid URL.
    /// </summary>
    internal static class Canonicalization
    {
        private static readonly Regex hostNameExpression = new Regex(@"^(?=[^&])(?:(?<scheme>[^:/?#]+):)?(?://(?<authority>[^/?#]*))?(?<path>[^?#]*)(?:\?(?<query>[^#]*))?(?:#(?<fragment>.*))?", RegexOptions.Compiled);
        private static readonly Regex reminderExpression = new Regex(@"^(?=[^&])(?:(?<scheme>[^:/?#]+):)?(?://(?<authority>[^/?#]*))?(?<path>[^?#]*)(?:\?(?<query>[^#]*))?(?:#(?<fragment>.*))?", RegexOptions.Compiled);
        private static readonly Regex removeSpecialCharacter1Expression = new Regex("[\x00-\x1f\x7f-\xff]+", RegexOptions.Compiled);
        private static readonly Regex removeSpecia2Character1Expression = new Regex("^\\.+|\\.+$", RegexOptions.Compiled);
        private static readonly Regex removeDotsExpression = new Regex(@"[\.]+", RegexOptions.Compiled);
        private static readonly Regex escapeSpecialCharacterExpression = new Regex(@"[^0-9A-Za-z\.\-]+", RegexOptions.Compiled);
        private static readonly Regex removeSequence1Expression = new Regex(@"(/\./)+", RegexOptions.Compiled);
        private static readonly Regex removeSequence2Expression = new Regex(@"/[0-9A-Za-z(\-)(\.)]+", RegexOptions.Compiled);

        /// <summary>
        /// Gets a url in general form and returns a canonicalized URL.
        /// </summary>
        /// <param name="url">A URL in general form.</param>
        /// <returns>RFC2396 valid URL.</returns>
        public static string GetCanonicalizedUrl(string url)
        {
            string hostName = GetHostName(url);
            string remainder = GetRemainder(url);

            hostName = RemoveSpecialCharacters(hostName);
            hostName = ReplaceConsecutiveDots(hostName);
            hostName = NormalizeIPAddress(hostName);
            hostName = EscapeSpecialChars(hostName);
            hostName = hostName.ToLower(Culture.Invariant);

            remainder = ResolveSequences(remainder);
            remainder = RemoveFragmentIdentifier(remainder);

            if (string.IsNullOrWhiteSpace(remainder))
            {
                hostName += "/";
            }

            return hostName + remainder;
        }

        /// <summary>
        /// Gets the hostname of the URL.
        /// </summary>
        /// <param name="url">A URL in general form.</param>
        /// <returns>Hostname.</returns>
        private static string GetHostName(string url)
        {
            Match matches = hostNameExpression.Match(url);

            return matches.Groups[2].Value;
        }

        /// <summary>
        /// Gets the remainder of the URL.
        /// </summary>
        /// <param name="url">A URL in general form.</param>
        /// <returns>Remainder.</returns>
        private static string GetRemainder(string url)
        {
            Match matches = reminderExpression.Match(url);

            return matches.Groups[3].Value;
        }

        /// <summary>
        /// Removes special characters from the hostname.
        /// </summary>
        /// <param name="hostname">The hostname of the URL.</param>
        /// <returns>Escaped hostname.</returns>
        private static string RemoveSpecialCharacters(string hostname)
        {
            string result = hostname;

            if (removeSpecialCharacter1Expression.IsMatch(result))
            {
                result = removeSpecialCharacter1Expression.Replace(result, string.Empty);
            }

            if (removeSpecia2Character1Expression.IsMatch(result))
            {
                result = removeSpecia2Character1Expression.Replace(result, string.Empty);
            }

            return result;
        }

        /// <summary>
        /// Replaces consecutive dots with a single dot.
        /// </summary>
        /// <param name="hostname">The hostname of the URL.</param>
        /// <returns>Escaped hostname.</returns>
        private static string ReplaceConsecutiveDots(string hostname)
        {
            string result = hostname;

            if (removeDotsExpression.IsMatch(result))
            {
                result = removeDotsExpression.Replace(result, ".");
            }

            return result;
        }

        /// <summary>
        /// Normalizes the hostname if it is an IP address.
        /// </summary>
        /// <param name="hostname">The hostname of the URL.</param>
        /// <returns>Normalized hostname.</returns>
        private static string NormalizeIPAddress(string hostname)
        {
            return hostname;
        }

        /// <summary>
        /// Escapes special characters from URL.
        /// </summary>
        /// <param name="hostname">Hostname of the URL.</param>
        /// <returns>Hostname with escaped URL.</returns>
        private static string EscapeSpecialChars(string hostname)
        {
            string result = hostname;

            if (escapeSpecialCharacterExpression.IsMatch(result))
            {
                result = escapeSpecialCharacterExpression.Replace(result, string.Empty);
            }

            return result;
        }

        /// <summary>
        /// Resolves sequences of dots.
        /// </summary>
        /// <param name="remainder">Remained of the URL.</param>
        /// <returns>Remainder without sequences of dots.</returns>
        private static string ResolveSequences(string remainder)
        {
            string result = remainder;

            if (removeSequence1Expression.IsMatch(result))
            {
                result = removeSequence1Expression.Replace(result, "/");
            }

            if (removeSequence2Expression.IsMatch(result))
            {
                List<string> validComponents = new List<string>();

                MatchCollection matches = removeSequence2Expression.Matches(result);

                foreach (Match match in matches)
                {
                    Match nextMatch = match.NextMatch();

                    if ((match.Value != "/..") && (nextMatch.Value != "/.."))
                    {
                        validComponents.Add(match.Value);
                    }
                }

                result = string.Join(string.Empty, validComponents.ToArray());
            }

            return result;
        }

        /// <summary>
        /// Removes fragment identifier (#) and everything after it.
        /// </summary>
        /// <param name="remainder">The remainder of the URL.</param>
        /// <returns>The remainder of the URL after removing fragment identifier.</returns>
        private static string RemoveFragmentIdentifier(string remainder)
        {
            int index = remainder.IndexOf("#", StringComparison.Ordinal);

            if (index > -1)
            {
                remainder = remainder.Remove(index);
            }

            return remainder;
        }
    }
}