var Moderation =
{
    _markCommentAsOffendedUrl : '',
    _spamCommentUrl : '',
    _deleteStoryUrl : '',
    _spamStoryUrl: '',
    _approveStoryUrl: '',
    _getStoryUrl: '',
    _generateMiniatureStoryUrl: '',
    _centerMeTimer: null,

    set_markCommentAsOffendedUrl : function(value)
    {
        Moderation._markCommentAsOffendedUrl = value;
    },

    set_spamCommentUrl : function(value)
    {
        Moderation._spamCommentUrl = value;
    },

    set_deleteStoryUrl : function(value)
    {
        Moderation._deleteStoryUrl = value;
    },

    set_spamStoryUrl : function(value)
    {
        Moderation._spamStoryUrl = value;
    },

    set_approveStoryUrl: function(value)
    {
        Moderation._approveStoryUrl = value;
    },

    set_getStoryUrl : function(value)
    {
        Moderation._getStoryUrl = value;
    },

    set_generateMiniatureStoryUrl: function (value)
    {
        Moderation._generateMiniatureStoryUrl = value;
    },

    init : function()
    {
        $('#storyEditorBox').keydown  (
                                            function(e)
                                            {
                                                if (e.keyCode === 27) // EscapeKey
                                                {
                                                    Moderation._hide();
                                                }
                                            }
                                        );

        $('#storyEditorClose').click(function() { Moderation._hide(); });

        $('#frmStoryUpdate').validate(
                                        {
                                            rules: {
                                                        name : 'required',
                                                        createdAt: 'required',
                                                        description :   {
                                                                                required: true,
                                                                                rangelength: [8, 2048]
                                                                        },
                                                        category : 'required',
                                                        tags: 'required'
                                                    },
                                            messages : {
                                                            name : 'Nazwa nie może być pusta.',
                                                            createdAt: 'Pole Utworzono nie może być puste.',
                                                            title :    {
                                                                                required: 'Tytuł nie może być pusty.',
                                                                                maxlength: 'Tytuł nie może być dłuższy niż 256 znaków.'
                                                                       },
                                                            description :  {
                                                                                    required: 'Opis nie może być pusty.',
                                                                                    rangelength: 'Opis musi zawierać się pomiędzy 8 a 2048 znakami.'
                                                                            },
                                                            category : 'Kategoria musi być wybrana.',
                                                            tags: 'Podaj przynajmniej jeden tag dla artykułu.'
                                                        },
                                            submitHandler : function(form)
                                            {
                                                var options =   {
                                                                    dataType : 'json',
                                                                    beforeSubmit : function(values, form, options)
                                                                    {
                                                                        $('#updateStoryMessage').text('').css('color', '').hide();
                                                                        $U.disableInputs('#frmStoryUpdate', true);
                                                                        $U.showProgress('Aktualizacja wpisu...', '#btnUpdateStory');
                                                                    },
                                                                    success : function(result)
                                                                    {
                                                                        $U.disableInputs('#frmStoryUpdate', false);
                                                                        $U.hideProgress();

                                                                        if (result.isSuccessful)
                                                                        {
                                                                            Moderation._hide();
                                                                        }
                                                                        else
                                                                        {
                                                                            $('#updateStoryMessage').text(result.errorMessage).css({color : '#ff0000', display : 'block'});
                                                                        }
                                                                    }
                                                                };

                                                $(form).ajaxSubmit(options);
                                            },
                                            errorPlacement : function(error, element)
                                            {
                                                element.parents('p:first').find('span.error').text(error.text());
                                            },
                                            highlight : function(element, errorClass)
                                            {
                                                $(element).parents('p:first').find('span.error').show();
                                            },
                                            unhighlight : function(element, errorClass)
                                            {
                                                $(element).parents('p:first').find('span.error').hide();
                                            }
                                        }
                                    );
    },

    dispose : function()
    {
    },

    editStory : function(storyId)
    {
        var data = 'id=' + encodeURIComponent(storyId);

        $U.disableInputs('#frmStoryUpdate', true);

        $.ajax  (
                    {
                        url : Moderation._getStoryUrl,
                        type : 'POST',
                        dataType : 'json',
                        data : data,
                        beforeSend :    function()
                                        {
                                            $U.showProgress('Wczytywanie artykułu...');
                                        },
                        success :   function(result)
                                    { 
                                        $U.hideProgress();
                                        $U.disableInputs('#frmStoryUpdate', false);

                                        if (result.errorMessage)
                                        {
                                            $U.messageBox('Error', result.errorMessage, true);
                                        }
                                        else
                                        {
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
                                                                                            function()
                                                                                            {
                                                                                                var rdo = $(this)[0];

                                                                                                if (rdo.value == result.category)
                                                                                                {
                                                                                                    rdo.checked = true;
                                                                                                }
                                                                                            }
                                                                                        );
                                            $U.center(modal);
                                            modal.fadeIn();
                                            Moderation._centerMe();
                                        }
                                    }
                    }
                );
    },

    approveStory: function(storyId)
    {
        function submit()
        {
            var data = 'id=' + encodeURIComponent(storyId);

            $.ajax  (
                        {
                            url: Moderation._approveStoryUrl,
                            type : 'POST',
                            dataType : 'json',
                            data : data,
                            beforeSend :    function()
                                            {
                                                $U.showProgress('Zatwierdzanie wpisu...');
                                            },
                            success :   function(result)
                                        {
                                            $U.hideProgress();

                                            if (result.isSuccessful)
                                            {
                                                $('#t-' + storyId).fadeOut('normal', function(){$(this).remove();});
                                            }
                                            else
                                            {
                                                $U.messageBox('Error', result.errorMessage, true);
                                            }
                                        }
                        }
                    );
        }

        $U.confirm('Zatwierdź artykuł?', 'Czy jesteś pewny, że chcesz zatwierdzić dany artykuł?', submit);
    },

    deleteStory: function(storyId)
    {
        function submit()
        {
            var data = 'id=' + encodeURIComponent(storyId);

            $.ajax(
                        {
                            url: Moderation._deleteStoryUrl,
                            type: 'POST',
                            dataType: 'json',
                            data: data,
                            beforeSend: function()
                            {
                                $U.showProgress('Usuwanie artykułu...');
                            },
                            success: function(result)
                            {
                                $U.hideProgress();

                                if (result.isSuccessful)
                                {
                                    $('#t-' + storyId).fadeOut('normal', function() { $(this).remove(); });
                                }
                                else
                                {
                                    $U.messageBox('Error', result.errorMessage, true);
                                }
                            }
                        }
                    );
        }

        $U.confirm('Usunięcie artykułu?', 'Czy jesteś pewny, że chcesz usunać dany artykuł?', submit);
    },

    confirmSpamStory: function(storyId)
    {
        function submit()
        {
            var data = 'id=' + encodeURIComponent(storyId);

            $.ajax  (
                        {
                            url: Moderation._spamStoryUrl,
                            type : 'POST',
                            dataType : 'json',
                            data : data,
                            beforeSend :    function()
                                            {
                                                $U.showProgress('Zatwierdzanie spamowego artykułu...');
                                            },
                            success :   function(result)
                                        {
                                            $U.hideProgress();

                                            if (result.isSuccessful)
                                            {
                                                $('#t-' + storyId).fadeOut('normal', function(){$(this).remove();});
                                            }
                                            else
                                            {
                                                $U.messageBox('Error', result.errorMessage, true);
                                            }
                                        }
                        }
                    );
        }

        $U.confirm('Oznacz jako spam?', 'Czy na pewno chcesz oznaczyć artykuł jako spam?', submit);
    },

    generateMiniatureStory: function(storyId)
    {
        function submit() {
            var data = 'id=' + encodeURIComponent(storyId);

            $.ajax(
                        {
                            url: Moderation._generateMiniatureStoryUrl,
                            type: 'POST',
                            dataType: 'json',
                            data: data,
                            beforeSend: function () {
                                $U.showProgress('Generowanie miniatury');
                            },
                            success: function (result) {
                                $U.hideProgress();

                                if (result.isSuccessful) {
                                    $U.messageBox('Sukces', 'Poprawnie wygenerowano miniaturę');
                                }
                                else {
                                    $U.messageBox('Error', result.errorMessage, true);
                                }
                            }
                        }
                    );
        }

        $U.confirm('Generowanie miniaturki', 'Czy jesteś pewny, że chcesz pobrać miniaturkę?', submit);
    },
    
    confirmSpamComment : function(storyId, commentId)
    {
        function submit()
        {
            var data = {
                            storyId : storyId,
                            commentId : commentId
                       };

            $.ajax  (
                        {
                            url : Moderation._spamCommentUrl,
                            type : 'POST',
                            dataType : 'json',
                            data : data,
                            beforeSend :    function()
                                            {
                                                $U.showProgress('Zatwierdzanie spamowego komentarza...');
                                            },
                            success :   function(result)
                                        {
                                            $U.hideProgress();

                                            if (result.isSuccessful)
                                            {
                                                $('#li-' + commentId).fadeOut('normal', function(){$(this).remove();});
                                            }
                                            else
                                            {
                                                $U.messageBox('Error', result.errorMessage, true);
                                            }
                                        }
                        }
                    );
        }

        $U.confirm('Zatwierdzić spamowy komentarz?', 'Jesteś pewien, że chcesz zatwierdzić komentarz jako spamowy?', submit);
    },

    markCommentAsOffended : function(storyId, commentId)
    {
        function submit()
        {
            var data = {
                            storyId : storyId,
                            commentId : commentId
                       };

            $.ajax  (
                        {
                            url : Moderation._markCommentAsOffendedUrl,
                            type : 'POST',
                            dataType : 'json',
                            data : data,
                            beforeSend :    function()
                                            {
                                                $U.showProgress('Zaznaczanie jako obraźliwy...');
                                            },
                            success :   function(result)
                                        {
                                            $U.hideProgress();

                                            if (result.isSuccessful)
                                            {
                                                $U.messageBox('Sukces', 'Komentarz został oznaczony jako obraźliwy.', false);
                                            }
                                            else
                                            {
                                                $U.messageBox('Błąd', result.errorMessage, true);
                                            }
                                        }
                        }
                    );
        }

        $U.confirm('Obraźliwy?', 'Jesteś pewien, że chcesz zaznaczyć komenatrz jako obraźliwy?', submit);
    },

    _hide : function()
    {
        $U.disableInputs('#storyEditorBox', false);
        $U.hideProgress();
        Moderation._clearCenterMeTimer();

        $('#storyEditorBox').fadeOut(    'normal',
                                        function()
                                        {
                                            $U.unblockUI();
                                        }
                                     );
    },

    _centerMe : function()
    {
        var e = $('#storyEditorBox');

        $U.center(e);
        Moderation._clearCenterMeTimer();
        Moderation._centerMeTimer = setInterval(
                                                    function()
                                                    {
                                                        Moderation._centerMe();
                                                    },
                                                    2000
                                                );
    },

    _clearCenterMeTimer : function()
    {
        if (Moderation._centerMeTimer != null)
        {
            clearInterval(Moderation._centerMeTimer);
            Moderation._centerMeTimer = null;
        }
    }
};