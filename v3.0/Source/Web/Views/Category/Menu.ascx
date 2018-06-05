<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ICollection<ICategory>>" %>
<script runat="server">
    private bool IsInSupportPages(string url)
    {
        return url.Contains("PromoteSite") || url.Contains("Faq") || url.Contains("Contact") || url.Contains("About");
    }
    </script>
<ul class="primary-nav">
    <li><a href="/" <%= Request.Url.AbsolutePath == "/" ? "class='active'" : string.Empty %>>Strona główna</a> </li>
</ul>
<div class="categories-container">
    <ul class="primary-nav">
        <li id="categories"><a href="#" <%= Request.Url.AbsolutePath.Contains("Category") ? "class='active'" : string.Empty %>><span>Kategorie</span> </a></li>
    </ul>
    <div class="categories">
        <ul>
            <%
                int count = Model.Count;
                int index = 0;
                foreach (ICategory category in Model.OrderBy(x => x.Name))
                {
            %>
            <li <%= index/4 == count/4 ? "class='last'" : "" %>><strong>
                <%= Html.ActionLink(category.Name, "Category", "Story", new { name = category.UniqueName }, new { rel = "tag directory", @class="" })%>
            </strong>
                <p>
                    <%: category.Description %></p>
            </li>
            <%
                    index++;
                }
            %>
        </ul>
    </div>
</div>
<div class="about-container">
    <ul class="primary-nav">
        <li id="about"><a href="#" <%= IsInSupportPages(Request.Url.AbsolutePath) ? "class='active'" : string.Empty %>><span>O dotNETomaniak</span></a></li>
    </ul>
    <div class="about">
        <ul>
            <li><a href="<%= Url.Action("All","Badges") %>">Odznaki</a></li>
            <li><a href="<%= Url.RouteUrl("PromoteSite")%>">Promocja</a> </li>
            <li><%= Html.ActionLink("FAQ", "Faq", "Support")%></li>
            <li><a href="<%= Url.RouteUrl("About") %>">O dotNETomaniak</a></li>
            <li><a href="<%= Url.Action("Policy","Support") %>">Polityka prywatności</a></li>
            <li class="last"><a href="<%= Url.RouteUrl("Contact") %>">Kontakt</a></li>
        </ul>
    </div>
</div>
<div class="">
    <ul class="primary-nav">
       <li><a href="http://dotnetomaniak.cupsell.pl" target="_blank">Sklep z gadżetami</a></li>
    </ul>
</div>
