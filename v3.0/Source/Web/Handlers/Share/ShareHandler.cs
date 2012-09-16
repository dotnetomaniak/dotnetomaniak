namespace Kigg.Web
{
    using System;
    using System.Web;

    using DomainObjects;
    using Infrastructure;
    using Repository;

    public class ShareHandler : BaseHandler
    {
        public ShareHandler()
        {
            IoC.Inject(this);
        }

        public IStoryRepository StoryRepository
        {
            get;
            set;
        }

        public static void RedirectToPrevious(HttpContextBase context)
        {
            string redirectUrl = "~/";

            if (context.Request.UrlReferrer != null)
            {
                redirectUrl = context.Request.UrlReferrer.ToString();
            }

            context.Response.Redirect(redirectUrl);
        }

        public override void ProcessRequest(HttpContextBase context)
        {
            const string DefaultService = "msdn";

            Guid id = context.Request.QueryString["id"].ToGuid();
            string service = (context.Request.QueryString["srv"] ?? DefaultService).ToLowerInvariant();

            if (id == Guid.Empty)
            {
                RedirectToPrevious(context);
                return;
            }

            IStory story = StoryRepository.FindById(id);

            if (story == null)
            {
                RedirectToPrevious(context);
                return;
            }

            ISocialServiceRedirector redirector = null;

            try
            {
                redirector = IoC.Resolve<ISocialServiceRedirector>(service);
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }

            if (redirector == null)
            {
                redirector = IoC.Resolve<ISocialServiceRedirector>(DefaultService);
            }

            redirector.Redirect(context, story);
        }
    }
}