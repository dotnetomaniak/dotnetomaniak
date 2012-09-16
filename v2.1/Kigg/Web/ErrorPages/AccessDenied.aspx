<%@ Page Language="C#"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="pl" lang="pl">
<head>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8"/>
    <meta name="robots" content="noindex,nofollow"/>
    <link href="<%= Page.ResolveClientUrl("~/Assets/StyleSheets/error.min.css") %>" rel="stylesheet" type="text/css"/>
    <title>Dostęp zabroniony</title>
</head>
<body>
    <div id="container">
        <div id="talker">
            <a href="<%= Page.ResolveClientUrl("~/")%>"><img src="<%= Page.ResolveClientUrl("~/Assets/Images/fav32.png")%>" alt="Talker"/></a>
        </div>
        <div id="notice">
            <h1>Dostęp zabroniony!</h1>
            <p>
                Twój adres Ip <strong><%=Request.UserHostAddress%></strong> jest obecnie zablokowany z racji niedozwolonego użycia strony. Jeśli uważasz, że niesłusznie
            zablokowanyliśmy twój adres Ip, sugerujemy skontaktować się z nami jak najszybciej.
            </p>
            <p>
                Zespół dotnetomaniak.pl
                <br/>
                support@dotnetomaniak.pl
            </p>
            <div class="clear"></div>
        </div>
        <div class="clear"></div>
    </div>
</body>
</html>