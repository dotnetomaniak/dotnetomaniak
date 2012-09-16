namespace Kigg.EF.DomainObjects
{
    using System.Collections;
    using System.Collections.Generic;
    
    public interface IEntityCollection<TInterface> : ICollection, IEnumerable<TInterface>
        where TInterface : class
    {
        bool IsLoaded { get; }
        void Load();
        void Clear();
        bool Remove(TInterface entity);
    }
}
