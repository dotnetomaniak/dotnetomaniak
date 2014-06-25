<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<UserDetailViewData>" %>

<%@ Import Namespace="System.Web.Routing" %>
<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        Page.Header.Title = "{0} - Użytkownik : {1}".FormatWith(Model.SiteTitle, Model.TheUser.UserName);
        if (Model.CurrentPage > 1)
            Page.Header.Title += " - Strona {0}".FormatWith(Model.CurrentPage);
    }

    private void StoryList()
    {
        string userId = Model.TheUser.Id.Shrink();
        int page = Model.CurrentPage;

        switch (Model.SelectedTab)
        {
            case UserDetailTab.Posted:
                {
                    Html.RenderAction("PostedBy", "Story", new RouteValueDictionary { { "name", userId }, { "page", page } });
                    break;
                }

            case UserDetailTab.Commented:
                {
                    Html.RenderAction("CommentedBy", "Story", new RouteValueDictionary { { "name", userId }, { "page", page } });
                    //Html.RenderAction<StoryController>(c => c.CommentedBy(userId, page));
                    break;
                }
            case UserDetailTab.Achivements:
                {
                    Html.RenderAction("AchievementsBy", "Membership", new RouteValueDictionary {{"name", userId}});
                    break;
                }
            default:
                {
                    Html.RenderAction("PromotedBy", "Story", new RouteValueDictionary { { "name", userId }, { "page", page } });
                    //Html.RenderAction<StoryController>(c => c.PromotedBy(userId, page));
                    break;
                }
        }
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server" />
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <% IUser user = Model.TheUser; %>
    <%bool isVisitingOwnPage = ((Model.IsCurrentUserAuthenticated) && (user.Id.Equals(Model.CurrentUser.Id)));%>
    <%bool isAdmin = ((Model.IsCurrentUserAuthenticated) && (Model.CurrentUser.IsAdministrator()));%>
    <%bool isTheUserAdmin = (user.IsAdministrator()); %>
    <%bool isTheUserMod = (user.IsModerator()); %>
    <%string rank = isTheUserAdmin ? "administrator" : (isTheUserMod ? "moderator" : "użytkownik"); %>
    
    <%= Html.ArticleHeader("", new string[] {"Strona główna", "Użytkownik"}) %>
    <h3><%= Model.TheUser.UserName %><span> | <%= rank %></span>
    </h3>
    <div style="clear: left">        
        <div class="avatar-card">
            <div class="avatar-big">
                <img alt="<%= Html.AttributeEncode(user.UserName) %>" class="smoothImage" src="<%= Html.AttributeEncode(user.GravatarUrl(128)) %>"
                    onload="javascript:SmoothImage.show(this)" />
                <%if (isVisitingOwnPage) %>
                <%{%>
                <br />
                <a href="http://gravatar.com" target="_blank" rel="external">Zmień avatar</a>
                <%}%>
            </div>
            <div class="avatar-content">
                <div class="avatar-content-row">
                    <div class="avatar-content-label">
                        Nazwa:</div>
                    <div class="avatar-content-content">
                        <%= user.UserName%>
                        <%if (isAdmin)%>
                        <%{%>
                        <a id="lnkLockUser" href="javascript:void(0)" onclick="javascript:Administration.lockUser('<%= user.Id.Shrink() %>')"
                            class="actionLink <%= user.IsLockedOut ? "hide" : string.Empty %>">zablokuj</a>
                        <a id="lnkUnlockUser" href="javascript:void(0)" onclick="javascript:Administration.unlockUser('<%= user.Id.Shrink() %>')"
                            class="actionLink <%= user.IsLockedOut ? string.Empty : "hide" %>">odblokuj</a>
                        <%}%>
                    </div>
                </div>
                <div class="avatar-content-row">
                    <div class="avatar-content-label">
                        Punkty:</div>
                    <div class="avatar-content-content">
                        <%= (user.IsPublicUser()) ? user.CurrentScore.ToString(FormatStrings.UserScore) : "brak danych" %>
                    </div>
                </div>
                <div class="avatar-content-row">
                    <div class="avatar-content-label">
                        Ostatnio widziany:
                    </div>
                    <div class="avatar-content-content">
                        <%= user.LastActivityAt.ToRelative()%>
                        temu</div>
                </div>
                <div class="avatar-content-row">
                    <div class="avatar-content-label">
                        Zarejestrowany od:
                    </div>
                    <div class="avatar-content-content">
                        <%= user.CreatedAt.ToString(FormatStrings.LongDate)%></div>
                </div>
                <%if (isAdmin || isVisitingOwnPage) %>
                <%{%>
                <div class="avatar-content-row">
                    <%string email = user.HasDefaultOpenIDEmail() ? "brak" : user.Email; %>
                    <div class="avatar-content-label">
                        Email:
                    </div>
                    <div id="emailViewSection" class="form">
                        <span id="spnEmail">
                            <%= email%></span>
                        <%if (isVisitingOwnPage) %>
                        <%{%>
                        <a id="lnkChangeEmail" href="javascript:void(0)" class="actionLink">zmień</a>
                        <%}%>
                    </div>
                    <%if (isVisitingOwnPage) %>
                    <%{%>
                    <div id="emailEditSection" class="form hide">
                        <% using (Html.BeginForm("ChangeEmail", "Membership", FormMethod.Post, new { id = "frmChangeEmail" }))%>
                        <% { %>
                        <%= Html.TextBox("email", email, new { id = "txtChangeEmail", @class = "textBox" })%>
                        <input id="btnChangeEmail" type="submit" class="button" value="Zapisz" />
                        lub <a id="lnkCancelEmail" href="javascript:void(0)" class="actionLink">anuluj</a>
                        <span id="spnChangeEmailError" class="error"></span>
                        <% } %>
                    </div>
                    <%}%>
                    </div>
                    <%if (isVisitingOwnPage) %>
                    <%{%>                         
                    <div class="avatar-content-row">
                        <div class="avatar-content-label">
                            Powiązane z:
                        </div>
                        <div class="avatar-content-content">
                            <% if(user.FbId==null) %>
                            <% { %>
                               <fb:login-button scope="public_profile,email" onlogin="synchronizeWithFb();">Facebook</fb:login-button>
                            <% }else %>
                            <% { %>
                                <span>Facebook</span>
                            <% } %>
                        </div>                                                                                                                                                               
                    </div>
                    <%} %>
                <%}%>
                <%if (!user.IsPublicUser() || isAdmin) %>
                <%{%>
                <div class="avatar-content-row">
                    <div class="avatar-content-label">
                        Rola:
                    </div>
                    <div class="avatar-content-content">
                        <div id="roleViewSection" class="form">
                            <span id="spnRole">
                                <%= user.Role.ToString() %></span>
                            <%if (isAdmin)%>
                            <%{%>
                            <a id="lnkChangeRole" href="javascript:void(0)" class="actionLink">zmień</a>
                            <%}%>
                        </div>
                        <%if (isAdmin)%>
                        <%{%>
                        <div id="roleEditSection" class="form hide">
                            <% using (Html.BeginForm("ChangeRole", "Membership", FormMethod.Post, new { id = "frmChangeRole" }))%>
                            <% { %>
                            <%= Html.Hidden("id", user.Id.Shrink()) %>
                            <select id="ddlRoles" name="role">
                                <%foreach (KeyValuePair<int, string> pair in Html.ToDictionary<Kigg.DomainObjects.Roles>())%>
                                <%{%>
                                <%bool select = (user.Role == (Kigg.DomainObjects.Roles)pair.Key); %>
                                <%if (select) %>
                                <%{%>
                                <option value="<%= pair.Value %>" selected="selected">
                                    <%= pair.Value %></option>
                                <%}%>
                                <%else%>
                                <%{%>
                                <option value="<%= pair.Key %>">
                                    <%= pair.Value %></option>
                                <%}%>
                                <%}%>
                            </select>
                            <input id="btnChangeRole" type="submit" class="button" value="Zmień" />
                            lub <a id="lnkCancelRole" href="javascript:void(0)" class="actionLink">anuluj</a>
                            <% } %>
                        </div>
                        <%}%>
                    </div>
                </div>
                <%}%>
                <%if ((isAdmin) && (!Model.IPAddresses.IsNullOrEmpty()))%>
                <%{%>
                <div class="avatar-content-row">
                    <div class="avatar-content-label">
                        Adresy IP:
                    </div>
                    <div class="avatar-content-content">
                        <%using (Html.BeginForm("AllowIps", "Membership", FormMethod.Post, new { id = "frmAllowIps", @class = "form" }))%>
                        <%{%>
                        <%= Html.Hidden("id", user.Id.Shrink()) %>
                        <ul style="list-style: none none outside; margin: 0px; padding: 0px; width: 32em">
                            <%foreach (KeyValuePair<string, bool> ipAddress in Model.IPAddresses) %>
                            <%{%>
                            <li style="float: left; width: 8em; padding: 4px; white-space: nowrap; overflow: hidden">
                                <label class="smallLabel">
                                    <%if (ipAddress.Value)%>
                                    <%{%>
                                    <input name="ipAddress" type="checkbox" value="<%= ipAddress.Key %>" checked="checked" /><%= ipAddress.Key %>
                                    <%}%>
                                    <%else%>
                                    <%{%>
                                    <input name="ipAddress" type="checkbox" value="<%= ipAddress.Key %>" /><%= ipAddress.Key %>
                                    <%}%>
                                </label>
                            </li>
                            <%}%>
                        </ul>
                        <div class="clearLeft" style="padding-top: 2px">
                            <input id="btnAllowIpAddresses" type="submit" class="button" value="Zezwól" />
                        </div>
                        <%}%>
                    </div>
                </div>
                <%}%>
            </div>
        </div>
    </div>
    <div class="divider">
    </div>
    <div class="tabs-nav" name="tabsy" id="top-nav2">
        <ul class="tabs-list">
            <% string userId = user.Id.Shrink(); %>
            <% UserDetailTab selectedTab = Model.SelectedTab; %>
            <li class="ui-tabs-nav-item">
                <a href="<%= Url.RouteUrl("User", new { name = userId, tab = UserDetailTab.Promoted, page = 1 }) %>" <%= ((selectedTab == UserDetailTab.Promoted) ? "class='active'" : string.Empty)%>>
                    Wypromowane</a></li>
            <li class="ui-tabs-nav-item">
                <a href="<%= Url.RouteUrl("User", new { name = userId, tab = UserDetailTab.Posted, page = 1 }) %>" <%= ((selectedTab == UserDetailTab.Posted) ? "class='active'" : string.Empty)%>>
                    Opublikowane</a></li>
            <li class="ui-tabs-nav-item">
                <a href="<%= Url.RouteUrl("User", new { name = userId, tab = UserDetailTab.Commented, page = 1}) %>" <%= ((selectedTab == UserDetailTab.Commented) ? "class='active'" : string.Empty)%>>
                    Skomentowane</a></li>
            <li class="ui-tabs-nav-item">
                <a href="<%= Url.RouteUrl("User", new { name = userId, tab = UserDetailTab.Achivements}) %>" <%= ((selectedTab == UserDetailTab.Achivements) ? "class='active'" : string.Empty) %>>
                Odznaki
                </a>
            </li>
        </ul>        
    </div>
    <div id="no-padding" class="tabs-contents">
            <span class="hide hfeed">
                <%= Model.SiteTitle%></span>
            <% StoryList(); %>
        </div>
</asp:Content>
