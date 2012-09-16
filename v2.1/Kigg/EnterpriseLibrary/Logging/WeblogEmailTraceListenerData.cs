namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using System.Configuration;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

    [Assembler(typeof(WeblogEmailTraceListenerAssembler))]
    public class WeblogEmailTraceListenerData : EmailTraceListenerData
    {
        private const string UserNameProperty = "userName";
        private const string PasswordProperty = "password";
        private const string EnableSslProperty = "enableSsl";

        [ConfigurationProperty(UserNameProperty, IsRequired = false)]
        public string UserName
        {
            get { return (string)base[UserNameProperty]; }
            set { base[UserNameProperty] = value; }
        }

        [ConfigurationProperty(PasswordProperty, IsRequired = false)]
        public string Password
        {
            get { return (string)base[PasswordProperty]; }
            set { base[PasswordProperty] = value; }
        }

        [ConfigurationProperty(EnableSslProperty, IsRequired = false)]
        public bool EnableSsl
        {
            get { return (bool) base[EnableSslProperty]; }
            set { base[EnableSslProperty] = value; }
        }
    }
}