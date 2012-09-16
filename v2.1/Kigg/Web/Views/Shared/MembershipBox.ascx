<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseViewData>" %>
<% bool isAuthenticated = Model.IsCurrentUserAuthenticated; %>
<div id="membershipBox" class="modalBox">
    <div class="titleContainer">
        <div class="title"><%= Model.SiteTitle %></div>
        <div id="membershipClose" class="closeButton" title="Close"></div>
    </div>
    <div class="contentContainer">
        <%if (!isAuthenticated) %>
        <%{ %>
            <div id="loginSection">
                <div class="box">
                    <form id="frmOpenIdLogin" action="<%= Url.Action("OpenId", "Membership") %>" method="post">
                        <h3>Open ID Login</h3>
                        <p>
                            <label for="openid_identifier" class="label">Open ID:</label>
                            <input id="openid_identifier" name="identifier" type="text" class="openIDTextBox"/>
                            <span class="error"></span>
                        </p>
                        <p>
                            <label class="label"></label>
                            <label><input id="openid_RememberMe" name="rememberMe" type="checkbox" value="true"/>Zapamiętaj mnie na tym komputerze</label>
                        </p>
                        <p>
                            <label class="label"></label>
                            <input id="btnOpenId" type="submit" class="button" value="Zaloguj"/>
                        </p>
                    </form>
                </div>
                <div class="divider"></div>
                <div class="box">
                    <form id="frmLogin" action="<%= Url.Action("Login", "Membership") %>" method="post">
                        <h3>Logowanie standardowe</h3>
                        <p>
                            <label for="txtLoginUserName" class="label">Użytkownik:</label>
                            <input id="txtLoginUserName" name="userName" type="text" class="textBox"/>
                            <span class="error"></span>
                        </p>
                        <p>
                            <label for="txtLoginPassword" class="label">Hasło:</label>
                            <input id="txtLoginPassword" name="password" type="password" class="textBox"/>
                            <span class="error"></span>
                        </p>
                        <p>
                            <label class="label"></label>
                            <label><input id="chkLoginRememberMe" name="rememberMe" type="checkbox" value="true"/>Zapamiętaj mnie na tym komputerze</label>
                        </p>
                        <p>
                            <span id="loginMessage" class="message"></span>
                        </p>
                        <p>
                            <label class="label"></label>
                            <input id="btnLogin" type="submit" class="button" value="Zaloguj"/>
                        </p>
                    </form>
                </div>
                <div class="divider"></div>
                <div class="box">
                    <form id="frmForgotPassword" action="<%= Url.Action("ForgotPassword", "Membership") %>" method="post">
                        <h3>Zapomniane hasło?</h3>
                        <p>
                            <label for="txtForgotEmail" class="label">Email:</label>
                            <input id="txtForgotEmail" name="email" type="text" class="textBox"/>
                            <span class="error"></span>
                            <span class="info">(tylko dla zarejestrowanych użytkowników)</span>
                        </p>
                        <p>
                            <span id="forgotPasswordMessage" class="message"></span>
                        </p>
                        <p>
                            <label class="label"></label>
                            <input id="btnForgotPassword" type="submit" class="button" value="Wyślij hasło"/>
                        </p>
                    </form>
                </div>
                <%--
                <div class="divider"></div>
                <div>
                    <iframe src="https://dotnetshoutout.rpxnow.com/openid/embed?token_url=http://dotnetshoutout.com/RpxNow" scrolling="no" frameBorder="no" style="width:400px;height:240px"></iframe>
                </div>
                --%>
            </div>
        <%}%>
        <%if ((isAuthenticated) && (!Model.CurrentUser.IsOpenIDAccount())) %>
        <%{%>
            <div id="changePasswordSection">
                <div class="box">
                    <form id="frmChangePassword" action="<%= Url.Action("ChangePassword", "Membership") %>" method="post">
                        <h3>Zmień hasło</h3>
                        <p>
                            <label for="txtChangeOldPassword" class="label">Stare:</label>
                            <input id="txtChangeOldPassword" name="oldPassword" type="password" class="textBox"/>
                            <span class="error"></span>
                        </p>
                        <p>
                            <label for="txtChangeNewPassword" class="label">Nowe:</label>
                            <input id="txtChangeNewPassword" name="newPassword" type="password" class="textBox"/>
                            <span class="error"></span>
                        </p>
                        <p>
                            <label for="txtChangenConfirmPassword" class="label">Potwierdź:</label>
                            <input id="txtChangenConfirmPassword" name="confirmPassword" type="password" class="textBox"/>
                            <span class="error"></span>
                        </p>
                        <p>
                            <span id="changePasswordMessage" class="message"></span>
                        </p>
                        <p>
                            <label class="label"></label>
                            <input id="btnChangePassword" type="submit" class="button" value="Zmień hasło"/>
                        </p>
                    </form>
                </div>
            </div>
        <%}%>
        <%if (!isAuthenticated) %>
        <%{%>
            <div id="signupSection">
                <div class="box">
                    <form id="frmSignup" action="<%= Url.Action("Signup", "Membership") %>" method="post">
                        <h3>Zapisz się</h3>
                        <p>
                            <label for="txtSignupUserName" class="label">Użytkownik:</label>
                            <input id="txtSignupUserName" name="userName" type="text" class="textBox"/>
                            <span class="error"></span>
                        </p>
                        <p>
                            <label for="txtSignupPassword" class="label">Hasło:</label>
                            <input id="txtSignupPassword" name="password" type="password" class="textBox"/>
                            <span class="error"></span>
                        </p>
                        <p>
                            <label for="txtSignupEmail" class="label">Email:</label>
                            <input id="txtSignupEmail" name="email" type="text" class="textBox"/>
                            <span class="error"></span>
                            <span class="info">(Proszę wprowadź poprawny adres e-mail. Będzie potrzebny do aktywacji konta)</span>
                        </p>
                        <p>
                            <label for="txtCaptcha" class="label">Człowiek?</label>
                            <input id="txtCaptcha" type="text" class="textBox" name="captcha" />
                            <span class="error"></span>
                            <span class="info">Wprowadź wynik działania: <%= ViewData["Arg1"] %>+<%= ViewData["Arg2"] %></span>
                        </p>
                        <p>
                            <span id="signupMessage" class="message"></span>
                        </p>                        
                        <p>
                            <label class="label"></label>
                            <input id="btnSignup" type="submit" class="button" value="Zarejestruj"/>
                        </p>
                    </form>
                </div>
            </div>
        <%}%>
    </div>
</div>
