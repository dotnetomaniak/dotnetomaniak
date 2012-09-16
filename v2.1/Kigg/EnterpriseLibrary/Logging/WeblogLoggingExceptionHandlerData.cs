namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using System.Configuration;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

    [Assembler(typeof(WeblogLoggingExceptionHandlerAssembler))]
    [ContainerPolicyCreator(typeof(WeblogLoggingExceptionHandlerPolicyCreator))]
    public class WeblogLoggingExceptionHandlerData : ExceptionHandlerData
    {
        private const string Category = "logCategory";

        public WeblogLoggingExceptionHandlerData()
        {
        }

        public WeblogLoggingExceptionHandlerData(string name, string logCategory) : base(name, typeof(WeblogLoggingExceptionHandler))
        {
            LogCategory = logCategory;
        }

        [ConfigurationProperty(Category, IsRequired = true)]
        public string LogCategory
        {
            get
            {
                return (string) this[Category];
            }

            set
            {
                this[Category] = value;
            }
        }
    }
}