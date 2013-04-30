namespace Kigg.LinqToSql.Repository
{
    using System.Data.Linq;

    using DomainObjects;
    using StackExchange.Profiling.Data;
    using StackExchange.Profiling;

    public class DatabaseFactory : DisposableResource, IDatabaseFactory
    {
        //private readonly string _connectionString;
        private readonly System.Data.IDbConnection _connection;
        private IDatabase _database;

        public DatabaseFactory(IConnectionString connectionString)
        {
            Check.Argument.IsNotNull(connectionString, "connectionString");
            _connection = new StackExchange.Profiling.Data.ProfiledDbConnection(
                new System.Data.SqlClient.SqlConnection(connectionString.Value),
                MiniProfiler.Current);
        }

        public virtual IDatabase Get()
        {
            if (_database == null)
            {
                DataLoadOptions options = new DataLoadOptions();

                options.LoadWith<UserTag>(ut => ut.Tag);
                options.LoadWith<Story>(s => s.Category);
                options.LoadWith<Story>(s => s.User);
                options.LoadWith<Story>(s => s.StoryTags);
                options.LoadWith<StoryTag>(st => st.Tag);
                options.LoadWith<StoryVote>(v => v.User);
                options.LoadWith<StoryMarkAsSpam>(s => s.User);
                options.LoadWith<StoryComment>(c => c.User);
                options.LoadWith<UserAchievement>(c => c.UserId);
                _database = new Database(_connection)
                                {
                                    LoadOptions = options,
                                };
            }

            return _database;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_database != null)
                {
                    _database.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}