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
        ICommingEvent FindById(Guid id);

        ICommingEvent FindByCommingEventName(string commingEventName);

        IQueryable<ICommingEvent> GetAll();

        IQueryable<ICommingEvent> GetAllVisible();
    }
}
