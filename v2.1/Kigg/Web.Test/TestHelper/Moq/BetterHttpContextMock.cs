using System.Collections;

namespace Kigg.Web.Test
{
    public class HttpContextMock : Moq.Mvc.HttpContextMock
    {
        public HttpContextMock()
        {
            User = new HttpPrincipalMock();
            ExpectGet(c => c.User).Returns(User.Object);
            ExpectGet(c => c.Items).Returns(new Hashtable());
        }

        public HttpPrincipalMock User
        {
            get;
            private set;
        }

        public override void Verify()
        {
            User.Verify();
            base.Verify();
        }

        public override void VerifyAll()
        {
            User.VerifyAll();
            base.VerifyAll();
        }
    }
}