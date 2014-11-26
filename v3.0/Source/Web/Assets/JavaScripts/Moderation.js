var Moderation =
{
    _markCommentAsOffendedUrl: '',
    _spamCommentUrl: '',
    _editAdUrl: '',
    _deleteStoryUrl: '',
    _spamStoryUrl: '',
    _approveStoryUrl: '',
    //_getStoryUrl: '',
    _generateMiniatureStoryUrl: '',
    _centerMeTimer: null,

    set_markCommentAsOffendedUrl: function(value) {
        Moderation._markCommentAsOffendedUrl = value;
    },

    set_spamCommentUrl: function(value) {
        Moderation._spamCommentUrl = value;
    },

    set_editAdUrl: function(value) {
        Moderation._editAdUrl = value;
    },
    
    set_deleteStoryUrl: function(value) {
        Moderation._deleteStoryUrl = value;
    },

    set_spamStoryUrl: function(value) {
        Moderation._spamStoryUrl = value;
    },

    set_approveStoryUrl: function(value) {
        Moderation._approveStoryUrl = value;
    },

    //set_getStoryUrl: function(value) {
    //    Moderation._getStoryUrl = value;
    //},

    set_generateMiniatureStoryUrl: function(value) {
        Moderation._generateMiniatureStoryUrl = value;
    },

    init: function() {

        /* Polish initialisation for the jQuery UI date picker plugin. */
        /* Written by Jacek Wysocki (jacek.wysocki@gmail.com). */
        jQuery(function($){
            $.datepicker.regional['pl'] = {
                closeText: 'Zamknij',
                prevText: '&#x3c;',
                nextText: '&#x3e;',
                currentText: 'Dziś',
                monthNames: ['Styczeń','Luty','Marzec','Kwiecień','Maj','Czerwiec',
                'Lipiec','Sierpień','Wrzesień','Październik','Listopad','Grudzień'],
                monthNamesShort: ['Sty','Lu','Mar','Kw','Maj','Cze',
                'Lip','Sie','Wrz','Pa','Lis','Gru'],
                dayNames: ['Niedziela','Poniedzialek','Wtorek','Środa','Czwartek','Piątek','Sobota'],
                dayNamesShort: ['Nie','Pn','Wt','Śr','Czw','Pt','So'],
                dayNamesMin: ['N','Pn','Wt','Śr','Cz','Pt','So'],
                weekHeader: 'Tydz',
                dateFormat: 'yy-mm-dd',
                firstDay: 1,
                isRTL: false,
                showMonthAfterYear: false,
                yearSuffix: ''};
        $.datepicker.setDefaults($.datepicker.regional['pl']);
    });
        $('#lnkAddRecomendation').click(
            function () {
                $('#hidAdId').val("");
                Moderation.showRecommendation();
            }
        );
        $('#lnkAddEvent').click(
            function () {
                $('#hidEventId').val("");
                Moderation.showEvent();
            }
        );
        $('a[data-id]').click(
            function() {
                Moderation.deleteAd($(this).data('id'));
            }
        );

        $('a[data-edit-id]').click(
            function() {
                Moderation.editAd($(this).data('edit-id'));
            });

        $('#frmRecommendation').validate(
                                            {
                                                rules: {
                                                    RecommendationLink: {
                                                        required: true,
                                                    },
                                                    RecommendationTitle: {
                                                        required: true,
                                                    },
                                                    ImageLink: {
                                                        required: true,
                                                    },
                                                    ImageTitle: {
                                                        required: true,
                                                    },
                                                    StartTime: {
                                                        required: true,
                                                    },
                                                    EndTime: {
                                                        required: true,
                                                    }
                                                },
                                                messages: {
                                                    RecommendationLink: {
                                                        required: 'Link rekomendacji nie może być pusty.',
                                                    },
                                                    RecommendationTitle: {
                                                        required: 'Tytuł rekomendacji nie może być pusty.',
                                                    },
                                                    ImageLink: {
                                                        required: 'Link obrazka nie może być pusty.',
                                                    },
                                                    ImageTitle: {
                                                        required: 'Tytuł rekomendacji nie może być pusty.',
                                                    },
                                                    StartTime: {
                                                        required: 'Data startowa jest wymagana.',
                                                    },
                                                    EndTime: {
                                                        required: 'Data końcowa jest wymagana.',
                                                    }
                                                },
                                                submitHandler: function (form) {
                                                    var options = {
                                                        dataType: 'json',
                                                        beforeSubmit: function () {
                                                            $('#RecommendationMessage').hide().text('Tworzenie...').css('color', '');

                                                            $U.disableInputs('#RecommendationSection', true);
                                                            $U.showProgress('Tworzenie...');
                                                        },
                                                        success: function (result) {
                                                            $U.disableInputs('#RecommendationSection', false);
                                                            $U.hideProgress();
                                                            Membership._hide(true);

                                                            if (result.isSuccessful) {
                                                                window.location.reload();
                                                            }
                                                            else {
                                                                $('#RecommendationMessage').text(result.errorMessage).css({ color: '#ff0000', display: 'block' });
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
    },
    
    dispose: function() {
    },
    
    deleteAd: function(adId) {

            function submit() {
                var data = 'id=' + encodeURIComponent(adId);

                $.ajax(
                    {
                        url: '/DeleteAd',
                        type: 'POST',
                        dataType: 'json',
                        data: data,
                        beforeSend: function() {
                            $U.showProgress('Usuwanie reklamy');
                        },
                        success: function(result) {
                            $U.hideProgress();

                            if (result.isSuccessful) {
                                window.location.reload();
                            } else {
                                $U.messageBox('Error', result.errorMessage, true);
                            }
                        }
                    }
                );
            }

            $U.confirm('Usunięcie reklamy?', 'Czy jesteś pewny, że chcesz usunać daną reklamę?', submit);
        },

    editAd: function (adId) {
        var data = 'id=' + encodeURIComponent(adId);

        $U.disableInputs('#frmRecommendation', true);

        $.ajax(
            {
                url: Moderation._editAdUrl,
                type: 'POST',
                dataType: 'json',
                data: data,
                beforeSend: function () {
                    $U.showProgress('Wczytywanie reklamy...');
                },
                success: function (result) {
                    $U.hideProgress();
                    $U.disableInputs('#frmRecommendation', false);

                    if (result.errorMessage) {
                        $U.messageBox('Error', result.errorMessage, true);
                    } else {
                        $U.blockUI();

                        $('span.error').hide();
                        $('span.message').hide();
                        Moderation.showRecommendation();                        
                        
                        $('#hidAdId').val(result.id);
                        $('#txtRecommendationLink').val(result.recommendationLink);
                        $('#txtRecommendationTitle').val(result.recommendationTitle);
                        $('#txtImageLink').val(result.imageLink);
                        $('#txtImageTitle').val(result.imageTitle);
                        $('#txtStartTime').val(result.startTime);
                        $('#txtEndTime').val(result.endTime);
                        $('#txtPosition').val(result.position);
                        $('#txtEmail').val(result.email);
                        $('#IsBanner').prop('checked', result.isBanner);
                        $('#IsBanner').val(result.isBanner);
                    }
                }
            }
        );
    },
    
    //function: formatJSONDate(jsonDate) {
    //var newDate = dateFormat(jsonDate, "mm/dd/yyyy");
    //return newDate;
    //},

    approveStory: function(storyId) {

        function submit() {
            var data = 'id=' + encodeURIComponent(storyId);

            $.ajax(
                {
                    url: Moderation._approveStoryUrl,
                    type: 'POST',
                    dataType: 'json',
                    data: data,
                    beforeSend: function() {
                        $U.showProgress('Zatwierdzanie wpisu...');
                    },
                    success: function(result) {
                        $U.hideProgress();

                        if (result.isSuccessful) {
                            $('#t-' + storyId).fadeOut('normal', function() { $(this).remove(); });
                        } else {
                            $U.messageBox('Error', result.errorMessage, true);
                        }
                    }
                }
            );
        }

        $U.confirm('Zatwierdź artykuł?', 'Czy jesteś pewny, że chcesz zatwierdzić dany artykuł?', submit);
    },

    deleteStory: function(storyId) {

        function submit() {
            var data = 'id=' + encodeURIComponent(storyId);

            $.ajax(
                {
                    url: Moderation._deleteStoryUrl,
                    type: 'POST',
                    dataType: 'json',
                    data: data,
                    beforeSend: function() {
                        $U.showProgress('Usuwanie artykułu...');
                    },
                    success: function(result) {
                        $U.hideProgress();

                        if (result.isSuccessful) {
                            $('#t-' + storyId).fadeOut('normal', function() { $(this).remove(); });
                        } else {
                            $U.messageBox('Error', result.errorMessage, true);
                        }
                    }
                }
            );
        }

        $U.confirm('Usunięcie artykułu?', 'Czy jesteś pewny, że chcesz usunać dany artykuł?', submit);
    },

    confirmSpamStory: function(storyId) {

        function submit() {
            var data = 'id=' + encodeURIComponent(storyId);

            $.ajax(
                {
                    url: Moderation._spamStoryUrl,
                    type: 'POST',
                    dataType: 'json',
                    data: data,
                    beforeSend: function() {
                        $U.showProgress('Zatwierdzanie spamowego artykułu...');
                    },
                    success: function(result) {
                        $U.hideProgress();

                        if (result.isSuccessful) {
                            $('#t-' + storyId).fadeOut('normal', function() { $(this).remove(); });
                        } else {
                            $U.messageBox('Error', result.errorMessage, true);
                        }
                    }
                }
            );
        }

        $U.confirm('Oznacz jako spam?', 'Czy na pewno chcesz oznaczyć artykuł jako spam?', submit);
    },

    generateMiniatureStory: function(storyId) {

        function submit() {
            var data = 'id=' + encodeURIComponent(storyId);

            $.ajax(
                {
                    url: Moderation._generateMiniatureStoryUrl,
                    type: 'POST',
                    dataType: 'json',
                    data: data,
                    beforeSend: function() {
                        $U.showProgress('Generowanie miniatury');
                    },
                    success: function(result) {
                        $U.hideProgress();

                        if (result.isSuccessful) {
                            $('.entry-thumb img').attr('src', result.url);
                        } else {
                            $U.messageBox('Error', result.errorMessage, true);
                        }
                    }
                }
            );
        }

        $U.confirm('Generowanie miniaturki', 'Czy jesteś pewny, że chcesz pobrać miniaturkę?', submit);
    },

    confirmSpamComment: function(storyId, commentId) {

        function submit() {
            var data = {
                storyId: storyId,
                commentId: commentId
            };

            $.ajax(
                {
                    url: Moderation._spamCommentUrl,
                    type: 'POST',
                    dataType: 'json',
                    data: data,
                    beforeSend: function() {
                        $U.showProgress('Zatwierdzanie spamowego komentarza...');
                    },
                    success: function(result) {
                        $U.hideProgress();

                        if (result.isSuccessful) {
                            $('#li-' + commentId).fadeOut('normal', function() { $(this).remove(); });
                        } else {
                            $U.messageBox('Error', result.errorMessage, true);
                        }
                    }
                }
            );
        }

        $U.confirm('Zatwierdzić spamowy komentarz?', 'Jesteś pewien, że chcesz zatwierdzić komentarz jako spamowy?', submit);
    },

    markCommentAsOffended: function(storyId, commentId) {

        function submit() {
            var data = {
                storyId: storyId,
                commentId: commentId
            };

            $.ajax(
                {
                    url: Moderation._markCommentAsOffendedUrl,
                    type: 'POST',
                    dataType: 'json',
                    data: data,
                    beforeSend: function() {
                        $U.showProgress('Zaznaczanie jako obraźliwy...');
                    },
                    success: function(result) {
                        $U.hideProgress();

                        if (result.isSuccessful) {
                            $U.messageBox('Sukces', 'Komentarz został oznaczony jako obraźliwy.', false);
                        } else {
                            $U.messageBox('Błąd', result.errorMessage, true);
                        }
                    }
                }
            );
        }

        $U.confirm('Obraźliwy?', 'Jesteś pewien, że chcesz zaznaczyć komenatrz jako obraźliwy?', submit);
    },

    _hide: function() {
        $U.disableInputs('#storyEditorBox', false);
        $U.hideProgress();
        Moderation._clearCenterMeTimer();

        $('#storyEditorBox').fadeOut('normal',
            function() {
                $U.unblockUI();
            }
        );
    },

    _centerMe: function() {
        var e = $('#storyEditorBox');

        $U.center(e);
        Moderation._clearCenterMeTimer();
        Moderation._centerMeTimer = setInterval(
            function() {
                Moderation._centerMe();
            },
            2000
        );
    },

    _clearCenterMeTimer: function() {
        if (Moderation._centerMeTimer != null) {
            clearInterval(Moderation._centerMeTimer);
            Moderation._centerMeTimer = null;
        }
    },

    showRecommendation: function () {
        $('input[name="EndTime"]').datepicker({ dateFormat: 'yy-mm-dd'}).val();
        $('input[name="StartTime"]').datepicker({ dateFormat: 'yy-mm-dd' }).val();
        $('.contentContainer > div').hide();
        $('#RecommendationSection').show();
        Membership._show('#membershipBox');
        $U.focus('txtRecommendationLink');
    },
    showEvent: function () {        
        $('input[name="EventTime"]').datepicker({ dateFormat: 'yy-mm-dd' }).val();
        $('.contentContainer > div').hide();
        $('#EventSection').show();
        Membership._show('#membershipBox');
        $U.focus('txtRecommendationLink');
    },
};
    