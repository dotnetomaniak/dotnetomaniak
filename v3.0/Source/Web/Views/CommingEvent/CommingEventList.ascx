<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CommingEventsViewData>" %>
<%@ Import Namespace="Kigg.Web.ViewData" %>

<% bool isOdd = true; %>
<div class="commingEventsList">
    <% foreach (var commingEventsInMonth in Model.CommingEvents.ToList().OrderBy(x => x.EventDate).GroupBy(x => x.EventDate.Month))
       { %>    
    <div class="commingEventsMonth" <%--<% if(!isOdd) { %> style="background-color:#F6F6F6;" <% } %>--%>>
            <h2><%= commingEventsInMonth.FirstOrDefault().EventDate.ToString("MMMM") %></h2>
        </div>
    <%--<div class="story odd hentry" style="text-align:center; border-bottom:none;" <%--<% if(!isOdd) { %> style="background-color:#F6F6F6;" <% } %>--%>
            <%--<h2><%= commingEventsInMonth.FirstOrDefault().EventDate.ToString("MMMM") %></h2>
        </div>--%>
<% foreach(var commingEvent in commingEventsInMonth)
   { %>
    <% string className = (isOdd) ? "odd" : "even"; %>
    <div class="story <%= className %> hentry" style="height:110px; padding-top:0px;">
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