<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AchievementsViewData>" %>
<%@ Import Namespace="System.Linq" %>
<ol class="badges">
<%
    int i = 0;
    foreach (var userAchievement in Model.Achievements.Result.OrderByDescending(ua => ua.DateAchieved))
{ %>
  <li class='custom-badge <%: i % 2 == 0 ?"odd" :"even" %>'><strong><%: Html.Translated(userAchievement.Achievement.Name) %></strong> - <%: userAchievement.Achievement.Description %><span><%: userAchievement.DateAchieved.ToRelative() %> temu</span></li>
<%
    i++;
} %>
</ol>
