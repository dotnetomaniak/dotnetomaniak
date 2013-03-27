
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<StoryDetailViewData>" %>

<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        jQueryScriptManager.Current.RegisterSource(Url.Asset("js3"));

        Page.Header.Title = Model.Title;
    }
    
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server" >
    <% if (Model.Story.IsAuthorshipOn())
       {%>
        <link rel="author" href="http://www.blogger.com/profile/<%:Model.Story.AuthorsProfile()%>" />
    <%
       }%>
       <link rel="image_src" href="<%= Html.AttributeEncode(Model.Story.GetMediumThumbnailPath(true)) %>" />
       <meta property="og:title" content="<%= Model.Story.Title %>" />
       <meta property="og:url" content="<%= Url.RouteUrl("Detail", new { name = Model.Story.UniqueName }, "http") %>" />
       <meta property="og:type" content="article" />
       <meta property="og:image" content="<%= Html.AttributeEncode(Model.Story.GetMediumThumbnailPath(true)) %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <% IStory story = Model.Story; %>
    <%= Html.ArticleHeader("", new string[] { "Strona główna", story.BelongsTo.Name}) %>
    <div id="t-<%= Html.AttributeEncode(story.Id.Shrink()) %>" class="story odd">
        <% Html.RenderPartial("Story", new StoryItemViewData { Story = story, User = Model.CurrentUser, PromoteText = Model.PromoteText, DemoteText = Model.DemoteText, CountText = Model.CountText, SocialServices = Model.SocialServices, DetailMode = true }); %>
        <div class="clearfix">
        </div>
    </div>
    <% Html.RenderPartial("ImageCode", Model); %>
    <div id="commentTabs" name="tabsy" class="tabs-nav">
        <ul class="tabs-list">
            <li class="ui-tabs-nav-item"><a id="first-tab" class="active" href="#questions">Pytania</a></li>
            <li class="ui-tabs-nav-item"><a id="second-tab" href="#comments">Komentarze</a></li>
            <li class="ui-tabs-nav-item"><a id="third-tab" href="#votes">Wypromowane przez</a></li>
        </ul>
    </div>
    <div class="tabs-contents">
        <div id="first-tab-content" class="tab-content">
            <% Html.RenderPartial("Questions", Model); %>
        </div>
        <div id="second-tab-content" class="tab-content">
            <% Html.RenderPartial("Comments", Model); %>
        </div>
        <div id="third-tab-content" class="tab-content">
            <% Html.RenderPartial("Votes", story.Votes); %>
        </div>
    </div>
    <%
        if (Model.CanCurrentUserModerate)
        {
            Html.RenderPartial("StoryEditorBox");
        }
    %>
</asp:Content>
