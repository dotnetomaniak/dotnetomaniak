using System.Collections.Generic;
using System.Data.Services;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace Kigg.DataServices
{
    using Data;
    using ServiceContracts;
    using Infrastructure;

    #if(DEBUG)
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    #else
    [ServiceBehavior(IncludeExceptionDetailInFaults = false)]
    #endif
    public class BaseDataService : DataService<IStoryDataService>
    {
        public static void InitializeService(IDataServiceConfiguration config)
        {
            #if(DEBUG)
            config.UseVerboseErrors = true;
            #else
            config.UseVerboseErrors = false;
            #endif

            config.SetEntitySetAccessRule("*", EntitySetRights.None);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.AllRead);
        }

        protected override IStoryDataService CreateDataSource()
        {
            return IoC.Resolve<IStoryDataService>();
        }

        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        public virtual ICollection<ShoutedStory> GetPublished(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");
            return CurrentDataSource.GetPublishedStories(start, max);
        }

        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        public virtual ICollection<ShoutedStory> GetUpcoming(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");
            return CurrentDataSource.GetUpcomingStories(start, max);
        }

        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        public virtual ICollection<ShoutedStory> GetPostedByTag(string tag, int start, int max)
        {
            Check.Argument.IsNotEmpty(tag, "tag");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");
            return CurrentDataSource.GetStoriesByTag(tag, start, max);
        }

        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        public virtual ICollection<ShoutedStory> GetPublishedByCategory(string category, int start, int max)
        {
            Check.Argument.IsNotEmpty(category, "category");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");
            return CurrentDataSource.GetStoriesByCategory(category, start, max);
        }

        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        public virtual ICollection<ShoutedStory> GetPostedByUser(string userName, int start, int max)
        {
            Check.Argument.IsNotEmpty(userName, "userName");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");
            return CurrentDataSource.GetStoriesPostedByUser(userName, start, max);
        }

        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        public virtual ICollection<ShoutedStory> GetShoutedByUser(string userName, int start, int max)
        {
            Check.Argument.IsNotEmpty(userName, "userName");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");
            return CurrentDataSource.GetStoriesShoutedByUser(userName, start, max);
        }

    }
}
