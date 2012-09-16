var Contact =
{
    init : function()
    {
        $('#frmContact').validate(
                                        {
                                            rules : {
                                                        email : {
                                                                    required: true,
                                                                    email: true
                                                                },
                                                        name : {
                                                                    required: true,
                                                                    minlength : 4
                                                                },
                                                        message :   {
                                                                        required: true,
                                                                        minlength : 16
                                                                    }
                                                    },
                                            messages : {
                                                            email : {
                                                                        required: 'Email cannot be blank.',
                                                                        email: 'Invalid email address format.'
                                                                    },
                                                            name :  {
                                                                        required: 'Name cannot be blank.',
                                                                        minlength: 'Name cannot be less than 4 character.'
                                                                    },
                                                            message :   {
                                                                            required: 'Message cannot be blank.',
                                                                            minlength: 'Message cannot be less than 16 character.'
                                                                        }
                                                        },
                                            submitHandler : function(form)
                                            {
                                                var options =   {
                                                                    dataType : 'json',
                                                                    beforeSubmit : function(values, form, options)
                                                                    {
                                                                        $('#contactMessage').text('').css('color', '').hide();
                                                                        $U.disableInputs('#frmContact', true);
                                                                        $U.showProgress('Submitting Feedback...', '#btnContactSubmit');
                                                                    },
                                                                    success : function(result)
                                                                    {
                                                                        $U.disableInputs('#frmContact', false);
                                                                        $U.hideProgress();

                                                                        if (result.isSuccessful)
                                                                        {
                                                                            $('#txtContactEmail').val('');
                                                                            $('#txtContactName').val('');
                                                                            $('#txtContactMessage').val('');

                                                                            $('#contactMessage').text('Thank your for contacting us. We will soon get in touch with you.').css('color', '').show();
                                                                        }
                                                                        else
                                                                        {
                                                                            $('#contactMessage').text(result.errorMessage).css('color', '#ff0000').show();
                                                                        }
                                                                    }
                                                                };

                                                $(form).ajaxSubmit(options);
                                                return false;
                                            },
                                            errorPlacement : function(error, element)
                                            {
                                                element.next('span.error').text(error.text());
                                            },
                                            highlight : function(element, errorClass)
                                            {
                                                $(element).next('span.error').show();
                                            },
                                            unhighlight : function(element, errorClass)
                                            {
                                                $(element).next('span.error').hide();
                                            }
                                        }
                                    );

        $U.focus('txtContactEmail');
    },

    dispose : function()
    {
    }
};