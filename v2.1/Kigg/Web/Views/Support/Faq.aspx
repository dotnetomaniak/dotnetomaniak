<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<SupportViewData>"%>
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
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <%= Html.PageHeader("Frequently Asked Questions")%>
    <ol id="faq">
        <li>
            <a class="q" href="javascript:void(0)">Które artyku³y mogê publikowaæ?</a>
            <div class="ans">Akceptujemy tylko artyku³y, które dotycz¹ platformy .Net, jeœli jednak znalaz³es/aœ artyku³, który nie dotyczy bezpoœrednio .Net, a uwa¿asz ¿e programiœci pisz¹cy w tej technologii mogliby na tym skorzystaæ, równie¿ mo¿esz opublikowaæ taki artyku³.</div>
        </li>
        <li>
            <a class="q" href="javascript:void(0)">Jaka jest róznica miêdzy logowaniem OpenId, a kontem w tym portalu?</a>
            <div class="ans"><a href="http://openid.net/" target="_blank" rel="nofollow">OpenID</a> eliminuje potrzebê posiadania kont w ró¿nych portalach. Wspieramy logowanie OpenId, co oznacza, ¿e jeœli masz konto w <a href="http://openid.yahoo.com/" target="_blank" rel="nofollow">Yahoo</a>, <a href="http://openid.aol.com/" target="_blank" rel="nofollow">AOL</a>, <a href="http://wordpress.com/" target="_blank" rel="nofollow">Wordpress</a>, <a href="http://blogspot.com/" target="_blank" rel="nofollow">Blogger</a> lub <a href="https://www.myopenid.com/" target="_blank" rel="nofollow">myOpenID</a> (itp.) mo¿esz zalogowaæ siê bez rejestracji na dotnetomaniak.pl. Jeœli nie masz ¿adnego loginu OpenID, mo¿esz je utworzyæ za darmo <a href="https://www.myopenid.com/signup?affiliate_id=22806&amp;openid.sreg.optional=email" target="_blank">tutaj</a>. U¿ytkowników loguj¹cych siê przez OpenID i bezpoœrednio poprzez konto w portalu traktujemy tak samo. Jednak niektóre portale dostarczaj¹ce logowanie OpenID nie podaj¹ adresu e-mail u¿ytykownika, co uniemo¿liwia korzystanie z niektórych funkcji portalu (n.p. powiadomienia o komenatrzach poprzez e-mail).</div>
        </li>
        <li>
            <a class="q" href="javascript:void(0)">Co to jest ".Netomaniak"?</a>
            <div class="ans"><em>.Netomaniak</em> odnosci siê do przydatnoœci artyku³u. Pokazuje, jak spo³ecznoœæ dotnetomaniak.pl ocenia artyku³ poprzez klikniêcie <em>Promuj</em>.</div>
        </li>
        <li>
            <a class="q" href="javascript:void(0)">Co to jest liczba punktów u¿ytkownika?  Jak jest obliczana?</a>
            <div class="ans">
                Liczba punktów u¿ytkownika pokazuje jego zaanga¿owanie w aktywne uczestnictwo w portalu. Wiêkszoœæ dzia³añ na rzecz portalu ma przypisan¹ iloœæ punktów:
                <table>
                    <colgroup span="1" style="text-align:left"></colgroup>
                    <colgroup span="1" style="text-align:right"></colgroup>
                    <tbody>
                        <tr>
                            <th>Aktywnoœæ</th>
                            <th>Punkty</th>
                        </tr>
                        <tr>
                            <td>Rejestracja/Przypisanie OpenID</td>
                            <td>05.00</td>
                        </tr>
                        <tr>
                            <td>Wys³anie artyku³u</td>
                            <td>10.00</td>
                        </tr>
                        <tr>
                            <td>Artyku³ wyœwietlony na pierwszej stronie</td>
                            <td>05.00</td>
                        </tr>
                        <tr>
                            <td>Link klikniêty<sup>*</sup></td>
                            <td>00.01</td>
                        </tr>
                        <tr>
                            <td>Nadchodz¹cy artyku³ wypromowany<sup>*</sup></td>
                            <td>03.00</td>
                        </tr>
                        <tr>
                            <td>Opublikowany artyku³ wypromowany<sup>*</sup></td>
                            <td>02.00</td>
                        </tr>
                        <tr>
                            <td>Oznaczenie artyku³u jako spam<sup>*</sup></td>
                            <td>05.00</td>
                        </tr>
                        <tr>
                            <td>Napisanie komentarza<sup>*</sup></td>
                            <td>01.00</td>
                        </tr>
                        <tr>
                            <td>Twój komentarz oznaczono jako obraŸliwy</td>
                            <td>-1.00</td>
                        </tr>
                        <tr>
                            <td>B³êdne oznaczenie komentarza jako spam</td>
                            <td>-1.00</td>
                        </tr>
                        <tr>
                            <td>Twój artyku³ oznaczono jako spam</td>
                            <td>-50.0</td>
                        </tr>
                        <tr>
                            <td>Twój komentarz oznaczono jako spam</td>
                            <td>-20.0</td>
                        </tr>
                    </tbody>
                </table>
                <sup>*</sup>Artyku³ nie jest starszy ni¿ 10 dni.
                <br />
                (Powy¿sze dane moga byæ zmienione bez wczeœniejszego ostrze¿enia)
                <br />
                Jeœli punkty u¿ytkownika spadn¹ poni¿ej zera, konto mo¿e zostaæ usuniête.
            </div>
        </li>
        <li>
            <a class="q" href="javascript:void(0)">Co to Top .Netomaniacy & Top dnia?</a>
            <div class="ans"><em>Top dnia</em> to lista u¿ytkowników którzy zarobili najwiêcej punktów w ci¹gu ostatnich 24 godzin, a <em>Top .Netomaniacy</em> to u¿ytkownicy posiadaj¹cy najwiêcej punktów w klasyfikacji ogólnej.</div>
        </li>
        <li>
            <a class="q" href="javascript:void(0)">W jaki sposób artyku³y zostaj¹ opublikowane? Dlaczego mój artyku³ nigdy nie zosta³ opublikowany?</a>
            <div class="ans">Artyku³y s¹ publikowane na podstawie .Netomaniaków, komentarzy, aktualnoœci, wyœwietleñ, punktów u¿ytkownika (który g³osowa³ na artyku³), Ÿród³a artyku³u itd. Im lepsze s¹ wymienione podstawy do oceny, tym wiêksza szansa na opublikowanie artyku³u. W momencie opublikowania 20 najlepszych artyku³ów jest umieszczanych na stronie g³ównej. Publikacja nastêpuje 4/5 razy dziennie, w zale¿noœci od iloœci artyku³ów w kolejce do publikacji.</div>
        </li>
        <li>
            <a class="q" href="javascript:void(0)">Mam swój blog, czy mogê umieœciæ licznik <em>Promuj</em> na blogu?</a>
            <div class="ans">
                Naj³atwiej jest skopiowaæ link <em>poka¿ kod licznika</em>, nastepnie wklej html na swój blog. Jeœli taka forma nie jest wystarczaj¹ca spróbuj:
                <ul>
                    <li><a href="http://download.live.com/writer" target="_blank" rel="nofollow">Live Writer</a> Plugin.</li>
                    <li><a href="http://www.dotnetblogengine.net/" target="_blank" rel="nofollow">BlogEngine.NET</a> Extension.</li>
                    <li><a href="http://graffiticms.com/" target="_blank" rel="nofollow">Graffiti Cms</a> Chalk.</li>
                    <li><a href="http://communityserver.com/" target="_blank" rel="nofollow">Community Server</a> ICSModule.</li>
                </ul>
                Wymienione komponenty maj¹ mo¿liwoœæ pe³nej zmiany ustawieñ licznika <em>Promuj</em> i dostepny kod Ÿród³owy. Aby œci¹gn¹æ wejdŸ na stronê <a href="http://www.codeplex.com/Kigg/Release/ProjectReleases.aspx" target="_blank">release'ów Kigg</a> i postêpuj zgodnie z instrukcjami.
                <br />
            </div>
        </li>	
    </ol>
</asp:Content>
