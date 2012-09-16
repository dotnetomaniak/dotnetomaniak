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
                                                    $('#lnkStoryPreview').text('pokaż podgląd');
                                                    Story._isPreviewOpen = false;
                                                }
                                                else
                                                {
                                                    $('#storyPreview').slideDown('normal');
                                                    $('#lnkStoryPreview').text('ukryj podgląd');
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
                                                        category : 'required',
                                                        tags: 'required'
                                                    },
                                            messages : {
                                                            url :   {
                                                                        required: 'Adres nie może być pusty.',
                                                                        url: 'Niepoprawny format adresu.'
                                                                    },
                                                            title : {
                                                                        required: 'Tytuł nie może być pusty.',
                                                                        maxlength: 'Tytuł nie może być dłuższy niż 256 znaków.'
                                                                    },
                                                            storyDescription :   {
                                                                                    required: 'Opis nie może być pusty.',
                                                                                    rangelength: 'Opis musi zawierać się pomiędzy 8 a 2048 znaków.'
                                                                                 },
                                                            category : 'Kategoria musi być wybrana.',
                                                            tags : 'Podaj przynajmniej jeden tag dla artykułu.'
                                                        },
                                            submitHandler : function(form)
                                            {
                                                var options =   {
                                                                    dataType : 'json',
                                                                    beforeSubmit : function(values, form, options)
                                                                    {
                                                                        $('#storyMessage').text('').css('color', '').hide();
                                                                        $U.disableInputs('#frmStorySubmit', true);
                                                                        $U.showProgress('Dodawanie artykułu...', '#btnStorySubmit');
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
                                                                                html = 'Artykuł z tym samym adresem url istnieje. <a href=\"' + result.url + '\">kliknij</a>, aby go zobaczyć.';
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
                                                element.css('border-color', '#CE0000');
                                                findErrorElement(element).text(error.text()).css('display','block');
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
            var t = element.parents('div:first').find('span.error');

            if ((t.length == 0) && (Membership.get_isLoggedIn()) && (Story._captchaEnabled))
            {            
                t = element.parents('div#recaptcha_area').find('span.error');
            }
            if (t.length == 0)
            {
                t = element.parents('div.radios-wrapper').find('span.error');
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
                                                $('#recaptcha_response_field').attr('title', 'Pole weryfikacji Captcha nie może być puste.')
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
        Array.prototype.last = function() {return this[this.length-1];}
        $('#txtStoryTags').autocomplete (
                                            {                                                
                                                source: function(request, response) {
                                                    jQuery.ajax({
                                                        url: Story._suggestTagsUrl,
                                                        data: {
                                                            q: request.term.split(',').last(),
                                                            limit: 10
                                                        },
                                                        success: function(data)
                                                        {
                                                            response($.map(
                                                                    eval(data), function(row)
                                                                                {
                                                                                    return {
                                                                                                label: row,
                                                                                                value: row+', ',
                                                                                                result: row
                                                                                            }
                                                                                }
                                                                    ));
                                                        }
                                                    });
                                                },                                                
                                                select: function(event, ui) {
                                                    event.preventDefault();
                                                    var value = $('#txtStoryTags').val();
                                                    var lastItem = value.split(',').last();
                                                    value = value.replace(lastItem.trim(), ui.item.value);
                                                    $('#txtStoryTags').val(value);                                                    
                                                },
                                                minLength: 2,
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
                                            $('#s-p-' + storyId).text('Poczekaj...').show();
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
                                            $('#s-p-' + storyId).text('Czekaj...').show();
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
                                                $('#s-p-' + storyId).text('Czekaj...').show();
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

        $U.confirm('Oznacz jako spam?', 'Jesteś pewny, że chcesz oznaczyć ten artykuł jako spam?', mark);
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
                                                                        $U.showProgress('Pobieranie treści...<a id="cancelRetrive" class="actionLink" href="javascript:void(0)">anulować?</a>', '#txtStoryUrl');

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
                                                                            html = 'Artykuł z tym samym adresem url istnieje. <a href=\"'+result.existingUrl+'\">Kliknij</a>, aby go wyświetlić.';
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