<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kigg.Web.ViewData.RecommendationsViewData>" %>
<%@ OutputCache Duration="86400" VaryByParam="None" %>

<!-- doba -->
<div class="pageHeader">
    <div class="pageTitle">
        <h2>
            Polecamy</h2>
    </div>
</div>
<% foreach (var recommendationViewData in Model.Recommendations)
   { %>  
    <div class="recommend-left-column">
        <a href="<%= recommendationViewData.UrlLink %>" title="<%= recommendationViewData.UrlTitle %>"><img src="<%= Url.Image(recommendationViewData.ImageName) %>" alt="<%= recommendationViewData.ImageAlt %>" /></a>
    </div>
  <% } %>
       <a id="lnkEditRecomendation" href="javascript:void(0)">Edytuj</a>