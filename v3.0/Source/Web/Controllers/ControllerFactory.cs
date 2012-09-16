namespace Kigg.Web
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Infrastructure;

    public class ControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                ? base.GetControllerInstance(requestContext, controllerType)
                : IoC.Resolve<IController>(controllerType);
        }
    }
}