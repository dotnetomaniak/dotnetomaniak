namespace Kigg.Web
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Configuration;

    using Infrastructure;

    public class ErrorHandler : BaseHttpModule
    {
        public override void OnError(HttpContextBase context)
        {
            Exception e = context.Server.GetLastError().GetBaseException();
            HttpException httpException = e as HttpException;
            int statusCode = (int) HttpStatusCode.InternalServerError;

            // Skip Page Not Found from logging
            if (!((httpException != null) && (httpException.GetHttpCode() == (int) HttpStatusCode.NotFound)))
            {
                Log.Exception(e);
            }

            CustomErrorsSection section = IoC.Resolve<IConfigurationManager>().GetSection<CustomErrorsSection>("system.web/customErrors");
            string redirectUrl = null;

            if ((section.Mode != CustomErrorsMode.Off) && (!((section.Mode == CustomErrorsMode.RemoteOnly) && (context.Request.IsLocal))))
            {
                redirectUrl = section.DefaultRedirect;

                if (httpException != null)
                {
                    statusCode = httpException.GetHttpCode();

                    if (section.Errors.Count > 0)
                    {
                        CustomError item = section.Errors[statusCode.ToString(Constants.CurrentCulture)];

                        if (item != null)
                        {
                            redirectUrl = item.Redirect;
                        }
                    }
                }
            }

            context.Response.StatusCode = statusCode;
            context.ClearError();

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                context.Server.Transfer(redirectUrl);
            }
        }
    }
}