using System.Web.Routing;

using Xunit;

namespace Kigg.Web.Test
{
    using Infrastructure;
    using Kigg.Test.Infrastructure;

    public class RegisterRoutesFixtures : BaseFixture
    {
        private readonly RouteCollection _routes;
        private readonly IBootstrapperTask _task;

        public RegisterRoutesFixtures()
        {
            _routes = new RouteCollection();
            _task = new RegisterRoutes(settings.Object, _routes);

            _task.Execute();
        }

        [Fact]
        public void Default_Constructor_Should_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new RegisterRoutes(settings.Object));
        }

        [Fact]
        public void Execute_Should_Register_Routes()
        {
            Assert.NotEqual(0, _routes.Count);
        }

        [Fact]
        public void Should_Route_Existing_File()
        {
            Assert.True(_routes.RouteExistingFiles);
        }

        [Fact]
        public void Should_Return_Published_Rss()
        {
            var routeData = GetRouteDataFor("~/Feed/Rss/Published");

            Assert.Equal("Feed", routeData.Values["controller"]);
            Assert.Equal("Published", routeData.Values["action"]);
            Assert.Equal("Rss", routeData.Values["format"]);
        }

        [Fact]
        public void Should_Return_Category_Rss()
        {
            var routeData = GetRouteDataFor("~/Feed/Rss/Category/ASP.NET");

            Assert.Equal("Feed", routeData.Values["controller"]);
            Assert.Equal("Category", routeData.Values["action"]);
            Assert.Equal("ASP.NET", routeData.Values["name"]);
            Assert.Equal("Rss", routeData.Values["format"]);
        }

        [Fact]
        public void Should_Return_Upcoming_Rss()
        {
            var routeData = GetRouteDataFor("~/Feed/Rss/Upcoming");

            Assert.Equal("Feed", routeData.Values["controller"]);
            Assert.Equal("Upcoming", routeData.Values["action"]);
            Assert.Equal("Rss", routeData.Values["format"]);
        }

        [Fact]
        public void Should_Return_Tags_Rss()
        {
            var routeData = GetRouteDataFor("~/Feed/Rss/Tags/ASP-MVC");

            Assert.Equal("Feed", routeData.Values["controller"]);
            Assert.Equal("Tags", routeData.Values["action"]);
            Assert.Equal("ASP-MVC", routeData.Values["name"]);
            Assert.Equal("Rss", routeData.Values["format"]);
        }

        [Fact]
        public void Should_Return_PromotedBy_Rss()
        {
            var routeData = GetRouteDataFor("~/Feed/Rss/PromotedBy/DummyUser");

            Assert.Equal("Feed", routeData.Values["controller"]);
            Assert.Equal("PromotedBy", routeData.Values["action"]);
            Assert.Equal("DummyUser", routeData.Values["name"]);
            Assert.Equal("Rss", routeData.Values["format"]);
        }

        [Fact]
        public void Should_Return_PostedBy_Rss()
        {
            var routeData = GetRouteDataFor("~/Feed/Rss/PostedBy/DummyUser");

            Assert.Equal("Feed", routeData.Values["controller"]);
            Assert.Equal("PostedBy", routeData.Values["action"]);
            Assert.Equal("DummyUser", routeData.Values["name"]);
            Assert.Equal("Rss", routeData.Values["format"]);
        }

        [Fact]
        public void Should_Return_CommentedBy_Rss()
        {
            var routeData = GetRouteDataFor("~/Feed/Rss/CommentedBy/DummyUser");

            Assert.Equal("Feed", routeData.Values["controller"]);
            Assert.Equal("CommentedBy", routeData.Values["action"]);
            Assert.Equal("DummyUser", routeData.Values["name"]);
            Assert.Equal("Rss", routeData.Values["format"]);
        }

        [Fact]
        public void Should_Return_Search_Rss()
        {
            var routeData = GetRouteDataFor("~/Feed/Rss/Search/Route");

            Assert.Equal("Feed", routeData.Values["controller"]);
            Assert.Equal("Search", routeData.Values["action"]);
            Assert.Equal("Route", routeData.Values["q"]);
            Assert.Equal("Rss", routeData.Values["format"]);
        }

        [Fact]
        public void Should_Return_Published_Atom()
        {
            var routeData = GetRouteDataFor("~/Feed/Atom/Published");

            Assert.Equal("Feed", routeData.Values["controller"]);
            Assert.Equal("Published", routeData.Values["action"]);
            Assert.Equal("Atom", routeData.Values["format"]);
        }

        [Fact]
        public void Should_Return_Category_Atom()
        {
            var routeData = GetRouteDataFor("~/Feed/Atom/Category/ASP.NET");

            Assert.Equal("Feed", routeData.Values["controller"]);
            Assert.Equal("Category", routeData.Values["action"]);
            Assert.Equal("ASP.NET", routeData.Values["name"]);
            Assert.Equal("Atom", routeData.Values["format"]);
        }

        [Fact]
        public void Should_Return_Upcoming_Atom()
        {
            var routeData = GetRouteDataFor("~/Feed/Atom/Upcoming");

            Assert.Equal("Feed", routeData.Values["controller"]);
            Assert.Equal("Upcoming", routeData.Values["action"]);
            Assert.Equal("Atom", routeData.Values["format"]);
        }

        [Fact]
        public void Should_Return_Tags_Atom()
        {
            var routeData = GetRouteDataFor("~/Feed/Atom/Tags/ASP-MVC");

            Assert.Equal("Feed", routeData.Values["controller"]);
            Assert.Equal("Tags", routeData.Values["action"]);
            Assert.Equal("ASP-MVC", routeData.Values["name"]);
            Assert.Equal("Atom", routeData.Values["format"]);
        }

        [Fact]
        public void Should_Return_PromotedBy_Atom()
        {
            var routeData = GetRouteDataFor("~/Feed/Atom/PromotedBy/DummyUser");

            Assert.Equal("Feed", routeData.Values["controller"]);
            Assert.Equal("PromotedBy", routeData.Values["action"]);
            Assert.Equal("DummyUser", routeData.Values["name"]);
            Assert.Equal("Atom", routeData.Values["format"]);
        }

        [Fact]
        public void Should_Return_PostedBy_Atom()
        {
            var routeData = GetRouteDataFor("~/Feed/Atom/PostedBy/DummyUser");

            Assert.Equal("Feed", routeData.Values["controller"]);
            Assert.Equal("PostedBy", routeData.Values["action"]);
            Assert.Equal("DummyUser", routeData.Values["name"]);
            Assert.Equal("Atom", routeData.Values["format"]);
        }

        [Fact]
        public void Should_Return_CommentedBy_Atom()
        {
            var routeData = GetRouteDataFor("~/Feed/Atom/CommentedBy/DummyUser");

            Assert.Equal("Feed", routeData.Values["controller"]);
            Assert.Equal("CommentedBy", routeData.Values["action"]);
            Assert.Equal("DummyUser", routeData.Values["name"]);
            Assert.Equal("Atom", routeData.Values["format"]);
        }

        [Fact]
        public void Should_Return_Search_Atom()
        {
            var routeData = GetRouteDataFor("~/Feed/Atom/Search/Route");

            Assert.Equal("Feed", routeData.Values["controller"]);
            Assert.Equal("Search", routeData.Values["action"]);
            Assert.Equal("Route", routeData.Values["q"]);
            Assert.Equal("Atom", routeData.Values["format"]);
        }

        [Fact]
        public void Should_Return_Faq()
        {
            var routeData = GetRouteDataFor("~/Faq", true);

            Assert.Equal("Support", routeData.Values["controller"]);
            Assert.Equal("Faq", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_Contact()
        {
            var routeData = GetRouteDataFor("~/Contact", true);

            Assert.Equal("Support", routeData.Values["controller"]);
            Assert.Equal("Contact", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_About()
        {
            var routeData = GetRouteDataFor("~/About", true);

            Assert.Equal("Support", routeData.Values["controller"]);
            Assert.Equal("About", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_Login()
        {
            var routeData = GetRouteDataFor("~/Login", true);

            Assert.Equal("Membership", routeData.Values["controller"]);
            Assert.Equal("Login", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_OpenId()
        {
            var routeData = GetRouteDataFor("~/OpenId", true);

            Assert.Equal("Membership", routeData.Values["controller"]);
            Assert.Equal("OpenId", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_Logout()
        {
            var routeData = GetRouteDataFor("~/Logout", true);

            Assert.Equal("Membership", routeData.Values["controller"]);
            Assert.Equal("Logout", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_Signup()
        {
            var routeData = GetRouteDataFor("~/Signup", true);

            Assert.Equal("Membership", routeData.Values["controller"]);
            Assert.Equal("Signup", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_ForgotPassword()
        {
            var routeData = GetRouteDataFor("~/ForgotPassword", true);

            Assert.Equal("Membership", routeData.Values["controller"]);
            Assert.Equal("ForgotPassword", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_ChangePassword()
        {
            var routeData = GetRouteDataFor("~/ChangePassword", true);

            Assert.Equal("Membership", routeData.Values["controller"]);
            Assert.Equal("ChangePassword", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_Activate()
        {
            var routeData = GetRouteDataFor("~/Activate/JKgxzYZ2dEeRQz_D7XlRDw");

            Assert.Equal("Membership", routeData.Values["controller"]);
            Assert.Equal("Activate", routeData.Values["action"]);
            Assert.Equal("JKgxzYZ2dEeRQz_D7XlRDw", routeData.Values["id"]);
        }

        [Fact]
        public void Should_Return_Users()
        {
            var routeData = GetRouteDataFor("~/Users");

            Assert.Equal("Membership", routeData.Values["controller"]);
            Assert.Equal("List", routeData.Values["action"]);
            Assert.Equal(1, routeData.Values["page"]);
        }

        [Fact]
        public void Should_Return_User()
        {
            var routeData = GetRouteDataFor("~/Users/kazimanzurrashid");

            Assert.Equal("Membership", routeData.Values["controller"]);
            Assert.Equal("Detail", routeData.Values["action"]);
            Assert.Equal("kazimanzurrashid", routeData.Values["name"]);
            Assert.Equal(UserDetailTab.Promoted, routeData.Values["tab"]);
            Assert.Equal(1, routeData.Values["page"]);
        }

        [Fact]
        public void Should_Return_User_Posted()
        {
            var routeData = GetRouteDataFor("~/Users/kazimanzurrashid/Posted");

            Assert.Equal("Membership", routeData.Values["controller"]);
            Assert.Equal("Detail", routeData.Values["action"]);
            Assert.Equal("kazimanzurrashid", routeData.Values["name"]);
            Assert.Equal("Posted", routeData.Values["tab"]);
            Assert.Equal(1, routeData.Values["page"]);
        }

        [Fact]
        public void Should_Return_User_Commented()
        {
            var routeData = GetRouteDataFor("~/Users/kazimanzurrashid/Commented");

            Assert.Equal("Membership", routeData.Values["controller"]);
            Assert.Equal("Detail", routeData.Values["action"]);
            Assert.Equal("kazimanzurrashid", routeData.Values["name"]);
            Assert.Equal("Commented", routeData.Values["tab"]);
            Assert.Equal(1, routeData.Values["page"]);
        }

        [Fact]
        public void Should_Return_ChangeEmail()
        {
            var routeData = GetRouteDataFor("~/ChangeEmail", true);

            Assert.Equal("Membership", routeData.Values["controller"]);
            Assert.Equal("ChangeEmail", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_ChangeRole()
        {
            var routeData = GetRouteDataFor("~/ChangeRole", true);

            Assert.Equal("Membership", routeData.Values["controller"]);
            Assert.Equal("ChangeRole", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_LockUser()
        {
            var routeData = GetRouteDataFor("~/LockUser", true);

            Assert.Equal("Membership", routeData.Values["controller"]);
            Assert.Equal("Lock", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_UnlockUser()
        {
            var routeData = GetRouteDataFor("~/UnlockUser", true);

            Assert.Equal("Membership", routeData.Values["controller"]);
            Assert.Equal("Unlock", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_AllowIps()
        {
            var routeData = GetRouteDataFor("~/AllowIps", true);

            Assert.Equal("Membership", routeData.Values["controller"]);
            Assert.Equal("AllowIps", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_GetPublished()
        {
            var routeData = GetRouteDataFor("~/GetPublished/11/10");

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("GetPublished", routeData.Values["action"]);
            Assert.Equal("11", routeData.Values["start"]);
            Assert.Equal("10", routeData.Values["max"]);
        }

        [Fact]
        public void Should_Return_GetUpcoming()
        {
            var routeData = GetRouteDataFor("~/GetUpcoming/11/10");

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("GetUpcoming", routeData.Values["action"]);
            Assert.Equal("11", routeData.Values["start"]);
            Assert.Equal("10", routeData.Values["max"]);
        }

        [Fact]
        public void Should_Return_SuggestTags()
        {
            var routeData = GetRouteDataFor("~/SuggestTags");

            Assert.Equal("Tag", routeData.Values["controller"]);
            Assert.Equal("SuggestTags", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_Submit()
        {
            var routeData = GetRouteDataFor("~/Submit", true);

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Submit", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_Retrieve()
        {
            var routeData = GetRouteDataFor(string.Format("~/Retrieve"));

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Retrieve", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_Click()
        {
            var routeData = GetRouteDataFor("~/Click", true);

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Click", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_Promote()
        {
            var routeData = GetRouteDataFor("~/Promote", true);

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Promote", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_Demote()
        {
            var routeData = GetRouteDataFor("~/Demote", true);

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Demote", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_MarkAsSpam()
        {
            var routeData = GetRouteDataFor("~/MarkAsSpam", true);

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("MarkAsSpam", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_Comment()
        {
            var routeData = GetRouteDataFor("~/Comment", true);

            Assert.Equal("Comment", routeData.Values["controller"]);
            Assert.Equal("Post", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_GetStory()
        {
            var routeData = GetRouteDataFor("~/GetStory", true);

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("GetStory", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_Update()
        {
            var routeData = GetRouteDataFor("~/Update", true);

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Update", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_Delete()
        {
            var routeData = GetRouteDataFor("~/Delete", true);

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Delete", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_ApproveStory()
        {
            var routeData = GetRouteDataFor("~/ApproveStory", true);

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Approve", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_ConfirmSpamStory()
        {
            var routeData = GetRouteDataFor("~/ConfirmSpamStory", true);

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("ConfirmSpam", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_ConfirmSpamComment()
        {
            var routeData = GetRouteDataFor("~/ConfirmSpamComment", true);

            Assert.Equal("Comment", routeData.Values["controller"]);
            Assert.Equal("ConfirmSpam", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_MarkCommentAsOffended()
        {
            var routeData = GetRouteDataFor("~/MarkCommentAsOffended", true);

            Assert.Equal("Comment", routeData.Values["controller"]);
            Assert.Equal("MarkAsOffended", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_Publish()
        {
            var routeData = GetRouteDataFor("~/Publish", true);

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Publish", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_Upcoming()
        {
            var routeData = GetRouteDataFor("~/Upcoming/50");

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Upcoming", routeData.Values["action"]);
            Assert.Equal("50", routeData.Values["page"]);
        }

        [Fact]
        public void Should_Return_Unapproved()
        {
            var routeData = GetRouteDataFor("~/Unapproved/2");

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Unapproved", routeData.Values["action"]);
            Assert.Equal("2", routeData.Values["page"]);
        }

        [Fact]
        public void Should_Return_New()
        {
            var routeData = GetRouteDataFor("~/New/2");

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("New", routeData.Values["action"]);
            Assert.Equal("2", routeData.Values["page"]);
        }

        [Fact]
        public void Should_Return_Publishable()
        {
            var routeData = GetRouteDataFor("~/Publishable/2");

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Publishable", routeData.Values["action"]);
            Assert.Equal("2", routeData.Values["page"]);
        }

        [Fact]
        public void Should_Return_Search()
        {
            var routeData = GetRouteDataFor("~/Search");

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Search", routeData.Values["action"]);
        }

        [Fact]
        public void Should_Return_Category()
        {
            var routeData = GetRouteDataFor("~/Category/ASP.NET/2", false);

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Category", routeData.Values["action"]);
            Assert.Equal("ASP.NET", routeData.Values["name"]);
            Assert.Equal("2", routeData.Values["page"]);
        }

        [Fact]
        public void Should_Return_Tags()
        {
            var routeData = GetRouteDataFor("~/Tags/Linq/3");

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Tags", routeData.Values["action"]);
            Assert.Equal("Linq", routeData.Values["name"]);
            Assert.Equal("3", routeData.Values["page"]);
        }

        [Fact]
        public void Should_Return_Published()
        {
            var routeData = GetRouteDataFor("~/2");

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Published", routeData.Values["action"]);
            Assert.Equal("2", routeData.Values["page"]);
        }

        [Fact]
        public void Should_Return_Detail()
        {
            var routeData = GetRouteDataFor("~/A-Dummy-Story");

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Detail", routeData.Values["action"]);
            Assert.Equal("A-Dummy-Story", routeData.Values["name"]);
        }

        [Fact]
        public void Should_Return_Home()
        {
            var routeData = GetRouteDataFor("~/");

            Assert.Equal("Story", routeData.Values["controller"]);
            Assert.Equal("Published", routeData.Values["action"]);
        }

        private RouteData GetRouteDataFor(string url)
        {
            return GetRouteDataFor(url, false);
        }

        private RouteData GetRouteDataFor(string url, bool forPost)
        {
            var context = MvcTestHelper.GetHttpContext("/", url, forPost ? "POST" : null );
            return _routes.GetRouteData(context.Object);
        }
    }
}