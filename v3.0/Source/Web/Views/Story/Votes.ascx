<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ICollection<IVote>>" %>
<ul id="voteList">
<% foreach (IVote vote in Model) %>
<% {%>
<%      string promotedByName = vote.ByUser.UserName; %>
        <li>
            <a href="<%= Url.RouteUrl("User", new { name = vote.ByUser.Id.Shrink(), tab = UserDetailTab.Promoted, page = 1 }) %>">
                <img alt="<%= Html.AttributeEncode(promotedByName) %>" src="<%= Html.AttributeEncode(vote.ByUser.GravatarUrl(24)) %>" class="smoothImage" onload="javascript:SmoothImage.show(this)"/>
                <%= Html.Encode(promotedByName)%>
            </a>
        </li>
<% }%>
</ul>