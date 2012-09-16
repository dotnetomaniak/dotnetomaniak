namespace Kigg.Web
{
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    public static class XDocumentExtension
    {
        public static string ToXml(this XDocument doc)
        {
            StringBuilder output = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(new StringWriterWithEncoding(output), new XmlWriterSettings { Indent = false, Encoding = Encoding.UTF8}))
            {
                if (writer != null)
                {
                    doc.Save(writer);
                }
            }

            return output.ToString();
        }
    }
}