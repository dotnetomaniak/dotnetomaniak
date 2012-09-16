<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ OutputCache Duration="86400" VaryByParam="None" %><!-- doba -->
<%@ Import Namespace="System.Xml" %>
<%
    var rssReader = new XmlTextReader("http://blog.dotnetomaniak.pl/syndication.axd");
    var rssDoc = new XmlDocument();
    XmlNode nodeRss = null;
    XmlNode nodeChannel = null;
    XmlNode nodeLastItem = null;
    rssDoc.Load(rssReader);

    for (int i = 0; i < rssDoc.ChildNodes.Count; i++)
    {
        if (rssDoc.ChildNodes[i].Name == "rss")
        {
            nodeRss = rssDoc.ChildNodes[i];
            break;
        }
    }

    for (int i = 0; i < nodeRss.ChildNodes.Count; i++)
    {
        if (nodeRss.ChildNodes[i].Name == "channel")
        {
            nodeChannel = nodeRss.ChildNodes[i];
            break;
        }
    }

    for (int i = 0; i < nodeChannel.ChildNodes.Count; i++)
    {
        if (nodeChannel.ChildNodes[i].Name == "item")
        {
            nodeLastItem = nodeChannel.ChildNodes[i];
            break;
        }
    }

    if (nodeLastItem != null)
    {
        var element = nodeLastItem;
        var pubDate = DateTime.Parse(nodeLastItem["pubDate"].InnerText);
        var text = element["description"].InnerText.StripHtml();
%>
<div id="feed">
    <h3>
        Ostatnio na blogu</h3>
    <div>
        <p>
            <a href="<%=element["link"].InnerText %>"><strong>
                <%=element["title"].InnerText %></strong></a><br />
            <em>
                <%=pubDate %></em>
        </p>
        <p>
            <%//glupi pomysl ale na szybko - ucina string na trzecim znaku '.' %>
            <%=text.Substring(0, text.IndexOf('.', text.IndexOf('.', text.IndexOf('.') + 1) + 1) + 1) %>
        </p>
        <p>
            <a href="<%=element["link"].InnerText %>">Przejdź do bloga</a>
        </p>
    </div>
</div>
<%
    }%>
