using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Kigg.Core.DomainObjects;
using Kigg.Core.DTO;
using Kigg.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kigg.Core.Service.GoogleService
{
    public class GoogleService : IGoogleService
    {
        private string calendarId = "";
        private string client_email = "";
        private string private_key = "";
        private ServiceCredential credential;
        private CalendarService service;
        private readonly string applicationName = "";
        private readonly string[] scopes = { CalendarService.Scope.Calendar };
        private readonly TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        private readonly IConfigurationManager _configurationManager;

        public GoogleService(IConfigurationManager configurationManager)
        {
            Check.Argument.IsNotNull(configurationManager, "configurationManager");
            _configurationManager = configurationManager;

            calendarId = _configurationManager.AppSettings["googleCalendarId"];
            applicationName = _configurationManager.AppSettings["googleApplicationName"];
            private_key = _configurationManager.AppSettings["googlePrivateKey"];
            client_email = _configurationManager.AppSettings["googleClientEmail"];

            credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(client_email)
            {
                Scopes = scopes
            }.FromPrivateKey(private_key));

            service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });
        }

        public string EventApproved(CommingEvent commingEvent)
        {
            var request = service.Events.Insert(
                 new Event()
                 {
                     Summary = "[" + (commingEvent.IsOnline ? "ONLINE" : commingEvent.EventCity) + "] " + commingEvent.EventName,
                     Location = commingEvent.IsOnline ? commingEvent.EventLink : commingEvent.EventCity + ", " + commingEvent.EventPlace,
                     Description = commingEvent.EventLead,
                     Start = new EventDateTime() { DateTime = TimeZoneInfo.ConvertTime(commingEvent.EventDate, timeZoneInfo) },
                     End = new EventDateTime() { DateTime = TimeZoneInfo.ConvertTime(commingEvent.EventEndDate, timeZoneInfo) },
                 }, calendarId);

            var createdEvent = request.Execute();

            return createdEvent.Id;
        }

        public string EditEvent(CommingEvent commingEvent)
        {
            if (!string.IsNullOrEmpty(commingEvent.GoogleEventId))
            {
                var request1 = service.Events.Get(calendarId, commingEvent.GoogleEventId);
                var eventForEdit = request1.Execute();

                eventForEdit.Summary = "[" + (commingEvent.IsOnline ? "ONLINE" : commingEvent.EventCity) + "] " + commingEvent.EventName;
                eventForEdit.Location = commingEvent.IsOnline ? commingEvent.EventLink : commingEvent.EventCity + ", " + commingEvent.EventPlace;
                eventForEdit.Description = commingEvent.EventLead;
                eventForEdit.Start = new EventDateTime() { DateTime = TimeZoneInfo.ConvertTime(commingEvent.EventDate, timeZoneInfo) };
                eventForEdit.End = new EventDateTime() { DateTime = TimeZoneInfo.ConvertTime(commingEvent.EventEndDate, timeZoneInfo) };


                var update = service.Events.Update(eventForEdit, calendarId, commingEvent.GoogleEventId);
                var updatedEvent = update.Execute();

                return updatedEvent.Id;
            }
            else
            {
                throw new ArgumentException("Event has to have Google ID.");
            }
        }

        public void DeleteEvent(string id)
        {
            var request = service.Events.Delete(calendarId, id);

            var deletedEvent = request.Execute();

        }
    }
}
