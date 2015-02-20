using Kigg.Repository;
using Kigg.DomainObjects;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using Kigg.Core.DomainObjects;
using Kigg.Service;
using Kigg.Web.ViewData;
using Kigg.Infrastructure;
using UnitOfWork = Kigg.Infrastructure.UnitOfWork;

namespace Kigg.Web
{
    public class CommingEventController : BaseController
    {
        private readonly IDomainObjectFactory _factory;
        private readonly ICommingEventRepository _commingEventRepository;
        private readonly IEventAggregator _aggregator;

        public CommingEventController(IDomainObjectFactory factory, ICommingEventRepository commingEventRepository, IEventAggregator aggregator)
        {
            Check.Argument.IsNotNull(factory, "factory");
            Check.Argument.IsNotNull(commingEventRepository, "commingEventRepository");
            Check.Argument.IsNotNull(aggregator, "aggregator");

            _factory = factory;
            _commingEventRepository = commingEventRepository;
            _aggregator = aggregator;
        }

        private static CommingEventViewData CreateCommingEventsViewData(ICommingEvent x)
        {
            return new CommingEventViewData
            {
                EventLink = x.EventLink,
                EventName = x.EventName,
                EventDate = x.EventDate,
                Id = x.Id.Shrink(),
                EventPlace = x.EventPlace,
                EventLead = x.EventLead,
                Email = x.Email,
                IsApproved = x.IsApproved.GetValueOrDefault(),
            };
        }

        public ViewResult CommingEventsBox()
        {
            IQueryable<ICommingEvent> commingEvents = _commingEventRepository
                .GetAllApproved().Where(x=>x.EventDate.Date >= DateTime.Now.Date)
                .OrderBy(x => x.EventDate)
                .Take(Settings.DefaultsNrOfEventsToDisplay);

            var viewModel = CreateViewData<CommingEventsViewData>();

            viewModel.CommingEvents = commingEvents.ToList().Select(CreateCommingEventsViewData).AsQueryable();

            return View(viewModel);
        }

        public ViewResult AllCommingEvent()
        {
            IQueryable<ICommingEvent> commingEvents = _commingEventRepository.GetAllApproved()
                .Where(x => x.EventDate.Date > DateTime.Now.Date).OrderBy(x => x.EventDate);
            var viewModel = CreateViewData<CommingEventsViewData>();

            viewModel.CommingEvents = commingEvents.ToList().Select(CreateCommingEventsViewData).AsQueryable();

            return View(viewModel);
        }

        public ActionResult CommingEventsEditBox()
        {
            var viewModel = CreateViewData<CommingEventsViewData>();
            if (IsCurrentUserAuthenticated && CurrentUser.IsAdministrator())
            {
                IQueryable<ICommingEvent> commingEvents = _commingEventRepository.GetAll();
                viewModel.CommingEvents = commingEvents.OrderByDescending(x => x.EventDate)
                                                       .Select(x => CreateCommingEventsViewData(x));
            }
            else
            {
                ThrowNotFound("Nie ma takiej strony");
            }
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult AddEvent(EventViewData model)
        {
            JsonViewData viewData = Validate<JsonViewData>(
                new Validation(() => string.IsNullOrWhiteSpace(model.EventLink.NullSafe()), "Link do wydarzenia nie może być pusty."),
                new Validation(() => string.IsNullOrWhiteSpace(model.EventName.NullSafe()), "Tytuł wydarzenia nie może być pusty."),
                new Validation(() => model.EventUserEmail.NullSafe().IsEmail() == false, "Nieprawidłowy adres e-mail."),
                new Validation(() => model.Id.ToGuid() != Guid.Empty, "Id wydarzenia nie może być podane")
                );
            if (viewData == null)
            {
                try
                {

                    var eventApproveStatus = CurrentUser != null && CurrentUser.IsAdministrator() &&
                                             model.IsApproved;

                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        var commingEvent = _factory.CreateCommingEvent(
                                    model.EventUserEmail,
                                    model.EventLink,
                                    model.EventName,
                                    model.EventDate,
                                    model.EventPlace,
                                    model.EventLead,
                                    eventApproveStatus
                                    );
                        _commingEventRepository.Add(commingEvent);

                        unitOfWork.Commit();
                        _aggregator.GetEvent<UpcommingEventEvent>()
                                   .Publish(new UpcommingEventEventArgs(model.EventName, model.EventLink));
                        Log.Info("Event registered: {0}", commingEvent.EventName);

                        viewData = new JsonViewData { isSuccessful = true };
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                    viewData = new JsonViewData
                        {
                            errorMessage = FormatStrings.UnknownError.FormatWith("dodawania wydarzenia")
                        };
                }
            }
            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false), Compress]
        public ActionResult EditEvent(EventViewData model)
        {
            if (model.EventLead == null) model.EventLead = "Brak opisu";

            JsonViewData viewData = Validate<JsonViewData>(
                new Validation(() => string.IsNullOrEmpty(model.EventLink.NullSafe()), "Link wydarzenia nie może być pusty."),
                new Validation(() => string.IsNullOrEmpty(model.EventName.NullSafe()), "Tytuł wydarzenia nie może być pusty."),
                new Validation(() => !model.EventUserEmail.NullSafe().IsEmail(), "Niepoprawny adres e-mail."),
                new Validation(()=> CurrentUser.IsAdministrator() == false, "Nie możesz edutować tego wydarzenia."),
                new Validation(()=>model.Id.NullSafe().ToGuid().IsEmpty(), "Nieprawidłowy identyfikator wydarzenia.")
                );

            if (viewData == null)
            {
                try
                {
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        ICommingEvent commingEvent = _commingEventRepository.FindById(model.Id.ToGuid());
                        var eventApproveStatus = CurrentUser.IsAdministrator() &&
                                     model.IsApproved;
                        if (commingEvent == null)
                        {
                            viewData = new JsonViewData { errorMessage = "Podane wydarzenie nie istnieje." };
                        }
                        else
                        {
                            _commingEventRepository.EditEvent(
                                commingEvent,
                                model.EventUserEmail.NullSafe(),
                                model.EventLink.NullSafe(),
                                model.EventName.NullSafe(),
                                model.EventDate,
                                model.EventPlace,
                                model.EventLead,
                                eventApproveStatus
                                );

                            unitOfWork.Commit();

                            viewData = new JsonViewData { isSuccessful = true };
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("edycji wydarzenia.") };
                }
            }
            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult DeleteEvent(string id)
        {
            id = id.NullSafe();

            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => !CurrentUser.CanModerate(), "Nie masz praw do wołania tej metody."),
                                                            new Validation(() => string.IsNullOrEmpty(id), "Identyfikator reklamy nie może być pusty."),
                                                            new Validation(() => id.ToGuid().IsEmpty(), "Niepoprawny identyfikator wydarzenia."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteś zalogowany.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        ICommingEvent commingEvent = _commingEventRepository.FindById(id.ToGuid());

                        if (commingEvent == null)
                        {
                            viewData = new JsonViewData { errorMessage = "Wydarzenie nie istnieje." };
                        }
                        else
                        {
                            _commingEventRepository.Remove(commingEvent);
                            unitOfWork.Commit();

                            viewData = new JsonViewData { isSuccessful = true };
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData
                    {
                        errorMessage = FormatStrings.UnknownError.FormatWith("usuwania wydarzenia")
                    };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetEvent(string id)
        {
            id = id.NullSafe();
            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(id), "Identyfikator wydarzenia nie może być pusty."),
                                                            new Validation(() => id.ToGuid().IsEmpty(), "Niepoprawne id wydarzenia."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteś zalogowany."),
                                                            new Validation(() => !CurrentUser.CanModerate(), "Nie masz praw do woływania tej metody.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    ICommingEvent commingEvent = _commingEventRepository.FindById(id.ToGuid()); // findById do zaimplementowania                    

                    if (commingEvent == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Podane wydarzenie nie istnieje." };
                    }
                    else
                    {
                        return Json(
                                        new
                                        {
                                            eventId = commingEvent.Id.Shrink(),
                                            eventLink = commingEvent.EventLink,
                                            eventName = commingEvent.EventName,
                                            eventDate = commingEvent.EventDate.ToString("yyyy-MM-dd"),
                                            eventPlace = commingEvent.EventPlace,
                                            eventLead = commingEvent.EventLead,
                                            eventUserEmail = commingEvent.Email,
                                            isApproved = commingEvent.IsApproved
                                        }
                                    );
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("pobierania wudarzenia") };
                }
            }

            return Json(viewData);
        }
    }
}
