<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TagTabsViewData>" %>
<%
bool shouldShowPopularTab = !Model.PopularTags.IsNullOrEmpty();
bool shouldShowMyTagsTab = !Model.UserTags.IsNullOrEmpty();

if (shouldShowPopularTab || shouldShowMyTagsTab)
{
%>
    <div id="tagTabs" class="hide">
        <ul>
            <% if (shouldShowPopularTab) %>
            <% { %>
                    <li class="sidebar-tabs-nav-item"><a href="#popularTags">Popularne tagi</a></li>
            <% } %>
            <% if (shouldShowMyTagsTab) %>
            <% { %>
                    <li class="sidebar-tabs-nav-item"><a href="#myTags">Moje tagi</a></li>
            <% } %>
        </ul>
        <% if (shouldShowPopularTab) %>
        <% { %>
            <div id="popularTags" class="tagCloud">
                <% Html.RenderPartial("Cloud", Model.PopularTags); %>
            </div>
        <% } %>
        <% if (shouldShowMyTagsTab) %>
        <% { %>
            <div id="myTags" class="tagCloud">
                <% Html.RenderPartial("Cloud", Model.UserTags); %>
            </div>
        <% } %>
    </div>
<%
}
%>
