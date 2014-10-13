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
                ImageTitle = x.ImageTitle,
                ImageLink = x.ImageLink,
                Position = x.Position,
                EventDate = x.EventDate,
                EndTime = x.EndTime,               
                Id = x.Id.Shrink(),                
            };
        }

        public ViewResult EventsBox()
        {
            int defaultsEventsNr = 3;

            IQueryable<ICommingEvent> commingEvents = _commingEventRepository.GetAllVisible().OrderBy(x => x.Position).Take(defaultsEventsNr);
            var viewModel = CreateViewData<CommingEventsViewData>();
            var data = commingEvents.ToList();
            
            viewModel.CommingEvents = commingEvents.Select(x => CreateCommingEventsViewData(x));            
            
            return View("CommingEventsBox", viewModel);
        }

        [AutoRefresh, Compress]
        public ActionResult EventList()
        {
            var viewModel = CreateViewData<CommingEventsViewData>();
            if (IsCurrentUserAuthenticated && CurrentUser.CanModerate())
            {
                IQueryable<ICommingEvent> commingEvents = _commingEventRepository.GetAll();
                viewModel.CommingEvents = commingEvents.Select(x => CreateCommingEventsViewData(x));
            }
            else
            {
                ThrowNotFound("Nie ma takiej strony");
            }
            return View("CommingEventListBox", viewModel);
        }
    }
}
