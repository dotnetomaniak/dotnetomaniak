namespace Kigg.Web
{
    using DotNetOpenAuth.OpenId;
    using DotNetOpenAuth.OpenId.RelyingParty;

    public interface IOpenIdRelyingParty
    {
        IAuthenticationResponse Response
        {
            get;
        }

        IAuthenticationRequest CreateRequest(Identifier userSuppliedIdentifier, Realm realm);
    }
}