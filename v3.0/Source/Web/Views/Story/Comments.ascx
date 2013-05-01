<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StoryDetailViewData>" %>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        GenerateScripts();
    }

    private void GenerateScripts()
    {
        jQueryScriptManager scriptManager = jQueryScriptManager.Current;

        scriptManager.RegisterOnReady("Comment.set_captchaEnabled({0});".FormatWith(Model.CaptchaEnabled.ToString().ToLowerInvariant()));
        scriptManager.RegisterOnReady("Comment.init();");
        scriptManager.RegisterOnDispose("Comment.dispose();");
    }

</script>
<%
    IStory story = Model.Story;
    string message = story.HasComments() ? "{0} {1}.".FormatWith(story.CommentCount, (story.CommentCount > 1 ? (story.CommentCount < 5 ? "komentarze" : "komentarzy") : "komentarz")) : "Brak komentarzy. Bądź pierwszy aby skomentować ten wpis.";
%>
<div class="commentMessage"><h2><%= message%></h2></div>
<div id="commentDisc"><span>Komentarze są własnością ich twórców i tylko oni są za <strong>nie odpowiedzialni</strong>.<br /> Serwis <%= Html.ActionLink("dotnetomaniak.pl", "Story", "Published")%> <strong>nie ponosi jakiejkolwiek odpowiedzialności za treść</strong> komentarzy.</span>
</div>
<div id="commentList">
    <%if (story.HasComments()) %>
    <%{%>
        <ul>
            <%int i = 0; %>
            <%foreach (IComment comment in story.Comments) %>
            <%{%>
                <% i += 1;%>
                <% string commentId = comment.Id.Shrink(); %>
                <% string userUrl = Url.RouteUrl("User", new { name = comment.ByUser.Id.Shrink(), tab = UserDetailTab.Promoted, page = 1 }); %>
                <% bool isOwner = story.IsPostedBy(comment.ByUser); %>
                <% bool canModarate = ((Model.CurrentUser != null) && Model.CurrentUser.CanModerate()); %>
                <li id="li-<%= Html.AttributeEncode(commentId)%>">
                    <div class="hreview">
                        <div class="hide">
                            <span class="type">url</span>
                            <div class="item">
                                <a href="<%= Html.AttributeEncode(story.Url) %>" class="fn url"><%= Html.Encode(story.Title) %></a>
                            </div>
                            <abbr class="dtreviewed" title="<%=comment.CreatedAt.ToString("yyyy-MM-ddThh:mm:ssZ")%>"><%=comment.CreatedAt.ToString("F")%> GMT</abbr>
                        </div>
                    </div>
                    <p class="meta <%= isOwner ? "metaOwner" : "metaVisitor" %>">
                        <span class="no">
                            <a name="c-<%=Html.AttributeEncode(commentId)%>" title="Permalink" class="permalink" rel="bookmark" href="#c-<%=Html.AttributeEncode(commentId)%>">#<%=i.ToString()%></a>
                        </span>
                        <span class="postedBy reviewer vcard">
                            <a href="<%= Html.AttributeEncode(userUrl) %>"><img class="photo" alt="<%= Html.AttributeEncode(comment.ByUser.UserName) %>" src="<%= Html.AttributeEncode(comment.ByUser.GravatarUrl(50)) %>"/></a>
                        </span>                        
                    </p>
                    <div class="body <%= isOwner ? "ownerBody" : "visitorBody" %>">
                        <span class="fn"><a href="<%= Html.AttributeEncode(userUrl) %>"><%= Html.Encode(comment.ByUser.UserName) %></a></span>&nbsp;<span class="time" title="<%=comment.CreatedAt.ToString("F")%> GMT"><%= comment.CreatedAt.ToRelative() %> temu napisał:</span>
                        <div class="description">
                            <%if (canModarate) %>
                            <%{%>
                                <%= comment.HtmlBody %>
                                <div class="summary">
                                    <%string storyId = story.Id.Shrink(); %>
                                    <%if (!comment.IsOffended)%>
                                    <%{%>
                                        <a class="flagOffended actionLink" href="javascript:void(0)" onclick="Moderation.markCommentAsOffended('<%= storyId %>', '<%= commentId %>')">zaznacz jako obraźliwy</a> | 
                                    <%}%>
                                    <a class="spam actionLink" href="javascript:void(0)" onclick="Moderation.confirmSpamComment('<%= storyId %>', '<%= commentId %>')">spam</a>
                                </div>
                            <%}%>
                            <%else%>
                            <%{%>
                                <%if (comment.IsOffended) %>
                                <%{%>
                                    <em>Ten komentarz został oznaczony jako obraźliwy.</em>
                                <%}%>
                                <%else%>
                                <%{%>
                                <%= comment.HtmlBody %>
                                <%}%>
                            <%}%>
                        </div>
                    </div>
                </li>
            <%}%>
        </ul>
    <%}%>
</div>
<% if (ViewData.Model.IsCurrentUserAuthenticated) %>
<% {%>
    <div class="form">
    <% string userUrl = Url.RouteUrl("User", new { name = ViewData.Model.CurrentUser.Id.Shrink(), tab = UserDetailTab.Promoted, page = 1 }); %>
        <form id="frmCommentSubmit" action="<%= Url.Action("Post", "Comment") %>" method="post">
            <h4>Dodaj komentarz</h4>
            <a href="<%= Html.AttributeEncode(userUrl) %>"><img class="photo" alt="<%= Html.AttributeEncode(ViewData.Model.CurrentUser.UserName) %>" src="<%= Html.AttributeEncode(ViewData.Model.CurrentUser.GravatarUrl(50)) %>"/></a>
            <p>
                <input type="hidden" id="hidId" name="id" value="<%= story.Id.Shrink() %>"/>
                <input type="hidden" id="hidbody" name="body"/>
                <textarea id="txtCommentBody" name="commentBody" cols="20" rows="4"></textarea>
                <span class="error"></span>
            </p>
            <div>
                <div id="commentPreview" class="livePreview"></div>
                <div><a id="lnkCommentPreview"  class="actionLink" href="javascript:void(0)">ukryj podgląd</a></div>
                <div class="add-article-row"><input id="chkSubscribe" name="subscribe" type="checkbox" value="true" checked="checked"/>Subskrybuj przez email</div>
            </div>
            <%if(ViewData.Model.CaptchaEnabled)%>
            <%{%>
                <kigg:reCAPTCHA id="captcha" runat="server"></kigg:reCAPTCHA>
            <%}%>
            <div class="add-article-row">
                <span id="commentMessage" class="message"></span>
            </div>
            <div class="add-article-row">
                <input id="btnCommentSubmit" type="submit" class="largeButton" value="Wyślij"/>
            </div>
        </form>
    </div>
<% }%>
<% else %>
<% {%>
    <div class="commentMessage">
        Aby dodać komentarz <a id="lnkCommentLogin" class="actionLink" href="javascript:void(0)">zaloguj się</a> lub <a id="lnkCommentSignup" class="actionLink" href="javascript:void(0)">zarejestruj</a>
    </div>
<% }%>