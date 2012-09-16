using System;
using System.Collections.Generic;

namespace Kigg.DataServices.ServiceContracts
{
    using Data;
    
    public interface IStoryDataService
    {
        ICollection<ShoutedStory> GetPublishedStories(int start, int max);
        ICollection<ShoutedStory> GetPublishedStories(int start, int max, DateTime timePeriod);
        ICollection<ShoutedStory> GetUpcomingStories(int start, int max);
        ICollection<ShoutedStory> GetUpcomingStories(int start, int max, DateTime timePeriod);
        ICollection<ShoutedStory> GetStoriesByTag(string tag, int start, int max);
        ICollection<ShoutedStory> GetStoriesByCategory(string category, int start, int max);
        ICollection<ShoutedStory> GetStoriesShoutedByUser(string userName, int start, int max);
        ICollection<ShoutedStory> GetStoriesPostedByUser(string userName, int start, int max);
    }
}