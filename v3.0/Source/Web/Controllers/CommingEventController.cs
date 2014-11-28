using Kigg.Repository;
using Kigg.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kigg.Core.DomainObjects;
using Kigg.Web.ViewData;
using Kigg.Infrastructure;
using UnitOfWork = Kigg.Infrastructure.UnitOfWork;

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

        public ViewResult CommingEventsBox()
        {
            IQueryable<ICommingEvent> commingEvents = _commingEventRepository.GetAllComming().OrderBy(x => x.EventDate).Take(Settings.DefaultsNrOfEventsToDisplay);
            var viewModel = CreateViewData<CommingEventsViewData>();
            var data = commingEvents.ToList();
            
            viewModel.CommingEvents = commingEvents.Select(x => CreateCommingEventsViewData(x));            
            
            return View(viewModel);
        }

        public ViewResult AllCommingEvent()
        {
            IQueryable<ICommingEvent> commingEvents = _commingEventRepository.GetAllComming().OrderBy(x => x.EventDate);
            var viewModel = CreateViewData<CommingEventsViewData>();
            var data = commingEvents.ToList();

            viewModel.CommingEvents = commingEvents.Select(x => CreateCommingEventsViewData(x));            

            return View(viewModel);
        }

        public ActionResult CommingEventsEditBox()
        {
            var viewModel = CreateViewData<CommingEventsViewData>();
            if (IsCurrentUserAuthenticated && CurrentUser.IsAdministrator())
            {
                IQueryable<ICommingEvent> commingEvents = _commingEventRepository.GetAll();
                viewModel.CommingEvents = commingEvents.Select(x => CreateCommingEventsViewData(x));
            }
            else
            {
                ThrowNotFound("Nie ma takiej strony");
            }
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false), Compress]
        public ActionResult EditEvent(EventViewData model)
        {
            JsonViewData viewData = Validate<JsonViewData>(
                new Validation(() => CurrentUser.CanModerate() == false, "Nie masz praw do wykonowania tej operacji."),
                new Validation(() => string.IsNullOrEmpty(model.EventLink.NullSafe()), "Link wydarzenia nie może być pusty."),
                new Validation(() => string.IsNullOrEmpty(model.EventName.NullSafe()), "Nazwa wydarzenia nie może być pusta.")
                );

            if (viewData == null)
            {
                try
                {
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        if (model.Id == null)
                        {
                            
                        }
                    }
                }
                catch (ArgumentException argument)
                {
                    viewData = new JsonViewData { errorMessage = argument.Message };
                }                
                catch (Exception e)
                {
                    Log.Exception(e);
                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("") };
                }
            }
            return Json(viewData);
        }
    }
}
