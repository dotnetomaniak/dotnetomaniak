<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BadgesViewData>" %>

<ol class="badges">
<% int i = 0;
    foreach (var badge in Model.Badges.OrderByDescending(x=>x.Count)) { %>
    <li class='badge <%: i++ % 2 == 0 ?"odd" :"even" %>'><strong><%= Html.Translated(badge.Name) %></strong> - <%= badge.Description %> <span><%= badge.Count %></span></li>
<% }%>
</ol>

