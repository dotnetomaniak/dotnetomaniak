var Membership =
{
    _isLoggedIn: false,
    _logOutUrl: '',
    _minUserNameLength: 4,
    _minPasswordLength: 4,
    _redirectToPrevious: false,
    _centerMeTimer: null,
    _randomCaptcha: 0,

    get_captcha: function () {
        return Membership._randomCaptcha;
    },

    set_captcha: function (value) {
        Membership._randomCaptcha = value;
    },

    get_isLoggedIn: function () {
        return Membership._isLoggedIn;
    },

    set_isLoggedIn: function (value) {
        Membership._isLoggedIn = value;
    },

    set_logOutUrl: function (value) {
        Membership._logOutUrl = value;
    },

    set_minPasswordLength: function (value) {
        Membership._minPasswordLength = value;
    },

    init: function () {
        $.validator.addMethod('userName',

                                        function (value, element) {
                                            return /^([a-zA-Z])[a-zA-Z_-]*[\w_-]*[\S]$|^([a-zA-Z])[0-9_-]*[\S]$|^[a-zA-Z]*[\S]$/.test(value);
                                        },
                                        'User name must be alphanumeric characters which starts with alphabet and can only contains special characters dash and underscore.'
                            );

        $('#topUserTabs > ul').tabs(
                                        defaults = {
                                            navClass: 'sidebar-tabs-nav',
                                            selectedClass: 'sidebar-tabs-selected',
                                            unselectClass: 'sidebar-tabs-nav-item',
                                            panelClass: 'sidebar-tabs-panel',
                                            hideClass: 'sidebar-tabs-hide'
                                        }
                                    );
        $('#topUserTabs').show();

        $('#lnkLogin').click(
                                    function () {
                                        Membership.showLogin(false);
                                    }
                             );

        $('#lnkSignup').click(
                                    function () {
                                        Membership.showSignUp();
                                    }
                             );

        $('#lnkChangePassword').click(
                                            function () {
                                                Membership.showChangePassword();
                                            }
                                        );

        $('#lnkLogout').click(
                                    function () {
                                        Membership.logout();
                                    }
                             );

        $('#membershipBox').keydown(
                                            function (e) {
                                                if (e.keyCode === 27) // EscapeKey
                                                {
                                                    Membership._hide(false);
                                                }
                                            }
                                        );

        $('#membershipClose').click(function () { Membership._hide(false); });

        $('#frmOpenIdLogin').validate(
                                        {
                                            rules: {
                                                identifier: 'required'
                                            },
                                            messages:
                                                    {
                                                        identifier: 'Open ID cannot be blank.'
                                                    },
                                            errorPlacement: onErrorPlacement,
                                            highlight: onHighlight,
                                            unhighlight: onUnhighlight
                                        }
                                    );

        $('#frmLogin').validate(
                                    {
                                        rules: {
                                            userName: 'required',
                                            password: 'required'
                                        },
                                        messages:
                                                {
                                                    userName: 'User name cannot be blank.',
                                                    password: 'Password cannot be blank.'
                                                },
                                        submitHandler: function (form) {
                                            var options = {
                                                dataType: 'json',
                                                beforeSubmit: function () {
                                                    $('#loginMessage').hide().text('').css('color', '');

                                                    $U.disableInputs('#loginSection', true);
                                                    $U.showProgress('Authenticating...', '#chkLoginRememberMe');
                                                },
                                                success: function (result) {
                                                    $U.disableInputs('#loginSection', false);
                                                    $U.hideProgress();

                                                    if (result.isSuccessful) {
                                                        Membership._hide(true);
                                                        window.location.reload();
                                                    }
                                                    else {
                                                        $('#loginMessage').text(result.errorMessage).css({ color: '#ff0000', display: 'block' });
                                                        $U.focus('txtLoginUserName');
                                                    }
                                                }
                                            };

                                            $(form).ajaxSubmit(options);
                                            return false;
                                        },
                                        errorPlacement: onErrorPlacement,
                                        highlight: onHighlight,
                                        unhighlight: onUnhighlight
                                    }
                                );

        $('#frmForgotPassword').validate(
                                            {
                                                rules: {
                                                    email: {
                                                        required: true,
                                                        email: true
                                                    }
                                                },
                                                messages: {
                                                    email: {
                                                        required: 'Email cannot be blank.',
                                                        email: 'Invalid email address format.'
                                                    }
                                                },
                                                submitHandler: function (form) {
                                                    var options = {
                                                        dataType: 'json',
                                                        beforeSubmit: function () {
                                                            $U.disableInputs('#loginSection', true);
                                                            $U.showProgress('Verifying...', '#txtForgotEmail');
                                                        },
                                                        success: function (result) {
                                                            $U.disableInputs('#loginSection', false);
                                                            $U.hideProgress();

                                                            if (result.isSuccessful) {
                                                                $U.messageBox('Password Sent', 'You new password is successfully sent to \r\n your email address.', false, function () {
                                                                    Membership._hide(true);
                                                                }
                                                                                              );
                                                            }
                                                            else {
                                                                $('#forgotPasswordMessage').text(result.errorMessage).css({ color: '#ff0000', display: 'block' });
                                                                $U.focus('txtForgotEmail');
                                                            }
                                                        }
                                                    };

                                                    $(form).ajaxSubmit(options);
                                                    return false;
                                                },
                                                errorPlacement: onErrorPlacement,
                                                highlight: onHighlight,
                                                unhighlight: onUnhighlight
                                            }
                                        );

        $('#frmChangePassword').validate(
                                            {
                                                rules: {
                                                    oldPassword: 'required',
                                                    newPassword: {
                                                        required: true,
                                                        minlength: Membership._minPasswordLength
                                                    },
                                                    confirmPassword: {
                                                        required: true,
                                                        equalTo: '#txtChangeNewPassword'
                                                    }
                                                },
                                                messages: {
                                                    oldPassword: 'Old password cannot be blank',
                                                    newPassword: {
                                                        required: 'New password cannot be blank',
                                                        minlength: 'New password cannot be less than ' + Membership._minPasswordLength + 'character.'
                                                    },
                                                    confirmPassword: {
                                                        required: 'Confirm password cannot be blank',
                                                        equalTo: 'Confirm password must match new password.'
                                                    }
                                                },
                                                submitHandler: function (form) {
                                                    var options = {
                                                        dataType: 'json',
                                                        beforeSubmit: function () {
                                                            $('#changePasswordMessage').hide().text('Changing...').css('color', '');
                                                            $U.disableInputs('#changePasswordSection', true);
                                                            $U.showProgress('Changing...', '#txtChangenConfirmPassword');
                                                        },
                                                        success: function (result) {
                                                            $U.disableInputs('#changePasswordSection', false);
                                                            $U.hideProgress();

                                                            if (result.isSuccessful) {
                                                                $U.messageBox('Password Changed', 'Your password is successfully changed.', false, function () {
                                                                    Membership._hide(true);
                                                                }
                                                                                             );
                                                            }
                                                            else {
                                                                $('#changePasswordMessage').text(result.errorMessage).css({ color: '#ff0000', display: 'block' });
                                                            }
                                                        }
                                                    };

                                                    $(form).ajaxSubmit(options);
                                                    return false;
                                                },
                                                errorPlacement: onErrorPlacement,
                                                highlight: onHighlight,
                                                unhighlight: onUnhighlight
                                            }
                                        );

        $('#frmSignup').validate(
                                    {
                                        rules: {
                                            userName: {
                                                required: true,
                                                minlength: Membership._minUserNameLength,
                                                userName: true
                                            },
                                            password: {
                                                required: true,
                                                minlength: Membership._minPasswordLength
                                            },
                                            email: {
                                                required: true,
                                                email: true
                                            },
                                            captcha: {
                                                required: true,
                                                captcha: true
                                            }
                                        },
                                        messages: {
                                            userName: {
                                                required: 'User name cannot be blank.',
                                                minlength: 'User name cannot be less than ' + Membership._minUserNameLength + 'character.'
                                            },
                                            password: {
                                                required: 'Password cannot be blank.',
                                                minlength: 'Password cannot be less than ' + Membership._minPasswordLength + 'character.'
                                            },
                                            email: {
                                                required: 'Email cannot be blank.',
                                                email: 'Invalid email address format.'
                                            },
                                            captcha: {
                                                required: 'Pole CAPTCHA jest wymagane'
                                            },
                                        },
                                        submitHandler: function (form) {
                                            var options = {
                                                dataType: 'json',
                                                beforeSubmit: function () {
                                                    $('#signupMessage').hide().text('Signing up...').css('color', '');

                                                    $U.disableInputs('#signupSection', true);
                                                    $U.showProgress('Signing up...', '#txtSignupEmail');
                                                },
                                                success: function (result) {
                                                    $U.disableInputs('#signupSection', false);
                                                    $U.hideProgress();

                                                    if (result.isSuccessful) {
                                                        $U.messageBox('Account Created', 'Your account is succcessfully created. Please click the link in registration email to activate your account.', false, function () {
                                                            Membership._hide(true);
                                                            window.location.reload();
                                                        }
                                                                                     );
                                                    }
                                                    else {
                                                        $('#signupMessage').text(result.errorMessage).css({ color: '#ff0000', display: 'block' });
                                                    }
                                                }
                                            };

                                            $(form).ajaxSubmit(options);
                                            return false;
                                        },
                                        errorPlacement: onErrorPlacement,
                                        highlight: onHighlight,
                                        unhighlight: onUnhighlight
                                    }
                                );

        $('#lnkChangeEmail').click(
                                        function () {
                                            $('#emailViewSection').hide();
                                            $('#emailEditSection').show();
                                            $U.focus('txtChangeEmail');
                                        }
                                    );

        $('#lnkCancelEmail').click(
                                        function () {
                                            $('#emailEditSection').hide();
                                            $('#emailViewSection').show();
                                        }
                                    );

        $('#frmChangeEmail').validate(
                                        {
                                            rules: {
                                                email: {
                                                    required: true,
                                                    email: true
                                                }
                                            },
                                            messages: {
                                                email: {
                                                    required: 'Email cannot be blank.',
                                                    email: 'Invalid email address format.'
                                                }
                                            },
                                            submitHandler: function (form) {
                                                var options = {
                                                    dataType: 'json',
                                                    beforeSubmit: function () {
                                                        $U.disableInputs('#emailEditSection', true);
                                                        $U.showProgress('Updating email...', '#btnChangeEmail');
                                                    },
                                                    success: function (result) {
                                                        $U.disableInputs('#emailEditSection', false);
                                                        $U.hideProgress();

                                                        if (result.isSuccessful) {
                                                            $('#emailEditSection').hide();
                                                            $('#emailViewSection').show();
                                                            $('#spnEmail').text($('#txtChangeEmail').val());
                                                        }
                                                        else {
                                                            $U.focus('txtChangeEmail');
                                                            $('#spnChangeEmailError').text(result.errorMessage).show();
                                                        }
                                                    }
                                                };

                                                $(form).ajaxSubmit(options);
                                                return false;
                                            },
                                            errorPlacement: function (error, element) {
                                                $('#spnChangeEmailError').text(error.text());
                                            },
                                            highlight: function (element, errorClass) {
                                                $('#spnChangeEmailError').show();
                                            },
                                            unhighlight: function (element, errorClass) {
                                                $('#spnChangeEmailError').hide();
                                            }
                                        }
                                );

        function onErrorPlacement(error, element) {
            element.next('span.error').text(error.text());
        }

        function onHighlight(element, errorClass) {
            $(element).next('span.error').show();
        }

        function onUnhighlight(element, errorClass) {
            $(element).next('span.error').hide();
        }
    },

    dispose: function () {
        Membership._clearCenterMeTimer();

        $('#membershipBox').unbind();
        $('#membershipClose').unbind();
        $('#lnkLogin').unbind();
        $('#lnkSignup').unbind();
        $('#lnkChangePassword').unbind();
        $('#lnkLogout').unbind();
    },

    showLogin: function (redirectToPrevious) {
        Membership._redirectToPrevious = redirectToPrevious;

        $('#signupSection').hide();
        $('#changePasswordSection').hide();
        $('#loginSection').show();
        Membership._show();

        $U.focus('txtOpenId');
    },

    showChangePassword: function () {
        $('#signupSection').hide();
        $('#loginSection').hide();
        $('#changePasswordSection').show();
        Membership._show();

        $U.focus('txtChangeOldPassword');
    },

    showSignUp: function () {
        Membership._redirectToPrevious = false;

        $('#loginSection').hide();
        $('#changePasswordSection').hide();
        $('#signupSection').show();
        Membership._show();

        $U.focus('txtSignupUserName');
    },

    logout: function () {
        $.ajax(
                    {
                        url: Membership._logOutUrl,
                        type: 'POST',
                        dataType: 'json',
                        data: '__MVCASYNCPOST=true', // a fake param to fool iis for content-lenth
                        success: function (result) {
                            if (result.isSuccessful) {
                                window.location.reload();
                            }
                            else {
                                $U.alert('Error', result.errorMessage, true);
                            }
                        }
                    }
                );
    },

    _show: function () {
        $U.blockUI();

        var modal = $('#membershipBox');
        $U.center(modal);
        modal.fadeIn();

        Membership._centerMe();

        modal.find('input').each(
                                        function () {
                                            var input = $(this)[0];
                                            var type = input.type.toLowerCase();

                                            if ((type == 'text') || (type == 'file') || (type == 'password')) {
                                                input.value = '';
                                            }
                                            else if ((type == 'checkbox') || (type == 'radio')) {
                                                input.checked = false;
                                            }
                                        }
                                    );

        $('span.error').hide();
        $('span.message').hide();
    },

    _hide: function (isSuccessful) {
        $U.disableInputs('#membershipBox', false);
        $U.hideProgress();
        Membership._clearCenterMeTimer();

        $('#membershipBox').fadeOut('normal',
                                        function () {
                                            $U.unblockUI();

                                            if ((!isSuccessful) && (Membership._redirectToPrevious)) {
                                                window.location = (document.referrer.length > 0) ? document.referrer : '/';
                                            }
                                        }
                                     );
    },

    _centerMe: function () {
        var e = $('#membershipBox');

        $U.center(e);
        Membership._clearCenterMeTimer();
        Membership._centerMeTimer = setInterval(
                                                    function () {
                                                        Membership._centerMe();
                                                    },
                                                    2000
                                                );
    },

    _clearCenterMeTimer: function () {
        if (Membership._centerMeTimer != null) {
            clearInterval(Membership._centerMeTimer);
            Membership._centerMeTimer = null;
        }
    }
};