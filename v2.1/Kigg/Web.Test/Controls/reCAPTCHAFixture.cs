using System.IO;
using System.Text;
using System.Web.UI;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using Infrastructure;
    using Kigg.Test.Infrastructure;

    public class reCAPTCHAFixture : BaseFixture
    {
        private readonly reCAPTCHATestDouble _reCAPTCHA;

        public reCAPTCHAFixture()
        {
            var validator = new reCAPTCHAValidator("http://api-verify.recaptcha.net/verify", "http://api.recaptcha.net", "https://api-secure.recaptcha.net", "foo", "bar", "hello", "world", new Mock<IHttpForm>().Object);

            resolver.Expect(r => r.Resolve<reCAPTCHAValidator>()).Returns(validator);

            _reCAPTCHA = new reCAPTCHATestDouble();
        }

        [Fact]
        public void Render_Should_Write_Html()
        {
            var html = new StringBuilder();

            using (var sw = new StringWriter(html))
            {
                using (var writer = new HtmlTextWriter(sw))
                {
                    _reCAPTCHA.RenderForTest(writer);
                }
            }

            Assert.True(html.Length > 0);
        }
    }

    public class reCAPTCHATestDouble : reCAPTCHA
    {
        public void RenderForTest(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }
}