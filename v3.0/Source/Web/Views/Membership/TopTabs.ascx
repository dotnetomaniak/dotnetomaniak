<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TopUserTabsViewData>" %>
<%
    bool shouldShowLeaderTab = (Model.TopLeaders.IsNullOrEmpty() == false);

    if (shouldShowLeaderTab)
    {%>
      <div class="pageTitle"><h2>Najaktywniejsi</h2></div>
      <div id="toplist">
        <img class="left" alt="" src="/Assets/Images/toplist_top.jpg" />        
        <% Html.RenderPartial("Top", Model.TopLeaders); %>
        <img class="left" alt="" src="/Assets/Images/toplist_bottom.jpg" />
      </div>
    <%}
     %>
<%--<%
bool shouldShowMoverTab = (!Model.TopMovers.IsNullOrEmpty());
bool shouldShowLeaderTab = (!Model.TopLeaders.IsNullOrEmpty());

if (shouldShowMoverTab || shouldShowLeaderTab)
{
%>
    <div id="topUserTabs" class="hide">
        <ul>
            <% if (shouldShowMoverTab) %>
            <% { %>
                    <li class="sidebar-tabs-nav-item"><a href="#topMovers">Top dnia</a></li>
            <% } %>
            <% if (shouldShowLeaderTab) %>
            <% { %>
                    <li class="sidebar-tabs-nav-item"><a href="#topLeaders">Top .netomaniacy</a></li>
            <% } %>
        </ul>
        <% if (shouldShowMoverTab) %>
        <% { %>
            <div id="topMovers" class="topUsers">
                <% Html.RenderPartial("Top", Model.TopMovers); %>
            </div>
        <% } %>
        <% if (shouldShowLeaderTab) %>
        <% { %>
            <div id="topLeaders" class="topUsers">
                <% Html.RenderPartial("Top", Model.TopLeaders); %>
            </div>
        <% } %>
    </div>
<%
}
%>--%>