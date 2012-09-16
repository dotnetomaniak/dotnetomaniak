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
<%= Html.ArticleHeader("Materia³y promocyjne", new string[] { "Strona g³ówna", "Materia³y promocyjne"}) %>
    <p>Jeœli chcia³byœ promowaæ dotnetomaniaka, na swojej stronie czy blogu, tutaj znajdziesz
        niezbêdê do tego celu narzêdzia. Jeœli masz jakieœ pytania odnoœnie promocji lub
        brakuje Ci tu czegoœ -
        <%= Html.ActionLink("napisz nam wiadomoœæ", "Contact", "Support")%></p>
    
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
            Jeœli masz swój blog, lub stronê mo¿esz umo¿liwiæ swoim czytelnikom wypromowanie
            mojego artyku³u na dotnetomaniak.pl. Naj³atwiej bêdzie Ci skorzystaæ z <a href="http://www.feedburner.com/fb/a/publishers/feedflare">
                FeedFlare</a>. Niezbêdny plik znajduje siê tu: <a href="http://static.dotnetomaniak.pl/feedflare-v2.0.0.2.xml">
                    http://static.dotnetomaniak.pl/feedflare-v2.0.0.2.xml</a>. Jeœli nie mo¿esz
            skorzystaæ z tego pliku skontaktuj siê z nami po wiêcej informacji.
        </p>      
        <p>
            Jeœli masz ochotê mo¿esz równie¿ nas reklamowaæ w realnym œwiecie. <a href="http://dotnetomaniak.cupsell.pl">
                OdwiedŸ nasz sklep</a> i wybierz coœ dla siebie. Dziêki temu bêdziesz mia³ fajny
            gad¿et a nam pomo¿esz w rozwoju i utrzymaniu strony.</p>
    </div>
</asp:Content>
