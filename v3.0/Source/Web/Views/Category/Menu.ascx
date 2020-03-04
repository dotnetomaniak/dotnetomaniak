<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ICollection<ICategory>>" %>
<script runat="server">
    private bool IsInSupportPages(string url)
    {
        return url.Contains("PromoteSite") || url.Contains("Faq") || url.Contains("Contact") || url.Contains("About") || url.Contains("Policy") || url.Contains("Odznaki");
    }
</script>
<nav class="navbar navbar-expand-sm navbar-light">
  <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
    <span class="navbar-toggler-icon"></span>
  </button>

  <div class="collapse navbar-collapse" id="navbarSupportedContent">
    <ul class="navbar-nav mr-auto primary-nav">
      <li class="nav-item">
        <a class="nav-link <%= Request.Url.AbsolutePath == "/" ? "active" : string.Empty %>" href="/">Strona główna</a>
      </li>
      <li class="nav-item" id="categories">
        <a class="nav-link dropdown-toggle <%= Request.Url.AbsolutePath.Contains("Category") ? "active" : string.Empty %>" href="#" id="navbarCategoryDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Kategorie</a>
        <div class="categories dropdown-menu clearfix" aria-labelledby="navbarCategoryDropdown">
        <ul>
            <%
                int count = Model.Count;
                int index = 0;
                foreach (ICategory category in Model.OrderBy(x => x.Name))
                {
            %>
            <li class="col-12 col-sm-3 <%= index/4 == count/4 ? "last" : "" %> <%= (index + 1) % 4 == 0 ? "last-col" : "" %>"><strong>
                <%= Html.ActionLink(category.Name, "Category", "Story", new { name = category.UniqueName }, new { rel = "tag directory", @class="sublink dropdown-item" })%>
            </strong>
                <p class="d-none d-sm-block">
                    <%: category.Description %></p>
            </li>
            <%
                    index++;
                }
            %>
        </ul>
        </div>
      </li>
      <li class="nav-item dropdown" id="about">
        <a class="nav-link dropdown-toggle <%= IsInSupportPages(Request.Url.AbsolutePath) ? "active" : string.Empty %>" href="#" id="navbarAboutDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
          O dotNETomaniaku
        </a>
        <div class="about dropdown-menu clearfix" aria-labelledby="navbarAboutDropdown">
            <ul>
                <li class="col-12"><a class="sublink" href="<%= Url.Action("All","Badges") %>">Odznaki</a></li>
                <li class="col-12"><a class="sublink" href="<%= Url.RouteUrl("PromoteSite")%>">Promocja</a> </li>
                <li class="col-12"><a class="sublink" href="<%= Url.Action("Faq", "Support") %>">FAQ</a></li>
                <li class="col-12"><a class="sublink" href="<%= Url.RouteUrl("About") %>">O dotNETomaniak</a></li>
                <li class="col-12"><a class="sublink" href="<%= Url.Action("Policy","Support") %>">Polityka prywatności</a></li>
                <li class="col-12 last"><a class="sublink" href="<%= Url.RouteUrl("Contact") %>">Kontakt</a></li>
            </ul>
        </div>
      </li>
        <li class="nav-item">
            <a class="nav-link" href="https://dotnetomaniak.cupsell.pl" target="_blank">Sklep z gadżetami</a>
        </li>
    </ul>

  </div>
</nav>