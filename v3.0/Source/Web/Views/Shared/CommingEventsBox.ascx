<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kigg.Web.ViewData.CommingEventsViewData>" %>
<%@ OutputCache Duration="86400" VaryByParam="None" %>
<!-- doba -->

<div class="pageHeader">
    <div class="pageTitle">
        <h2>Nadchodzące wydarzenia</h2>
    </div>
</div>
<div class="commingEventsList">
    <% foreach (var commingEventsInMonth in Model.CommingEvents.ToList().OrderBy(x => x.EventDate).GroupBy(x => x.EventDate.Month))
       { %>    
    <div class="commingEventsMonth">
            <h2><%= commingEventsInMonth.FirstOrDefault().EventDate.ToString("MMMM") %></h2>
        </div>
<% foreach(var commingEvent in commingEventsInMonth)
   { %>
                <div class="commingEventColumn">                    
                    <div class="commingEventDay"><%= commingEvent.EventDate.ToString("dd") %>.</div>
                    <div class="commingEventBody">                            
                        <a class="commingEventTitle" href="<%= commingEvent.EventLink %>" title="<%= commingEvent.EventName %>" target="_blank">
                            <%= commingEvent.EventName.WrapAt(20) %>
                        </a>
                        <div class="commingEventPlace"><%= commingEvent.EventPlace %></div>
                    </div>                    
                </div>
<% }   
   } %>
</div>
<div style="text-align:left; margin-bottom:15px; margin-bottom:20px; margin-bottom:10px;">
    <a id="lnkAddEventUser" href="javascript:void(0)" onclick="Moderation.showEvent()" style="">Dodaj nowe</a>
    <%= Html.ActionLink("Zobacz wszystkie", "AllCommingEvent", "CommingEvent", new { style = "float: right" })%> 
</div>