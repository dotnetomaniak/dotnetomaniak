using System.Xml.Linq;

using Xunit;

namespace Kigg.Web.Test
{
    public class XDocumentExtensionFixture
    {
        [Fact]
        public void ToXml_Should_Return_The_Xml_In_String()
        {
            XDocument doc = new XDocument();

            doc.Add(new XElement("test"));

            string xml = doc.ToXml();

            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?><test />", xml);
        }
    }
}