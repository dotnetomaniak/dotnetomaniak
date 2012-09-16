<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StoryListUserViewData>" %>
<div style="float:left;width:100%;text-align:right">
    <%= Html.SyndicationIcons(Model.RssUrl, Model.AtomUrl) %>
</div>
<%if (!Model.Stories.IsNullOrEmpty()) %>
<%{ %>
    <% bool isOdd = true; %>
    <% foreach (IStory story in Model.Stories) %>
    <% { %>
    <%      string className = (isOdd) ? "odd" : "even"; %>
            <table id="t-<%= Html.AttributeEncode(story.Id.Shrink()) %>" class="story <%= className %> hentry">
                <tbody>
                    <tr>
                        <% Html.RenderPartial("Story", new StoryItemViewData { Story = story, User = Model.CurrentUser, PromoteText = ViewData.Model.PromoteText, DemoteText = ViewData.Model.DemoteText, CountText = Model.CountText, SocialServices = Model.SocialServices, DetailMode = false }); %>
                    </tr>
                </tbody>
            </table>
    <%   isOdd = !isOdd;%>
    <% } %>
    <%= Html.UserStoryListPager(Model.SelectedTab)%>
    <% if (Model.CanCurrentUserModerate)
       {
           Html.RenderPartial("StoryEditorBox");
       }
    %>
<%} %>
<%else%>
<%{%>
    <span class="pageMessage"><%= Model.NoStoryExistMessage%></span>
<%}%>