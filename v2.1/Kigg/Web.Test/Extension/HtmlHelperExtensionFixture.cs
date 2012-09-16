using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.Routing;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;
    using Kigg.Test.Infrastructure;

    public class HtmlHelperExtensionFixture : BaseFixture
    {
        private const string AppName = "/Kigg";
        private const string AppPath = MvcTestHelper.AppPathModifier + AppName;

        const string Title = "This is a dummy title";

        private readonly HttpContextMock _httpContext;

        public HtmlHelperExtensionFixture()
        {
            _httpContext = MvcTestHelper.GetHttpContext(AppPath, null, null);
        }

        [Fact]
        public void PageHeader_With_SyndicationIcons_Should_Return_Correct_Html()
        {
            var html = GetHtmlHelper().PageHeader("This is a dummy title", "/feed/rss/published", "/feed/atom/published");

            Assert.Contains("<h2>{0}</h2>".FormatWith(Title), html);
        }

        [Fact]
        public void PageHeader_Should_Return_Correct_Html()
        {
            var html = GetHtmlHelper().PageHeader("This is a dummy title");

            Assert.Contains("<h2>{0}</h2>".FormatWith(Title), html);
        }

        [Fact]
        public void SyndicationIcons_Should_Return_Both_Icons()
        {
            var html = GetHtmlHelper().SyndicationIcons("/Feed/Rss/Published", "/Feed/Atom/Published");

            Assert.Contains("rss", html);
            Assert.Contains("atom", html);
        }

        [Fact]
        public void ToDictionary_Should_Return_Dictionary_Which_Contains_Enum_Value_In_Key_And_Enum_Name_In_Value()
        {
            var dictionary = GetHtmlHelper().ToDictionary<Roles>();

            Assert.True(dictionary.ContainsKey((int) Roles.Administrator));
            Assert.True(dictionary.ContainsKey((int) Roles.Moderator));
            Assert.True(dictionary.ContainsKey((int) Roles.Bot));
            Assert.True(dictionary.ContainsKey((int) Roles.User));
        }

        [Fact]
        public void StoryListPager_For_Published_Should_Return_Correct_Html_When_Current_Page_Is_One_And_Total_Story_Count_Is_Two_Hundred()
        {
            string output = "<div class=\"pager\"> " +
                                "<span class=\"disabled\">Previous</span> " +
                                "<span class=\"current\">1</span> " +
                                "<a href=\"{0}/2\">2</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/3\">3</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/4\">4</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/5\">5</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/6\">6</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/7\">7</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/8\">8</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/9\">9</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/10\">10</a> ".FormatWith(AppPath) +
                                "... " +
                                "<a href=\"{0}/19\">19</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/20\">20</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/2\">Next</a>".FormatWith(AppPath) +
                            "</div>";

            var helper = GetHtmlHelperForStoryListPager("Published", 1, 200);

            var html = helper.StoryListPager();

            Assert.Equal(output, html);
        }

        [Fact]
        public void StoryListPager_For_Published_Should_Return_Correct_Html_When_Current_Page_Is_Ten_And_Total_Story_Count_Is_Two_Hundred()
        {
            string output = "<div class=\"pager\"> " +
                                "<a href=\"{0}/9\">Previous</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/\">1</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/2\">2</a> ".FormatWith(AppPath) +
                                "... " +
                                "<a href=\"{0}/6\">6</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/7\">7</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/8\">8</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/9\">9</a> ".FormatWith(AppPath) +
                                "<span class=\"current\">10</span> " +
                                "<a href=\"{0}/11\">11</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/12\">12</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/13\">13</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/14\">14</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/15\">15</a> ".FormatWith(AppPath) +
                                "... " +
                                "<a href=\"{0}/19\">19</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/20\">20</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/11\">Next</a>".FormatWith(AppPath) +
                            "</div>";

            var helper = GetHtmlHelperForStoryListPager("Published", 10, 200);

            var html = helper.StoryListPager();

            Assert.Equal(output, html);
        }

        [Fact]
        public void StoryListPager_For_Published_Should_Return_Correct_Html_When_Current_Page_Is_Twenty_And_Total_Story_Count_Is_Two_Hundred()
        {
            string output = "<div class=\"pager\"> " +
                                "<a href=\"{0}/19\">Previous</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/\">1</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/2\">2</a> ".FormatWith(AppPath) +
                                "... " +
                                "<a href=\"{0}/11\">11</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/12\">12</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/13\">13</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/14\">14</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/15\">15</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/16\">16</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/17\">17</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/18\">18</a> ".FormatWith(AppPath) +
                                "<a href=\"{0}/19\">19</a> ".FormatWith(AppPath) +
                                "<span class=\"current\">20</span> " +
                                "<span class=\"disabled\">Next</span>" +
                            "</div>";

            var helper = GetHtmlHelperForStoryListPager("Published", 20, 200);

            var html = helper.StoryListPager();

            Assert.Equal(output, html);
        }

        [Fact]
        public void StoryListPager_For_Search_Should_Return_Correct_Html_When_Current_Page_Is_One_And_Total_Story_Count_Is_SixtyTwo()
        {
            const string Query = "Test";

            string output = "<div class=\"pager\"> " + 
                                "<span class=\"disabled\">Previous</span> " +
                                "<span class=\"current\">1</span> " +
                                "<a href=\"{0}/Search?page=2&amp;q={1}\">2</a> ".FormatWith(AppPath, Query) +
                                "<a href=\"{0}/Search?page=3&amp;q={1}\">3</a> ".FormatWith(AppPath, Query) +
                                "<a href=\"{0}/Search?page=4&amp;q={1}\">4</a> ".FormatWith(AppPath, Query) +
                                "<a href=\"{0}/Search?page=5&amp;q={1}\">5</a> ".FormatWith(AppPath, Query) +
                                "<a href=\"{0}/Search?page=6&amp;q={1}\">6</a> ".FormatWith(AppPath, Query) +
                                "<a href=\"{0}/Search?page=7&amp;q={1}\">7</a> ".FormatWith(AppPath, Query) +
                                "<a href=\"{0}/Search?page=2&amp;q={1}\">Next</a>".FormatWith(AppPath, Query) +
                            "</div>";

            var html = GetHtmlHelperForSearchStoryListPager("Test", 1, 62).StoryListPager();

            Assert.Equal(output, html);
        }

        [Fact]
        public void StoryListPager_For_User_Should_Return_Correct_Html_When_Current_Page_Is_One_And_Total_Story_Count_Is_Two_Hundred()
        {
            string userId = Guid.NewGuid().Shrink();

            string output = "<div class=\"pager\"> " +
                                "<span class=\"disabled\">Previous</span> " +
                                "<span class=\"current\">1</span> " +
                                "<a href=\"{0}/Users/{1}/Promoted/2\">2</a> ".FormatWith(AppPath, userId) +
                                "<a href=\"{0}/Users/{1}/Promoted/3\">3</a> ".FormatWith(AppPath, userId) +
                                "<a href=\"{0}/Users/{1}/Promoted/4\">4</a> ".FormatWith(AppPath, userId) +
                                "<a href=\"{0}/Users/{1}/Promoted/5\">5</a> ".FormatWith(AppPath, userId) +
                                "<a href=\"{0}/Users/{1}/Promoted/6\">6</a> ".FormatWith(AppPath, userId) +
                                "<a href=\"{0}/Users/{1}/Promoted/7\">7</a> ".FormatWith(AppPath, userId) +
                                "<a href=\"{0}/Users/{1}/Promoted/8\">8</a> ".FormatWith(AppPath, userId) +
                                "<a href=\"{0}/Users/{1}/Promoted/9\">9</a> ".FormatWith(AppPath, userId) +
                                "<a href=\"{0}/Users/{1}/Promoted/10\">10</a> ".FormatWith(AppPath, userId) +
                                "... " +
                                "<a href=\"{0}/Users/{1}/Promoted/19\">19</a> ".FormatWith(AppPath, userId) +
                                "<a href=\"{0}/Users/{1}/Promoted/20\">20</a> ".FormatWith(AppPath, userId) +
                                "<a href=\"{0}/Users/{1}/Promoted/2\">Next</a>".FormatWith(AppPath, userId) +
                            "</div>";

            var routeData = new RouteData();

            routeData.Values["name"] = userId;

            var helper = GetHtmlHelperForStoryListPager("PromotedBy", routeData, 1, 200);

            var html = helper.UserStoryListPager(UserDetailTab.Promoted);

            Assert.Equal(output, html);
        }

        private HtmlHelper GetHtmlHelper()
        {
            return GetHtmlHelper(new ViewDataDictionary(), new RouteData());
        }

        private HtmlHelper GetHtmlHelper(ViewDataDictionary vdd, RouteData routeData)
        {
            RouteTable.Routes.Clear();
            new RegisterRoutes(settings.Object).Execute();

            ControllerContext controllerContext = new ControllerContext(_httpContext.Object, routeData, new Mock<ControllerBase>().Object);
            ViewContext viewContext = new ViewContext(controllerContext, new Mock<IView>().Object, vdd, new TempDataDictionary());

            Mock<IViewDataContainer> mockVdc = new Mock<IViewDataContainer>();
            mockVdc.Expect(vdc => vdc.ViewData).Returns(vdd);

            HtmlHelper htmlHelper = new HtmlHelper(viewContext, mockVdc.Object);

            return htmlHelper;
        }

        private HtmlHelper GetHtmlHelperForStoryListPager(string actionName, int currentPage, int totalStoryCount)
        {
            return GetHtmlHelperForStoryListPager(actionName, new RouteData(), currentPage, totalStoryCount);
        }

        private HtmlHelper GetHtmlHelperForStoryListPager(string actionName, RouteData routeData, int currentPage, int totalStoryCount)
        {
            routeData.Values["controller"] = "Story";
            routeData.Values["action"] = actionName;
            routeData.Values["page"] = currentPage;

            StoryListViewData model = new StoryListViewData
                                          {
                                              StoryPerPage = 10,
                                              TotalStoryCount = totalStoryCount,
                                              CurrentPage = currentPage
                                          };

            return GetHtmlHelper(new ViewDataDictionary { Model = model }, routeData);
        }

        private HtmlHelper GetHtmlHelperForSearchStoryListPager(string query, int currentPage, int totalStoryCount)
        {
            var routeData = new RouteData();

            routeData.Values["controller"] = "Story";
            routeData.Values["action"] = "Search";
            routeData.Values["page"] = currentPage;

            _httpContext.HttpRequest.ExpectGet(r => r.QueryString).Returns(new NameValueCollection { { "q", query }, { "page", currentPage.ToString() } });

            StoryListSearchViewData model =   new StoryListSearchViewData
                                              {
                                                  StoryPerPage = 10,
                                                  TotalStoryCount = totalStoryCount,
                                                  CurrentPage = currentPage,
                                                  Query = query
                                              };

            return GetHtmlHelper(new ViewDataDictionary { Model = model }, routeData);
        }
    }
}