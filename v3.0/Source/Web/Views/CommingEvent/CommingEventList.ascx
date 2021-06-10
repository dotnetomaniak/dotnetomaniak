<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CommingEventsViewData>" %>
<%@ Import Namespace="Kigg.Web.ViewData" %>

<% bool isOdd = true; %>
<div class="commingEventsList">
    <% foreach (var commingEventsInMonth in Model.CommingEvents.OrderBy(x => x.EventDate).GroupBy(x => x.EventDate.Month))
       {  %>    
    <div class="commingEventsMonth" style="border-bottom:dotted; border-bottom-color:lightgray; border-bottom-width:thin;">
            <h2><%= commingEventsInMonth.FirstOrDefault().EventDate.ToString("MMMM") %></h2>
        </div>
<% foreach(var commingEvent in commingEventsInMonth)
   { %>
    <% string styles = "min-height:110px; padding-top:0px; border-color:transparent; background:center; margin-bottom: 15px;"; %>
    <div style='<%= styles %><% if(!isOdd) { %> "background-color:#F6F6F6;" <% }%>' >
        
        <div class="commingEventDay"><%= commingEvent.EventDate.ToString("dd") %>.</div>
        <div class="commingEventBody">                            
            <a class="commingEventTitle" href="<%= commingEvent.EventLink %>" title="<%= commingEvent.EventName %>" target="_blank" rel="noopener">
                <%= commingEvent.EventName %>
            </a>
            <div class="commingEventPlace">
                <% if(commingEvent.IsOnline) { %>
                       <b>ONLINE</b>
                    <% } %>

                <% else if(!string.IsNullOrEmpty(commingEvent.EventCity)) { %>
                       <b> <%= commingEvent.EventCity %></b>, 
                        <%= commingEvent.EventPlace %>
                    <% } %>
            </div>
            <div class="commingEventDates">
                <%= commingEvent.EventDate %>

                <% if(commingEvent.EventEndDate.HasValue) { %>
                       - <%= commingEvent.EventEndDate %>
                    <% } %>
            </div>
        </div>
        <% if(commingEvent.EventLead == null)
                           { %>
            <div class="commingEventLead">Brak opisu</div>
            <% }
                       else
                       { %>
            <div class="commingEventLead"><%= commingEvent.EventLead %></div>
            <% } %>
    </div>    
    <% isOdd = !isOdd; %>   
<% }          
   } %>
</div>