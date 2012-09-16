using System.Web.Mvc;
using System.Web.Routing;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    public class AutoRefreshAttributeFixture
    {
        private const string AppPath = MvcTestHelper.AppPathModifier + "/Kigg";
        private const int DurationInSeconds = 10;

        private readonly HttpContextMock _httpContext;

        public AutoRefreshAttributeFixture()
        {
            _httpContext = MvcTestHelper.GetHttpContext("/Kigg", null, null);

            var settings = new Mock<IConfigurationSettings>();

            settings.ExpectGet(s => s.FeedStoryPerPage).Returns(25);
            settings.ExpectGet(s => s.CarouselStoryCount).Returns(30);

            RouteTable.Routes.Clear();
            new RegisterRoutes(settings.Object).Execute();
        }

        [Fact]
        public void DurationInSeconds_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            var attribute = new AutoRefreshAttribute(DurationInSeconds);

            Assert.Equal(DurationInSeconds, attribute.DurationInSeconds);
        }

        [Fact]
        public void Should_Not_Throw_Exception_On_Default_Constructor()
        {
            Assert.DoesNotThrow(() => new AutoRefreshAttribute());
        }

        [Fact]
        public void BuildUrl_Should_Return_Correct_Url_When_Action_Controller_And_Route_Value_Is_Passed_As_RouteDictionary()
        {
            var values = new RouteValueDictionary
                             {
                                 {"start", 2 },
                                 {"max", 20}
                             };

            var resultContext = BuildResultContext();
            var attribute = new AutoRefreshAttribute("Published", "Feed", values);

            string url = attribute.BuildUrl(resultContext);

            Assert.Equal("{0}/Feed/Rss/Published/2/20".FormatWith(AppPath), url);
        }

        [Fact]
        public void BuildUrl_Returns_Correct_Url_When_Action_Controller_And_Route_Value_Is_Passed_As_Object()
        {
            var values = new
                             {
                                 start = 2,
                                 max = 20
                             };

            var resultContext = BuildResultContext();
            var attribute = new AutoRefreshAttribute("Published", "Feed", values);

            string url = attribute.BuildUrl(resultContext);

            Assert.Equal("{0}/Feed/Rss/Published/2/20".FormatWith(AppPath), url);
        }

        [Fact]
        public void BuildUrl_Returns_Correct_Url_When_Action_And_Route_Value_Is_Passed_As_RouteDictionary()
        {
            var values = new
                             {
                                 start = 2,
                                 max = 20
                             };

            var resultContext = BuildResultContext();
            var attribute = new AutoRefreshAttribute("Published", values);

            string url = attribute.BuildUrl(resultContext);

            Assert.Equal("{0}/Feed/Rss/Published/2/20".FormatWith(AppPath), url);
        }

        [Fact]
        public void BuildUrl_Returns_Correct_Url_When_Action_And_Route_Value_Is_Passed_As_Object()
        {
            var values = new RouteValueDictionary
                             {
                                 {"start", 2 },
                                 {"max", 20}
                             };

            var resultContext = BuildResultContext();
            var attribute = new AutoRefreshAttribute("Published", values);

            string url = attribute.BuildUrl(resultContext);

            Assert.Equal("{0}/Feed/Rss/Published/2/20".FormatWith(AppPath), url);
        }

        [Fact]
        public void BuildUrl_Returns_Correct_Url_When_Action_And_Controller_Is_Passed()
        {
            var resultContext = BuildResultContext();
            var attribute = new AutoRefreshAttribute("Published", "Feed");

            string url = attribute.BuildUrl(resultContext);

            Assert.Equal("{0}/Feed/Rss/Published".FormatWith(AppPath), url);
        }

        [Fact]
        public void BuildUrl_Returns_Correct_Url_When_Action_Is_Passed()
        {
            var resultContext = BuildResultContext();
            var attribute = new AutoRefreshAttribute("Published");

            string url = attribute.BuildUrl(resultContext);

            Assert.Equal("{0}/Feed/Rss/Published".FormatWith(AppPath), url);
        }

        [Fact]
        public void OnResultExecuting_Should_Add_Refresh_Header()
        {
            var resultContext = BuildResultContext();

            _httpContext.HttpResponse.Expect(r => r.AppendHeader(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            var attribute = new AutoRefreshAttribute();
            attribute.OnResultExecuting(resultContext);

            _httpContext.HttpResponse.Verify();
        }

        private ResultExecutingContext BuildResultContext()
        {
            var route = new RouteData();
            route.Values.Add("controller", "Feed");

            var controllerContext = new ControllerContext(_httpContext.Object, route, new Mock<ControllerBase>().Object);
            var resultContext = new ResultExecutingContext(controllerContext, new EmptyResult());

            return resultContext;
        }
    }
}