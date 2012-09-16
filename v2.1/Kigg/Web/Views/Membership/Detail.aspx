<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<UserDetailViewData>"%>
    <script runat="server">

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Page.Header.Title = "{0} - Użytkownik : {1}".FormatWith(Model.SiteTitle, Model.TheUser.UserName);
        }

        private void StoryList()
        {
        string userId = Model.TheUser.Id.Shrink();
        int page = Model.CurrentPage;

        switch (Model.SelectedTab)
            {
                case UserDetailTab.Posted:
                    {
                        Html.RenderAction<StoryController>(c => c.PostedBy(userId, page));
                        break;
                    }

                case UserDetailTab.Commented:
                    {
                        Html.RenderAction<StoryController>(c => c.CommentedBy(userId, page));
                        break;
                    }

                default:
                    {
                        Html.RenderAction<StoryController>(c => c.PromotedBy(userId, page));
                        break;
                    }
            }
        }
    </script>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <%= Html.PageHeader(Model.TheUser.UserName)%>
    <div style="clear:left">
        <% IUser user = Model.TheUser; %>
        <%bool isVisitingOwnPage = ((Model.IsCurrentUserAuthenticated) && (user.Id.Equals(Model.CurrentUser.Id)));%>
        <%bool isAdmin = ((Model.IsCurrentUserAuthenticated) && (Model.CurrentUser.IsAdministrator()));%>
        <table>
            <tbody>
                <tr>
                    <td style="vertical-align:top;text-align:center">
                        <img alt="<%= Html.AttributeEncode(user.UserName) %>" class="smoothImage" src="<%= Html.AttributeEncode(user.GravatarUrl(128)) %>" onload="javascript:SmoothImage.show(this)"/>
                        <%if (isVisitingOwnPage) %>
                        <%{%>
                            <br/>
                            <a href="http://gravatar.com" target="_blank">Zmień avatara</a>
                        <%}%>
                    </td>
                    <td style="padding-left:10px">
                        <table border="0" cellspacing="1" cellpadding="3">
                            <tbody>
                                <tr>
                                    <td>Nazwa: </td>
                                    <td>
                                        <%= user.UserName%>
                                        <%if (isAdmin)%>
                                        <%{%>
                                            <a id="lnkLockUser" href="javascript:void(0)" onclick="javascript:Administration.lockUser('<%= user.Id.Shrink() %>')" class="actionLink <%= user.IsLockedOut ? "hide" : string.Empty %>">zablokuj</a>
                                            <a id="lnkUnlockUser" href="javascript:void(0)" onclick="javascript:Administration.unlockUser('<%= user.Id.Shrink() %>')" class="actionLink <%= user.IsLockedOut ? string.Empty : "hide" %>">odblokuj</a>
                                        <%}%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Punkty:</td>
                                    <td>
                                        <%= (user.IsPublicUser()) ? user.CurrentScore.ToString(FormatStrings.UserScore) : "brak danych" %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Ostatnio widziany: </td>
                                    <td><%= user.LastActivityAt.ToRelative()%> temu</td>
                                </tr>
                                <tr>
                                    <td>Zarejestrowany od: </td>
                                    <td><%= user.CreatedAt.ToString(FormatStrings.LongDate, new System.Globalization.CultureInfo("pl-PL").DateTimeFormat)%></td>
                                </tr>
                                <%if (isAdmin || isVisitingOwnPage) %>
                                <%{%>
                                    <tr>
                                        <%string email = user.HasDefaultOpenIDEmail() ? "brak" : user.Email; %>
                                        <td>Email: </td>
                                        <td>
                                            <div id="emailViewSection" class="form">
                                                <span id="spnEmail"><%= email%></span>
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
                                                            <input id="btnChangeEmail" type="submit" class="button" value="Zapisz"/> lub
                                                            <a id="lnkCancelEmail" href="javascript:void(0)" class="actionLink">anuluj</a>
                                                            <span id="spnChangeEmailError" class="error"></span>
                                                    <% } %>
                                                </div>
                                            <%}%>
                                        </td>
                                    </tr>
                                <%}%>
                                <%if (!user.IsPublicUser() || isAdmin) %>
                                <%{%>
                                    <tr>
                                        <td>Rola: </td>
                                        <td>
                                            <div id="roleViewSection" class="form">
                                                <span id="spnRole"><%= user.Role.ToString() %></span>
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
                                                                    <%bool select = (user.Role == (Kigg.DomainObjects.Roles) pair.Key); %>
                                                                    <%if (select) %>
                                                                    <%{%>
                                                                        <option value="<%= pair.Value %>" selected="selected"><%= pair.Value %></option>
                                                                    <%}%>
                                                                    <%else%>
                                                                    <%{%>
                                                                        <option value="<%= pair.Key %>"><%= pair.Value %></option>
                                                                    <%}%>
                                                                <%}%>
                                                            </select>
                                                            <input id="btnChangeRole" type="submit" class="button" value="Zmień"/> lub
                                                            <a id="lnkCancelRole" href="javascript:void(0)" class="actionLink">anuluj</a>
                                                    <% } %>
                                                </div>
                                            <%}%>
                                        </td>
                                    </tr>
                                <%}%>
                                <%if ((isAdmin) && (!Model.IPAddresses.IsNullOrEmpty()))%>
                                <%{%>
                                    <tr>
                                        <td style="vertical-align:top">
                                            Adresy IP:
                                        </td>
                                        <td>
                                            <%using (Html.BeginForm("AllowIps", "Membership", FormMethod.Post, new { id = "frmAllowIps", @class = "form" }))%>
                                            <%{%>
                                                <%= Html.Hidden("id", user.Id.Shrink()) %>
                                                <ul style="list-style:none none outside;margin:0px;padding:0px;width:32em">
                                                    <%foreach(KeyValuePair<string, bool> ipAddress in Model.IPAddresses) %>
                                                    <%{%>
                                                        <li style="float:left;width:8em;padding:4px;white-space:nowrap;overflow:hidden">
                                                            <label class="smallLabel">
                                                                <%if (ipAddress.Value)%>
                                                                <%{%>
                                                                    <input name="ipAddress" type="checkbox" value="<%= ipAddress.Key %>" checked="checked"/><%= ipAddress.Key %>
                                                                <%}%>
                                                                <%else%>
                                                                <%{%>
                                                                    <input name="ipAddress" type="checkbox" value="<%= ipAddress.Key %>"/><%= ipAddress.Key %>
                                                                <%}%>
                                                            </label>
                                                        </li>
                                                    <%}%>
                                                </ul>
                                                <div class="clearLeft" style="padding-top:2px">
                                                    <input id="btnAllowIpAddresses" type="submit" class="button" value="Zezwól"/>
                                                </div>
                                            <%}%>
                                        </td>
                                    </tr>
                                <%}%>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="divider"></div>
    <div id="userTabs">
        <ul class="ui-tabs-nav">
            <% string userId = user.Id.Shrink(); %>
            <% UserDetailTab selectedTab = Model.SelectedTab; %>
            <li class="ui-tabs-nav-item<%= ((selectedTab == UserDetailTab.Promoted) ? " ui-tabs-selected" : string.Empty) %>"><a href="<%= Url.RouteUrl("User", new { name = userId, tab = UserDetailTab.Promoted, page = 1}) %>">Wypromowane</a></li>
            <li class="ui-tabs-nav-item<%= ((selectedTab == UserDetailTab.Posted) ? " ui-tabs-selected" : string.Empty) %>"><a href="<%= Url.RouteUrl("User", new { name = userId, tab = UserDetailTab.Posted, page = 1 }) %>">Opublikowane</a></li>
            <li class="ui-tabs-nav-item<%= ((selectedTab == UserDetailTab.Commented) ? " ui-tabs-selected" : string.Empty) %>"><a href="<%= Url.RouteUrl("User", new { name = userId, tab = UserDetailTab.Commented, page = 1}) %>">Skomentowane</a></li>
        </ul>
        <div id="userTabContent" class="ui-tabs-panel">
            <span class="hide hfeed"><%= Model.SiteTitle%></span>
            <% StoryList(); %>
        </div>
    </div>
</asp:Content>
