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

            // Skip Page Not Found and Service not unavailable from logging
            if (httpException != null)
            {
                statusCode = httpException.GetHttpCode();

                if ((statusCode != (int) HttpStatusCode.NotFound) && (statusCode != (int) HttpStatusCode.ServiceUnavailable))
                {
                    Log.Exception(e);
                }
            }

            string redirectUrl = null;

            if (context.IsCustomErrorEnabled)
            {
                CustomErrorsSection section = IoC.Resolve<IConfigurationManager>().GetSection<CustomErrorsSection>("system.web/customErrors");

                if (section != null)
                {
                    redirectUrl = section.DefaultRedirect;

                    if (httpException != null)
                    {
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
            }

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.TrySkipIisCustomErrors = true;

            context.ClearError();

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                context.Server.Transfer(redirectUrl);
            }
        }
    }
}