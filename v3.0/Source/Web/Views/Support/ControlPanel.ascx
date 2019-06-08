<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ControlPanelViewData>" %>
<div class="controlPanel" style="padding-bottom: 11px;">
    <div class="pageHeader">
        <div class="pageTitle">
            <h2>
                Panel sterowania</h2>
        </div>
    </div>
    <div>
        <% if (string.IsNullOrEmpty(Model.ErrorMessage)) %>
        <% {%>
        <ul>
            <li style="padding: 5px 0"><%= Html.ActionLink("Lista reklam", "AdList", "Recommendation")%></li>
            <li style="padding: 5px 0"><%= Html.ActionLink("Lista nadchodzących wydarzeń ({0})".FormatWith(Model.UnapprovedEventsCount), "CommingEventsEditBox", "CommingEvent")%></li>
            <li style="padding: 5px 0">
                <%= Html.ActionLink("Nowe ({0})".FormatWith(Model.NewCount), "New", "Story") %></li>
            <li style="padding: 5px 0">
                <%= Html.ActionLink("Niezatwierdzone ({0})".FormatWith(Model.UnapprovedCount), "Unapproved", "Story")%></li>
            <li style="padding: 5px 0">
                <%= Html.ActionLink("Do opublikowania ({0})".FormatWith(Model.PublishableCount), "Publishable", "Story")%></li>
            <% if (Model.IsAdministrator) %>
            <% {%>
            <li style="padding: 5px 0"><a id="lnkPublish" href="javascript:void(0)">Opublikuj</a></li>
            <% }%>
        </ul>
        <% }%>
        <% else%>
        <% {%>
        <span style="font-weight: bold; color: #f00">
            <%= Model.ErrorMessage %></span>
        <% }%>
    </div>
</div>
<div class="divider">
</div>
