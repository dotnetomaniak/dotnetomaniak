namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using System;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

    [Assembler(typeof(WeblogTraceTextFormatterAssembler))]
    public class WeblogTraceTextFormatterData : TextFormatterData
    {
        public WeblogTraceTextFormatterData()
        {
        }

        public WeblogTraceTextFormatterData(string templateData) : base(templateData)
        {
        }

        public WeblogTraceTextFormatterData(string name, string templateData) : base(name, templateData)
        {
        }

        public WeblogTraceTextFormatterData(string name, Type formatterType, string templateData) : base(name, formatterType, templateData)
        {
        }
    }
}