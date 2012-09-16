<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseViewData>" %>
<%StoryListSearchViewData searchData = Model as StoryListSearchViewData; %>
<%string queryText = null;  %>
<%if (searchData != null) queryText = searchData.Query; %>
<%using (Html.BeginForm("Search", "Story", FormMethod.Get, new { id = "frmSearch" }))%>
<%{%>
    <div class="searchBox">
        <%= Html.TextBox("q", queryText, new { id = "txtSearch", @class = "searchTextBox" })%>
        <input type="submit" value="" />  
    </div>
<%}%>