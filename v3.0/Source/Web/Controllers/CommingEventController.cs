using Kigg.Repository;
using Kigg.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kigg.Core.DomainObjects;
using Kigg.Web.ViewData;

namespace Kigg.Web
{
    public class CommingEventController : BaseController
    {
        private readonly IDomainObjectFactory _factory;
        private readonly ICommingEventRepository _commingEventRepository;

        public CommingEventController(IDomainObjectFactory factory, ICommingEventRepository commingEventRepository)
        {
            Check.Argument.IsNotNull(factory, "factory");
            Check.Argument.IsNotNull(commingEventRepository, "commingEventRepository");            
            
            _factory = factory;
            _commingEventRepository = commingEventRepository;
        }

        private static CommingEventViewData CreateCommingEventsViewData(ICommingEvent x)
        {
            return new CommingEventViewData()
            {
                EventLink = x.EventLink,
                EventName = x.EventName,
                EventDate = x.EventDate,
                Id = x.Id.Shrink(),
                EventPlace = x.EventPlace,
                EventLead = x.EventLead,
            };
        }

        public ViewResult EventsBox()
        {
            IQueryable<ICommingEvent> commingEvents = _commingEventRepository.GetAllComming().OrderBy(x => x.EventDate).Take(Settings.DefaultsNrOfEventsToDisplay);
            var viewModel = CreateViewData<CommingEventsViewData>();
            var data = commingEvents.ToList();
            
            viewModel.CommingEvents = commingEvents.Select(x => CreateCommingEventsViewData(x));            
            
            return View("CommingEventsBox", viewModel);
        }

        public ViewResult AllCommingEvent()
        {
            IQueryable<ICommingEvent> commingEvents = _commingEventRepository.GetAllComming().OrderBy(x => x.EventDate);
            var viewModel = CreateViewData<CommingEventsViewData>();
            var data = commingEvents.ToList();

            viewModel.CommingEvents = commingEvents.Select(x => CreateCommingEventsViewData(x));            

            return View(viewModel);
        }
    }
}
