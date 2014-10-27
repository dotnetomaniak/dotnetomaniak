<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StoryListViewData>" %>

<h2 style="padding-bottom:10px;">Podobne artykuły:</h2>
<% if (Model.TotalStoryCount > 0)
   { %>
<ul>
<% foreach (var article in Model.Stories)
   { 
    string detailUrl = Url.RouteUrl("Detail", new { name = article.UniqueName }); %>
    <li style="padding-top:10px;">
    
        <a href="<%= Html.AttributeEncode(detailUrl) %>" target="_blank">
            <span><%= Html.Encode(article.Title) %></span>
        </a>

    </li>
   <%} %>
 </ul>
<% }
   else
   { %>
    <span style="padding-top:10px;">Nie znaleziono podobnych artykułów. </span>
<% } %>

