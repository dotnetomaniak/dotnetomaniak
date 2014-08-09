
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
       <meta property="og:title" content="<%= Model.Story.Title.Replace(@"""","'")%>" />
       <meta property="og:description" content="<%= Html.AttributeEncode(Model.Story.TextDescription) %>" />
       <meta property="og:url" content="<%= Url.RouteUrl("Detail", new { name = Model.Story.UniqueName }, "http") %>" />
       <meta property="og:type" content="article" />
       <meta property="og:image" content="<%= Html.AttributeEncode(Model.Story.GetMediumThumbnailPath(true)) %>" />
       <% foreach (var tag in Model.Story.Tags.Select(x=>x.UniqueName))
          { %>
              <meta property="article:tag" content="<%= tag %>" />
          <%} %>    
       <meta itemprop="description" content="<%= Html.AttributeEncode(Model.Story.TextDescription) %>" />	  
       <meta property="article:published_time" content="<%= Model.Story.PublishedAt.GetValueOrDefault().ToString("yyyy-MM-ddThh:mm:sszzz") %>" />
       <meta property="article:publisher" content="http://www.facebook.com/dotnetomaniakpl" />
       <meta property="fb:app_id" content ="<%= Model.FacebookAppId %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <% IStory story = Model.Story; %>
    <% IUser user = Model.CurrentUser;%>
    <%= Html.ArticleHeader("", new string[] { "Strona główna", story.BelongsTo.Name}) %>
    <div id="t-<%= Html.AttributeEncode(story.Id.Shrink()) %>" class="story odd">
        <% Html.RenderPartial("Story", new StoryItemViewData { Story = story, User = Model.CurrentUser, PromoteText = Model.PromoteText, DemoteText = Model.DemoteText, CountText = Model.CountText, SocialServices = Model.SocialServices, DetailMode = true }); %>
        <div class="clearfix">
        </div>
    </div>
    <% Html.RenderPartial("ImageCode", Model); %>
    <div id="commentTabs" name="tabsy" class="tabs-nav">
        <ul class="tabs-list">
            <li class="ui-tabs-nav-item"><a id="first-tab" class="active" href="#similar">Podobne</a></li>
            <li class="ui-tabs-nav-item"><a id="second-tab" href="#questions">Pytania</a></li>
            <li class="ui-tabs-nav-item"><a id="third-tab" href="#comments">Komentarze</a></li>
            <li class="ui-tabs-nav-item"><a id="fourth-tab" href="#votes">Wypromowane przez</a></li>
        </ul>
    </div>
    <div class="tabs-contents">
        <div id="first-tab-content" class="tab-content partialContents" data-url="<%=Url.Action("Similars", "Story", new { id = Model.Story.Id }) %>">
            Wczytywanie artykułów...            
        </div>
        <div id="second-tab-content" class="tab-content partialContents" data-url="<%= Model.Story.TagCount > 0 ? Url.Action("Questions", "Story", new { tags = Model.Story.Tags.Select(x=>x.UniqueName).Aggregate((x,y)=>x+","+y) }) : string.Empty%>">
            <% if (Model.Story.TagCount > 0)
               { %>
                   Wczytywanie
                   pytań...
               <% }
               else
               { %>
                    Brak powiązanych pytań.
               <% } %>
        </div>
        <div id="third-tab-content" class="tab-content">
            <div class="fb-comments" data-href="<%= Url.RouteUrl("Detail", new { name = Model.Story.UniqueName }, "http")  %>" data-numposts="5" data-colorscheme="light" data-width="600"></div>
            <%--<% Html.RenderPartial("Comments", Model); %>--%>
        </div>
        <div id="fourth-tab-content" class="tab-content">
            <% Html.RenderPartial("Votes", story.Votes); %>
        </div>        
    </div>
    <%
        if (Model.CanCurrentUserModerate || (user.HasRightsToEditStory(story)))
        {
            Html.RenderPartial("StoryEditorBox");
        }
    %>
</asp:Content>
