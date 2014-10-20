<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CommingEventsViewData>" %>
<%@ Import Namespace="Kigg.Web.ViewData" %>

<% bool isOdd = true; %>
<div class="commingEventsList">
    <% foreach (var commingEventsInMonth in Model.CommingEvents.ToList().OrderBy(x => x.EventDate).GroupBy(x => x.EventDate.Month))
       { %>    
    <div class="commingEventsMonth" style="border-bottom:dotted; border-bottom-color:lightgray; border-bottom-width:thin;">
            <h2><%= commingEventsInMonth.FirstOrDefault().EventDate.ToString("MMMM") %></h2>
        </div>
<% foreach(var commingEvent in commingEventsInMonth)
   { %>
    <% string className = (isOdd) ? "odd" : "even"; %>
    <div <% if(!isOdd) { %> style="height:110px; padding-top:0px; border-color:transparent; background:center; background-color:#F6F6F6;" <% 
    }else{ %> style="height:110px; padding-top:0px; border-color:transparent; background:center;" <% } %> >
        
        <div class="commingEventDay"><%= commingEvent.EventDate.ToString("dd") %>.</div>
        <div class="commingEventBody">                            
            <a class="commingEventTitle" href="<%= commingEvent.EventLink %>" title="<%= commingEvent.EventName %>" target="_blank">
                <%= commingEvent.EventName %>
            </a>
            <div class="commingEventPlace"><%= commingEvent.EventPlace %></div>            
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