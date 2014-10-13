using Kigg.Core.DomainObjects;
using Kigg.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kigg.Repository
{
    public interface ICommingEventRepository : IRepository<ICommingEvent>
    {
        IQueryable<ICommingEvent> GetAll();        
    }
}
