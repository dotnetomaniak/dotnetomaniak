namespace Kigg.Repository.LinqToSql
{
    using System.Data.Linq;

    using DomainObjects;

    public class DatabaseFactory : DisposableResource, IDatabaseFactory
    {
        private readonly string _connectionString;
        private IDatabase _database;

        public DatabaseFactory(IConnectionString connectionString)
        {
            Check.Argument.IsNotNull(connectionString, "connectionString");

            _connectionString = connectionString.Value;
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

                _database = new Database(_connectionString)
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