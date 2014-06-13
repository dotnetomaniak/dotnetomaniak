<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h2>Użytkownik o podanym przez Ciebie adresie e-mail nie istnieje w naszej bazie danych.</h2>

<h2>Jeżeli nie masz konta na dotnetomaniak.pl, utwórz nowe konto za pomocą Facebooka.</h2>
<div class="box">
    <fb:login-button scope="public_profile,email" onlogin="createUserByFb();"></fb:login-button>
</div> 
<h2>Jeżeli posiadasz konto na dotnetomaniak.pl, zaloguj się do naszego portalu, aby zsynchronizować je z Facebookiem.</h2>
     
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
