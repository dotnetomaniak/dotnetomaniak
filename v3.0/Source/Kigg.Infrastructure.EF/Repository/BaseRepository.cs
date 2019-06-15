using System.Collections;
using System.Linq;
using Kigg.Repository;

namespace Kigg.Infrastructure.EF.Repository
{
    public class BaseRepository<TEntity>: IRepository<TEntity> where TEntity: class 
    {
        private readonly DotnetomaniakContext _context;

        public BaseRepository(DotnetomaniakContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            _context.Add<TEntity>(entity);
        }

        public void Remove(TEntity entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            _context.Remove<TEntity>(entity);
        }

        protected static PagedResult<T> BuildPagedResult<T>(IEnumerable entities, int total)
        {
            return new PagedResult<T>(entities.Cast<T>(), total);
        }
    }
}