var Contact =
{
    init: function () {
        jQuery.validator.addMethod(
            "checkHtml", function (value) {
                return $(value).length == 0;
            }
        );
        $('#frmContact').validate(
                                        {
                                            rules: {
                                                email: {
                                                    required: true,
                                                    email: true
                                                },
                                                name: {
                                                    required: true,
                                                    minlength: 3
                                                },
                                                message: {
                                                    required: true,
                                                    minlength: 16,
                                                    checkHtml: true
                                                }
                                            },
                                            messages: {
                                                email: {
                                                    required: 'Email nie może być pusty.',
                                                    email: 'Niepoprawny format adresu email.'
                                                },
                                                name: {
                                                    required: 'Imię nie może być puste.',
                                                    minlength: 'Imię nie może może być krótsze niż 3 znaki.'
                                                },
                                                message: {
                                                    required: 'Wiadomość nie może być pusta.',
                                                    minlength: 'Wiadomość nie może być krótsza niż 16 znaków.',
                                                    checkHtml: 'Pole nie może zawierać HTML.'
                                                }
                                            },
                                            submitHandler: function (form) {
                                                var options = {
                                                    dataType: 'json',
                                                    beforeSubmit: function (values, form, options) {
                                                        $('#contactMessage').text('').css('color', '').hide();
                                                        $U.disableInputs('#frmContact', true);
                                                        $U.showProgress('Wysyłanie...', '#btnContactSubmit');
                                                    },
                                                    success: function (result) {
                                                        $U.disableInputs('#frmContact', false);
                                                        $U.hideProgress();

                                                        if (result.isSuccessful) {
                                                            $('#txtContactEmail').val('');
                                                            $('#txtContactName').val('');
                                                            $('#txtContactMessage').val('');

                                                            $('#contactMessage').text('Dziękujemy za kontakt. Skontaktujemy się wkrótce.').css('color', '').show();
                                                        }
                                                        else {
                                                            $('#contactMessage').text(result.errorMessage).css('color', '#ff0000').show();
                                                        }
                                                    },
                                                    error: function (result) {
                                                        $U.disableInputs('#frmContact', false);
                                                        $U.hideProgress();
                                                        $('#contactMessage').text('Wprowadzone dane są nieprawidłowe. Popraw powyższe pola i spróbuj ponownie.').css('color', '#ff0000').show();
                                                    }
                                                };

                                                $(form).ajaxSubmit(options);
                                                return false;
                                            },
                                            errorPlacement: function (error, element) {
                                                element.css('border-color', '#CE0000');
                                                element.next('span.error').text(error.text()).css('display', 'block');
                                            },
                                            highlight: function (element, errorClass) {
                                                $(element).next('span.error').show();
                                            },
                                            unhighlight: function (element, errorClass) {
                                                $(element).next('span.error').hide();
                                            }
                                        }
                                    );

        $U.focus('txtContactEmail');
    },

    dispose: function () {
    }
};