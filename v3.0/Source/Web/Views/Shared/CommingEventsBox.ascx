<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kigg.Web.ViewData.CommingEventsViewData>" %>
<%@ OutputCache Duration="86400" VaryByParam="None" %>
<!-- doba -->

<div class="pageHeader">
    <div class="pageTitle">
        <h2>Nadchodz¹ce wydarzenia</h2>
    </div>
</div>
<% foreach(var year in Model.EventsYears.ToList().OrderBy(x => x))
   { %>
        <h2><%= year %></h2>
    <% foreach(var month in Model.EventsMonths.ToList())
        { %>
            <h2><%= month %></h2>
    <% foreach (var commingEventViewData in Model.CommingEvents.ToList().OrderBy(x => x.EventDate))
       { %>          
<% if(commingEventViewData.EventYear == year) 
   {%>        
<% if(commingEventViewData.EventMonth == month) 
   {%>
                <div class="recommend-left-column">
                    <a href="<%= commingEventViewData.EventLink %>" title="<%= commingEventViewData.EventName %>" target="_blank">
                        <img src="<%= Url.Image(commingEventViewData.ImageLink) %>" alt="<%= commingEventViewData.ImageTitle %>" />
                    </a>
                </div>
<% }
   }
   }
   }    
   } %>
