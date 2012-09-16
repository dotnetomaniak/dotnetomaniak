namespace Kigg.EF.Repository
{
    using System;
    using System.Linq;
    using System.Data.Objects;
    using System.Data.Metadata.Edm;
    using System.Data.EntityClient;
    using System.Data.Objects.DataClasses;
    using System.Collections.Generic;

    using DomainObjects;

    public class Database : ObjectContext, IDatabase
    {
        internal const string _defaultContainerName = "KiggEntityContainer";
        private const string esql = @"select value distinct resultset.Id from
                                    ((select s.Id as Id from KiggEntityContainer.Story as s 
                                      where Kigg.EF.DomainObjects.Store.EFSearchStory(s.Id, @query) = true)
                                      UNION ALL
                                     (select sc.Story.Id as Id from KiggEntityContainer.StoryComment as sc 
                                      where Kigg.EF.DomainObjects.Store.EFSearchComment(sc.Story.Id, @query) = true)
                                    ) as resultset";

        private readonly static IDictionary<string,string> EntitySetNames = new Dictionary<string, string>(10);

        internal DataLoadOptions LoadOptions { get; set; }

        private IQueryable<User> _userDataSource;
        private IQueryable<UserScore> _userScoreDataSource;
        private IQueryable<Story> _storyDataSource;
        private IQueryable<Category> _categoryDataSource;
        private IQueryable<Tag> _tagDataSource;
        private IQueryable<StoryView> _storyViewDataSource;
        private IQueryable<StoryVote> _storyVoteDataSource;
        private IQueryable<KnownSource> _knownSourceDataSource;
        private IQueryable<StoryMarkAsSpam> _markAsSpamDataSource;
        private IQueryable<StoryComment> _commentDataSource;

        public Database() :
            base (System.Configuration.ConfigurationManager.ConnectionStrings["KiGGDatabase"].ConnectionString, _defaultContainerName)
        {
            
        }

        public Database(string connectionString) :
            base(connectionString, _defaultContainerName)
        {   
        }

        public Database(EntityConnection connection) :
            base(connection, _defaultContainerName)
        {
        }

        public string GetEntitySetName(Type entitySetType)
        {
            lock (EntitySetNames)
            {
                if (EntitySetNames.ContainsKey(entitySetType.FullName))
                {
                    return EntitySetNames[entitySetType.FullName];
                }

                var container = MetadataWorkspace.GetEntityContainer(DefaultContainerName, DataSpace.CSpace);

                var entitySetName = (from meta in container.BaseEntitySets
                                     where meta.BuiltInTypeKind == BuiltInTypeKind.EntitySet &&
                                     meta.ElementType.Name == entitySetType.Name
                                     select meta.Name).First();

                EntitySetNames.Add(entitySetType.FullName, entitySetName);

                return entitySetName;
            }
        }

        internal ObjectQuery<Category> CategorySet
        {
            get
            {
                return CategoryDataSource as ObjectQuery<Category>;
            }
        }
        public IQueryable<Category> CategoryDataSource
        {
            get
            {
                if (_categoryDataSource==null)
                {
                    _categoryDataSource  = GetQueryable<Category>();
                }
                return _categoryDataSource;
            }
        }

        internal ObjectQuery<Tag> TagSet
        {
            get
            {
                return TagDataSource as ObjectQuery<Tag>;
            }
        }
        public IQueryable<Tag> TagDataSource
        {
            get
            {
                if(_tagDataSource==null)
                {
                    _tagDataSource = GetQueryable<Tag>();
                }
                return _tagDataSource;
            }
        }

        internal ObjectQuery<Story> StorySet
        {
            get
            {
                return StoryDataSource as ObjectQuery<Story>;
            }
        }
        public IQueryable<Story> StoryDataSource
        {
            get
            {
                if(_storyDataSource==null)
                {
                    _storyDataSource = GetQueryable<Story>();
                }
                return _storyDataSource;
            }
        }

        internal ObjectQuery<StoryComment> CommentSet
        {
            get
            {
                return CommentDataSource as ObjectQuery<StoryComment>;
            }
        }
        public IQueryable<StoryComment> CommentDataSource
        {
            get
            {
                if(_commentDataSource==null)
                {
                    _commentDataSource = GetQueryable<StoryComment>();
                }
                return _commentDataSource;
            }
        }

        internal ObjectQuery<StoryVote> VoteSet
        {
            get
            {
                return VoteDataSource as ObjectQuery<StoryVote>;
            }
        }
        public IQueryable<StoryVote> VoteDataSource
        {
            get
            {
                if(_storyVoteDataSource==null)
                {
                    _storyVoteDataSource = GetQueryable<StoryVote>();
                }
                return _storyVoteDataSource;
            }
        }

        internal ObjectQuery<StoryMarkAsSpam> MarkAsSpamSet
        {
            get
            {
                return MarkAsSpamDataSource as ObjectQuery<StoryMarkAsSpam>;
            }
        }
        public IQueryable<StoryMarkAsSpam> MarkAsSpamDataSource
        {
            get
            {
                if (_markAsSpamDataSource == null)
                {
                    _markAsSpamDataSource = GetQueryable<StoryMarkAsSpam>();
                }
                return _markAsSpamDataSource;
            }
        }

        internal ObjectQuery<StoryView> StoryViewSet
        {
            get
            {
                return StoryViewDataSource as ObjectQuery<StoryView>;
            }
        }
        public IQueryable<StoryView> StoryViewDataSource
        {
            get
            {
                if (_storyViewDataSource == null)
                {
                    _storyViewDataSource = GetQueryable<StoryView>();
                }
                return _storyViewDataSource;
            }
        }

        internal ObjectQuery<User> UserSet
        {
            get
            {
                return UserDataSource as ObjectQuery<User>;
            }
        }
        public IQueryable<User> UserDataSource
        {
            get
            {
                if (_userDataSource == null)
                {
                    _userDataSource = GetQueryable<User>();
                }
                return _userDataSource;
            }
        }

        internal ObjectQuery<UserScore> UserScoreSet
        {
            get
            {
                return UserScoreDataSource as ObjectQuery<UserScore>;
            }
        }
        public IQueryable<UserScore> UserScoreDataSource
        {
            get
            {
                if (_userScoreDataSource==null)
                {
                    _userScoreDataSource = GetQueryable<UserScore>();
                }
                return _userScoreDataSource;
            }
        }

        internal ObjectQuery<KnownSource> KnownSourceSet
        {
            get
            {
                return KnownSourceDataSource as ObjectQuery<KnownSource>;
            }
        }
        public IQueryable<KnownSource> KnownSourceDataSource
        {
            get
            {
                if (_knownSourceDataSource == null)
                {
                    _knownSourceDataSource = GetQueryable<KnownSource>();
                }
                return _knownSourceDataSource;
            }
        }

#if(DEBUG)
        public virtual IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : EntityObject
#else
        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : EntityObject
#endif
        {
            var entitySetName = GetEntitySetName(typeof(TEntity));
            return GetQueryable<TEntity>(entitySetName);
        }

        public void SetSearchQuery(string query)
        {
            SearchQuery = query;
        }
        private string SearchQuery { get; set; }
        public IQueryable<Guid> StorySearchResult
        {
            get
            {
                var queryParameter = (!String.IsNullOrEmpty(SearchQuery)) ? new ObjectParameter("query", SearchQuery) : new ObjectParameter("query", typeof(string));
                return CreateQuery<Guid>(esql, queryParameter);
            }
        }

        public void InsertOnSubmit<TEntity>(TEntity entity) 
            where TEntity : EntityObject
        {
            var entitySetName = GetEntitySetName(typeof(TEntity));
            AddObject(entitySetName, entity);
        }

        public void InsertAllOnSubmit<TEntity>(IEnumerable<TEntity> instances)
            where TEntity : EntityObject
        {
            var entitySetName = GetEntitySetName(typeof(TEntity));
            foreach (var entity in instances)
            {
                AddObject(entitySetName, entity);
            }
        }

        public void DeleteOnSubmit<TEntity>(TEntity entity) 
            where TEntity : EntityObject
        {
            DeleteObject(entity);
        }

        public void DeleteAllOnSubmit<TEntity>(IEnumerable<TEntity> instances) 
            where TEntity : EntityObject
        {
            foreach(var entity in instances)
            {
                DeleteOnSubmit(entity);
            }
        }

        public void SubmitChanges()
        {
            SaveChanges(true);
        }

        private IQueryable<TEntity> GetQueryable<TEntity>(string queryString) where TEntity : EntityObject
        {
            return ApplyDataLoadOptions<TEntity>(queryString);
        }

        private ObjectQuery<TEntity> ApplyDataLoadOptions<TEntity>(string queryString)
        {
            var query = CreateQuery<TEntity>(queryString);

            if (LoadOptions != null)
            {
                var members = LoadOptions.GetPreloadedMembers<TEntity>();

                foreach (var member in members)
                {
                    query = query.Include(member.Name);
                }
            }
            return query;
        }
    }
}
