namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using System.Diagnostics;

    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
    using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

    public class WeblogEmailTraceListener : EmailTraceListener
    {
        private readonly string _toAddress;
        private readonly string _fromAddress;
        private readonly string _subjectLineStarter;
        private readonly string _subjectLineEnder;
        private readonly string _smtpServer;
        private readonly int _smtpPort;

        private readonly bool _enableSsl;
        private readonly string _userName;
        private readonly string _password;

        public WeblogEmailTraceListener(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, bool enableSsl, string userName, string password, ILogFormatter formatter) : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, formatter)
        {
            _toAddress = toAddress;
            _fromAddress = fromAddress;
            _subjectLineStarter = subjectLineStarter;
            _subjectLineEnder = subjectLineEnder;
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _enableSsl = enableSsl;
            _userName = userName;
            _password = password;
        }

        public override void Write(string message)
        {
            WeblogEmailMessage mailMessage = new WeblogEmailMessage(_toAddress, _fromAddress, _subjectLineStarter, _subjectLineEnder, _smtpServer, _smtpPort, _enableSsl, _userName, _password, message, Formatter);
            mailMessage.Send();
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if ((Filter != null) && !Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                return;
            }

            LogEntry entry = data as LogEntry;

            if (entry != null)
            {
                WeblogEmailMessage message = new WeblogEmailMessage(_toAddress, _fromAddress, _subjectLineStarter, _subjectLineEnder, _smtpServer, _smtpPort, _enableSsl, _userName, _password, entry, Formatter);
                message.Send();
                InstrumentationProvider.FireTraceListenerEntryWrittenEvent();
            }
            else if (data is string)
            {
                Write(data);
            }
            else
            {
                base.TraceData(eventCache, source, eventType, id, data);
            }
        }

        protected override string[] GetSupportedAttributes()
        {
            int baseLength = base.GetSupportedAttributes().Length;

            string[] attributes = new string[baseLength + 3];
            base.GetSupportedAttributes().CopyTo(attributes, 0);

            attributes[baseLength] = "enableSsl";
            attributes[baseLength + 1] = "userName";
            attributes[baseLength + 2] = "password";

            return attributes;
        }
    }
}