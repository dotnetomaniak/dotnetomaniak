<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" Inherits="System.Web.Mvc.ViewPage<FbEmailViewData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<p>Użytkownik o adresie e-mail <b><%= Model.Email %></b> nie istnieje w naszej bazie danych.</p>

<p>Utwórz nowe konto za pomocą Facebooka klikając na przycisk poniżej.</p> 
    <br />
        <fb:login-button scope="public_profile,email" onlogin="createUserByFb();"></fb:login-button>
    <br />
<p>Jeśli masz już konto, połącz je z Facebookiem w panelu użytkownika.</p>
    
</asp:Content>
