namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using System.Configuration;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
    using Microsoft.Practices.ObjectBuilder2;

    public class WeblogLoggingExceptionHandlerPolicyCreator : IContainerPolicyCreator
    {
        public void CreatePolicies(IPolicyList policyList, string instanceName, ConfigurationElement configurationObject, IConfigurationSource configurationSource)
        {
            WeblogLoggingExceptionHandlerData castConfigurationObject = (WeblogLoggingExceptionHandlerData) configurationObject;

            new PolicyBuilder<WeblogLoggingExceptionHandler, WeblogLoggingExceptionHandlerData>(NamedTypeBuildKey.Make<WeblogLoggingExceptionHandler>(instanceName), castConfigurationObject, c => new WeblogLoggingExceptionHandler(castConfigurationObject.LogCategory, new LoggerWrapper())).AddPoliciesToPolicyList(policyList);
        }
    }
}