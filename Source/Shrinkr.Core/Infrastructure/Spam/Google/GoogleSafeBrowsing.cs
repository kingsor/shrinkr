namespace Shrinkr.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Xml.Linq;

    using Extensions;

    public class GoogleSafeBrowsing : Disposable, IGoogleSafeBrowsing
    {
        private static readonly MD5 hasher = MD5.Create();
        private static readonly Regex versionExpression = new Regex(@"\[.+(\d{1,20}\.\d{1,20})\]", RegexOptions.Multiline | RegexOptions.Compiled);
        private static readonly Regex fingerPrintExpression = new Regex(@"([\+|\-][a-zA-Z0-9]+)", RegexOptions.Multiline);

        private readonly ISet<string> phishingFingerPrints = new HashSet<string>();
        private readonly ReaderWriterLockSlim phishingFingerPrintsLock = new ReaderWriterLockSlim();
        private readonly ISet<string> malwareFingerPrints = new HashSet<string>();
        private readonly ReaderWriterLockSlim malwareFingerPrintsLock = new ReaderWriterLockSlim();

        private readonly Settings setting;
        private readonly IHttp http;

        public GoogleSafeBrowsing(Settings setting, IHttp http)
        {
            Check.Argument.IsNotNull(setting, "setting");
            Check.Argument.IsNotNull(http, "http");

            this.setting = setting;
            this.http = http;

            LoadFromFile(phishingFingerPrints, phishingFingerPrintsLock, setting.Google.PhishingFile);
            LoadFromFile(malwareFingerPrints, malwareFingerPrintsLock, setting.Google.MalwareFile);
        }

        public void Verify(string url, out int phishingCount, out int malwareCount)
        {
            Check.Argument.IsNotNullOrEmpty(url, "url");

            IEnumerable<string> fingerPrints = ConvertToFingerPrints(url);

            phishingCount = 0;
            malwareCount = 0;

            using (phishingFingerPrintsLock.Read())
            {
                phishingCount += fingerPrints.Count(fingerPrint => phishingFingerPrints.Contains(fingerPrint));
            }

            using (malwareFingerPrintsLock.Read())
            {
                malwareCount += fingerPrints.Count(fingerPrint => malwareFingerPrints.Contains(fingerPrint));
            }
        }

        public void Update()
        {
            string endPoint = setting.Google.EndPoint + "?client=api&apikey={0}&version=".FormatWith(setting.Google.ApiKey);
            string phishingEndPoint = endPoint + "goog-black-hash:";
            string malwareEndPoint = endPoint + "goog-malware-hash:";

            phishingEndPoint = MakeVersionable(phishingEndPoint, setting.Google.PhishingFile);
            malwareEndPoint = MakeVersionable(malwareEndPoint, setting.Google.MalwareFile);

            http.GetAsync(phishingEndPoint, httpResponse => UpdatePhishingFingerPrints(httpResponse.Content));
            http.GetAsync(malwareEndPoint, httpResponse => UpdateMalwareFingerPrints(httpResponse.Content));
        }

        protected override void DisposeCore()
        {
            using (phishingFingerPrintsLock.Write())
            {
                phishingFingerPrints.Clear();
            }

            phishingFingerPrintsLock.Dispose();

            using (malwareFingerPrintsLock.Write())
            {
                malwareFingerPrints.Clear();
            }

            malwareFingerPrintsLock.Dispose();
        }

        private static void LoadFromFile(ISet<string> memoryCopy, ReaderWriterLockSlim syncLock, string file)
        {
            if (File.Exists(file))
            {
                using (Stream stream = File.OpenRead(file))
                {
                    XDocument document = XDocument.Load(stream);

                    using (syncLock.Write())
                    {
                        foreach (XElement key in document.Descendants("key").Where(key => !string.IsNullOrWhiteSpace(key.Value) && !memoryCopy.Contains(key.Value)))
                        {
                            memoryCopy.Add(key.Value);
                        }
                    }
                }
            }
        }

        private static IEnumerable<string> ConvertToFingerPrints(string url)
        {
            string canonicalizaedLink = Canonicalization.GetCanonicalizedUrl(url);

            IEnumerable<string> fingerPrints = Lookup.GetUrls(canonicalizaedLink)
                                                     .Distinct()
                                                     .Select(Hash)
                                                     .ToList();

            return fingerPrints;
        }

        private static string Hash(string input)
        {
            byte[] data = hasher.ComputeHash(Encoding.Default.GetBytes(input));

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2", Culture.Invariant));
            }

            return builder.ToString();
        }

        private static string MakeVersionable(string endPoint, string file)
        {
            string version = GetVersionFromFile(file);

            version = !string.IsNullOrWhiteSpace(version) ? version.Replace(".", ":") : "1:-1";

            return endPoint + version;
        }

        private static string GetVersionFromFile(string file)
        {
            if (File.Exists(file))
            {
                using (Stream stream = File.OpenRead(file))
                {
                    XDocument document = XDocument.Load(stream);
                    XElement rootNode = document.Root;

                    if (rootNode != null)
                    {
                        XAttribute version = rootNode.Attribute("version");

                        if (version != null)
                        {
                            return version.Value;
                        }
                    }
                }
            }

            return null;
        }

        private static string GetVersionFromData(string data)
        {
            string version = string.Empty;

            if (versionExpression.IsMatch(data))
            {
                version = versionExpression.Match(data).Groups[1].Captures[0].Value;
            }

            return version;
        }

        private static bool IsUpdate(string data)
        {
            return data.Contains("update");
        }

        private static void PopulateInsertAndDeleteList(string data, ICollection<string> insertList, ICollection<string> removeList)
        {
            MatchCollection matches = fingerPrintExpression.Matches(data);

            foreach (Match match in matches)
            {
                string fingerPrintWithVerb = match.Captures[0].Value;

                if (fingerPrintWithVerb.Length > 10)
                {
                    string fingerPrint = fingerPrintWithVerb.Remove(0, 1);

                    if (fingerPrintWithVerb.StartsWith("+", StringComparison.Ordinal))
                    {
                        insertList.Add(fingerPrint);
                    }
                    else if (fingerPrintWithVerb.StartsWith("-", StringComparison.Ordinal))
                    {
                        removeList.Add(fingerPrint);
                    }
                }
            }
        }

        private static void UpdateInMemoryCopy(ISet<string> memoryCopy, ReaderWriterLockSlim syncLock, bool isUpdate, IEnumerable<string> insertList, IEnumerable<string> removeList)
        {
            using (syncLock.Write())
            {
                if (!isUpdate)
                {
                    memoryCopy.Clear();
                }

                foreach (string fingerPrint in insertList.Where(fp => !memoryCopy.Contains(fp)))
                {
                    memoryCopy.Add(fingerPrint);
                }

                foreach (string fingerPrint in removeList)
                {
                    memoryCopy.Remove(fingerPrint);
                }
            }
        }

        private static void UpdateStorageCopy(IEnumerable<string> memoryCopy, ReaderWriterLockSlim syncLock, string version, string file)
        {
            using (syncLock.Read())
            {
                XDocument document = new XDocument(new XElement("list", new XAttribute("version", version), new XElement("keys", memoryCopy.Select(fp => new XElement("key", fp)))));

                document.Save(file);
            }
        }

        private void UpdatePhishingFingerPrints(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return;
            }

            string newVersion = GetVersionFromData(content);
            string oldVersion = GetVersionFromFile(setting.Google.PhishingFile);

            string version = string.IsNullOrWhiteSpace(newVersion) ? oldVersion : newVersion;

            bool isUpdate = IsUpdate(content);
            ICollection<string> insertList = new List<string>();
            ICollection<string> removeList = new List<string>();

            PopulateInsertAndDeleteList(content, insertList, removeList);
            UpdateInMemoryCopy(phishingFingerPrints, phishingFingerPrintsLock, isUpdate, insertList, removeList);
            UpdateStorageCopy(phishingFingerPrints, phishingFingerPrintsLock, version, setting.Google.PhishingFile);
        }

        private void UpdateMalwareFingerPrints(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return;
            }

            string newVersion = GetVersionFromData(content);
            string oldVersion = GetVersionFromFile(setting.Google.MalwareFile);

            string version = string.IsNullOrWhiteSpace(newVersion) ? oldVersion : newVersion;

            bool isUpdate = IsUpdate(content);
            ICollection<string> insertList = new List<string>();
            ICollection<string> removeList = new List<string>();

            PopulateInsertAndDeleteList(content, insertList, removeList);
            UpdateInMemoryCopy(malwareFingerPrints, malwareFingerPrintsLock, isUpdate, insertList, removeList);
            UpdateStorageCopy(malwareFingerPrints, malwareFingerPrintsLock, version, setting.Google.MalwareFile);
        }
    }
}