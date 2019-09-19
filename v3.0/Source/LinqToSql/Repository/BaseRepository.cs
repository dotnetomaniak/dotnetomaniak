namespace Kigg.LinqToSql.Repository
{
    using System.Collections;
    using System.Diagnostics;
    using System.Linq;

    using Kigg.Repository;

    public abstract class BaseRepository<TInterface, TClass> : IRepository<TInterface> where TClass : class
    {
        private readonly IDatabase _database;

        protected BaseRepository(IDatabase database)
        {
            Check.Argument.IsNotNull(database, "database");

            _database = database;
        }

        protected BaseRepository(IDatabaseFactory factory) : this(factory.Get())
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

        public virtual void Add(TInterface entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Database.Insert(entity as TClass);
        }

        public virtual void Remove(TInterface entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Database.Delete(entity as TClass);
        }

        public void Commit()
        {
            throw new System.NotImplementedException();
        }

        protected static PagedResult<T> BuildPagedResult<T>(IEnumerable entities, int total)
        {
            return new PagedResult<T>(entities.Cast<T>(), total);
        }
    }
}