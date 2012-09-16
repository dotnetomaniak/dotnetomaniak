namespace Kigg.Web
{
    using System.Web;

    using Infrastructure;

    public class GlobalApplication : HttpApplication
    {
        public static void OnStart()
        {
            Bootstrapper.Run();
            Log.Info("Application Started");
        }

        public static void OnEnd()
        {
            Log.Warning("Application Ended");
            IoC.Reset();
        }

        protected void Application_Start()
        {
            OnStart();
        }

        protected void Application_End()
        {
            OnEnd();
        }
    }
}