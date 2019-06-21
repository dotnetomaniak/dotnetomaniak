<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ICollection<UserWithScore>>" %>
<%
    if (!Model.IsNullOrEmpty())
    {
        int i = 1;
%>
<div class="toplist-entry col-12">
    <% foreach (UserWithScore userScore in Model) %>
    <% { %>
    <%      string userName = userScore.User.UserName; %>
    <div class="toplist-row">
        <div class="toplist-count">
            <%= i++ %></div>
            <div class="toplist-thumb">
        <a title="<%= Html.AttributeEncode(userName) %>" href="<%= Url.RouteUrl("User", new { name = userScore.User.Id.Shrink(), tab = UserDetailTab.Promoted, page = 1 }) %>">            
                <img alt="<%= Html.AttributeEncode(userName) %>" src="<%= Html.AttributeEncode(userScore.User.GravatarUrl(24)) %>"
                    class="smoothImage" style="vertical-align: top" onload="javascript:SmoothImage.show(this)" />            
        </a>
        </div>
        <div class="toplist-name">
                <a title="<%= Html.AttributeEncode(userName) %>" href="<%= Url.RouteUrl("User", new { name = userScore.User.Id.Shrink(), tab = UserDetailTab.Promoted, page = 1 }) %>"><%= Html.Encode(userName.WrapAt(20)) %></a>
        </div>
        <div class="toplist-score">
                (<%= userScore.Score.ToString(FormatStrings.UserScore) %>)
        </div>
    </div>
    <% } %>
    <div style="text-align: right">
        <%= Html.RouteLink("więcej »", "Users", new { page = 1 }, new { rel = "directory" })%>
    </div>
</div>
<%
    }
%>
