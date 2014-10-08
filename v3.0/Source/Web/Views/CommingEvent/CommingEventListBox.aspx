<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<Kigg.Web.ViewData.CommingEventsViewData>" %>
<%@ Import Namespace="Kigg.Web.ViewData" %>
<asp:Content runat="server" ContentPlaceHolderID="MainContentPlaceHolder">
    <% %>
    <div>
        <%= Html.PageHeader("Lista nadchodzących wydarzeń") %>
        <% bool AdTimeEnd = false; %>
         <table id="adsTable">             
             <tr>
                 <th>Obrazek</th>
                 <th>Nazwa</th>
                 <th>Link</th>
                 <th>Pozycja</th>
             </tr>
             <% foreach (var commingEventsViewData in Model.CommingEvents)
                        { %>
                                <tr class="<%=commingEventsViewData.HowLongEventShows() %>">
                                    <td><a><img src="<%= Url.Image(commingEventsViewData.ImageTitle) %>" height="50"/></a></td>
                                    <td><%= commingEventsViewData.EventName %></td>
                                    <td><a href="<%= commingEventsViewData.EventLink %>"/><%= commingEventsViewData.EventLink.WrapAt(40) %></a></td>
                                    <td><%= commingEventsViewData.Position %></td>                                
                                </tr>             
                     <% } %>
          </table>
    </div>
</asp:Content>
