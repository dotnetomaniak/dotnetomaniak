namespace Kigg.LinqToSql.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;

    using DomainObjects;

    public interface IDatabase : IDisposable
    {
        IQueryable<Category> CategoryDataSource
        {
            get;
        }

        IQueryable<Tag> TagDataSource
        {
            get;
        }

        IQueryable<Story> StoryDataSource
        {
            get;
        }

        IQueryable<StoryComment> CommentDataSource
        {
            get;
        }

        IQueryable<StoryVote> VoteDataSource
        {
            get;
        }

        IQueryable<StoryMarkAsSpam> MarkAsSpamDataSource
        {
            get;
        }

        IQueryable<StoryTag> StoryTagDataSource
        {
            get;
        }

        IQueryable<StoryView> StoryViewDataSource
        {
            get;
        }

        IQueryable<UserTag> UserTagDataSource
        {
            get;
        }

        IQueryable<User> UserDataSource
        {
            get;
        }

        IQueryable<UserScore> UserScoreDataSource
        {
            get;
        }

        IQueryable<Achievement> AchievementsDataSource
        {
            get;
        }

        IQueryable<UserAchievement> UserAchievementsDataSource
        {
            get;
        }

        IQueryable<CommentSubscribtion> CommentSubscribtionDataSource
        {
            get;
        }

        IQueryable<KnownSource> KnownSourceDataSource
        {
            get;
        }

        IQueryable<Recommendation> RecommendationDataSource
        {
            get;
        }

        IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class;

        ITable GetEditable<TEntity>() where TEntity : class;

        IEnumerable<T> ExecuteQuery<T>(string query, params object[] parameters);

        void Insert<TEntity>(TEntity instance) where TEntity : class;

        void InsertAll<TEntity>(IEnumerable<TEntity> instances) where TEntity : class;

        void Delete<TEntity>(TEntity instance) where TEntity : class;

        void DeleteAll<TEntity>(IEnumerable<TEntity> instances) where TEntity : class;

        void SubmitChanges();

        IQueryable<StorySearchResult> StorySearch(string query);

        IQueryable<CommentSearchResult> CommentSearch(string query);

        int _10kPoints();
        int StoryAdder();
        int EarlyBird();
        int _1kPoints();
        int Commeter();
        int NightOwl();
        int UpVoter();
        int GoodStory();
        int GreatStory();
        int PopularStory();
        int MultiAdder();
        int Globetrotter();
        int Dotnetomaniak();
        int Facebook();
    }
}