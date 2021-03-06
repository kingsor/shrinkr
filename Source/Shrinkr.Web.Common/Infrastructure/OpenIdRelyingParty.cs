namespace Shrinkr.Web
{
    using System.Diagnostics;

    using DotNetOpenAuth.OpenId;
    using DotNetOpenAuth.OpenId.RelyingParty;

    using OpenId = DotNetOpenAuth.OpenId.RelyingParty.OpenIdRelyingParty;

    public interface IOpenIdRelyingParty
    {
        IAuthenticationResponse Response
        {
            get;
        }

        IAuthenticationRequest CreateRequest(Identifier userSuppliedIdentifier, Realm realm);
    }

    public class OpenIdRelyingParty : IOpenIdRelyingParty
    {
        private OpenId openId;

        public IAuthenticationResponse Response
        {
            [DebuggerStepThrough]
            get
            {
                return OpenId.GetResponse();
            }
        }

        private OpenId OpenId
        {
            [DebuggerStepThrough]
            get
            {
                return openId ?? (openId = new OpenId());
            }
        }

        public IAuthenticationRequest CreateRequest(Identifier userSuppliedIdentifier, Realm realm)
        {
            return OpenId.CreateRequest(userSuppliedIdentifier, realm);
        }
    }
}