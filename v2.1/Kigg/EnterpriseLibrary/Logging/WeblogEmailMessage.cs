namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using System.Net;
    using System.Net.Mail;
    using System.Threading;

    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

    public class WeblogEmailMessage : EmailMessage
    {
        private readonly WeblogEmailTraceListenerData _configuration;

        public WeblogEmailMessage(WeblogEmailTraceListenerData configuration, LogEntry logEntry, ILogFormatter formatter) : base(configuration, logEntry, formatter)
        {
            _configuration = configuration;
        }

        public WeblogEmailMessage(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, bool enableSsl, string userName, string password, LogEntry logEntry, ILogFormatter formatter) : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, logEntry, formatter)
        {
            _configuration = new WeblogEmailTraceListenerData
                                 {
                                     SmtpServer = smtpServer,
                                     SmtpPort = smtpPort,
                                     EnableSsl = enableSsl,
                                     UserName = userName,
                                     Password = password,
                                 };
        }

        public WeblogEmailMessage(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, bool enableSsl, string userName, string password, string message, ILogFormatter formatter) : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, message, formatter)
        {
            _configuration = new WeblogEmailTraceListenerData
                                 {
                                     SmtpServer = smtpServer,
                                     SmtpPort = smtpPort,
                                     EnableSsl = enableSsl,
                                     UserName = userName,
                                     Password = password
                                 };
        }

        public override void Send()
        {
            SendMessage(CreateMailMessage());
        }

        public override void SendMessage(MailMessage message)
        {
            ThreadPool.QueueUserWorkItem(
                                            state =>
                                            {
                                                MailMessage mail = (MailMessage) state;

                                                SmtpClient smtpClient = new SmtpClient();

                                                if (!string.IsNullOrEmpty(_configuration.SmtpServer))
                                                {
                                                    smtpClient.Host = _configuration.SmtpServer;
                                                }

                                                if (_configuration.SmtpPort > 0)
                                                {
                                                    smtpClient.Port = _configuration.SmtpPort;
                                                }

                                                if ((!string.IsNullOrEmpty(_configuration.UserName)) &&
                                                    (!string.IsNullOrEmpty(_configuration.Password)))
                                                {
                                                    smtpClient.Credentials = new NetworkCredential(_configuration.UserName, _configuration.Password);
                                                }

                                                smtpClient.EnableSsl = _configuration.EnableSsl;

                                                try
                                                {
                                                    smtpClient.Send(mail);
                                                }
                                                catch (SmtpException)
                                                {
                                                }
                                                finally
                                                {
                                                    mail.Dispose();
                                                }
                                            },
                                            message);
        }
    }
}