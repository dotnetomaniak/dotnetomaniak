var CommingEvent =
{
    _editEventUrl: '',

    set_editEventUrl: function (value) {
        CommingEvent._editEventUrl = value;
    },

    init: function () {

        /* Polish initialisation for the jQuery UI date picker plugin. */
        /* Written by Jacek Wysocki (jacek.wysocki@gmail.com). */
        jQuery(function ($) {
            $.datepicker.regional['pl'] = {
                closeText: 'Zamknij',
                prevText: '&#x3c;',
                nextText: '&#x3e;',
                currentText: 'Dziś',
                monthNames: ['Styczeń', 'Luty', 'Marzec', 'Kwiecień', 'Maj', 'Czerwiec',
                'Lipiec', 'Sierpień', 'Wrzesień', 'Październik', 'Listopad', 'Grudzień'],
                monthNamesShort: ['Sty', 'Lu', 'Mar', 'Kw', 'Maj', 'Cze',
                'Lip', 'Sie', 'Wrz', 'Pa', 'Lis', 'Gru'],
                dayNames: ['Niedziela', 'Poniedzialek', 'Wtorek', 'Środa', 'Czwartek', 'Piątek', 'Sobota'],
                dayNamesShort: ['Nie', 'Pn', 'Wt', 'Śr', 'Czw', 'Pt', 'So'],
                dayNamesMin: ['N', 'Pn', 'Wt', 'Śr', 'Cz', 'Pt', 'So'],
                weekHeader: 'Tydz',
                dateFormat: 'yy-mm-dd',
                firstDay: 1,
                isRTL: false,
                showMonthAfterYear: false,
                yearSuffix: ''
            };
            $.datepicker.setDefaults($.datepicker.regional['pl']);
        });
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

        $('a[data-event-id]').click(
            function () {
                CommingEvent.deleteEvent($(this).data('eventId'));
            }
        );

        $('a[data-edit-event-id]').click(
            function () {
                CommingEvent.editEvent($(this).data('editEventId'));
            });

        $('#lnkAddEvent').click(
            function () {
                $('#hidEventId').val("");
                CommingEvent.showEvent();
            }
        );

        $('#frmEvent').validate(
                                            {
                                                rules: {
                                                    EventUserEmail:{
                                                        required: true,
                                                    },
                                                    EventLink: {
                                                        required: true,
                                                    },
                                                    EventName: {
                                                        required: true,
                                                    },
                                                    EventDate: {
                                                        required: true,
                                                    }
                                                },
                                                messages: {
                                                    EventLink: {
                                                        required: 'Email użytkkownia nie może być pusty.',
                                                    },
                                                    EventLink: {
                                                        required: 'Link wydarzenia nie może być pusty.',
                                                    },
                                                    EventName: {
                                                        required: 'Nazwa wydarzenia nie może być pusta.',
                                                    },
                                                    EventDate: {
                                                        required: 'Data wydarzenia jest wymagana.',
                                                    }
                                                },
                                                submitHandler: function (form) {
                                                    var options = {
                                                        dataType: 'json',
                                                        beforeSubmit: function () {
                                                            $('#EventMessage').hide().text('Tworzenie...').css('color', '');

                                                            $U.disableInputs('#EventSection', true);
                                                            $U.showProgress('Tworzenie...');
                                                        },
                                                        success: function (result) {
                                                            $U.disableInputs('#EventSection', false);
                                                            $U.hideProgress();
                                                            Membership._hide(true);

                                                            if (result.isSuccessful) {
                                                                window.location.reload();
                                                            }
                                                            else {
                                                                $('#EventMessage').text(result.errorMessage).css({ color: '#ff0000', display: 'block' });
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
    },

    dispose: function () {
    },

    deleteEvent: function (eventId) {

        function submit() {
            var data = 'id=' + encodeURIComponent(eventId);

            $.ajax(
                {
                    url: '/DeleteEvent',
                    type: 'POST',
                    dataType: 'json',
                    data: data,
                    beforeSend: function () {
                        $U.showProgress('Usuwanie wydarzenia');
                    },
                    success: function (result) {
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

        $U.confirm('Usunięcie wydarzenia?', 'Czy jesteś pewny, że chcesz usunać dane wydarzenie?', submit);
    },

    editEvent: function (eventId) {
        var data = 'id=' + encodeURIComponent(eventId);

        $U.disableInputs('#frmEvent', true);

        $.ajax(
            {
                url: CommingEvent._editEventUrl,
                type: 'POST',
                dataType: 'json',
                data: data,
                beforeSend: function () {
                    $U.showProgress('Wczytywanie wydarzenia...');
                },
                success: function (result) {
                    $U.hideProgress();
                    $U.disableInputs('#frmEvent', false);

                    if (result.errorMessage) {
                        $U.messageBox('Error', result.errorMessage, true);
                    } else {
                        $U.blockUI();

                        $('span.error').hide();
                        $('span.message').hide();
                        CommingEvent.showEvent();

                        $('#txtUserEmail').val(result.eventUserEmail);
                        $('#hidEventId').val(result.eventId);
                        $('#txtEventLink').val(result.eventLink);
                        $('#txtEventName').val(result.eventName);
                        $('#txtEventDate').val(result.eventDate);
                        $('#txtEventPlace').val(result.eventPlace);
                        $('#txtEventLead').val(result.eventLead);
                    }
                }
            }
        );
    },

    showEvent: function () {
        $('input[name="EventDate"]').datepicker({ dateFormat: 'yy-mm-dd' }).val();
        $('.contentContainer > div').hide();
        $('#EventSection').show();
        Membership._show('#membershipBox');
        $U.focus('txtEventLink');
    },
};
