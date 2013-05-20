<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<Kigg.Web.ViewData.RecommendationsViewData>" %>
<%@ Import Namespace="Kigg.Web.ViewData" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContentPlaceHolder">
    <div>
        <%= Html.PageHeader("Lista reklam") %>
         <table width="90%" id="adsTable">             
             <tr>
                 <th>Obrazek</th>
                 <th>Tytuł</th>
                 <th>Link</th>
                 <th>Pozycja</th>
                 <th>Przyciski</th>
             </tr>
                 <% foreach (var recommendationViewData in Model.Recommendations)
                        { %>
                                <tr>
                                    <td><a><img src="<%= Url.Image(recommendationViewData.ImageName) %>" height="50"/></a></td>
                                    <td><%= recommendationViewData.UrlTitle %></td>
                                    <td><a href="<%= recommendationViewData.UrlLink %>"/><%= recommendationViewData.UrlLink %></a></td>
                                    <td><%= recommendationViewData.Position %></td>
                                    <td><a href="javascript:void(0);" data-edit-id="<%= recommendationViewData.Id %>">Edytuj</a>
                                    <a  href="javascript:void(0);" data-id="<%= recommendationViewData.Id %>">Usuń</a></td>
                                </tr>
                     <% } %>
          </table>
        <a id="lnkAddRecomendation" href="javascript:void(0)">Dodaj reklamę</a>
    </div>
</asp:Content>
