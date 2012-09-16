using System.Text;

using Xunit;

namespace Kigg.Web.Test
{
    public class StringWriterWithEncodingFixture
    {
        private readonly StringWriterWithEncoding _writer;

        public StringWriterWithEncodingFixture()
        {
            _writer = new StringWriterWithEncoding(new StringBuilder(), Encoding.UTF8);
        }

        [Fact]
        public void Encoding_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            Assert.Same(Encoding.UTF8, _writer.Encoding);
        }

        [Fact]
        public void Default_Should_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new StringWriterWithEncoding(new StringBuilder()));
        }
    }
}