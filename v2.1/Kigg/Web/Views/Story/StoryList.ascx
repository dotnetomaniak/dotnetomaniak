<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StoryListViewData>" %>
<%= Html.PageHeader(Model.Subtitle, Model.RssUrl, Model.AtomUrl)%>
<%if (!Model.Stories.IsNullOrEmpty()) %>
<%{ %>
    <% bool isOdd = true; %>
    <% foreach (IStory story in Model.Stories) %>
    <% { %>
    <%      string className = (isOdd) ? "odd" : "even"; %>
            <table id="t-<%= Html.AttributeEncode(story.Id.Shrink()) %>" class="story <%= className %> hentry">
                <tbody>
                    <tr>
                        <% Html.RenderPartial("Story", new StoryItemViewData { Story = story, User = Model.CurrentUser, PromoteText = Model.PromoteText, DemoteText = Model.DemoteText, CountText = Model.CountText, SocialServices = Model.SocialServices, DetailMode = false }); %>
                    </tr>
                </tbody>
            </table>
    <%   isOdd = !isOdd;%>
    <% } %>
    <%= Html.StoryListPager() %>
    <% if (Model.CanCurrentUserModerate)
       {
           Html.RenderPartial("StoryEditorBox");
       }
    %>
<%} %>
<%else%>
<%{%>
    <span class="pageMessage"><%= ViewData.Model.NoStoryExistMessage%></span>
<%}%>