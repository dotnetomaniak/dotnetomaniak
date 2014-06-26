<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<FbEmailViewData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<%= Html.ArticleHeader("Logowanie przez Fb", new string[] {"Strona główna", "Logowanie przez Fb"})%>
    <div id ="logWithFbInfo">
    <p>Użytkownik o adresie e-mail <b><%= Model.Email %></b> nie istnieje w naszej bazie danych.</p>
    <p>Utwórz nowe konto za pomocą Facebooka klikając na przycisk poniżej.</p> 
    <br />
        <center><fb:login-button scope="public_profile,email" onlogin="createUserByFb();">Utwórz konto</fb:login-button></center>
    <br />
    <p>Jeśli masz już konto, połącz je z Facebookiem w panelu użytkownika.</p>
    </div>
</asp:Content>
