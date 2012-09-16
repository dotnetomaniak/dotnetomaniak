using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kigg.Core.DomainObjects;
using Kigg.Core.Repository;
using Kigg.Repository.LinqToSql;

namespace Kigg.Infrastructure.LinqToSql.DomainObjects
{
    class PromoteSiteRepository : BaseRepository<IPromoteSiteItem, PromoteSiteItem>, IPromoteSiteRepository
    {
    }
}
