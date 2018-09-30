<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master"
    Inherits="System.Web.Mvc.ViewPage<PromoteSiteViewData>" %>

<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        Page.Header.Title = "{0} - Promocja".FormatWith(Model.SiteTitle);
    }        
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<%= Html.ArticleHeader("Materiały promocyjne", new string[] { "Strona główna", "Materiały promocyjne"}) %>
    <p>Jeśli chciałbyś promować dotnetomaniaka, na swojej stronie czy blogu, tutaj znajdziesz
        niezbędę do tego celu narzędzia. Jeśli masz jakieś pytania odnośnie promocji lub
        brakuje Ci tu czegoś -
        <%= Html.ActionLink("napisz nam wiadomość", "Contact", "Support")%></p>
    
            <%
                foreach (var key in Model.Items.Keys)
                {
%>
            <div class="articleHeader">
                <p>
                    <span class="promo-header"><%=key%></span>
                </p>
            </div>
            <%
                    foreach (var promoteItem in Model.Items[key])
                    {%>
                  <div class="promo">
                    <div class="promo-sample">
                        <img src="<%=promoteItem.Url %>" alt="promocja"  />
                    </div>
                    <div class="promo-download">
                        <p>Pliki do pobrania:</p>
                        <ul>
                            <li><a href="<%=promoteItem.Eps %>" target="_blank">eps</a></li>
                            <li><a href="<%=promoteItem.Pdf %>" target="_blank">pdf</a></li>
                            <li><a href="<%=promoteItem.Jpg %>" target="_blank">jpg</a></li>
                            <li><a href="<%=promoteItem.Png %>" target="_blank">png</a></li>
                        </ul>
                    </div>
                  </div>
                    <%}
                }%>  
                    
        <div class="articleHeader">
            <p>
                <span class="promo-header">Skrypt na bloga</span>
            </p>
        </div>
        <div class="promo">
        <p>
            Jeśli masz swój blog, lub stronę możesz umożliwić swoim czytelnikom wypromowanie
            swojego artykułu na dotnetomaniak.pl. Najłatwiej będzie Ci skorzystać z <a href="http://www.feedburner.com/fb/a/publishers/feedflare">
                FeedFlare</a>. Niezbędny plik znajduje się tu: <a href="http://static.dotnetomaniak.pl/feedflare-v2.0.0.2.xml">
                    http://static.dotnetomaniak.pl/feedflare-v2.0.0.2.xml</a>. Jeśli nie możesz
            skorzystać z tego pliku skontaktuj się z nami po więcej informacji.
        </p>      
        <p>
            Jeśli masz ochotę możesz również nas reklamować w realnym świecie. <a href="http://dotnetomaniak.cupsell.pl">
                Odwiedź nasz sklep</a> i wybierz coś dla siebie. Dzięki temu będziesz miał fajny
            gadżet a nam pomożesz w rozwoju i utrzymaniu strony.</p>
    </div>
</asp:Content>
