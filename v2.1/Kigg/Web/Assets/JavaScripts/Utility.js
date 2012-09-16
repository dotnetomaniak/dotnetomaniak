var $U =
{
    focus : function(c)
    {
        c = $('#' + c)[0];

        try
        {
            c.select();
        }
        catch(e)
        {
        }

        try
        {
            c.focus();
        }
        catch(e)
        {
        }
    },

    disableInputs : function(target, disable)
    {
        $(target).find('input,textarea,select').each(
                                                        function()
                                                        {
                                                                    $(this).attr('disabled', disable);
                                                        }
                                                    );
    },

    showProgress : function(message, target)
    {
        $('#progressBox').remove();
        $('<div id=\"progressBox\" class=\"modalBox\" style=\"z-index:1002\"></div>').appendTo('body');

        var modal = $('#progressBox');
        var messageHtml =   '<div class=\"progressSmall\">' +
                            '    <span style=\"padding-left:15px\">' + message + '</span>' +
                            '</div>';

        modal.html(messageHtml);

        var x;
        var y;

        if (target)
        {
            var elm = $(target);
            var pos = elm.offset();

            x = pos.left;
            y = (pos.top + elm[0].offsetHeight);
        }

        if ((!x) && (!y))
        {
            $U.center(modal);
        }
        else
        {
            $U.setLocation(modal, x, y)
        }

        modal.fadeIn('normal');
    },

    hideProgress : function()
    {
        $('#progressBox').fadeOut(  'normal',
                                    function()
                                    {
                                        $('#progressBox').remove();
                                    }
                                 );
    },

    confirm : function(title, message, okHandler)
    {
        $('#confirmationBox').empty().append('<div class=\"confirmMessage\">' + message + '</div>').css('display', '');
        $('#confirmationBox').dialog(
                                        {
                                            modal : true,
                                            resizable : false,
                                            title : title,
                                            width : 460,
                                            overlay: {
                                                        opacity: 0.5,
                                                        background: '#999'
                                                     },
                                            buttons:
                                            {
                                                "Ok" : function()
                                                {
                                                    okHandler();
                                                    $(this).dialog('close');
                                                },
                                                "Cancel" : function()
                                                {
                                                    $(this).dialog('close'); 
                                                }
                                            },
                                            close : function()
                                            {
                                                $(this).dialog('destroy'); 
                                            }
                                        }
                                    );
    },

    messageBox : function(title, message, isError, onOk)
    {
        var className = isError ? 'errorMessage' : 'notifyMessage';

        $('#messageBox').empty().append('<div class=\"' + className + '\">' + message + '</div>').css('display', '');
        $('#messageBox').dialog(
                                {
                                    modal: true,
                                    resizable: false,
                                    title: title,
                                    width: 460,
                                    overlay: {
                                                opacity: 0.5,
                                                background: '#999'
                                             },
                                    buttons:
                                    {
                                        "Ok" : function()
                                        {
                                            if (typeof(onOk) == 'function')
                                            {
                                                onOk();
                                            }

                                            $(this).dialog('close'); 
                                        }
                                    },
                                    close:function()
                                    {
                                        $(this).dialog('destroy'); 
                                    }
                                }
                            );
    },

    blockUI : function()
    {
        var dimBackground = $('#dimBackground');
        var width = $(document).width() + 'px';
        var height = $(document).height() + 'px';

        dimBackground.css({width : width, height : height, opacity : 0.5});

        $U.setLocation(dimBackground, 0, 0);
        dimBackground.show();
    },

    unblockUI : function()
    {
        $('#dimBackground').hide();
    },

    center : function(e)
    {
        var x = (($U.getViewPortWidth() - e[0].offsetWidth) /2);
        var y = (($U.getViewPortHeight() - e[0].offsetHeight) /2) + $U.getViewPortScrollY();

        $U.setLocation(e, x, y);
    },

    setLocation : function(e, x, y)
    {
        e.css({position : 'absolute', left : (x + 'px'), top : (y + 'px')});
    },

    getViewPortWidth : function()
    {
        var width = 0;

        if ((document.documentElement) && (document.documentElement.clientWidth))
        {
            width = document.documentElement.clientWidth;
        }
        else if ((document.body) && (document.body.clientWidth))
        {
            width = document.body.clientWidth;
        }
        else if (window.innerWidth)
        {
            width = window.innerWidth;
        }

        return width;
    },

    getViewPortHeight : function()
    {
        var height = 0;

        if (window.innerHeight)
        {
            height = (window.innerHeight);
        }
        else if ((document.documentElement) && (document.documentElement.clientHeight))
        {
            height = document.documentElement.clientHeight;
        }

        return height;
    },

    getContentHeight : function()
    {
        if (document.body)
        {
            if (document.body.scrollHeight)
            {
                return document.body.scrollHeight;
            }

            if (document.body.offsetHeight)
            {
                return document.body.offsetHeight;
            }
        }

        return 0;
    },

    getViewPortScrollX : function()
    {
        var scrollX = 0;

        if ((document.documentElement) && (document.documentElement.scrollLeft))
        {
            scrollX = document.documentElement.scrollLeft;
        }
        else if ((document.body) && (document.body.scrollLeft))
        {
            scrollX = document.body.scrollLeft;
        }
        else if (window.pageXOffset)
        {
            scrollX = window.pageXOffset;
        }
        else if (window.scrollX)
        {
            scrollX = window.scrollX;
        }

        return scrollX;
    },

    getViewPortScrollY : function()
    {
        var scrollY = 0;

        if ((document.documentElement) && (document.documentElement.scrollTop))
        {
            scrollY = document.documentElement.scrollTop;
        }
        else if ((document.body) && (document.body.scrollTop))
        {
            scrollY = document.body.scrollTop;
        }
        else if (window.pageYOffset)
        {
            scrollY = window.pageYOffset;
        }
        else if (window.scrollY)
        {
            scrollY = window.scrollY;
        }

        return scrollY;
    }
};