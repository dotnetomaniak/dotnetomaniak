namespace Kigg.Web
{
    using System;
    using System.Web.Mvc;

    using Infrastructure;

    public class ControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(Type controllerType)
        {
            return (controllerType == null) ? base.GetControllerInstance(controllerType) : IoC.Resolve<IController>(controllerType);
        }
    }
}