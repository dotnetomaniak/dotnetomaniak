namespace Kigg.EF.DomainObjects
{
    using System;
    using System.Threading;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Objects.DataClasses;

    using Infrastructure;
    
    public class EntityCollection<TInterface, TEntity> : IEntityCollection<TInterface>
        where TInterface : class
        where TEntity : class, IEntityWithRelationships, TInterface
    {
        private readonly EntityCollection<TEntity> _entityCollection;
        
        private bool _isLoaded;
        private object _syncRoot;

        public virtual bool IsSynchronized
        {
            get { return false; }
        }
        public virtual object SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                }
                return _syncRoot;
            }
        }
        public EntityCollection(EntityCollection<TEntity> entityCollection)
        {
            _isLoaded = entityCollection.IsLoaded;
            _entityCollection = entityCollection;
        }
        public int Count { get { return _entityCollection.Count; } }
        public bool IsReadOnly { get { return _entityCollection.IsReadOnly; } }
        
        #if(DEBUG)
        public virtual bool IsLoaded
        #else
        public bool IsLoaded
        #endif
        {
            get
            {
                try
                {
                    return _entityCollection.IsLoaded || _isLoaded;
                }
                catch (ObjectDisposedException ex)
                {
                    Log.Exception(ex);
                    return _isLoaded;
                }
                
            }
        }
        
        #if(DEBUG)
        public virtual void Load()
        #else
        public void Load()
        #endif
        {
            try
            {
                _entityCollection.Load();
            }
            catch (ObjectDisposedException ex)
            {
                Log.Exception(ex);
                _isLoaded = true;
            }
            catch (InvalidOperationException)
            {
                //An exception will thrown in case Object EntityState is Detached or Deleted
                //Ignore this exception, it will be always occure in lazy loading operation
                //Log.Exception(ex);
                _isLoaded = true;
            }
        }
        
        #if(DEBUG)
        public virtual IQueryable<TEntity> CreateSourceQuery()
        #else
        public  IQueryable<TEntity> CreateSourceQuery()
        #endif
        {
            IQueryable<TEntity> query;
            try
            {
                query = _entityCollection.CreateSourceQuery();   
            }
            catch (ObjectDisposedException ex)
            {
                Log.Exception(ex);
                query = _entityCollection.AsQueryable();
            }
            catch (InvalidOperationException)
            {
                //An exception will thrown in case Object EntityState is Detached or Deleted
                //Ignore this exception, it will be always occure in lazy loading operation
                //Log.Exception(ex);
                query = _entityCollection.AsQueryable();
            }
            return query ?? _entityCollection.AsQueryable();
        }
        
        public bool Contains(TInterface entity)
        {
            return _entityCollection.Contains(entity as TEntity);
        }
        public void Add(TInterface entity)
        {
            _entityCollection.Add(entity as TEntity);
        }
        public bool Remove(TInterface entity)
        {
            return _entityCollection.Remove(entity as TEntity);
        }
        public void Clear()
        {
            _entityCollection.Clear();
        }
        
        public void CopyTo(Array array, int arrayIndex)
        {
            Check.Argument.IsNotNull(array, "array");
            Check.Argument.IsNotOutOfRange(arrayIndex, 0, int.MaxValue, "arrayIndex");
            
            var entitiesArray = array.Cast<TEntity>().ToArray();
            CopyTo(entitiesArray, arrayIndex);
        }
        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            Check.Argument.IsNotNull(array, "array");
            Check.Argument.IsNotOutOfRange(arrayIndex, 0, int.MaxValue, "arrayIndex");

            _entityCollection.CopyTo(array, arrayIndex);
        }
        public IEnumerator<TInterface> GetEnumerator()
        {
            foreach (var entity in _entityCollection)
            {
                yield return entity;
            }
        }

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            CopyTo(array, arrayIndex);
        }

        int ICollection.Count
        {
            get { return Count; }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return SyncRoot;
            }
        }

        #endregion

        #region IEnumerable<TInterface> Members

        IEnumerator<TInterface> IEnumerable<TInterface>.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
