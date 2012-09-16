using System;
using System.Data;
using System.IO;

namespace Kigg.LinqToSql.Repository
{
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Diagnostics;
    using System.Linq;

    using DomainObjects;
    
    public partial class Database : IDatabase
    {
        partial void OnCreated()
        {
            Log = new StringWriter();
        }

        public IQueryable<Category> CategoryDataSource
        {
            [DebuggerStepThrough]
            get
            {
                return GetQueryable<Category>();
            }
        }

        public IQueryable<Tag> TagDataSource
        {
            [DebuggerStepThrough]
            get
            {
                return GetQueryable<Tag>();
            }
        }

        public IQueryable<Story> StoryDataSource
        {
            [DebuggerStepThrough]
            get
            {
                return GetQueryable<Story>();
            }
        }

        public IQueryable<StoryComment> CommentDataSource
        {
            [DebuggerStepThrough]
            get
            {
                return GetQueryable<StoryComment>();
            }
        }

        public IQueryable<StoryVote> VoteDataSource
        {
            [DebuggerStepThrough]
            get
            {
                return GetQueryable<StoryVote>();
            }
        }

        public IQueryable<StoryMarkAsSpam> MarkAsSpamDataSource
        {
            [DebuggerStepThrough]
            get
            {
                return GetQueryable<StoryMarkAsSpam>();
            }
        }

        public IQueryable<StoryTag> StoryTagDataSource
        {
            [DebuggerStepThrough]
            get
            {
                return GetQueryable<StoryTag>();
            }
        }

        public IQueryable<StoryView> StoryViewDataSource
        {
            [DebuggerStepThrough]
            get
            {
                return GetQueryable<StoryView>();
            }
        }

        public IQueryable<UserTag> UserTagDataSource
        {
            [DebuggerStepThrough]
            get
            {
                return GetQueryable<UserTag>();
            }
        }

        public IQueryable<User> UserDataSource
        {
            [DebuggerStepThrough]
            get
            {
                return GetQueryable<User>();
            }
        }

        public IQueryable<UserScore> UserScoreDataSource
        {
            [DebuggerStepThrough]
            get
            {
                return GetQueryable<UserScore>();
            }
        }

        public IQueryable<Achievement> AchievementsDataSource
        {
            [DebuggerStepThrough]
            get 
            { 
                return GetQueryable<Achievement>(); 
            }
        }

        public IQueryable<UserAchievement> UserAchievementsDataSource
        {
            [DebuggerStepThrough]
            get
            {
                return GetQueryable<UserAchievement>();
            }
        }

        public IQueryable<CommentSubscribtion> CommentSubscribtionDataSource
        {
            [DebuggerStepThrough]
            get
            {
                return GetQueryable<CommentSubscribtion>();
            }
        }

        public IQueryable<KnownSource> KnownSourceDataSource
        {
            [DebuggerStepThrough]
            get
            {
                return GetQueryable<KnownSource>();
            }
        }

        public virtual IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class
        {
            return GetTable<TEntity>();
        }

        public virtual ITable GetEditable<TEntity>() where TEntity : class
        {
            return GetTable<TEntity>();
        }

        public void Insert<TEntity>(TEntity instance) where TEntity : class
        {
            GetEditable<TEntity>().InsertOnSubmit(instance);
        }

        public void InsertAll<TEntity>(IEnumerable<TEntity> instances) where TEntity : class
        {
            GetEditable<TEntity>().InsertAllOnSubmit(instances);
        }

        public void Delete<TEntity>(TEntity instance) where TEntity : class
        {
            GetEditable<TEntity>().DeleteOnSubmit(instance);
        }

        public void DeleteAll<TEntity>(IEnumerable<TEntity> instances) where TEntity : class
        {
            GetEditable<TEntity>().DeleteAllOnSubmit(instances);
        }        
    }
}