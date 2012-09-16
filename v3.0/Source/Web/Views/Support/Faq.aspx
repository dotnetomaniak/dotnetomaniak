<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<SupportViewData>" %>

<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        GenerateScript();

        Page.Header.Title = "{0} - FAQ".FormatWith(Model.SiteTitle);
    }

    private void GenerateScript()
    {
        jQueryScriptManager scriptManager = jQueryScriptManager.Current;

        scriptManager.RegisterSource(Url.Asset("faq"));

        scriptManager.RegisterOnReady("Faq.init();");
        scriptManager.RegisterOnDispose("Faq.dispose();");
    }

</script>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">    
    <%= Html.ArticleHeader("FAQ", new string[] {"Strona główna", "FAQ"})%>
    <ol id="faq">
        <li class="faq"><a class="q" href="javascript:void(0)">
            <h4>
                Które artykuły mogę publikować?</h4>
        </a>
            <div class="ans">
                Akceptujemy tylko artykuły, które dotyczą platformy .Net, jeśli jednak znalazłes/aś
                artykuł, który nie dotyczy bezpośrednio .Net, a uważasz że programiści piszący w
                tej technologii mogliby na tym skorzystać, również możesz opublikować taki artykuł.</div>
        </li>
        <li class="faq"><a class="q" href="javascript:void(0)">
            <h4>
                Jaka jest róznica między logowaniem OpenId, a kontem w tym portalu?</h4>
        </a>
            <div class="ans">
                <a href="http://openid.net/" target="_blank" rel="nofollow">OpenID</a> eliminuje
                potrzebę posiadania kont w różnych portalach. Wspieramy logowanie OpenId, co oznacza,
                że jeśli masz konto w <a href="http://openid.yahoo.com/" target="_blank" rel="nofollow">
                    Yahoo</a>, <a href="http://openid.aol.com/" target="_blank" rel="nofollow">AOL</a>,
                <a href="http://wordpress.com/" target="_blank" rel="nofollow">Wordpress</a>, <a
                    href="http://blogspot.com/" target="_blank" rel="nofollow">Blogger</a> lub <a href="https://www.myopenid.com/"
                        target="_blank" rel="nofollow">myOpenID</a> (itp.) możesz zalogować się
                bez rejestracji na dotnetomaniak.pl. Jeśli nie masz żadnego loginu OpenID, możesz
                je utworzyć za darmo <a href="https://www.myopenid.com/signup?affiliate_id=22806&amp;openid.sreg.optional=email"
                    target="_blank">tutaj</a>. Użytkowników logujących się przez OpenID i bezpośrednio
                poprzez konto w portalu traktujemy tak samo. Jednak niektóre portale dostarczające
                logowanie OpenID nie podają adresu e-mail użytykownika, co uniemożliwia korzystanie
                z niektórych funkcji portalu (n.p. powiadomienia o komenatrzach poprzez e-mail).</div>
        </li>
        <li class="faq"><a class="q" href="javascript:void(0)">
            <h4>
                Co to jest ".Netomaniak"?</h4>
        </a>
            <div class="ans">
                <em>.Netomaniak</em> odnosci się do przydatności artykułu. Pokazuje, jak społeczność
                dotnetomaniak.pl ocenia artykuł poprzez kliknięcie <em>Promuj</em>.</div>
        </li>
        <li class="faq"><a class="q" href="javascript:void(0)">
            <h4>
                Co to jest liczba punktów użytkownika? Jak jest obliczana?</h4>
        </a>
            <div class="ans">
                Liczba punktów użytkownika pokazuje jego zaangażowanie w aktywne uczestnictwo w
                portalu. Większość działań na rzecz portalu ma przypisaną ilość punktów:
                <ol>
                    <li>
                        <span>
                            Aktywność
                        </span>
                        <span>
                            Punkty
                        </span>
                    </li>
                    <li>
                        <span>
                            Rejestracja/Przypisanie OpenID
                        </span>
                        <span>
                            05.00
                        </span>
                    </li>
                    <li>
                        <span>
                            Wysłanie artykułu
                        </span>
                        <span>
                            10.00
                        </span>
                    </li>
                    <li>
                        <span>
                            Artykuł wyświetlony na pierwszej stronie
                        </span>
                        <span>
                            05.00
                        </span>
                    </li>
                    <li>
                        <span>
                            Link kliknięty<sup>*</sup>
                        </span>
                        <span>
                            00.01
                        </span>
                    </li>
                    <li>
                        <span>
                            Nadchodzący artykuł wypromowany<sup>*</sup>
                        </span>
                        <span>
                            03.00
                        </span>
                    </li>
                    <li>
                        <span>
                            Opublikowany artykuł wypromowany<sup>*</sup>
                        </span>
                        <span>
                            02.00
                        </span>
                    </li>
                    <li>
                        <span>
                            Oznaczenie artykułu jako spam<sup>*</sup>
                        </span>
                        <span>
                            05.00
                        </span>
                    </li>
                    <li>
                        <span>
                            Napisanie komentarza<sup>*</sup>
                        </span>
                        <span>
                            01.00
                        </span>
                    </li>
                    <li>
                        <span>
                            Twój komentarz oznaczono jako obraźliwy
                        </span>
                        <span>
                            -1.00
                        </span>
                    </li>
                    <li>
                        <span>
                            Błędne oznaczenie komentarza jako spam
                        </span>
                        <span>
                            -1.00
                        </span>
                    </li>
                    <li>
                        <span>
                            Twój artykuł oznaczono jako spam
                        </span>
                        <span>
                            -50.0
                        </span>
                    </li>
                    <li>
                        <span>
                            Twój komentarz oznaczono jako spam
                        </span>
                        <span>
                            -20.0
                        </span>
                    </li>
                </ol>
                <sup>*</sup>Artykuł nie jest starszy niż 10 dni.
                <br />
                (Powyższe dane moga być zmienione bez wcześniejszego ostrzeżenia)
                <br />
                Jeśli punkty użytkownika spadną poniżej zera, konto może zostać usunięte.
            </div>
        </li>
        <li class="faq"><a class="q" href="javascript:void(0)">
            <h4>
                Co to Top .Netomaniacy & Top dnia?</h4>
        </a>
            <div class="ans">
                <em>Top dnia</em> to lista użytkowników którzy zarobili najwięcej punktów w ciągu
                ostatnich 24 godzin, a <em>Top .Netomaniacy</em> to użytkownicy posiadający najwięcej
                punktów w klasyfikacji ogólnej.</div>
        </li>
        <li class="faq"><a class="q" href="javascript:void(0)">
            <h4>
                W jaki sposób artykuły zostają opublikowane? Dlaczego mój artykuł nigdy nie został
                opublikowany?</h4>
        </a>
            <div class="ans">
                Artykuły są publikowane na podstawie .Netomaniaków, komentarzy, aktualności, wyświetleń,
                punktów użytkownika (który głosował na artykuł), źródła artykułu itd. Im lepsze
                są wymienione podstawy do oceny, tym większa szansa na opublikowanie artykułu. W
                momencie opublikowania 20 najlepszych artykułów jest umieszczanych na stronie głównej.
                Publikacja następuje 4/5 razy dziennie, w zależności od ilości artykułów w kolejce
                do publikacji.</div>
        </li>
        <li class="faq"><a class="q" href="javascript:void(0)">
            <h4>
                Mam swój blog, czy mogę umieścić licznik <em>Promuj</em> na blogu?</h4>
        </a>
            <div class="ans">
                Najłatwiej jest skopiować link <em>pokaż kod licznika</em>, nastepnie wklej html
                na swój blog. Jeśli taka forma nie jest wystarczająca spróbuj:
                <ul>
                    <li><a href="http://download.live.com/writer" target="_blank" rel="nofollow external">
                        Live Writer</a> Plugin.</li>
                    <li><a href="http://www.dotnetblogengine.net/" target="_blank" rel="nofollow external">
                        BlogEngine.NET</a> Extension.</li>
                    <li><a href="http://graffiticms.com/" target="_blank" rel="nofollow external">Graffiti
                        Cms</a> Chalk.</li>
                    <li><a href="http://communityserver.com/" target="_blank" rel="nofollow external">Community
                        Server</a> ICSModule.</li>
                </ul>
                Wymienione komponenty mają możliwość pełnej zmiany ustawień licznika <em>Promuj</em>
                i dostepny kod źródłowy. Aby ściągnąć wejdź na stronę <a href="http://www.codeplex.com/Kigg/Release/ProjectReleases.aspx"
                    target="_blank">release'ów Kigg</a> i postępuj zgodnie z instrukcjami.
                <br />
                <pre style="background-color: #ddd; font-size: 10px; margin-top: 5px;">
<code>&lt;div class=&quot;kigg&quot;&gt; &lt;a rev=&quot;vote-for&quot; href=&quot;http://dotnetomaniak.pl/Submit?url=URL&quot;&gt;
&lt;img alt=&quot;Promuj&quot; src=&quot;http://dotnetomaniak.pl/image.axd?url=URL&quot;
style=&quot;border:0px&quot;/&gt; &lt;/a&gt; &lt;div&gt;</code></pre>
            </div>
        </li>
    </ol>
</asp:Content>
