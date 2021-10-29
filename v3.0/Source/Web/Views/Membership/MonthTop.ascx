<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TopUserTabsViewData>" %>
<%
    bool shouldShowLeaderTab = (Model.TopLeaders.IsNullOrEmpty() == false);

    if (shouldShowLeaderTab)
    {%>
      <div class="pageTitle"><h2>Top miesiąca</h2></div>
      <div id="toplist" class="col-12">
<%--        <img class="left" alt="" src="/Assets/Images/toplist_top.jpg" />        --%>
        <% Html.RenderPartial("Top", Model.TopLeaders); %>
<%--        <img class="left" alt="" src="/Assets/Images/toplist_bottom.jpg" />--%>
      </div>
    <%}
     %>