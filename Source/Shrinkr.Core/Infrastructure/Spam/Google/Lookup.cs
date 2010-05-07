// This class is copied from Subkismet

namespace Shrinkr.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Manages the lookup process for match URLs according to Google spec.
    /// </summary>
    internal static class Lookup
    {
        private static readonly Regex hostNameExpression = new Regex("http://[^/]*/", RegexOptions.Compiled);
        private static readonly Regex fourHostNameExpression = new Regex(@"[a-zA-Z0-9-]+", RegexOptions.Compiled);
        private static readonly Regex fourPathExpression = new Regex(@"/[^/]+", RegexOptions.Compiled);

        /// <summary>
        /// Returns a list of matched URLs for lookup process.
        /// </summary>
        /// <param name="url">String value of the incoming URL.</param>
        /// <returns>A list of strings for matched URLs.</returns>
        public static IList<string> GetUrls(string url)
        {
            List<string> results = new List<string>();

            string hostname = GetExactHostname(url);
            string urlWithoutParameters = GetExactPathWithoutParameters(url);

            results.Add(hostname);

            results.AddRange(GetFourHostNames(hostname));

            results.Add(GetExactPath(url));

            results.Add(urlWithoutParameters);

            results.AddRange(GetFourPaths(hostname, urlWithoutParameters));

            return RemoveDuplicates(results);
        }

        /// <summary>
        /// Returns the exact hostname for the URL.
        /// </summary>
        /// <param name="url">URL to get its exact hostname.</param>
        /// <returns>The hostname.</returns>
        private static string GetExactHostname(string url)
        {
            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                url = "http://" + url;
            }

            string result = url;

            if (hostNameExpression.IsMatch(url))
            {
                result = hostNameExpression.Match(url).Value;
            }

            if (result.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                result = result.Remove(0, 7);
            }

            if (!result.EndsWith("/", StringComparison.Ordinal))
            {
                result = result + "/";
            }

            return result;
        }

        /// <summary>
        /// Returns four hostnames that match the incoming hostname by
        /// removing its components from the left.
        /// </summary>
        /// <param name="hostname">The hostname.</param>
        /// <returns>A list of up to four hostnames matching the input.</returns>
        private static IEnumerable<string> GetFourHostNames(string hostname)
        {
            List<string> results = new List<string>();

            MatchCollection matches = null;

            if (fourHostNameExpression.IsMatch(hostname))
            {
                matches = fourHostNameExpression.Matches(hostname);
            }

            if (matches != null)
            {
                int count = matches.Count;

                if (count > 2)
                {
                    for (int index = count - 2; (index > (count - 6)) && (index >= 0); index--)
                    {
                        List<string> components = new List<string>();

                        for (int reverseIndex = index; reverseIndex < count; reverseIndex++)
                        {
                            components.Add(matches[reverseIndex].Value);
                        }

                        string newHostname = string.Join(".", components.ToArray());

                        if (!newHostname.EndsWith("/", StringComparison.Ordinal))
                        {
                            newHostname = newHostname + "/";
                        }

                        results.Add(newHostname);
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Returns the exact path for the incoming URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>String value of the exact path for the input.</returns>
        private static string GetExactPath(string url)
        {
            string result = url;

            if (result.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                result = result.Remove(0, 7);
            }

            string exactPathWithoutParameters = GetExactPathWithoutParameters(url);

            int dotIndex = exactPathWithoutParameters.LastIndexOf(".", StringComparison.Ordinal);
            int slashIndex = exactPathWithoutParameters.LastIndexOf("/", StringComparison.Ordinal);

            if (dotIndex < slashIndex)
            {
                if (!result.EndsWith("/", StringComparison.Ordinal))
                {
                    result = result + "/";
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the exact path for a URL without parameters.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>String value of the exact path without parameters for the input.</returns>
        private static string GetExactPathWithoutParameters(string url)
        {
            string result = url;

            if (result.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                result = result.Remove(0, 7);
            }

            if (result.Contains("?"))
            {
                int pos = result.LastIndexOf("?", StringComparison.Ordinal);

                result = result.Remove(pos);
            }

            return result;
        }

        /// <summary>
        /// Returns four paths for the hostname and the URL without parameters.
        /// </summary>
        /// <param name="hostname">The hostname.</param>
        /// <param name="urlWithoutParameters">The URL without parameters.</param>
        /// <returns>A list of up to four strings for the matched paths.</returns>
        private static IEnumerable<string> GetFourPaths(string hostname, string urlWithoutParameters)
        {
            List<string> results = new List<string>();

            MatchCollection matches = null;

            if (fourPathExpression.IsMatch(urlWithoutParameters))
            {
                matches = fourPathExpression.Matches(urlWithoutParameters);
            }

            int count = 0;

            if (matches != null)
            {
                count = matches.Count;
            }

            if (!urlWithoutParameters.EndsWith("/", StringComparison.Ordinal))
            {
                count--;
            }

            if (matches != null)
            {
                for (int index1 = 0; (index1 < 4) && (index1 < count); index1++)
                {
                    List<string> components = new List<string>();

                    for (int index2 = 0; index2 <= index1; index2++)
                    {
                        components.Add(matches[index2].Value);
                    }

                    string tempPath = string.Join(string.Empty, components.ToArray());

                    results.Add(hostname.Remove(hostname.Length - 1, 1) + tempPath + "/");
                }
            }

            return results;
        }

        /// <summary>
        /// Removes duplicate items from the list.
        /// </summary>
        /// <param name="results">A list of strings with duplicate items.</param>
        /// <returns>A list of strings without duplicate items.</returns>
        private static List<string> RemoveDuplicates(IEnumerable<string> results)
        {
            List<string> finalResults = new List<string>();

            foreach (string item in results.Where(item => !finalResults.Contains(item)))
            {
                finalResults.Add(item);
            }

            return finalResults;
        }
    }
}
