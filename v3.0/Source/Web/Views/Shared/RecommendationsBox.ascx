<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kigg.Web.ViewData.RecommendationsViewData>" %>
<%@ OutputCache Duration="86400" VaryByParam="None" %>
<%@ Import Namespace="Kigg.Core.DomainObjects" %>
<% IUser user = Model.CurrentUser; %>
<!-- doba -->


<div class="pageHeader">
    <div class="pageTitle">
        <h2>
            Polecamy</h2>
    </div>
</div>
<% foreach (var recommendationViewData in Model.Recommendations)
   { 
       %>  
    <div class="recommend-left-column">
        <a href="<%= recommendationViewData.UrlLink %>" title="<%=recommendationViewData.UrlTitle %>"><img src="<%= Url.Image(recommendationViewData.ImageName) %>" alt="<%= recommendationViewData.ImageAlt %>" /></a>
        <% if (user != null && user.CanModerate())
           { %> 
        <p>
        <span><a  href="javascript:void(0);" data-id="<%= recommendationViewData.Id %>">Usuñ</a></span>
        <span><a href="javascript:void(0);" data-edit-id="<%= recommendationViewData.Id %>"> Edytuj</a></span>
        </p>
        
        <% } %>
    </div>
  <% } %>
<% if (user != null && user.CanModerate())
   { %>
       <a id="lnkAddRecomendation" href="javascript:void(0)">Dodaj</a>
<% } %>