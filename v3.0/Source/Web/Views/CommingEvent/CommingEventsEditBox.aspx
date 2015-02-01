<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master"
    Inherits="System.Web.Mvc.ViewPage<Kigg.Web.ViewData.CommingEventsViewData>" %>

<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContentPlaceHolder">
    <% %>
    <div>
        <%= Html.PageHeader("Lista wydarzeń:") %>
        <br />
        <a id="lnkAddEventAdmin" href="javascript:void(0)" style="">Dodaj wydarzenie</a>
        <table id="adsTable">
            <tr>
                <th>
                    Nazwa i Link
                </th>
                <th>
                    Data
                </th>
                <th>
                    Miejsce
                </th>
                <th>
                    Opis
                </th>
            </tr>
            <% foreach (var commingEventViewData in Model.CommingEvents)
               { %>
            <tr <% if (commingEventViewData.IsApproved != true){ %> style="background: lightgray"
                <% } %>>
                <td>
                    <a href="<%= commingEventViewData.EventLink %>">
                        <%= commingEventViewData.EventName %>
                    </a>
                </td>
                <td>
                    <%= commingEventViewData.EventDate.ToShortDateString() %>
                </td>
                <td>
                    <%= commingEventViewData.EventPlace %>
                </td>
                <td>
                    <%= commingEventViewData.EventLead %>
                </td>
            </tr>
            <tr <% if (commingEventViewData.IsApproved != true){ %> style="background: lightgray"
                <% } %>>
                <td>
                    Akcje:
                </td>
                <td>
                    <a href="javascript:void(0);" data-edit-event-id="<%= commingEventViewData.Id %>">Edytuj</a>
                </td>
                <td>
                    <a href="javascript:void(0);" data-event-id="<%= commingEventViewData.Id %>">Usuń</a>
                </td>
                <td>
                </td>
            </tr>
            <% } %>
        </table>
        <div class="input-color">
            <div class="color-box" style="background-color: lightgray; text-align: center;">
                Niezatwierdzone wydarzenie</div>
        </div>
    </div>
</asp:Content>
