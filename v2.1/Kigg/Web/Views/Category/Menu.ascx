<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ICollection<ICategory>>" %>
<%@ OutputCache Duration="86400" VaryByParam="None" %><!-- doba -->
<div class="category">
    <ul>
        <li><a rel="home" href="<%= Url.Content("~") %>">Wszystkie</a> </li>
        <%
            for (var i = 0; i < Model.Count;++i )
            {
                ICategory category = Model.ElementAt(i);
                
                //znaczne uproszczenie tego rozwiazania
                bool isLast = Model.Count == i + 1;
        %>
        
        <li <%if(isLast){%> class="last" <%}%> >
        
            <%= Html.ActionLink(category.Name, "Category", "Story", new { name = category.UniqueName }, new { rel = "tag directory" })%>
        
        </li>

        <%
            }
        %>
    </ul>
</div>
