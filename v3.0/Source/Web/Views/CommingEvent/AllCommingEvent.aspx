<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<CommingEventsViewData>" %>
<%@ Import Namespace="Kigg.Web.ViewData" %>

<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContentPlaceHolder">
    <div>
        <%= Html.PageHeader("Nadchące wydarzenia") %>
        <% Html.RenderPartial("CommingEventList", Model); %>
    </div>
</asp:Content>