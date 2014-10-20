<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<Kigg.Web.ViewData.RecommendationsViewData>" %>
<%@ Import Namespace="Kigg.Web.ViewData" %>
<asp:Content runat="server" ContentPlaceHolderID="MainContentPlaceHolder">
    <% %>
    <div>
        <%= Html.PageHeader("Lista reklam") %>
        <% bool AdTimeEnd = false; %>
         <table id="adsTable">             
             <tr>
                 <th>Obrazek</th>
                 <th>Tytuł</th>
                 <th>Link</th>
                 <th>Typ</th>
                 <th>Pozycja</th>
             </tr>
                 <% foreach (var recommendationViewData in Model.Recommendations)
                        { %>
                                <tr class="<%=recommendationViewData.HowLongAdShows() %>">
                                    <td><a><img src="<%= Url.Image(recommendationViewData.ImageName) %>" height="50"/></a></td>
                                    <td><%= recommendationViewData.UrlTitle %></td>
                                    <td><a href="<%= recommendationViewData.UrlLink %>"/><%= recommendationViewData.UrlLink.WrapAt(40) %></a></td>
                                    <td><%= recommendationViewData.IsBanner ? "Banner" : "Normal"%></td>
                                    <td><%= recommendationViewData.Position %></td>                                
                                </tr>
                                <tr class="<%=recommendationViewData.HowLongAdShows() %>">
                                    <td>Akcje:</td>
                                    <td><a href="javascript:void(0);" data-edit-id="<%= recommendationViewData.Id %>">Edytuj</a></td>
                                    <td></td>
                                    <td></td>
                                    <td><a href="javascript:void(0);" data-id="<%= recommendationViewData.Id %>">Usuń</a></td>
                                </tr>
             <%AdTimeEnd = !AdTimeEnd;%>
                     <% } %>
          </table>
        <div class="input-color" >
            <div class="color-box" style="background-color: #FFE4E1; text-align: center;">Przeterminowany</div>
            <div class="color-box" style="background-color: #FFF0F5; text-align: center;">Do jutra</div>
            <div class="color-box" style="background-color: #FFFAF0; text-align: center;">Koniec do pięciu dni</div>
            <div class="color-box" style="background-color: #c2e078; text-align: center;">Reklama domyślna</div>
        </div>
        <a id="lnkAddRecomendation" href="javascript:void(0)">Dodaj reklamę</a>
    </div>
</asp:Content>
