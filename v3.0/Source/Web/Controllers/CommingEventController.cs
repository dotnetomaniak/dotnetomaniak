using Kigg.Repository;
using Kigg.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
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
                viewModel.CommingEvents = commingEvents.OrderByDescending(x => x.EventDate).Select(x => CreateCommingEventsViewData(x));
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
            if (model.EventLead == null) model.EventLead = "Brak opisu";

            if (viewData == null)
            {
                try
                {
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        if (model.Id == null || model.Id.IsEmpty())
                        {
                            ICommingEvent commingEvent = _factory.CreateCommingEvent(
                                model.EventLink,
                                model.EventName,
                                model.EventDate,
                                model.EventPlace,
                                model.EventLead
                                );
                            _commingEventRepository.Add(commingEvent);

                            unitOfWork.Commit();

                            Log.Info("Event registered: {0}", commingEvent.EventName);

                            viewData = new JsonViewData { isSuccessful = true };
                        }
                        else
                        {
                            ICommingEvent commingEvent = _commingEventRepository.FindById(model.Id.ToGuid());

                            if (commingEvent == null)
                            {
                                viewData = new JsonViewData { errorMessage = "Podane wydarzenie nie istnieje." };
                            }
                            else
                            {
                                _commingEventRepository.EditEvent(
                                    commingEvent,
                                    model.EventLink.NullSafe(),
                                    model.EventName.NullSafe(),
                                    model.EventDate,
                                    model.EventPlace,
                                    model.EventLead);

                                unitOfWork.Commit();

                                viewData = new JsonViewData { isSuccessful = true };
                            }
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

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult DeleteEvent(string id)
        {
            id = id.NullSafe();

            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => !CurrentUser.CanModerate(), "Nie masz praw do wołania tej metody."),
                                                            new Validation(() => string.IsNullOrEmpty(id), "Identyfikator reklamy nie może być pusty."),
                                                            new Validation(() => id.ToGuid().IsEmpty(), "Niepoprawny identyfikator reklamy."),
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
                                            eventLead = commingEvent.EventLead
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
