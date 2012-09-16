namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using System.Collections.Generic;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

    [ConfigurationElementType(typeof(WeblogTraceTextFormatterData))]
    public class WeblogTraceTextFormatter : TextFormatter
    {
        internal const string DefaultTextFormat = "\"{machine}\",\"{timestamp}\",\"{severity}\",\"{namaspace}\",\"{className}\",\"{methodSignature}\",\"{message}\",\"{user}\",\"{ipAddress}\",\"{userAgent}\",\"{url}\",\"{referrer}\"";

        private static readonly IDictionary<string, TokenHandler<LogEntry>> _extraTokenHandlers = GetExtraTokenHandlers();

        public WeblogTraceTextFormatter() : this(DefaultTextFormat)
        {
        }

        public WeblogTraceTextFormatter(string template) : base(template, _extraTokenHandlers)
        {
        }

        private static IDictionary<string, TokenHandler<LogEntry>> GetExtraTokenHandlers()
        {
            IDictionary<string, TokenHandler<LogEntry>> extraHandlers = new Dictionary<string, TokenHandler<LogEntry>>();

            // Web Application parameters
            extraHandlers["namaspace"] = GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => ((WeblogEntry)le).NamespaceName);
            extraHandlers["className"] = GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => ((WeblogEntry)le).ClassName);
            extraHandlers["methodSignature"] = GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => ((WeblogEntry)le).MethodSignature);
            extraHandlers["user"] = GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => ((WeblogEntry)le).CurrentUserName);
            extraHandlers["ipAddress"] = GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => ((WeblogEntry)le).CurrentUserIPAddress);
            extraHandlers["userAgent"] = GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => ((WeblogEntry)le).CurrentUserAgent);
            extraHandlers["url"] = GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => ((WeblogEntry)le).CurrentUrl);
            extraHandlers["referrer"] = GenericTextFormatter<LogEntry>.CreateSimpleTokenHandler(le => ((WeblogEntry)le).CurrentUrl);

            return extraHandlers;
        }
    }
}