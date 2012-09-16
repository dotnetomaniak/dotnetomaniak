<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="pl" lang="pl">
<head>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8"/>
    <meta http-equiv="Pragma" content="no-cache"/>
    <meta http-equiv="Expires" content="-1"/>
    <meta name="robots" content="noindex,nofollow"/>
    <link href="<%= Page.ResolveClientUrl("~/Assets/StyleSheets/error.min.css") %>" rel="stylesheet" type="text/css"/>
    <title>Strony nie odnaleziono</title>
</head>
<body>
    <div id="container">
        <div id="talker">
            <a href="<%= Page.ResolveClientUrl("~/")%>"><img src="<%= Page.ResolveClientUrl("~/Assets/Images/fav32.png")%>" alt="Talker"/></a>
        </div>
        <div id="notice">
            <h1>Witamy na stronie błędu 404!</h1>
            <p>
                Witamy na spersonalizowanie stronie błędu. Jesteś tu ponieważ kliknąłeś na link do strony która nie istnieje. jest to prawdopodobnie nasza wina... ale zamiast pokazywać standardową stronę 'Błąd 404', która jest myląca i nie wyjaśnia co tak na prawdę się stało, 
                stworzyliśmy tę stronę aby wyjaśnić co zaszło źle.
            </p>
            <p>
                Możesz albo (a) kliknąć przycisk 'Wstecz' w twojej przeglądarce lub (b) kliknać poniższy link, aby powrócić na stronę główną.
            </p>
            <div class="back">
                <a href="<%= Page.ResolveClientUrl("~/")%>">Powrót na stronę główną.</a>
            </div>
            <div class="clear"></div>
        </div>
        <div class="clear"></div>
    </div>
</body>
</html>