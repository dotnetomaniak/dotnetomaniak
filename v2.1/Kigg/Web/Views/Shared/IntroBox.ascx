<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ OutputCache Duration="86400" VaryByParam="None" %><!-- doba -->
<div id="intro">
    <h3>
        Witaj!</h3>
    <div>
        <p>
            <strong>dotnetomaniak.pl</strong> to miejsce gdzie możesz znaleźć najnowsze artykuły
            o <strong>technologii .NET</strong> znajdujących się na stronach polskiej społeczności.
            Dziel się wiedzą i korzystaj z artykułów innych, aby poszerzać swoją wiedzę i budować
            jeszcze lepszą społeczność <strong>.NET</strong> w Polsce.
        </p>
        <p>
            <!--Enjoy participating in this community and see which other people in our industry are getting <em>Shoutouts</em>.-->
            <strong>dotnetomaniak.pl</strong> to portal dla fanów <strong>.NET</strong>
        </p>
        <p style="float: left">
            <%= Html.ActionLink("promuj »","PromoteSite","Support")%>&nbsp;
            <%= Html.ActionLink("konkurs »","List","Quiz") %>
        </p>        
        <p style="text-align: right">
            <%= Html.ActionLink("faq »", "Faq", "Support")%>
        </p>
    </div>
</div>
