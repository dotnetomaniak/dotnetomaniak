<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ICollection<UserWithScore>>" %>
<%
if (!Model.IsNullOrEmpty())
{
%>
    <ol>
        <% foreach (UserWithScore userScore in Model) %>
        <% { %>
        <%      string userName = userScore.User.UserName; %>
                <li>
                    <a title="<%= Html.AttributeEncode(userName) %>" href="<%= Url.RouteUrl("User", new { name = userScore.User.Id.Shrink(), tab = UserDetailTab.Promoted, page = 1 }) %>">
                        <img alt="<%= Html.AttributeEncode(userName) %>" src="<%= Html.AttributeEncode(userScore.User.GravatarUrl(24)) %>" class="smoothImage" style="vertical-align:top" onload="javascript:SmoothImage.show(this)"/>
                        <%= Html.Encode(userName.WrapAt(28)) %>
                    </a>
                    (<%= userScore.Score.ToString(FormatStrings.UserScore) %>)
                </li>
        <% } %>
    </ol>
<%
}
%>
<div style="text-align:right">
    <%= Html.RouteLink("więcej »", "Users", new { page = 1 }, new { rel = "directory" })%>
</div>
