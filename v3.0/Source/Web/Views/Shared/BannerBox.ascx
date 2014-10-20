<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kigg.Web.ViewData.RecommendationsViewData>" %>
<%@ OutputCache Duration="86400" VaryByParam="None" %>
<!-- doba -->

<% if (Model.Recommendations.Any())
   { %>
<% var ad = Model.Recommendations.FirstOrDefault();
%>
<a href="<%= ad.UrlLink %>" style="display: block; width: 960px; margin-left: -22px" title="<%= ad.UrlTitle %>" target="_blank">
    <img src="<%= Url.Image(ad.ImageName) %>" alt="<%= ad.ImageAlt %>" /></a>
<% } %>
