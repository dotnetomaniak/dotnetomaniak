<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<Kigg.Web.ViewData.CommingEventsViewData>" %>
<%@ Import Namespace="Kigg.Web.ViewData" %>

<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContentPlaceHolder">
    <% %>
    <div>
        <%= Html.PageHeader("Lista wydarzeń:") %>
        <% bool AdTimeEnd = false; %>
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
                                    <td><%= commingEventViewData.SiteTitle %></td>
                                    <td><a href="<%= commingEventViewData.EventLink %>"/><%= commingEventViewData.EventLink.WrapAt(40) %></a></td>                                    
                                    <td><%= commingEventViewData.EventDate %></td>
                                    <td><%= commingEventViewData.EventPlace %></td>
                                    <td><%= commingEventViewData.EventLead %></td>
                                </tr>                                
                     <% } %>
          </table>        
        <a id="lnkAddRecomendation" href="javascript:void(0)">Dodaj wydarzenie</a>
    </div>
</asp:Content>