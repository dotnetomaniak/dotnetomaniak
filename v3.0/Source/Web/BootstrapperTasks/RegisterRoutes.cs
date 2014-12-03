namespace Kigg.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using Infrastructure;

    public class RegisterRoutes : IBootstrapperTask
    {
        private readonly IConfigurationSettings _settings;
        private readonly RouteCollection _routes;

        public RegisterRoutes(IConfigurationSettings settings, RouteCollection routes)
        {
            Check.Argument.IsNotNull(settings, "settings");
            Check.Argument.IsNotNull(routes, "routes");

            _settings = settings;
            _routes = routes;
        }

        public RegisterRoutes(IConfigurationSettings settings) : this(settings, RouteTable.Routes)
        {
        }

        public void Execute()
        {
            // Turns off the unnecessary file exists check
            _routes.RouteExistingFiles = true;

            // Ignore text, html, xml, png files.
            _routes.IgnoreRoute("{file}.txt");
            _routes.IgnoreRoute("{file}.htm");
            _routes.IgnoreRoute("{file}.html");
            _routes.IgnoreRoute("{file}.xml");
            _routes.IgnoreRoute("Data/Thumbnails/{file}.png");

            // Ignore axd files such as assest, image, sitemap etc
            _routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Ignore the assets directory which contains images, js, css & html
            _routes.IgnoreRoute("Assets/{*pathInfo}");

            // Ignore the error directory which contains error pages
            _routes.IgnoreRoute("ErrorPages/{*pathInfo}");

            // Ignore the ado.net data services directory which contains kigg data services
            _routes.IgnoreRoute("DataServices/{*pathInfo}");

            //Exclude favicon (google toolbar request gif file as fav icon)
            _routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.([iI][cC][oO]|[gG][iI][fF])(/.*)?" });

            _routes.MapRoute("ThumbnailPath", "Story/ThumbnailPath", new { controller = "Story", action = "GetThumbnailPath" });

            _routes.MapRoute("FeedUpcoming", "Feed/{format}/Upcoming/{start}/{max}", new { controller = "Feed", action = "Upcoming", format = "Rss", start = 1, max = _settings.FeedStoryPerPage });
            _routes.MapRoute("FeedSearch", "Feed/{format}/Search/{q}/{start}/{max}", new { controller = "Feed", action = "Search", format = "Rss", q = string.Empty, start = 1, max = _settings.FeedStoryPerPage });
            _routes.MapRoute("FeedPublished", "Feed/{format}/Published/{start}/{max}", new { controller = "Feed", action = "Published", format = "Rss", start = 1, max = _settings.FeedStoryPerPage });
            _routes.MapRoute("FeedList", "Feed/{format}/{action}/{name}/{start}/{max}", new { controller = "Feed", action = "Published", format = "Rss", start = 1, max = _settings.FeedStoryPerPage });
            _routes.MapRoute("FeedDefault", "Feed", new { controller = "Feed", action = "Published", format = "Rss", start = 1, max = _settings.FeedStoryPerPage });

            _routes.MapRoute("Faq", "Faq", new { controller = "Support", action = "Faq" });
            _routes.MapRoute("Contact", "Contact", new { controller = "Support", action = "Contact" });
            _routes.MapRoute("About", "About", new { controller = "Support", action = "About" });
            _routes.MapRoute("PromoteSite", "PromoteSite", new {controller = "Support", action = "PromoteSite"});
            _routes.MapRoute("Badges", "Odznaki", new {controller = "Badges", action = "All"});
            
            _routes.MapRoute("Facebook", "Facebook/{action}", new { controller = "Facebook", action = "LogByFbData" });

            _routes.MapRoute("Login", "Login", new { controller = "Membership", action = "Login" });
            _routes.MapRoute("OpenId", "OpenId", new { controller = "Membership", action = "OpenId" });
            _routes.MapRoute("Logout", "Logout", new { controller = "Membership", action = "Logout" });
            _routes.MapRoute("Signup", "Signup", new { controller = "Membership", action = "Signup" });
            _routes.MapRoute("DeleteAd", "DeleteAd", new { controller = "Recommendation", action = "DeleteAd" });
            _routes.MapRoute("GetAd", "GatAd", new { controller = "Recommendation", action = "GetAd" });
            _routes.MapRoute("EditAd", "EditAd", new { controller = "Recommendation", action = "EditAd" });
            _routes.MapRoute("AdList", "AdList", new { controller = "Recommendation", action = "AdList" });            
            _routes.MapRoute("EventsBox", "EventsBox", new { controller = "CommingEvent", action = "EventsBox" });
            _routes.MapRoute("AllCommingEvent", "AllCommingEvent", new { controller = "CommingEvent", action = "AllCommingEvent" });
            _routes.MapRoute("GetEvent", "GetEvent", new { controller = "CommingEvent", action = "GetEvent" });
            _routes.MapRoute("EditEvent", "EditEvent", new { controller = "CommingEvent", action = "EditEvent" });
            _routes.MapRoute("DeleteEvent", "DeleteEvent", new { controller = "CommingEvent", action = "DeleteEvent" });
            _routes.MapRoute("CommingEventsEditBox", "CommingEventsEditBox", new { controller = "CommingEvent", action = "CommingEventsEditBox" });
            _routes.MapRoute("ForgotPassword", "ForgotPassword", new { controller = "Membership", action = "ForgotPassword" });
            _routes.MapRoute("ChangePassword", "ChangePassword", new { controller = "Membership", action = "ChangePassword" });
            _routes.MapRoute("Activate", "Activate/{id}", new { controller = "Membership", action = "Activate", id = string.Empty });
            _routes.MapRoute("Users", "Users/{page}", new { controller = "Membership", action = "List", page = 1 }, new { page = @"^\d+$" });
            _routes.MapRoute("User", "Users/{name}/{tab}/{page}", new { controller = "Membership", action = "Detail", tab = UserDetailTab.Promoted, page = 1 });
            _routes.MapRoute("ChangeEmail", "ChangeEmail", new { controller = "Membership", action = "ChangeEmail" });
            _routes.MapRoute("ChangeRole", "ChangeRole", new { controller = "Membership", action = "ChangeRole" });
            _routes.MapRoute("LockUser", "LockUser", new { controller = "Membership", action = "Lock" });
            _routes.MapRoute("UnlockUser", "UnlockUser", new { controller = "Membership", action = "Unlock" });
            _routes.MapRoute("AllowIps", "AllowIps", new { controller = "Membership", action = "AllowIps" });

            _routes.MapRoute("GetPublished", "GetPublished/{start}/{max}", new { controller = "Story", action = "GetPublished", start = 1, max = _settings.CarouselStoryCount });
            _routes.MapRoute("GetUpcoming", "GetUpcoming/{start}/{max}", new { controller = "Story", action = "GetUpcoming", start = 1, max = _settings.CarouselStoryCount });
            _routes.MapRoute("SuggestTags", "SuggestTags", new { controller = "Tag", action = "SuggestTags" });

            _routes.MapRoute("Submit", "Submit", new { controller = "Story", action = "Submit" });
            _routes.MapRoute("Retrieve", "Retrieve", new { controller = "Story", action = "Retrieve" });

            _routes.MapRoute("Click", "Click", new { controller = "Story", action = "Click" });
            _routes.MapRoute("Promote", "Promote", new { controller = "Story", action = "Promote" });
            _routes.MapRoute("Demote", "Demote", new { controller = "Story", action = "Demote" });
            _routes.MapRoute("MarkAsSpam", "MarkAsSpam", new { controller = "Story", action = "MarkAsSpam" });
            _routes.MapRoute("Comment", "Comment", new { controller = "Comment", action = "Post" });
            _routes.MapRoute("GetStory", "GetStory", new { controller = "Story", action = "GetStory" });
            _routes.MapRoute("Update", "Update", new { controller = "Story", action = "Update" });
            _routes.MapRoute("Delete", "Delete", new { controller = "Story", action = "Delete" });
            _routes.MapRoute("ApproveStory", "ApproveStory", new { controller = "Story", action = "Approve" });
            _routes.MapRoute("ConfirmSpamStory", "ConfirmSpamStory", new { controller = "Story", action = "ConfirmSpam" });
            _routes.MapRoute("ConfirmSpamComment", "ConfirmSpamComment", new { controller = "Comment", action = "ConfirmSpam" });
            _routes.MapRoute("MarkCommentAsOffended", "MarkCommentAsOffended", new { controller = "Comment", action = "MarkAsOffended" });
            _routes.MapRoute("GenerateMiniatureStory", "GenerateMiniatureStory", new { controller = "Story", action = "GenerateMiniature" });

            _routes.MapRoute("Publish", "Publish", new { controller = "Story", action = "Publish" });

            _routes.MapRoute("Upcoming", "Upcoming/{page}", new { controller = "Story", action = "Upcoming", page = 1 });
            _routes.MapRoute("New", "New/{page}", new { controller = "Story", action = "New", page = 1 });
            _routes.MapRoute("Unapproved", "Unapproved/{page}", new { controller = "Story", action = "Unapproved", page = 1 });
            _routes.MapRoute("Publishable", "Publishable/{page}", new { controller = "Story", action = "Publishable", page = 1 });
            _routes.MapRoute("Questions", "Questions", new { controller = "Story", action = "Questions" });

            _routes.MapRoute("Search", "Search", new { controller = "Story", action = "Search" });
            _routes.MapRoute("Similar", "Similar", new {controller = "Story", action = "Similars"});
            _routes.MapRoute("StoryList", "{action}/{name}/{page}", new { controller = "Story", page = 1 });
            _routes.MapRoute("Published", "{page}", new { controller = "Story", action = "Published", page = 1 }, new { page = @"^\d+$" });
            _routes.MapRoute("Detail", "{name}", new { controller = "Story", action = "Detail" });
            
            _routes.MapRoute("Default", "{controller}/{action}", new { controller = "Story", action = "Published" });     

        }
    }
}