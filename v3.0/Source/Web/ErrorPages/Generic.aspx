<%@ Page Language="C#"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="pl" lang="pl">
<head>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8"/>
    <meta http-equiv="Pragma" content="no-cache"/>
    <meta http-equiv="Expires" content="-1"/>
    <meta name="robots" content="noindex,nofollow"/>
    <link href="<%= Page.ResolveClientUrl("~/Assets/StyleSheets/error.css") %>" rel="stylesheet" type="text/css"/>
    <title>Błąd</title>
</head>
<body>
    <div id="container">
        <div id="talker">
            <a href="<%= Page.ResolveClientUrl("~/")%>"><img src="<%= Page.ResolveClientUrl("~/Assets/Images/logo-dotnetomaniak.png")%>" alt="Talker"/></a>
        </div>
        <div id="notice">
            <h1>Oho!</h1>
            <h2>
                Coś niedobrego stało się pomiędzy neuronami naszego procesora. 
            </h2><!---->
            <p>
                Nie byliśmy w stanie przetworzyć twojego żądania. Przejdź do strony głównej.
            </p>
            <div class="back">
                <a href="<%= Page.ResolveClientUrl("~/")%>" title="Powrót na stronę główną."><img src="<%= Page.ResolveClientUrl("~/Assets/Images/error_mainpage.png") %>" alt="Powrót na stronę główną" /></a>
            </div>
            <div class="clear"></div>
        </div>
        <div class="clear"></div>
    </div>
    <div id="footer">
            <p>
                <strong>&copy; dotnetomaniak.pl.</strong>
                    <%= DateTime.Today.Year.ToString("0000") %>
                    - Treść opublikowana przez użytkowników portalu umieszczana jest na zasadzie <a target="_blank"
                        rel="license" href="http://creativecommons.org/licenses/publicdomain/">public domain</a>.		
            </p>
        </div>
</body>
</html>