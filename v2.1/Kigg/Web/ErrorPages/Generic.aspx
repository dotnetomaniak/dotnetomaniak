<%@ Page Language="C#"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="pl" lang="pl">
<head>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8"/>
    <meta http-equiv="Pragma" content="no-cache"/>
    <meta http-equiv="Expires" content="-1"/>
    <meta name="robots" content="noindex,nofollow"/>
    <link href="<%= Page.ResolveClientUrl("~/Assets/StyleSheets/error.min.css") %>" rel="stylesheet" type="text/css"/>
    <title>Błąd</title>
</head>
<body>
    <div id="container">
        <div id="talker">
            <a href="<%= Page.ResolveClientUrl("~/")%>"><img src="<%= Page.ResolveClientUrl("~/Assets/Images/fav32.png")%>" alt="Talker"/></a>
        </div>
        <div id="notice">
            <h1>Oho!</h1>
            <p>
                Coś niedobrego stało się pomiędzy neuronami naszego procesora. Nie byliśmy w stanie przetworzyć twojego żądania.
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