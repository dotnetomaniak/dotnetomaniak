<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TagTabsViewData>" %>
<%
    bool shouldShowPopularTab = !Model.PopularTags.IsNullOrEmpty();

    if (shouldShowPopularTab)
    {
%>
<div id="tags" class="pageHeader">
    <div class="pageTitle">
        <h2>Tagi</h2>
    </div>
</div>
<div id="popularTags" class="home-tags">
    <% Html.RenderPartial("Cloud", Model.PopularTags); %>
</div>
<%
    }
%>