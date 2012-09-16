<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<BadgesViewData>" %>
<%@ OutputCache Duration="300" VaryByParam="None" %>

<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContentPlaceHolder">
<%= Html.PageHeader("Odznaki do zdobycia") %>
<% Html.RenderPartial("List", Model); %>
</asp:Content>
