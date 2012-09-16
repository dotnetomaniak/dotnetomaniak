namespace Kigg.Web
{
    using System.Web.Mvc;

    using Infrastructure;

    public class RegisterControllerFactory : IBootstrapperTask
    {
        private readonly IControllerFactory _controllerFactory;

        public RegisterControllerFactory(IControllerFactory controllerFactory)
        {
            Check.Argument.IsNotNull(controllerFactory, "controllerFactory");

            _controllerFactory = controllerFactory;
        }

        public void Execute()
        {
            ControllerBuilder.Current.SetControllerFactory(_controllerFactory);
        }
    }
}