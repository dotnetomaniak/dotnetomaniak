using System.Security.Principal;

using Moq;

namespace Kigg.Web.Test
{
    public class HttpPrincipalMock : Mock<IPrincipal>
    {
        public HttpPrincipalMock()
        {
            Identity = new HttpIdentityMock();
            ExpectGet(u => u.Identity).Returns(Identity.Object);
        }

        public HttpIdentityMock Identity
        {
            get;
            private set;
        }

        public override void Verify()
        {
            Identity.Verify();
            base.Verify();
        }

        public override void VerifyAll()
        {
            Identity.VerifyAll();
            base.VerifyAll();
        }
    }
}