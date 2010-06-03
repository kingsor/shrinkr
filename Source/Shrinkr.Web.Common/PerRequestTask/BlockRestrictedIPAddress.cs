namespace Shrinkr.Web
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web;

    using MvcExtensions;

    using Repositories;

    public class BlockRestrictedIPAddress : PerRequestTask
    {
        private static readonly HashSet<string> localIpCache = new HashSet<string>(StringComparer.Ordinal);

        private readonly HttpContextBase httpContext;
        private readonly IBannedIPAddressRepository bannedIPAddressRepository;

        public BlockRestrictedIPAddress(HttpContextBase httpContext, IBannedIPAddressRepository bannedIPAddressRepository)
        {
            Check.Argument.IsNotNull(httpContext, "httpContext");
            Check.Argument.IsNotNull(bannedIPAddressRepository, "bannedIPAddressRepository");

            this.httpContext = httpContext;
            this.bannedIPAddressRepository = bannedIPAddressRepository;
        }

        public override TaskContinuation Execute()
        {
            bool shouldContinue = true;

            if (!httpContext.Request.IsLocal)
            {
                string ipAddress = httpContext.Request.UserHostAddress;

                HttpResponseBase httpResponse = httpContext.Response;

                if (string.IsNullOrWhiteSpace(ipAddress))
                {
                    Block(httpResponse);
                    shouldContinue = false;
                }
                else if (localIpCache.Contains(ipAddress))
                {
                    Block(httpResponse);
                    shouldContinue = false;
                }
                else if (bannedIPAddressRepository.IsMatching(ipAddress))
                {
                    localIpCache.Add(ipAddress);
                    Block(httpResponse);
                    shouldContinue = false;
                }
            }

            return shouldContinue ? TaskContinuation.Continue : TaskContinuation.Break;
        }

        private static void Block(HttpResponseBase httpResponse)
        {
            httpResponse.StatusCode = (int)HttpStatusCode.Forbidden;
            httpResponse.StatusDescription = "IPAddress blocked.";
            httpResponse.End();
        }
    }
}