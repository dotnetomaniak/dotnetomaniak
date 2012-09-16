<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ControlPanelViewData>" %>
<div class="box">
    <h3>Panel sterowania</h3>
    <div>
        <% if (string.IsNullOrEmpty(Model.ErrorMessage)) %>
        <% {%>
                <ul>
                    <li><%= Html.ActionLink("Nowe ({0})".FormatWith(Model.NewCount), "New", "Story") %></li>
                    <li><%= Html.ActionLink("Niezatwierdzone ({0})".FormatWith(Model.UnapprovedCount), "Unapproved", "Story")%></li>
                    <li><%= Html.ActionLink("Do opublikowania ({0})".FormatWith(Model.PublishableCount), "Publishable", "Story")%></li>
                    <% if (Model.IsAdministrator) %>
                    <% {%>
                        <li><a id="lnkPublish" href="javascript:void(0)">Opublikuj</a></li>
                    <% }%>
                </ul>
        <% }%>
        <% else%>
        <% {%>
                <span style="font-weight:bold;color:#f00"><%= Model.ErrorMessage %></span>
        <% }%>
    </div>
</div>
<div class="divider"></div>