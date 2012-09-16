using System.Collections;

namespace Kigg.Web.Test
{
    public class HttpContextMock : Moq.Mvc.HttpContextMock
    {
        public HttpContextMock()
        {
            User = new HttpPrincipalMock();
            SetupGet(c => c.User).Returns(User.Object);
            SetupGet(c => c.Items).Returns(new Hashtable());
        }

        public HttpPrincipalMock User
        {
            get;
            private set;
        }

        public new void Verify()
        {
            User.Verify();
            base.Verify();
        }

        public new void VerifyAll()
        {
            User.VerifyAll();
            base.VerifyAll();
        }
    }
}