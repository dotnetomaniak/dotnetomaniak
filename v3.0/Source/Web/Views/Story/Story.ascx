<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StoryItemViewData>" %>
<%@ Import Namespace="Kigg.LinqToSql.DomainObjects" %>
<script runat="server">
    string ShareLinks(string id, IEnumerable<string> socialServices)
    {
        string shareUrl = Url.Content("~/share.axd");
        StringBuilder shareHtml = new StringBuilder();
        shareHtml.Append(@"<ul class=""share"">");
        Func<string, string> shareLink = service => "<li><a href=\"{0}?id={1}&amp;srv={2}\" rel=\"nofollow\" title=\"{2}\" class=\"{2}\" target=\"_blank\"><span>&nbsp;</span></a></li>".FormatWith(Html.AttributeEncode(shareUrl), Html.AttributeEncode(id), Html.AttributeEncode(service));

        foreach (string service in socialServices)
        {
            shareHtml.Append(shareLink(service));
        }
        shareHtml.Append(@"</ul>");
        return shareHtml.ToString();
    }

    string TagLinks(IEnumerable<ITag> tags)
    {
        StringBuilder tagHtml = new StringBuilder();

        int i = 0;

        foreach (ITag tag in tags)
        {
            if (i > 0)
            {
                tagHtml.Append(", ");
            }

            tagHtml.Append(Html.ActionLink(tag.Name, "Tags", "Story", new {name = tag.UniqueName}, new {rel = "tag directory"}));

            i += 1;
        }

        return tagHtml.ToString();
    }

</script>

<script type="text/javascript" >
    function requestThumbnail() {
        $.ajax({
            type: "POST",
            url: "Story/GetSmallThumbnailPath",
            data: '{storyId: "<%=Model.Story.Id%>"}',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            timeout: 100000,
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                if (xmlHttpRequest.status === 200)
                    document.getElementById("thumb_img_id").src = xmlHttpRequest.responseText;
                else
                    console.log(errorThrown);
            },
            success: function (result) {
                if(!result.d.localeCompare(""))
                    document.getElementById("thumb_img_id").src = result.d;
            }
        });
    }

    $(function () {
        var img = $('meta[property="og:image"]').attr('content');
        if (typeof img != "undefined" && !img.localeCompare("")) {
            $.ajax({
                type: "POST",
                url: "Story/GetMediumThumbnailFullPath",
                data: '{storyId: "<%=Model.Story.Id%>" }',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                timeout: 100000,
                error: function(xmlHttpRequest, textStatus, errorThrown) {
                    if (xmlHttpRequest.status === 200) {
                        document.getElementById("thumb_link_id").href = xmlHttpRequest.responseText;
                        $('meta[property="og:image"]').attr('content', xmlHttpRequest.responseText);
                    } else
                        console.log(errorThrown);
                },
                success: function(result) {
                    if (!result.d.localeCompare("")) {
                        document.getElementById("thumb_link_id").href = result.d;
                        $('meta[property="og:image"]').attr('content', result.d);
                    }
                }
            });
        }
    });


    window.onload = () => {
        var img = document.getElementById("thumb_img_id");
        if (!img.currentSrc.localeCompare("")) {
            requestThumbnail();
        }
    };
</script>

<% const string hDateFormat = "yyyy-MM-ddThh:mm:ssZ"; %>
<% const string LongDateFormat = "F"; %>
<% IStory story = Model.Story; %>
<% IUser user = Model.User; %>
<%
    string attributedEncodedStoryId = Html.AttributeEncode(story.Id.Shrink());
    bool detailsMode = Model.DetailMode; %>
<div class="kigg col-3 col-sm-2 col-xl-1">
    <div class="count" id="s-c-<%= attributedEncodedStoryId %>">
        <%= story.VoteCount %>
    </div>
    <%
        string actionClass = "none";
        string spamClass = "hide";
        string kiggClass = "hide";
        string unkiggClass = "hide";

        if (user == null)
        {
            actionClass = "do";
            kiggClass = string.Empty;
        }
        else
        {
            if (!story.IsPostedBy(user))
            {
                if (story.CanPromote(user))
                {
                    actionClass = "do";
                    kiggClass = string.Empty;
                }
                else if (story.CanDemote(user))
                {
                    actionClass = "undo";
                    unkiggClass = string.Empty;
                }
                else if (story.HasMarkedAsSpam(user))
                {
                    spamClass = string.Empty;
                }
            }
        }
    %>
    <div class="action <%= actionClass %>">
        <% if (story.IsPostedBy(user)) %>
        <% { %>
        <span title="Ty wysłałeś/-aś ten artykuł">mój</span>
        <% } %>
        <% else %>
        <% { %>
        <span id="s-s-<%= attributedEncodedStoryId %>" title="Ty oznaczyłeś/-aś ten artykuł jako spam."
            class="<%= spamClass %>">spam</span> <a id="a-k-<%= attributedEncodedStoryId%>" href="javascript:void(0)"
                onclick="javascript:Story.promote('<%= attributedEncodedStoryId %>')" class="<%= kiggClass %>">
                <%= ViewData.Model.PromoteText %></a> <a id="a-u-<%= attributedEncodedStoryId%>"
                    href="javascript:void(0)" onclick="javascript:Story.demote('<%= attributedEncodedStoryId %>')"
                    class="<%= unkiggClass %>">
                    <%= ViewData.Model.DemoteText %></a>
        <% } %>
        <span id="s-p-<%= attributedEncodedStoryId%>"></span>
    </div>
</div>
<% string detailUrl = Url.RouteUrl("Detail", new { name = story.UniqueName });
   string onClick = string.Empty;
   string rel = "bookmark";
   %>
<% if (detailsMode)
   {
       detailUrl = Model.Story.Url;
       onClick = @"onclick=""javascript:Story.click('" + attributedEncodedStoryId + @"')""";
       rel += " external";
   } %>
<div class="col-9 col-sm-10 col-xl-11">
    <div itemscope itemtype="http://schema.org/Article" class="article">
        <div class="title">
            <h2>
                <a class="entry-title taggedlink" rel="<%= rel %>" href="<%= Html.AttributeEncode(detailUrl) %>"
                    <%= onClick %> >
                    <span itemprop="name"><%= Html.Encode(story.Title)%></span></a></h2>
        </div>
        <div class="entry-content description" <%= detailsMode ? "style='height: auto'" : "" %>>        
            <p itemprop="description"><span itemprop="articleBody">
            <% if (detailsMode) %>
            <% { %>
            <%= story.StrippedDescription() %>
            <% } %>
            <% else %>
            <% { %>
            <%= story.StrippedDescription() %>
            <% } %>
            </span></p>
            <div class="more">
                <div class="more-row">
                    Tagi:
                    <% if (story.HasTags())
                       { %>
                    <%= TagLinks(story.Tags) %>
                    <% } %>
                </div>
                <div class="more-row">
                    Źródło: <a href="http://<%= Html.AttributeEncode(story.Host()) %>" target="_blank" rel="noopener">
                        <%= story.Host()%></a>
                </div>
                <div class="more-row nobg">
                    <span>Dziel się z innymi:</span><%= ShareLinks(story.Id.Shrink(), Model.SocialServices)%>
                </div>
            </div>
            <div class="entry-thumb">
                <a href="<%= Html.AttributeEncode(detailUrl) %>" target="_blank" rel="external noopener" <%= onClick %>>
                    <% if (detailsMode) %>
                    <% { %>
                        <img id="thumb_img_id" itemprop="image" alt="<%= Html.AttributeEncode(story.Title) %>" src="<%= Html.AttributeEncode(story.GetSmallThumbnailPath()) %>"
                             class="smoothImage" onload="javascript:SmoothImage.show(this)" />
                    <% } %>
                    <% else %>
                    <% { %>
                        <img itemprop="image" alt="<%= Html.AttributeEncode(story.Title) %>" src="" data-story-id="<%= story.Id.Shrink() %>" class="smoothImage" onload="javascript:SmoothImage.show(this)" />
                    <% } %>
                </a>
            </div>
        </div>
    </div>
    <div class="summary">
        <p>
            <span class="system">
                <%= Html.ActionLink(story.BelongsTo.Name, "Category", "Story", new { name = story.BelongsTo.UniqueName }, new { rel = "tag directory" })%></span>
            <% if (story.IsPublished()) %>
            <% {
                 bool wasStoryPublishedInSummerTime = story.PublishedAt.Value.IsSummerTime();
                 string storyPublishedTimeLocalName = story.PublishedAt.Value.GetLocalTimeName(wasStoryPublishedInSummerTime);
                 int hoursToAddToUtcTime = story.PublishedAt.Value.GetHoursDifferenceForLocalTime(wasStoryPublishedInSummerTime);
                 %>
            <span class="time" title="<%= story.PublishedAt.Value.AddHours(hoursToAddToUtcTime).ToString(LongDateFormat) %> <%= storyPublishedTimeLocalName %>"><%= story.PublishedAt.Value.ToRelative()%>
                temu</span>
            <% }
               else
               {%>
               <span class="time" title="<%= story.CreatedAt.ToString(hDateFormat) %>"><%= story.CreatedAt.ToString(LongDateFormat) %> GMT</span>
               <%
               }%>
            <% string userUrl = Url.RouteUrl("User", new { name = story.PostedBy.Id.Shrink(), tab = UserDetailTab.Promoted, page = 1 }); %>
            <span class="author"><a class="vcard author" href="<%= Html.AttributeEncode(userUrl) %>">
                <%=Html.Encode(story.PostedBy.UserName.WrapAt(30))%>
            </a></span><span class="clicks">
                <%= story.ViewCount %></span>
            <% if (detailsMode) {%>
                <span class="counter"><a id="a-c" href="javascript:void(0)" class="imageCode actionLink">pokaż kod licznika</a></span>
            <% }%>
            <% if ((user != null) && (detailsMode) && (user.CanModerate() || user.HasRightsToEditStory(story))) %>
            <% { %>
            <span class="edit"><a class="edit actionLink" href="javascript:void(0)" onclick="Membership.editStory('<%= attributedEncodedStoryId %>')">
                edycja</a></span>
            <% } %>
            <% if ((user != null) && user.CanModerate()) %>
            <% { %>
            <span class="delete"><a class="delete actionLink" href="javascript:void(0)" onclick="Moderation.deleteStory('<%= attributedEncodedStoryId %>')">
                    usuń</a></span>
                    <span class="spam"><a class="spam actionLink" href="javascript:void(0)" onclick="Moderation.confirmSpamStory('<%= attributedEncodedStoryId %>')">spam</a></span>
                        <span class="miniature"><a class="miniature actionLink" href="javascript:void(0)" onclick="Moderation.generateMiniatureStory('<%= attributedEncodedStoryId %>')">miniaturka</a></span>
            <% } %>
            <% else if (story.IsPublished() == false)
               {%>
            <span class="spam"><a class="spam actionLink" href="javascript:void(0)" onclick="Story.markAsSpam('<%=attributedEncodedStoryId%>')">
                        spam?</a></span>
            <%
               }%>
            <% if (!story.IsApproved() && user.CanModerate()) %>
            <% { %>
            <span class="approve"><a class="approve actionLink" href="javascript:void(0)" onclick="Moderation.approveStory('<%= attributedEncodedStoryId %>')">
                zaakceptuj</a></span>
            <% } %>
            <% if (detailsMode == false)
               {%>
            <a class="rozwin">rozwiń</a>
            <% } else {%>
            <a class="zwin">zwiń</a>
            <% }%>
        </p>
    </div>
</div>
