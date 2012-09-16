using Xunit;
using Xunit.Extensions;

namespace Kigg.Infrastructure.HtmlAgilityPack.Test
{
    public class HtmlSanitizerFixture
    {
        private readonly IHtmlSanitizer _sanitizer;

        public HtmlSanitizerFixture()
        {
            _sanitizer = new HtmlSanitizer();
        }

        [Theory]
        [InlineData("script")]
        [InlineData("javascript")]
        [InlineData("onclick")]
        public void Sanitize_Should_Return_Clean_Html(string target)
        {
            const string MalformHtml = "<p><strong>This is a malformed html</strong><label>A label</label> <script>alert('I am running')</script><a href=\"javascript:void:(0)\"></a><div>This is under a div.<a href=\"jscript:alert('foobar')\">A script link</a></div><pre onclick=\"alert('Another alert')\"><code> a line of code</code></pre></p>";

            var result = _sanitizer.Sanitize(MalformHtml);

            Assert.DoesNotContain(target, result);
        }
    }
}