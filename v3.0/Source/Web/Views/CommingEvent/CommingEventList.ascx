<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CommingEventsViewData>" %>
<%@ Import Namespace="Kigg.Web.ViewData" %>

<div class="commingEventsList">
    <% foreach (var commingEventsInMonth in Model.CommingEvents.ToList().OrderBy(x => x.EventDate).GroupBy(x => x.EventDate.Month))
       { %>    
    <div class="commingEventsMonth">
            <h2><%= commingEventsInMonth.FirstOrDefault().EventDate.ToString("MMMM") %></h2>
        </div>
<% foreach(var commingEvent in commingEventsInMonth)
   { %>
                <div class="commingEventList">                    
                    <div class="commingEventDay"><%= commingEvent.EventDate.ToString("dd") %>.</div>
                    <div class="commingEventBody">                            
                        <a class="commingEventTitle" href="<%= commingEvent.EventLink %>" title="<%= commingEvent.EventName %>" target="_blank">
                            <%= commingEvent.EventName %>
                        </a>
                        <div class="commingEventPlace"><%= commingEvent.EventPlace %></div>
                    </div>                    
                </div>
<% }   
   } %>
</div>