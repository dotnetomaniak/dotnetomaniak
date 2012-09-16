<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<StoryListViewData>"%>
<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        Page.Header.Title = Model.Title;
    }

</script>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
    <link href="<%= Model.RssUrl %>" title="<%= Model.Title %> (rss)" type="application/rss+xml" rel="alternate"/>
    <link href="<%= Model.AtomUrl %>" title="<%= Model.Title %> (atom)" type="application/atom+xml" rel="alternate"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <span class="hide hfeed"><%= Model.Title %></span>
    <% Html.RenderPartial("StoryList"); %>
</asp:Content>