<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<Kigg.Web.ViewData.RecommendationsViewData>" %>
<%@ Import Namespace="Kigg.Web.ViewData" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContentPlaceHolder">
    <div>
         <table width="90%">
             <caption>Lista reklam z bazy danych:</caption>
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
                                    <td><%= recommendationViewData.UrlLink %></td>
                                    <td><%= recommendationViewData.Position %></td>
                                    <td><a href="javascript:void(0);" data-edit-id="<%= recommendationViewData.Id %>">Edytuj</a>
                                    <div><a  href="javascript:void(0);" data-id="<%= recommendationViewData.Id %>">Usuń</a></td></div>
                                </tr>
                     <% } %>
          </table>
        <a id="lnkAddRecomendation" href="javascript:void(0)">Dodaj reklamę</a>
    </div>
</asp:Content>
