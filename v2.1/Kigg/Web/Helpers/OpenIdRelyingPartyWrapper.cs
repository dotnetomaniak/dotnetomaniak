namespace Kigg.Web
{
    using DotNetOpenId;
    using DotNetOpenId.RelyingParty;

    public class OpenIdRelyingPartyWrapper : IOpenIdRelyingParty
    {
        private OpenIdRelyingParty _openId;

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
                return Internal.Response;
            }
        }

        public IAuthenticationRequest CreateRequest(Identifier userSuppliedIdentifier, Realm realm)
        {
            return Internal.CreateRequest(userSuppliedIdentifier, realm);
        }
    }
}