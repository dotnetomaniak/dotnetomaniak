namespace Kigg.EF.Repository
{
    using System.Collections;
    using System.Diagnostics;
    using System.Linq;
    using System.Data.Objects.DataClasses;

    using Kigg.Repository;
    
    public abstract class BaseRepository<TInterface, TEntity> : IRepository<TInterface>
        where TInterface : class
        where TEntity : EntityObject, TInterface
    {
        private readonly IDatabase _database;

        protected BaseRepository(IDatabase database)
        {
            Check.Argument.IsNotNull(database, "database");

            _database = database;
        }

        protected BaseRepository(IDatabaseFactory factory)
            : this(factory.Create())
        {
        }

        protected internal IDatabase Database
        {
            [DebuggerStepThrough]
            get
            {
                return _database;
            }
        }
        protected Database DataContext
        {
            [DebuggerStepThrough]
            get
            {
                return _database as Database;
            }
        }

        public virtual void Add(TInterface entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Database.InsertOnSubmit(entity as TEntity);
        }

        public virtual void Remove(TInterface entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Database.DeleteOnSubmit(entity as TEntity);
        }

        protected static PagedResult<T> BuildPagedResult<T>(IEnumerable entities, int total)
        {
            return new PagedResult<T>(entities.Cast<T>(), total);
        }
    }
}