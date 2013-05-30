var Membership =
{
    _isLoggedIn: false,
    _logOutUrl: '',
    _minUserNameLength: 3,
    _minPasswordLength: 5,
    _redirectToPrevious: false,
    _centerMeTimer: null,
    _getStoryUrl: '',
    _MembershipBox: '#membershipBox',
    _StoryEditorBox: '#storyEditorBox',
    _Box:'',
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
    
    set_getStoryUrl: function (value) {
        Membership._getStoryUrl = value;
    },

    init: function () {
        $.validator.addMethod('userName',

                                                function (value, element) {
                                                    return /^([a-zA-Z])[a-zA-Z_-]*[\w_-]*[\S]$|^([a-zA-Z])[0-9_-]*[\S]$|^[a-zA-Z]*[\S]$/.test(value);
                                                },
                                                "Nazwa użytkownika musi składać się ze znaków alfanumerycznych (plus '-','_') i zaczynać się literą."
                                    );

        $('#lnkLogin').click(
                                    function () {
                                        Membership.showLogin(false);
                                    }
                             );

        $('#lostPassword').click(function () {
            Membership.showLostPassword(false);
        });
        
        $('#lnkSignup, #lnkSignup2').click(
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
                                                            Membership._hide(false, Membership._MembershipBox);
                                                        }
                                                    }
                                                );

        $('#membershipClose').bind('click', function () { Membership._hide(false,Membership._MembershipBox); });

        $('#frmOpenIdLogin').validate(
                                                {
                                                    rules: {
                                                        identifier: 'required'
                                                    },
                                                    messages:
                                                            {
                                                                identifier: 'Pole Open ID nie może być puste.'
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
                                                            userName: 'Nazwa użytkownika jest wymagana.',
                                                            password: 'Hasło jest wymagane.'
                                                        },
                                                submitHandler: function (form) {
                                                    var options = {
                                                        dataType: 'json',
                                                        beforeSubmit: function () {
                                                            $('#loginMessage').hide().text('').css('color', '');

                                                            $U.disableInputs('#loginSection', true);
                                                            $U.showProgress('Walidacja...', '#chkLoginRememberMe');
                                                        },
                                                        success: function (result) {
                                                            $U.disableInputs('#loginSection', false);
                                                            $U.hideProgress();

                                                            if (result.isSuccessful) {
                                                                Membership._hide(true,Membership._MembershipBox);
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
                                                                        required: 'Email nie może być pusty.',
                                                                        email: 'Niepoprawny adres e-mail.'
                                                                    }
                                                                },
                                                                submitHandler: function (form) {
                                                                    var options = {
                                                                        dataType: 'json',
                                                                        beforeSubmit: function () {
                                                                            $U.disableInputs('#loginSection', true);
                                                                            $U.showProgress('Weryfikacja...', '#txtForgotEmail');
                                                                        },
                                                                        success: function (result) {
                                                                            $U.disableInputs('#loginSection', false);
                                                                            $U.hideProgress();

                                                                            if (result.isSuccessful) {
                                                                                $U.messageBox('Hasło wysłane', 'Twoje nowe hasło zostało wysłane\r\n na twój adres e-mail.', false, function () {
                                                                                    Membership._hide(true,Membership._MembershipBox);
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
                                                            oldPassword: 'Stare hasło nie może być puste',
                                                            newPassword: {
                                                                required: 'Nowe hasło nie może być puste',
                                                                minlength: 'Nowe hasło nie może być krótsze niż ' + Membership._minPasswordLength + ' znaków.'
                                                            },
                                                            confirmPassword: {
                                                                required: 'Potwierdzenie hasła nie może być puste',
                                                                equalTo: 'Hasła muszą się zgadzać.'
                                                            }
                                                        },
                                                        submitHandler: function (form) {
                                                            var options = {
                                                                dataType: 'json',
                                                                beforeSubmit: function () {
                                                                    $('#changePasswordMessage').hide().text('Changing...').css('color', '');
                                                                    $U.disableInputs('#changePasswordSection', true);
                                                                    $U.showProgress('Zmiana...', '#txtChangenConfirmPassword');
                                                                },
                                                                success: function (result) {
                                                                    $U.disableInputs('#changePasswordSection', false);
                                                                    $U.hideProgress();

                                                                    if (result.isSuccessful) {
                                                                        $U.messageBox('Hasło zmienione', 'Twoje hasło zostało zmienione.', false, function () {
                                                                            Membership._hide(true,Membership._MembershipBox);
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
                                                    }
                                                },
                                                messages: {
                                                    userName: {
                                                        required: 'Nazwa użytkownika nie może być pusta.',
                                                        minlength: "Nazwa użytkownika nie może być krótsza niż " + Membership._minUserNameLength + " znaki/-ów."
                                                    },
                                                    password: {
                                                        required: 'Hasło nie może być puste.',
                                                        minlength: "Hasło nie może być krótsze niż " + Membership._minPasswordLength + " znaki/-ów."
                                                    },
                                                    email: {
                                                        required: 'Email nie może być pusty.',
                                                        email: 'Niepoprawny format adresu email.'
                                                    }
                                                },
                                                submitHandler: function (form) {
                                                    var options = {
                                                        dataType: 'json',
                                                        beforeSubmit: function () {
                                                            $('#signupMessage').hide().text('Rejestracja...').css('color', '');

                                                            $U.disableInputs('#signupSection', true);
                                                            $U.showProgress('Rejestracja...', '#txtSignupEmail');
                                                        },
                                                        success: function (result) {
                                                            $U.disableInputs('#signupSection', false);
                                                            $U.hideProgress();

                                                            if (result.isSuccessful) {
                                                                $U.messageBox("Konto utworzone", "Twoje konto zostało poprawnie utworzone. Proszę postępuj zgodnie z informacjami w mailu aby aktywować konto.", false, function () {
                                                                    Membership._hide(true,Membership._MembershipBox);
                                                                    window.location.reload();
                                                                });
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
                                                            required: 'E-mail nie może być pusty.',
                                                            email: 'Niepoprawny format adresu.'
                                                        }
                                                    },
                                                    submitHandler: function (form) {
                                                        var options = {
                                                            dataType: 'json',
                                                            beforeSubmit: function () {
                                                                $U.disableInputs('#emailEditSection', true);
                                                                $U.showProgress('Aktualizacja adresu...', '#btnChangeEmail');
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
            element.css('border-color', '#CE0000');
            element.next('span.error').text(error.text()).css('display', 'block').css('width', parseInt(element.css('width')) - 31);
        }

        function onHighlight(element, errorClass) {
            $(element).next('span.error').show();
        }

        function onUnhighlight(element, errorClass) {
            $(element).next('span.error').hide();
        }
        
        $('#storyEditorBox').keydown(
            function (e) {
                if (e.keyCode === 27) // EscapeKey
                {
                    Membership._hide(false, Membership._StoryEditorBox);
                }
            }
        );

        $('#storyEditorClose').click(function () { Membership._hide(false, Membership._StoryEditorBox); });
        debugger;
        $('#frmStoryUpdate').validate(
            {
                rules: {
                    name: 'required',
                    createdAt: 'required',
                    description: {
                        required: true,
                        rangelength: [8, 2048]
                    },
                    category: 'required',
                    tags: 'required'
                },
                messages: {
                    name: 'Nazwa nie może być pusta.',
                    createdAt: 'Pole Utworzono nie może być puste.',
                    title: {
                        required: 'Tytuł nie może być pusty.',
                        maxlength: 'Tytuł nie może być dłuższy niż 256 znaków.'
                    },
                    description: {
                        required: 'Opis nie może być pusty.',
                        rangelength: 'Opis musi zawierać się pomiędzy 8 a 2048 znakami.'
                    },
                    category: 'Kategoria musi być wybrana.',
                    tags: 'Podaj przynajmniej jeden tag dla artykułu.'
                },
                submitHandler: function (form) {
                    var options = {
                        dataType: 'json',
                        beforeSubmit: function (values, form, options) {
                            $('#updateStoryMessage').text('').css('color', '').hide();
                            $U.disableInputs('#frmStoryUpdate', true);
                            $U.showProgress('Aktualizacja wpisu...', '#btnUpdateStory');
                        },
                        success: function (result) {
                            $U.disableInputs('#frmStoryUpdate', false);
                            $U.hideProgress();

                            if (result.isSuccessful) {
                                Membership._hide(true, Membership._StoryEditorBox);
                            } else {
                                $('#updateStoryMessage').text(result.errorMessage).css({ color: '#ff0000', display: 'block' });
                            }
                        }
                    };

                    $(form).ajaxSubmit(options);
                },
                errorPlacement: function (error, element) {
                    element.parents('p:first').find('span.error').text(error.text());
                },
                highlight: function (element, errorClass) {
                    $(element).parents('p:first').find('span.error').show();
                },
                unhighlight: function (element, errorClass) {
                    $(element).parents('p:first').find('span.error').hide();
                }
            }
        );
    },

    dispose: function () {
        Membership._clearCenterMeTimer();

        $('#membershipBox').unbind();
        $('#membershipClose').unbind();
        $('#lnkLogin').unbind();
        $('#lnkSignup').unbind();
        $('#lnkSignup2').unbind();
        $('#lnkChangePassword').unbind();
        $('#lnkLogout').unbind();
        $('#lostPasswordSection').unbind();
        $('#storyEditorBox').unbind();
    },

    showLostPassword: function (redirectToPrevious) {
        Membership._redirectToPrevious = redirectToPrevious;
        $('.contentContainer > div').hide();
        $('#lostPasswordSection').show();
        Membership._show(Membership._MembershipBox);
    },

    showLogin: function (redirectToPrevious) {
        Membership._redirectToPrevious = redirectToPrevious;

        $('.contentContainer > div').hide();
        $('#loginSection').show();
        Membership._show(Membership._MembershipBox);

        $U.focus('txtOpenId');
    },

    showChangePassword: function () {
        $('.contentContainer > div').hide();
        $('#changePasswordSection').show();
        Membership._show(Membership._MembershipBox);

        $U.focus('txtChangeOldPassword');
    },

    showSignUp: function () {
        Membership._redirectToPrevious = false;

        $('.contentContainer > div').hide();
        $('#signupSection').show();
        $('#changePasswordSection').show();
        Membership._show(Membership._MembershipBox);

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

    _show: function (box) {
        $U.blockUI();

        var modal = $(box);
        $U.center(modal);
        modal.fadeIn();

        Membership._centerMe(box);

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

    _hide: function (isSuccessful,box) {
        //tb_remove();
        $U.disableInputs(box, false);
        $U.hideProgress();
        Membership._clearCenterMeTimer();

        $(box).fadeOut('normal',
                                        function () {
                                            $U.unblockUI();

                                            if ((!isSuccessful) && (Membership._redirectToPrevious)) {
                                                window.location = (document.referrer.length > 0) ? document.referrer : '/';
                                            }
                                        }
                                     );
    },

    _centerMe: function (box) {
        var e = $(box);

        $U.center(e);
        Membership._clearCenterMeTimer();
        Membership._centerMeTimer = setInterval(
                                                    function () {
                                                        Membership._centerMe(box);
                                                    },
                                                    2000
                                                );
    },

    _clearCenterMeTimer: function () {
        if (Membership._centerMeTimer != null) {
            clearInterval(Membership._centerMeTimer);
            Membership._centerMeTimer = null;
        }
    },
    
    editStory: function(storyId) {
        var data = 'id=' + encodeURIComponent(storyId);

        $U.disableInputs('#frmStoryUpdate', true);

        $.ajax(
            {
                url: Membership._getStoryUrl,
                type: 'POST',
                dataType: 'json',
                data: data,
                beforeSend: function() {
                    $U.showProgress('Wczytywanie artykułu...');
                },
                success: function(result) {
                    $U.hideProgress();
                    $U.disableInputs('#frmStoryUpdate', false);

                    if (result.errorMessage) {
                        $U.messageBox('Error', result.errorMessage, true);
                    } else {
                        $U.blockUI();

                        var modal = $('#storyEditorBox');

                        $('span.error').hide();
                        $('span.message').hide();

                        $('#hidStoryId').val(result.id);
                        $('#txtStoryName').val(result.name);
                        $('#txtStoryCreatedAt').val(result.createdAt);
                        $('#txtStoryTitle').val(result.title);
                        $('#txtStoryDescription').val(result.description);
                        $('#txtStoryTags').val(result.tags);

                        $('#frmStoryUpdate input[type=radio]').each(
                            function() {
                                var rdo = $(this)[0];

                                if (rdo.value == result.category) {
                                    rdo.checked = true;
                                }
                            }
                        );
                        $U.center(modal);
                        modal.fadeIn();
                        Membership._centerMe('#storyEditorBox');
                    }
                }
            }
        );
    }
};