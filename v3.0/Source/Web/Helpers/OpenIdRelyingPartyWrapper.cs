namespace Kigg.Web
{
    using DotNetOpenAuth.OpenId;
    using DotNetOpenAuth.OpenId.RelyingParty;

    public class OpenIdRelyingPartyWrapper : IOpenIdRelyingParty
    {
        private OpenIdRelyingParty _openId;
        private IAuthenticationResponse _response;
        private OpenIdRelyingParty Internal
        {
            get
            {
                if (_openId == null)
                {
                    _openId = new OpenIdRelyingParty();
                }

                return _openId;
            }
        }

        public IAuthenticationResponse Response
        {
            get
            {
                if (_response == null)
                {
                    _response = Internal.GetResponse();
                }
                return _response;
            }
        }

        public IAuthenticationRequest CreateRequest(Identifier userSuppliedIdentifier, Realm realm)
        {
            return Internal.CreateRequest(userSuppliedIdentifier, realm);
        }
    }
}