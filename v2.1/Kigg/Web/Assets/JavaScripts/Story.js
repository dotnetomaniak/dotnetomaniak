var Story =
{
    _retrieveStoryUrl : '',
    _suggestTagsUrl : '',
    _clickUrl : '',
    _promoteUrl : '',
    _demoteUrl : '',
    _markAsSpamUrl : '',
    _lastRetrievedUrl : '',
    _autoDiscover : false,
    _captchaEnabled : false,
    _isPreviewOpen : true,

    set_retrieveStoryUrl : function(value)
    {
        Story._retrieveStoryUrl = value;
    },

    set_suggestTagsUrl : function(value)
    {
        Story._suggestTagsUrl = value;
    },

    set_clickUrl : function(value)
    {
        Story._clickUrl = value;
    },

    set_promoteUrl : function(value)
    {
        Story._promoteUrl = value;
    },

    set_demoteUrl : function(value)
    {
        Story._demoteUrl = value;
    },

    set_markAsSpamUrl : function(value)
    {
        Story._markAsSpamUrl = value;
    },

    set_autoDiscover : function(value)
    {
        Story._autoDiscover = value;
    },

    set_captchaEnabled : function(value)
    {
        Story._captchaEnabled = value;
    },

    init : function()
    {
        $('#lnkStoryPreview').click (
                                            function()
                                            {
                                                if (Story._isPreviewOpen)
                                                {
                                                    $('#storyPreview').slideUp('normal');
                                                    $('#lnkStoryPreview').text('show preview');
                                                    Story._isPreviewOpen = false;
                                                }
                                                else
                                                {
                                                    $('#storyPreview').slideDown('normal');
                                                    $('#lnkStoryPreview').text('hide preview');
                                                    Story._isPreviewOpen = true;
                                                }
                                            }
                                        );

        RichEditor.create($('#txtStoryDescription'), $('#hidDescription'), $('#storyPreview'));

        $('#frmStorySubmit').validate(
                                        {
                                            rules : {
                                                        url :   {
                                                                    required: true,
                                                                    url: true
                                                                },
                                                        title : {
                                                                    required: true,
                                                                    maxlength: 256
                                                                },
                                                        storyDescription :      {
                                                                                    required: true,
                                                                                    rangelength: [8, 2048]
                                                                                },
                                                        category : 'required'
                                                    },
                                            messages : {
                                                            url :   {
                                                                        required: 'Url cannot be blank.',
                                                                        url: 'Invalid url format.'
                                                                    },
                                                            title : {
                                                                        required: 'Title cannot be blank.',
                                                                        maxlength: 'Title cannot be more than 256 character.'
                                                                    },
                                                            storyDescription :   {
                                                                                    required: 'Description cannot be blank.',
                                                                                    rangelength: 'Description must be between 8 to 2048 character.'
                                                                                 },
                                                            category : 'Category must be selected.'
                                                        },
                                            submitHandler : function(form)
                                            {
                                                var options =   {
                                                                    dataType : 'json',
                                                                    beforeSubmit : function(values, form, options)
                                                                    {
                                                                        $('#storyMessage').text('').css('color', '').hide();
                                                                        $U.disableInputs('#frmStorySubmit', true);
                                                                        $U.showProgress('Submitting Story...', '#btnStorySubmit');
                                                                    },
                                                                    success : function(result)
                                                                    {
                                                                        $U.disableInputs('#frmStorySubmit', false);
                                                                        $U.hideProgress();

                                                                        if (result.isSuccessful)
                                                                        {
                                                                            window.location = result.url;
                                                                        }
                                                                        else
                                                                        {
                                                                            html = result.errorMessage;

                                                                            if (result.url)
                                                                            {
                                                                                html = 'Story with the same url already exists. <a href=\"' + result.url +'\">Click here</a> to view the story.';
                                                                            }

                                                                            $('#storyMessage').html(html).css('color', '#ff0000').show();
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

            if ((t.length == 0) && (Membership.get_isLoggedIn()) && (Story._captchaEnabled))
            {
                t = element.parents('div#recaptcha_area').find('span.error');
            }

            return t;
        }

        if ((Membership.get_isLoggedIn()) && (Story._captchaEnabled))
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

        var txtStoryUrl = $('#txtStoryUrl');

        txtStoryUrl.blur(
                            function()
                            {
                                Story._urlChanged();
                            }
                        );

        Story._lastRetrievedUrl = txtStoryUrl.val();

        $('#txtStoryTags').autocomplete (
                                            {
                                                url:Story._suggestTagsUrl,
                                                multiple:true,
                                                max:10,
                                                scrollHeight:330,
                                                parse: function(data)
                                                {
                                                    return $.map(
                                                                    eval(data), function(row)
                                                                                {
                                                                                    return {
                                                                                                data: row,
                                                                                                value: row,
                                                                                                result: row
                                                                                            }
                                                                                }
                                                                );
                                                },
                                                formatItem: function(item)
                                                {
                                                    return item;
                                                }
                                            }
                                        );

        if (!Membership.get_isLoggedIn())
        {
            Membership.showLogin(true);
        }
        else
        {
            $U.focus('txtStoryUrl');
        }
    },

    dispose : function()
    {
        $('#lnkStoryPreview').unbind();
        $('#txtStoryDescription').unbind();
        $('#txtStoryUrl').unbind();
    },

    click : function(storyId)
    {
        var data = 'id=' + encodeURIComponent(storyId);

        $.ajax  (
                    {
                        url : Story._clickUrl,
                        type : 'POST',
                        dataType : 'json',
                        data : data
                    }
                );
    },

    promote : function(storyId)
    {
        if (!Membership.get_isLoggedIn())
        {
            Membership.showLogin(false);
            return;
        }

        var data = 'id=' + encodeURIComponent(storyId);

        $.ajax  (
                    {
                        url : Story._promoteUrl,
                        type : 'POST',
                        dataType : 'json',
                        data : data,
                        beforeSend :    function()
                                        {
                                            $('#a-k-' + storyId).hide();
                                            $('#d-m-' + storyId).removeClass('inline').addClass('hide');
                                            $('#s-p-' + storyId).text('Wait...').show();
                                        },
                        success :   function(result)
                                    {
                                        $('#s-p-' + storyId).text('').hide();

                                        if (result.isSuccessful)
                                        {
                                            $('#s-p-' + storyId).parent().removeClass('do').addClass('undo');
                                            $('#s-c-' + storyId).hide().text(result.votes).fadeIn();
                                            $('#a-u-' + storyId).show();
                                        }
                                        else
                                        {
                                            $('#a-k-' + storyId).show();
                                            $('#d-m-' + storyId).show();
                                            $('#d-m-' + storyId).removeClass('hide').addClass('inline');
                                            $U.messageBox('Error', result.errorMessage, true);
                                        }
                                    }
                    }
                );
    },

    demote : function(storyId)
    {
        if (!Membership.get_isLoggedIn())
        {
            Membership.showLogin(false);
            return;
        }

        var data = 'id=' + encodeURIComponent(storyId);

        $.ajax  (
                    {
                        url : Story._demoteUrl,
                        type : 'POST',
                        dataType : 'json',
                        data : data,
                        beforeSend :    function()
                                        {
                                            $('#a-u-' + storyId).hide();
                                            $('#s-p-' + storyId).text('Wait...').show();
                                        },
                        success :   function(result)
                                    {
                                        $('#s-p-' + storyId).text('').hide();

                                        if (result.isSuccessful)
                                        {
                                            $('#s-p-' + storyId).parent().removeClass('undo').addClass('do');
                                            $('#s-c-' + storyId).hide().text(result.votes).fadeIn();
                                            $('#a-k-' + storyId).show();
                                            $('#d-m-' + storyId).removeClass('hide').addClass('inline');
                                        }
                                        else
                                        {
                                            $('#a-u-' + storyId).show();
                                            $U.messageBox('Error', result.errorMessage, true);
                                        }
                                    }
                    }
                );
    },

    markAsSpam : function(storyId)
    {
        if (!Membership.get_isLoggedIn())
        {
            Membership.showLogin(false);
            return;
        }

        function mark()
        {
            var data = 'id=' + encodeURIComponent(storyId);

            $.ajax  (
                        {
                            url : Story._markAsSpamUrl,
                            type : 'POST',
                            dataType : 'json',
                            data : data,
                            beforeSend :    function()
                                            {
                                                $('#a-k-' + storyId).hide();
                                                $('#d-m-' + storyId).removeClass('inline').addClass('hide');
                                                $('#s-p-' + storyId).text('Wait...').show();
                                            },
                            success :   function(result)
                                        {
                                            $('#s-p-' + storyId).text('').hide();

                                            if (result.isSuccessful)
                                            {
                                                $('#s-p-' + storyId).parent().removeClass('do').addClass('none');
                                                $('#s-s-' + storyId).show();
                                                $('#t-' + storyId).fadeOut('normal', function(){$(this).remove();});
                                            }
                                            else
                                            {
                                                $('#a-k-' + storyId).show();
                                                $('#d-m-' + storyId).removeClass('hide').addClass('inline');
                                                $U.messageBox('Error', result.errorMessage, true);
                                            }
                                        }
                        }
                    );
        }

        $U.confirm('Flag as Spam?', 'Are you sure you want to flag this story as spam?', mark);
    },

    _urlChanged : function()
    {
        if (!Story._autoDiscover) return;

        var txtStoryUrl = $('#txtStoryUrl');
        var frmStorySubmit = $("#frmStorySubmit");

        if (frmStorySubmit.validate().element(txtStoryUrl))
        {
            if (Story._lastRetrievedUrl != txtStoryUrl.val())
            {
                var url = 'url=' + encodeURIComponent(txtStoryUrl.val());

                var xmlHttpRequest = $.ajax  (
                                                {
                                                    url : Story._retrieveStoryUrl,
                                                    dataType : 'json',
                                                    data : url,
                                                    beforeSend :    function()
                                                                    {
                                                                        $U.disableInputs('#frmStorySubmit', true);
                                                                        $U.showProgress('Retrieving content...<a id=\"cancelRetrive\" class=\"actionLink\" href="javascript:void(0)">cancel?</a>', '#txtStoryUrl');

                                                                        Story._lastRetrievedUrl = txtStoryUrl.val();

                                                                        $('#cancelRetrive').click  (
                                                                                                        function()
                                                                                                        {
                                                                                                            xmlHttpRequest.abort();
                                                                                                            $U.disableInputs('#frmStorySubmit', false);
                                                                                                            $U.focus('txtStoryTitle');
                                                                                                            $U.hideProgress();
                                                                                                            Story._lastRetrievedUrl = '';
                                                                                                        }
                                                                                                    );
                                                                    },
                                                    success :   function(result)
                                                                {
                                                                    $U.disableInputs('#frmStorySubmit', false);
                                                                    $U.focus('txtStoryTitle');
                                                                    $U.hideProgress();

                                                                    if (result.isSuccessful)
                                                                    {
                                                                        $('#txtStoryTitle').val(result.title);
                                                                        $('#txtStoryDescription').val(result.description);

                                                                        frmStorySubmit.validate().element($('#txtStoryTitle'));
                                                                        frmStorySubmit.validate().element($('#txtStoryDescription'));
                                                                    }
                                                                    else
                                                                    {
                                                                        var html = result.errorMessage;

                                                                        if (result.alreadyExists)
                                                                        {
                                                                            html = 'Story with the same url already exists. <a href=\"' +
                                                                                    result.existingUrl +
                                                                                    '\">Click here</a> to view the story.';
                                                                        }

                                                                        $('#errorStoryUrl').html(html).show();
                                                                    }
                                                                }
                                                }
                                            );
            }
        }
    }
};