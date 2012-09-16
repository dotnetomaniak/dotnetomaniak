namespace Kigg.EF.Repository
{
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

#if(DEBUG)
        public virtual IDatabase Create()
#else
        public IDatabase Create()
#endif

        {
            if (_database == null)
            {
                var options = new DataLoadOptions();

                options.LoadWith<Story>(s => s.Category);
                options.LoadWith<Story>(s => s.User);
                options.LoadWith<Story>(s => s.StoryTagsInternal);
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