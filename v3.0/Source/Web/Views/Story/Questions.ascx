<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<XmlNode>>" %>
<%@ OutputCache VaryByParam="None" Duration="7200" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Linq" %>

<h2>Powi¹zane pytania z devPytania:</h2>
<ul>
    <% if (Model == null || (Model != null && Model.Count() == 0))
       { %>
        <span>Nie znaleziono powi¹zanych pytañ.</span>
    <% }
       else
       {
           foreach (var entry in Model.Take(7))
           { %>
        <li>
            <a href="<%= Html.Encode(entry.SelectSingleNode("link").InnerText) %>">
                <%= Html.Encode(entry.SelectSingleNode("title").InnerText)%>
            </a>
        </li>
    <% }
       } %>
</ul>
