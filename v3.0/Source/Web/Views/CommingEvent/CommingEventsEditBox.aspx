<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<Kigg.Web.ViewData.CommingEventsViewData>" %>
<%@ Import Namespace="Kigg.Web.ViewData" %>

<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContentPlaceHolder">
    <% %>
    <div>
        <%= Html.PageHeader("Lista wydarzeń:") %>
        
        <br />
        <a id="lnkAddEvent" href="javascript:void(0)" style="">Dodaj wydarzenie</a>
         <table id="adsTable">             
             <tr>                 
                 <th>Tytuł</th>
                 <th>Link</th>
                 <th>Data</th>
                 <th>Miejsce</th>
                 <th>Opis</th>
             </tr>
                 <% foreach (var commingEventViewData in Model.CommingEvents)
                        { %>
                                <tr>
                                    <td><%= commingEventViewData.EventName %></td>
                                    <td><a href="<%= commingEventViewData.EventLink %>"/><%= commingEventViewData.EventLink.WrapAt(40) %></a></td>
                                    <td><%= commingEventViewData.EventDate.ToShortDateString() %></td>
                                    <td><%= commingEventViewData.EventPlace %></td>
                                    <td><%= commingEventViewData.EventLead %></td>
                                </tr>
                                <tr>
                                    <td>Akcje:</td>
                                    <td><a href="javascript:void(0);" data-edit-event-id="<%= commingEventViewData.Id %>">Edytuj</a></td>
                                    <td></td>
                                    <td></td>
                                    <td><a href="javascript:void(0);" data-event-id="<%= commingEventViewData.Id %>">Usuń</a></td>
                                </tr>
                     <% } %>
          </table>
        <div class="input-color" >
            <div class="color-box" style="background-color: #FFE4E1; text-align: center;">Przeterminowany</div>            
            <div class="color-box" style="background-color: #FFFAF0; text-align: center;">W przyszłości</div>
            <div class="color-box" style="background-color: #c2e078; text-align: center;">Aktualne wyświetlane</div>
        </div>        
    </div>
</asp:Content>