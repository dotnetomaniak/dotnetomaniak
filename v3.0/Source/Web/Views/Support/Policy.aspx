<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<SupportViewData>" %>

<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);


        Page.Header.Title = "{0} - Polityka prywatności".FormatWith(Model.SiteTitle);
    }

</script>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">    
    <%= Html.ArticleHeader("Polityka prywatności", new string[] {"Strona główna", "Polityka prywatności"})%>
    <ol id="faq">
        <li class="faq">
            <h4>§1. Zasady ochrony prywatności</h4>

            <div class="ans">
                <p>1. Administratorem danych osobowych jest dotnetomaniak.pl</p>
                <p>2. Administrator danych osobowych przykłada dużą wagę do ochrony prywatności i poufności danych
                osobowych użytkowników serwisu dotnetomaniak.pl dostępnego pod adresem
                http://www.dotnetomaniak.pl/ (zwanego dalej: „dotnetomaniak.pl” lub „Serwisem”).</p>
                <p>3. Kontakt z Inspektorem Ochrony Danych odbywa się poprzez adres e-mail kontakt@dotnetomaniak.pl</p>
                <p>4. Administrator z należytą starannością dobiera i stosuje odpowiednie środki techniczne i organizacyjne
                zapewniające ochronę przetwarzanych danych osobowych. Pełen dostęp do baz danych posiadają
                jedynie osoby należycie uprawnione przez Administratora.</p>
                <p>5. Administrator zabezpiecza dane osobowe przed ich udostępnieniem osobom nieupoważnionym, jak
                również przed ich przetwarzaniem z naruszeniem obowiązujących przepisów prawa.</p>
                <p>6. Osoby odwiedzające Serwis mogą przeglądać treści umieszczone w Serwisie bez podawania danych
                osobowych.</p>
            </div>
        </li>
        <li class="faq">
            <h4>§2. Podstawa przetwarzania danych osobowych</h4>
            <div class="ans">
                <p>1. Dane osobowe są przetwarzane przez Administratora zgodnie z przepisami prawa, w tym w
                szczególności z Rozporządzeniem Parlamentu Europejskiego i Rady (UE) 2016/679 z dnia 27
                kwietnia 2016 roku w sprawie ochrony osób fizycznych w związku z przetwarzaniem danych
                osobowych i w sprawie swobodnego przepływu takich danych oraz uchylenia dyrektywy 95/46/WE
                (zwanym dalej „GDPR”) w celu:</p>
                <p>&nbsp;&nbsp;a. zawarcia i wykonania umowy zawartej na warunkach określonych w regulaminie Serwisu (na
                podstawie art. 6 ust. 1 lit. b GDPR);</p>
                <p>&nbsp;&nbsp;b. udzielania odpowiedzi użytkownikom Internetu na pytania związane z Serwisem oraz jego
                funkcjonowaniem (na podstawie art. 6 ust. 1 lit. f GDPR);</p>
                <p>&nbsp;&nbsp;c. korzystania z usługi newsletter, obejmującej przesyłanie informacji handlowych, na podstawie
                wyrażonej zgody (art. 6 ust. 1 lit. a GDPR);</p>
                <p>&nbsp;&nbsp;d. wypełnienia prawnie ciążących obowiązków na Administratorze danych, na podstawie art. 6 ust. 1 lit. c
                RODO (np. obowiązki rachunkowe i podatkowe);</p>
                <p>&nbsp;&nbsp;e. dochodzenia lub zabezpieczenia roszczeń (na podstawie art. 6 ust. 1 lit. f GDPR).</p>
                <p>2. Podanie danych osobowych jest dobrowolne, jednakże konsekwencją niepodania danych może być,
                w zależności od przypadku, niemożność korzystania z usług świadczonych przez Serwis, niemożność
                uzyskania odpowiedzi na zadane pytania lub niemożność otrzymania newslettera.</p>
                <p>3. Użytkownik nie powinien przekazywać Administratorowi danych osobowych osób trzecich. Jeżeli
                natomiast przekazuje takie dane każdorazowo oświadcza, że posiada stosowną zgodę osób trzecich
                na przekazanie danych Administratorowi.</p>
            </div>
            
        </li>
        <li class="faq">
            <h4>§3. Zakres przetwarzania danych osobowych</h4>
            <div class="ans">
                <p>1. Administrator przetwarza zakres danych podanych przez użytkownika w treści zagadnienia
                kierowanego do Administratora.</p>
                <p>2. Dane podawane przez użytkowników wykorzystywane są jedynie do: realizacji umowy zawartej na
                warunkach określonych w regulaminie Serwisu, udzielania odpowiedzi na zadawane pytania,
                przesyłania newslettera, w tym informacji handlowych o Administratorze i jego produktach oraz
                usługach oraz w celach statystycznych.</p>
                <p>3. Administrator wykorzystuje adresy IP zbierane w trakcie połączeń internetowych w celach
                technicznych, związanych z administracją serwerami. Ponadto adresy IP służą do zbierania ogólnych,
                statystycznych informacji demograficznych (np. o regionie, z którego następuje połączenie).</p>
            </div>
        </li>
        <li class="faq">
            <h4>§4. Kontrola przetwarzania danych osobowych</h4>
            <div class="ans">
                <p>1. Użytkownik jest zobowiązany do wskazywania danych pełnych, aktualnych i prawdziwych.</p>
                <p>2. Każdy użytkownik, którego dane osobowe są przetwarzane przez Administratora ma prawo dostępu
                do treści swoich danych oraz prawo ich sprostowania, usunięcia, ograniczenia przetwarzania, prawo
                do przenoszenia danych, prawo wniesienia sprzeciwu wobec przetwarzania danych na podstawie
                uzasadnionego interesu Administratora, prawo do cofnięcia zgody w dowolnym momencie bez wpływu
                na zgodność z prawem przetwarzania (jeżeli przetwarzanie odbywa się na podstawie zgody), którego
                dokonano na podstawie zgody przed jej cofnięciem.</p>
                <p>3. Skorzystanie z uprawnień określonych w ustępie powyżej może być realizowane poprzez wysłanie na
                adres kontakt@dotnetomaniak.pl stosownego żądania wraz z podaniem imienia i nazwiska oraz
                adresu poczty elektronicznej (e-mail) użytkownika.</p>
                <p>4. Użytkownik posiada prawo wniesienia skargi do organu nadzorczego, w przypadku gdy uzna, że
                przetwarzanie jego danych osobowych narusza przepisy GDPR.</p>
            </div>
        </li>
        <li class="faq">
            <h4>§5. Udostępnianie danych osobowych</h4>
            <div class="ans">
                <p>1. Dane użytkowników mogą być udostępniane podmiotom uprawnionym do ich otrzymania na mocy
                    obowiązujących przepisów prawa, w tym właściwym organom wymiaru sprawiedliwości. Dane
                    osobowe mogą być przekazywane podmiotom przetwarzającym je na zlecenie Administratora, to
                    jest agencjom marketingowym, partnerom świadczącym usługi techniczne (rozwijanie i
                    utrzymywanie systemów informatycznych i serwisów internetowych). Dane osobowe nie będą
                    przekazywane do państwa trzeciego/organizacji międzynarodowej.
                </p>
            </div>
        </li>
        <li class="faq">
            <h4>§6. Okres przechowywania i pozostałe informacje dotyczące przetwarzania danych</h4>
            <div class="ans">
                <p>1. Dane osobowe będą przechowywane tylko przez okres niezbędny do zrealizowania określonego celu,
                w którym zostały przesłane albo na potrzeby zachowania zgodności z przepisami prawa.</p>
                <p>2. Jeżeli użytkownik wyraził zgodę na wykorzystanie danych osobowych w związku z korzystaniem z
                usługi newsletter, dane osobowe będą przetwarzane do czasu odwołania zgody.</p>
                <p>3. Dane osobowe nie będą przetwarzane w sposób zautomatyzowany przez Administratora.</p>
            </div>
        </li>
        <li class="faq">
            <h4>§ 7. Pliki Cookies</h4>
            <div class="ans">
                <p>1. dotnetomaniak.pl zbiera informacje zawarte w Plikach Cookies.</p>
                <p>2. dotnetomaniak.pl wykorzystuje następujące rodzaje Plików Cookies:</p>
                <p>&nbsp;&nbsp;a. reklamowe - umożliwiające dostarczanie treści reklamowych dostosowanych do zainteresowań Użytkownika;</p>
                <p>&nbsp;&nbsp;b.analityczne - gromadzą informacje o sposobie korzystania z dotnetomaniak.pl, rodzaju strony www, z jakiej Użytkownik został przekierowany do dotnetomaniak.pl, oraz liczbie odwiedzin i czasie wizyty Użytkownika na dotnetomaniak.pl;</p>
                <p>&nbsp;&nbsp;c. funkcjonalne – pozwalają na dostosowanie zawartości strony internetowej dotnetomaniak.pl do preferencji Użytkownika oraz optymalizacji korzystania z tej strony internetowej; w szczególności pliki te pozwalają rozpoznać urządzenie Użytkownika i odpowiednio wyświetlać zawartość strony internetowej dotnetomaniak.pl.</p>
                <p>3. dotnetomaniak.pl korzysta z Plików Cookies o charakterze:</p>
                <p>&nbsp;&nbsp;a. stałym - pozostają zapamiętane w urządzeniu po zamknięciu przeglądarki (zakończeniu sesji) i wygasają po upływie jednego roku;</p>
                <p>&nbsp;&nbsp;b. sesyjnym - pozostają zapamiętane w urządzeniu po zamknięciu przeglądarki (zakończeniu sesji) i są stosowane do zapisywania informacji z danej sesji.</p>
                <p>4. Użytkownik może sprzeciwić się używaniu Plików Cookies poprzez zmianę ustawień przeglądarki internetowej.</p>
                <p>5. Ograniczenia stosowania Plików Cookies mogą wpłynąć na niektóre funkcjonalności dotnetomaniak.pl, a nawet uniemożliwić korzystanie z dotnetomaniak.pl .</p>
            </div>
        </li>
    </ol>
</asp:Content>
