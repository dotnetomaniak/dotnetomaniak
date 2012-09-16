namespace Kigg.Web
{
    using DotNetOpenId;
    using DotNetOpenId.RelyingParty;

    public interface IOpenIdRelyingParty
    {
        IAuthenticationResponse Response
        {
            get;
        }

        IAuthenticationRequest CreateRequest(Identifier userSuppliedIdentifier, Realm realm);
    }
}