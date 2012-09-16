var Comment =
{
    _isPreviewOpen : true,
    _captchaEnabled : false,

    set_captchaEnabled : function(value)
    {
        Comment._captchaEnabled = value;
    },

    init : function()
    {
        mytabs.init();

        if ($('#txtCommentBody').length > 0)
        {
            $('#lnkCommentPreview').click (
                                                function()
                                                {
                                                    if (Comment._isPreviewOpen)
                                                    {
                                                        $('#commentPreview').slideUp('normal');
                                                        $('#lnkCommentPreview').text('show Preview');
                                                        Comment._isPreviewOpen = false;
                                                    }
                                                    else
                                                    {
                                                        $('#commentPreview').slideDown('normal');
                                                        $('#lnkCommentPreview').text('hide Preview');
                                                        Comment._isPreviewOpen = true;
                                                    }
                                                }
                                            );

            RichEditor.create($('#txtCommentBody'), $('#hidbody'), $('#commentPreview'));
        }

        $('#frmCommentSubmit').validate(
                                        {
                                            rules : {
                                                        commentBody :  {
                                                                            required: true,
                                                                            maxlength: 2048
                                                                        }
                                                    },
                                            messages : {
                                                            commentBody :  {
                                                                                required: 'Comment cannot be blank.',
                                                                                maxlength: 'Comment cannot be more than 2048 character.'
                                                                            }
                                                        },
                                            submitHandler : function(form)
                                            {
                                                var options =   {
                                                                    dataType : 'json',
                                                                    beforeSubmit : function(values, form, options)
                                                                    {
                                                                        if (!Membership.get_isLoggedIn())
                                                                        {
                                                                            Membership.showLogin(false);
                                                                            return false;
                                                                        }

                                                                        $('#commentMessage').text('').css('color', '').hide();
                                                                        $U.disableInputs('#frmCommentSubmit', true);
                                                                        $U.showProgress('Submitting Comment...', '#btnCommentSubmit');
                                                                    },
                                                                    success : function(result)
                                                                    {
                                                                        $U.disableInputs('#frmCommentSubmit', false);
                                                                        $U.hideProgress();

                                                                        if (result.isSuccessful)
                                                                        {
                                                                            $('#txtCommentBody').val('');
                                                                            $('#commentMessage').text('Thank your for submitting your comment. It will appear here shortly.').css('color', '').show();
                                                                        }
                                                                        else
                                                                        {
                                                                            $('#commentMessage').text(result.errorMessage).css('color', '#ff0000').show();
                                                                        }
                                                                    }
                                                                };

                                                $(form).ajaxSubmit(options);
                                                return false;
                                            },
                                            errorPlacement : function(error, element)
                                            {
                                                findErrorElement(element).text(error.text());
                                            },
                                            highlight : function(element, errorClass)
                                            {
                                                findErrorElement($(element)).show();
                                            },
                                            unhighlight : function(element, errorClass)
                                            {
                                                findErrorElement($(element)).hide();
                                            }
                                        }
                                    );

        function findErrorElement(element)
        {
            var t = element.parents('p:first').find('span.error');

            if ((t.length == 0) && (Membership.get_isLoggedIn()) && (Comment._captchaEnabled))
            {
                t = element.parents('div#recaptcha_area').find('span.error');
            }

            return t;
        }

        if ((Membership.get_isLoggedIn()) && (Comment._captchaEnabled))
        {
            //reCAPTCHA might not be available, so we have to poll for its availability
            var timerId = setInterval(
                                        function()
                                        {
                                            if (($('#recaptcha_area').length > 0) && ($('#recaptcha_response_field').length > 0))
                                            {
                                                $('<span class=\"error\"></span>').appendTo('#recaptcha_area');
                                                $('#recaptcha_response_field').attr('title', 'Captcha verification words cannot be blank.')
                                                                              .rules('add', { required: true });
                                                window.clearInterval(timerId);
                                            }
                                        },
                                        500
                                    );
        }

        $('#lnkCommentLogin').click  (
                                        function()
                                        {
                                            Membership.showLogin(false);
                                        }
                                    );

        $('#lnkCommentSignup').click  (
                                        function()
                                        {
                                            Membership.showSignUp();
                                        }
                                    );
    },

    dispose : function()
    {
        $('#lnkCommentLogin').unbind();
        $('#lnkCommentSignup').unbind();
        $('#lnkCommentPreview').unbind();
        $('#txtCommentBody').unbind();
    }
};