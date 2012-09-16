<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StoryDetailViewData>" %>
<%@ OutputCache VaryByParam="None" Duration="7200" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Linq" %>

<h2>Powi¹zane pytania z devPytania:</h2>
<%
   IEnumerable<XmlNode> entries = null;
   try
   {   
   foreach(var tag in Model.Story.Tags)
   {
       var address = "http://devpytania.pl/szukaj?type=rss&q=[{0}]".FormatWith(tag.UniqueName);
       var rssReader = new XmlTextReader(address);
       var rssDoc = new XmlDocument();

       rssDoc.Load(rssReader);
       entries = rssDoc.SelectNodes("//item").Cast<XmlNode>();
       if (entries !=null && entries.Count() > 0)
           break;
   }   
   }
   catch { }      
  %>
<ul>
    <% if (entries == null || (entries != null && entries.Count() == 0))
       { %>
        <span>Nie znaleziono powi¹zanych pytañ.</span>
    <% }
       else
       {
           foreach (var entry in entries.Take(5))
           { %>
        <li>
            <a href="<%= Html.Encode(entry.SelectSingleNode("link").InnerText) %>">
                <%= Html.Encode(entry.SelectSingleNode("title").InnerText)%>
            </a>
        </li>
    <% }
       } %>
</ul>
