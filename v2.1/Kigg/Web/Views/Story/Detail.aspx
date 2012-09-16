<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<StoryDetailViewData>"%>
<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        jQueryScriptManager.Current.RegisterSource(Url.Asset("js3"));

        Page.Header.Title = Model.Title;
    }

</script>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <% IStory story = Model.Story; %>
    <table id="t-<%= Html.AttributeEncode(story.Id.Shrink()) %>" class="story odd">
        <tbody>
            <tr>
                <% Html.RenderPartial("Story", new StoryItemViewData { Story = story, User = Model.CurrentUser, PromoteText = Model.PromoteText, DemoteText = Model.DemoteText, CountText = Model.CountText, SocialServices = Model.SocialServices, DetailMode = true }); %>
            </tr>
            <tr>
                <td></td>
                <td class="content">
                    <% Html.RenderPartial("ImageCode", Model); %>
                </td>
            </tr>
            <tr>
                <td></td>
                <td class="content">
                    <div id="commentTabs" class="hide">
                        <ul>
                            <li class="ui-tabs-nav-item"><a href="#questions">Pytania</a></li>
                            <li class="ui-tabs-nav-item"><a href="#comments">Komentarze</a></li>
                            <li class="ui-tabs-nav-item"><a href="#votes">Wypromowane przez</a></li>
                        </ul>
                        <div id="questions">
                            <% Html.RenderPartial("Questions", Model); %>
                        </div>
                        <div id="comments">
                            <% Html.RenderPartial("Comments", Model); %>
                        </div>
                        <div id="votes">
                            <% Html.RenderPartial("Votes", story.Votes); %>
                        </div>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <%
        if (Model.CanCurrentUserModerate)
        {
            Html.RenderPartial("StoryEditorBox");
        }
    %>
</asp:Content>
