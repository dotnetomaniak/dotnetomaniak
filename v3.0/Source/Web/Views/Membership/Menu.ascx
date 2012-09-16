<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UserMenuViewData>" %>
<% bool isAuthenticated = Model.IsUserAuthenticated; %>
<% IUser user = Model.CurrentUser; %>
<p class="userLinks">
    <strong>
    <span>Witaj 
    <%if (isAuthenticated) %>
    <%{%>
        <% string userName = user.UserName; %>
        <img class="smoothImage" onload="javascript:SmoothImage.show(this)" alt="<%= Html.AttributeEncode(userName) %>" src="<%= Html.AttributeEncode(user.GravatarUrl(24)) %>"/> 
        <%= Html.RouteLink(userName, "User", new { name = user.Id.Shrink(), tab = UserDetailTab.Promoted, page = 1 })%>
    <%} %>
    <%else%>
    <%{%>
        Gościu
    <%}%>
    ,</span></strong>
    <%if (isAuthenticated) %>
    <%{%>
        <%if (!user.IsOpenIDAccount()) %>
        <%{ %>
                <a id="lnkChangePassword" href="javascript:void(0)">Zmień hasło</a><span>lub</span>
        <%} %>
        <a id="lnkLogout" href="javascript:void(0)">Wyloguj</a>
    <%}%>
    <%else%>
    <%{%>
        <a id="lnkLogin" href="javascript:void(0)">Zaloguj się</a><span>lub</span>
        <a id="lnkSignup" href="javascript:void(0)">Zarejestruj</a>
    <%}%>
</p>