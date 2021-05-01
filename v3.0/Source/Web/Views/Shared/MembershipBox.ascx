<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseViewData>" %>
<% bool isAuthenticated = Model.IsCurrentUserAuthenticated; %>
<div id="membershipBox" class="modalBox">
    <div class="titleContainer">
        <div class="title">
            <%--<% Model.SiteTitle%>--%></div>
        <div id="membershipClose" class="closeButton" title="Close">
        </div>
    </div>
    <div class="contentContainer">
        <%if (!isAuthenticated) %>
        <%{ %>
        <div id="loginSection">
            <div class="box">
                <h5>
                    Logowanie</h5>
                <p>
                    Zewnętrzni dostawcy tożsamości korzystają z technologii OpenID, dzięki której Twoje
                    hasło zawsze jest poufne i nie musisz zapamiętywać kolejnego.
                </p>
                <form id="frmOpenIdLogin" action="<%= Url.Action("OpenId", "Membership") %>" method="post">
                <fieldset>         
                    <div class="row">
                    <div class="col-12 col-sm-3">
                        <label for="openid_identifier" class="label" id="openId">
                            Wprowadź URL swojego OpenID:</label>
                    </div>
                    <div class="col-12 col-sm-9">
                        <input id="openid_identifier" name="identifier" type="text" class="openIDTextBox" />
                        <span class="error"></span> <br />
                        <input id="openid_RememberMe" name="rememberMe" type="checkbox" value="true" />
                        <label for="openid_RememberMe">Zapamiętaj mnie na tym komputerze</label>
                    </div>
                   </div>
                   <div class="add-article-row">
                        <input id="btnOpenId" type="submit" class="button" value="Login" />
                    </div>
                </fieldset>
                </form>
            </div>           
            <h6>
                lub...</h6>
            <p>
                Wprowadź nazwę użytkownika/e-mail oraz hasło</p>
            <div class="box">
                <form id="frmLogin" action="<%= Url.Action("Login", "Membership") %>" method="post">
                <fieldset>
                    <div class="row">
                        <div class="col-12 col-sm-4 add-article-row resp-label">
                            <label for="txtLoginUserName" class="label col-12" id="userNameOrPassword">
                            Nazwa <br /> użytkownika/e-mail:</label>
                        </div>
                        <div class="col-12 col-sm-8 add-article-row">
                            <input id="txtLoginUserName" name="userNameOrEmail" type="text" class="textBox" />
                            <span class="error"></span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12 col-sm-4 add-article-row resp-label">
                            <label for="txtLoginPassword" class="label">
                            Hasło:</label>
                        </div>
                        <div class="col-12 col-sm-8 add-article-row">
                            <input id="txtLoginPassword" name="password" type="password" class="textBox" />
                            <span class="error"></span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="offset-sm-4 col-sm-8">
                            <label class="label">
                            </label>
                            <input id="chkLoginRememberMe" name="rememberMe" type="checkbox" value="true" />
                            <label for="chkLoginRememberMe">Zapamiętaj mnie na tym komputerze</label>
                        </div>
                    </div>
                    <div class="add-article-row">
                        <span id="loginMessage" class="message"></span>
                    </div>
                    <div class="add-article-row">
                        <input id="btnLogin" type="submit" class="button" value="Login" />
                        <a href="#" id="lostPassword" name="lostPassword">zapomniałeś hasła?</a>
                    </div>
                </fieldset>
                </form>
            </div>
            <h6>
                lub...
            </h6>
            <p>
                Zaloguj się przy użyciu Facebooka.
            </p>
            <div class="box">                
                <div class="add-article-row text-center">
                        <div class="fb-login-button" data-onlogin="checkLoginState()" data-max-rows="1" 
                             data-size="large" data-width="300" data-button-type="login_with" data-show-faces="false" data-auto-logout-link="false" data-use-continue-as="true"></div>
                    <br />
                </div>
            </div>         
        </div> 
        <%}%>
        <%if ((isAuthenticated) && (!Model.CurrentUser.IsOpenIDAccount())) %>
        <%{%>
        <div id="changePasswordSection">
            <div class="box">
                <h5>Zmień hasło</h5>                
                <form id="frmChangePassword" action="<%= Url.Action("ChangePassword", "Membership") %>"
                method="post">
                <fieldset>
                <div class="add-article-row">
                    <label for="txtChangeOldPassword" class="label">
                        Stare:</label>
                    <input id="txtChangeOldPassword" name="oldPassword" type="password" class="textBox" />
                    <span class="error"></span>
                </div>
                <div class="add-article-row">
                    <label for="txtChangeNewPassword" class="label">
                        Nowe:</label>
                    <input id="txtChangeNewPassword" name="newPassword" type="password" class="textBox" />
                    <span class="error"></span>
                </div>
                <div class="add-article-row">
                    <label for="txtChangenConfirmPassword" class="label">
                        Potwierdź:</label>
                    <input id="txtChangenConfirmPassword" name="confirmPassword" type="password" class="textBox" />
                    <span class="error"></span>
                </div>
                <div class="add-article-row">
                    <span id="changePasswordMessage" class="message"></span>
                </div>
                <div class="add-article-row">
                    <input id="btnChangePassword" type="submit" class="button" value="Change Password" />
                </div>
                </fieldset>
                </form>
            </div>
        </div>        
        <%}%>
        <%if (!isAuthenticated) %>
        <%{%>
        <div id="signupSection">
            <div class="box">
                <h5>
                    Załóż konto</h5>
                <p>
                    Podaj nazwę użytkownika i adres e-mail, aby założyć konto. Dzięki niemu będziesz
                    na bieżąco informowany o zmianach w serwisie.</p>
                <form id="frmSignup" action="<%= Url.Action("Signup", "Membership") %>" method="post">
                <fieldset>
                    <div class="add-article-row row">
                        <label for="txtSignupUserName" class="label col-3 text-right">
                            Nazwa użytkownika:</label>
                        <div class="col-9">
                            <input id="txtSignupUserName" name="userName" type="text" class="textBox" />
                            <span class="error"></span>
                        </div>
                    </div>
                    <div class="add-article-row row">
                        <label for="txtSignupPassword" class="label col-3 text-right">
                            Hasło:</label>
                        <div class="col-9">
                            <input id="txtSignupPassword" name="password" type="password" class="textBox" />
                            <span class="error"></span>
                        </div>
                    </div>
                    <div class="add-article-row row">
                        <label for="txtSignupEmail" class="label col-3 text-right">
                            Email:</label>
                        <div class="col-9">
                            <input id="txtSignupEmail" name="email" type="text" class="textBox" />
                            <span class="error"></span><span class="info">(Proszę wprowadź poprawny adres e-mail,
                            będzie potrzebny do weryfikacji)</span>
                        </div>
                    </div>
                    <div class="add-article-row">
                        <span id="signupMessage" class="message"></span>
                    </div>
                    <div class="add-article-row">
                        <input id="btnSignup" type="submit" class="button" value="Sign up" />
                    </div>                    
                </fieldset>
                </form>                
            </div>
        </div>
        <div id="lostPasswordSection">
            <div class="box">
                <h5>Zapomniałeś hasła</h5>
                <form id="frmForgotPassword" action="<%= Url.Action("ForgotPassword", "Membership") %>" method="post">
                <fieldset>
                   <div class="row">
                        <div class="col-12 col-sm-3 add-article-row resp-label">
                            <label for="txtForgotEmail" class="label">
                            Email:</label>
                        </div>
                        <div class="col-12 col-sm-9 add-article-row">
                            <input id="txtForgotEmail" name="email" type="text" class="textBox" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="offset-sm-3 col-sm-9">
                            <span class="error"></span><span class="info">(działa tylko dla zarejestrowanych)</span>
                        </div>
                    </div>
                    <div class="add-article-row row">
                        <div class="col-12">
                            <input id="btnForgotPassword" type="submit" class="button" value="Wyślij hasło" />
                        </div>
                    </div>


<%--                    <div class="add-article-row text-right">
                        <label for="txtForgotEmail" class="label">
                            Email:</label>
                        <input id="txtForgotEmail" name="email" type="text" class="textBox" />
                        <span class="error"></span><span class="info">(działa tylko dla zarejestrowanych)</span>
                    </div>
                    <div class="add-article-row">
                        <span id="forgotPasswordMessage" class="message"></span>
                    </div>
                    <div class="add-article-row">
                        <input id="btnForgotPassword" type="submit" class="button" value="Wyślij hasło" />
                    </div>--%>
                </fieldset>
                </form>
            </div>            
        </div> 
        <%}%>
        <div id="RecommendationSection">
            <div class="box">
                <h5>
                    Edycja reklamy</h5>
                <form id="frmRecommendation" action="<%= Url.Action("EditAd", "Recommendation") %>" method="post">
                <fieldset>
                    <input type="hidden" id="hidAdId" name="id"/>
                    <div class="add-article-row">
                        <label for="txtRecommendationLink" class="label">
                            Link rekomendacji:</label>
                        <input id="txtRecommendationLink" name="RecommendationLink" type="text" class="textBox" />
                        <span class="error"></span>
                    </div>
                    <div class="add-article-row">
                        <label for="txtRecommendationTitle" class="label">
                            Tytuł rekomendacji:</label>
                        <input id="txtRecommendationTitle" name="RecommendationTitle" type="text" class="textBox" />
                        <span class="error"></span>
                    </div>
                    <div class="add-article-row">
                        <label for="txtImageLink" class="label">
                            Link zdjęcia:</label>
                        <input id="txtImageLink" name="ImageLink" type="text" class="textBox" />
                        <span class="error"></span>
                    </div>
                    <div class="add-article-row">
                        <label for="txtImageTitle" class="label">
                            Tytuł zdjęcia:</label>
                        <input id="txtImageTitle" name="ImageTitle" type="text" class="textBox"/>
                    </div>
                    <div class="add-article-row">
                        <label for="txtStartTime" class="label">
                            Reklama od:</label>
                        <input id="txtStartTime" name="StartTime" type="text" class="textBox"/>
                    </div>
                    <div class="add-article-row">
                        <label for="txtEndTime" class="label">
                            Reklama do:</label>
                        <input id="txtEndTime" name="EndTime" type="text" class="textBox"/>
                    </div>
                    <div class="add-article-row">
                        <label for="txtPosition" class="label">
                            Pozycja reklamy:</label>
                        <input id="txtPosition" name="Position" type="text" class="textBox"/>
                    </div>
                    <div class="add-article-row">
                        <label for="txtEmail" class="label">
                            Email:</label>
                        <input id="txtEmail" name="Email" type="text" class="textBox"/>
                    </div>
                    <div class="add-article-row">
                        <label for="IsBanner" class="label" class="textBox">
                            Banner:</label>
                        <input id="IsBanner" name="isBanner" type="checkbox"/>
                        <input name="isBanner" type="hidden"/>
                    </div>
                    <div class="add-article-row">
                        <span id="RecommendationMessage" class="message"></span>
                    </div>
                    <div class="add-article-row">
                        <input id="btnSubmitAd" type="submit" class="button" value="Sign up" />
                    </div>
                </fieldset>
                </form>
            </div>
        </div>
        <div id="EventSection">
            <div class="box">
                <h5>
                    Edycja wydarzenia</h5>
                <form id="frmEvent" action="<%= Url.Action("AddEvent", "CommingEvent") %>" method="post">
                <fieldset>
                    <input type="hidden" id="hidEventId" name="id"/>                    
                    <div class="add-article-row row">
                        <label for="txtEventName" class="label col-3 text-right">
                            Nazwa:</label>
                        <div class="col-9">
                            <input id="txtEventName" name="EventName" type="text" class="textBox" />
                            <span class="error"></span>
                        </div>
                    </div>
                    <div class="add-article-row row">
                        <label for="txtEventLink" class="label col-3 text-right">
                            Link do wydarzenia:</label>
                        <div class="col-9">
                            <input id="txtEventLink" name="EventLink" type="text" class="textBox" />
                            <span class="error"></span>
                        </div>
                    </div>
                    <div class="add-article-row row">
                        <label for="txtEventDate" class="label col-3 text-right">
                            Data rozpoczęcia:</label>
                        <div class="col-9">
                            <input id="txtEventDate" name="EventDate" type="text" class="textBox"/>
                        </div>
                    </div>
                    <div class="add-article-row row">
                        <label for="txtEventEndDate" class="label col-3 text-right">
                            Data zakończenia:</label>
                        <div class="col-9">
                            <input id="txtEventEndDate" name="EventEndDate" type="text" class="textBox"/>
                        </div>
                    </div>
                    <div class="add-article-row row">
                        <label for="IsOnline" class="label col-3 text-right">
                            Online:</label>
                        <div class="col-9">
                        <%= Html.CheckBox("IsOnline") %>               
                        </div>
                    </div>
                    <div id="localization">
                        <div class="add-article-row row">
                            <label for="txtEventCity" class="label col-3 text-right">
                                Miasto:</label>
                            <div class="col-9">
                                <input id="txtEventCity" name="EventCity" type="text" class="textBox"/>
                                <span class="error"></span>
                            </div>
                        </div>
                        <div class="add-article-row row">
                            <label for="txtEventPlace" class="label col-3 text-right">
                                Lokalizacja:</label>
                            <div class="col-9">
                                <input id="txtEventPlace" name="EventPlace" type="text" class="textBox"/>
                                <span class="error"></span>
                            </div>
                        </div>
                     </div>
                    <div class="add-article-row row">
                        <label for="txtEventLead" class="label col-3 text-right">
                            Opis:</label>
                        <div class="col-9">
                            <textarea id="txtEventLead" name="EventLead" cols="40" rows="5" maxlength="500" class="largeTextArea" style="width: 100% !important;">Brak opisu</textarea>                        
                            <span class="error"></span>
                        </div>
                    </div>
                    <div class="add-article-row row">
                        <label for="txtUserEmail" class="label col-3 text-right">
                            Email zgłaszającego:
                        </label>
                        <div class="col-9">
                            <input id="txtUserEmail" name="EventUserEmail" type="text" class="textBox" value="true" />
                            <span class="error"></span>
                        </div>
                    </div>
                    <% if (Model.IsCurrentUserAuthenticated && Model.CurrentUser != null &&
                           Model.CurrentUser.IsAdministrator())
                       { %>
                     <div class="add-article-row row">
                        <label for="IsApproved" class="label col-3 text-right">
                            Jest zatwierdzony:</label>
                        <div class="col-9">
                        <%= Html.CheckBox("IsApproved") %>               
                        </div>
                    </div>
                    <% } %>
                    <div class="add-article-row">
                        <span id="EventMessage" class="message"></span>
                    </div>
                    <div class="add-article-row">
                        <input id="btnSubmitEvent" type="submit" class="button" value="Sign up" />
                    </div>
                </fieldset>
                </form>
            </div>
        </div>
    </div>
</div>