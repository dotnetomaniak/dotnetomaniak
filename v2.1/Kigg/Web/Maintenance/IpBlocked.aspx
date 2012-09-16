<%@ Page Language="C#" AutoEventWireup="true"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8"/>
    <title>Dostęp zablokowany</title>
    <style type="text/css">
        body
        {
            background:#59A5CF url(http://dotnetomaniak.pl/Assets/Images/logo.png) no-repeat fixed 100% 100%;
        }
        .backSoon
        {
            margin-top:100px;
            margin-left:20px;
            color:#fff;
            font-family:Trebuchet MS, Tahoma, Arial, sans-serif;
            width:300px;
        }
    </style>
</head>
<body>
    <div class="backSoon">
        <h2>Dostęp zablokowany!</h2>
        Twój adres Ip <strong><%=Request.UserHostAddress%></strong> jest obecnie blokowany z powodu niedozowolnego użycia strony. Jeśli uważasz, że niesłusznie
        zablokowanyliśmy twój adres Ip, sugerujemy skontaktować się z nami jak najszybciej.
        <br/>
        <br/>
        Zespół .netomaniak!
        <br/>
        support@dotnetomaniak.pl
    </div>
    <script type="text/javascript">
var gaJsHost = (("https:" == document.location.protocol) ? "https://ssl." : "http://www.");
document.write(unescape("%3Cscript src='" + gaJsHost + "google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E"));
</script>
<script type="text/javascript">
try {
var pageTracker = _gat._getTracker("UA-7150596-1");
pageTracker._trackPageview();
} catch(err) {}</script>
</body>
</html>