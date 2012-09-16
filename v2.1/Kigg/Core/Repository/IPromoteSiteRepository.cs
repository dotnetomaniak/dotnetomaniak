namespace Kigg.Core.Repository
{
    using System.Collections.Generic;
    using DomainObjects;
    using Kigg.Repository;

    public interface IPromoteSiteRepository : IUniqueNameEntityRepository<IPromoteSiteItem>
    {
        ICollection<IPromoteSiteItem> FindAll();
    }
}
