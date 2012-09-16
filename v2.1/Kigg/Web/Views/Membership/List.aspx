<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<UserListViewData>"%>
<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        Page.Header.Title = Model.Title;
    }

</script>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <%= Html.PageHeader(Model.Subtitle)%>
    <%if (!Model.Users.IsNullOrEmpty()) %>
    <%{ %>
        <div id="userList">
            <table>
                <thead>
                    <tr>
                        <th>Użytkownik</th>
                        <th>Ostatnio widziany</th>
                        <th>Punkty</th>
                    </tr>
                </thead>
                <tbody>
                    <% foreach (IUser user in Model.Users) %>
                    <% { %>
                            <tr>
                                <td>
                                    <% string userName = user.UserName; %>
                                    <a href="<%= Url.RouteUrl("User", new { name = user.Id.Shrink(), tab = UserDetailTab.Promoted, page = 1 }) %>">
                                        <img alt="<%= Html.AttributeEncode(userName) %>" src="<%= Html.AttributeEncode(user.GravatarUrl(24)) %>" class="smoothImage" onload="javascript:SmoothImage.show(this)"/>
                                        <%= Html.Encode(userName)%>
                                    </a>
                                </td>
                                <td>
                                    <%= user.LastActivityAt.ToRelative()%> temu
                                </td>
                                <td style="text-align:right">
                                    <%= (user.IsPublicUser()) ? user.CurrentScore.ToString(FormatStrings.UserScore) : "brak danych" %>
                                </td>
                            </tr>
                    <% } %>
                </tbody>
                <tfoot>
                    <tr>
                        <th colspan="3">Liczba użytkowników: <%= Model.TotalUserCount %></th>
                    </tr>
                </tfoot>
            </table>
        </div>
        <%= Html.Pager("Users", null, null, ViewContext.RouteData.Values, "page", false, Model.PageCount, 10, 2, Model.CurrentPage)%>
    <%} %>
    <%else%>
    <%{%>
        <span class="pageMessage"><%= Model.NoUserExistMessage %></span>
    <%}%>
</asp:Content>