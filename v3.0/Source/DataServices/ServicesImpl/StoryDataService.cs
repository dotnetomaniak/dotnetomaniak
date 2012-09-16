using System;
using System.Collections.Generic;


namespace Kigg.DataServices.ServiceImpl
{
    using DomainObjects;
    using Repository;
    
    using ServiceContracts;
    using Data;
    
    public class StoryDataService : IStoryDataService
    {
        private readonly IStoryRepository _storyRepository;
        
        public StoryDataService(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }

        public ICollection<ShoutedStory> GetPublishedStories(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            PagedResult<IStory> publishedStories = _storyRepository.FindPublished(start, max);

            return ExtractCollection(publishedStories);
        }

        public ICollection<ShoutedStory> GetPublishedStories(int sart, int max, DateTime timePeriod)
        {
            throw new NotSupportedException("The interface IStoryRepository doesn't support fetching with time period");
        }

        public ICollection<ShoutedStory> GetUpcomingStories(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            PagedResult<IStory> upcomingStories = _storyRepository.FindUpcoming(start, max);

            return ExtractCollection(upcomingStories);
        }

        public ICollection<ShoutedStory> GetUpcomingStories(int sart, int max, DateTime timePeriod)
        {
            throw new NotSupportedException("The interface IStoryRepository doesn't support fetching with time period");
        }

        public ICollection<ShoutedStory> GetStoriesByTag(string tag, int start, int max)
        {
            Check.Argument.IsNotEmpty(tag, "tag");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            var taggedStories = _storyRepository.FindByTag(tag, start, max);

            return ExtractCollection(taggedStories);
        }

        public ICollection<ShoutedStory> GetStoriesByCategory(string category, int start, int max)
        {
            Check.Argument.IsNotEmpty(category, "category");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            var categoryStories = _storyRepository.FindPublishedByCategory(category, start, max);

            return ExtractCollection(categoryStories);
        }

        public ICollection<ShoutedStory> GetStoriesShoutedByUser(string userName, int start, int max)
        {
            Check.Argument.IsNotEmpty(userName, "userName");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            var userShoutedStories = _storyRepository.FindPromotedByUser(userName, start, max);

            return ExtractCollection(userShoutedStories);
        }

        public ICollection<ShoutedStory> GetStoriesPostedByUser(string userName, int start, int max)
        {
            Check.Argument.IsNotEmpty(userName, "userName");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            var userPostedStories = _storyRepository.FindPostedByUser(userName, start, max);

            return ExtractCollection(userPostedStories);
        }

        private static ICollection<ShoutedStory> ExtractCollection(PagedResult<IStory> stories)
        {
            if(stories == null)
            {
                return null;
            }

            var resultSet = stories.Result;
            
            var convertedStories = new List<ShoutedStory>(resultSet.Count);

            resultSet.ForEach(s => convertedStories.Add(s.Convert()));

            return convertedStories.AsReadOnly();
        }
    }
}
