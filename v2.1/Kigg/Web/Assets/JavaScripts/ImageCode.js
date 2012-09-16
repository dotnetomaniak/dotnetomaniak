var ImageCode =
{
    _promoteText : '',

    _isOpen : false,

    set_promoteText : function(value)
    {
        ImageCode._promoteText = value;
    },

    init : function()
    {
        $('#a-c').click (
                            function()
                            {
                                if (ImageCode._isOpen)
                                {
                                    $('#imageCode').slideUp('normal');
                                    $('#a-c').text('show counter code');
                                    ImageCode._isOpen = false;
                                }
                                else
                                {
                                    $('#imageCode').slideDown('normal');
                                    $('#a-c').text('hide counter code');
                                    ImageCode._isOpen = true;
                                }
                            }
                        );

        $('#lnkUpdateCode').click   (
                                        function()
                                        {
                                            ImageCode.generateUrl();
                                        }
                                    );

        $('#lnkResetCode').click    (
                                        function()
                                        {
                                            ImageCode.reset();
                                        }
                                    );

        $('#txtBorderColor').ColorPicker   (
                                                {
                                                    onBeforeShow : function()
                                                    {
                                                        $(this).ColorPickerSetColor(this.value);
                                                    },
                                                    onShow : function(colpkr)
                                                    {
                                                        $(colpkr).slideDown('normal');
                                                        return false;
                                                    },
                                                    onHide : function (colpkr)
                                                    {
                                                        $(colpkr).slideUp('normal');
                                                        return false;
                                                    },
                                                    onChange : function(hsb, hex, rgb)
                                                    {
                                                        $('#txtBorderColor').val(hex);
                                                        $('#spnBorderColor').css('backgroundColor', '#' + hex);
                                                    },
                                                    onSubmit : function(hsb, hex, rgb)
                                                    {
                                                        $('#txtBorderColor').val(hex);
                                                        $('#spnBorderColor').css('backgroundColor', '#' + hex);
                                                    }
                                                }
                                            )
                            .change (
                                        function()
                                        {
                                            $('#spnBorderColor').css('backgroundColor', '#' + $(this).val());
                                        }
                                    );

        $('#txtTextBackColor').ColorPicker   (
                                                {
                                                    onBeforeShow : function()
                                                    {
                                                        $(this).ColorPickerSetColor(this.value);
                                                    },
                                                    onShow : function(colpkr)
                                                    {
                                                        $(colpkr).slideDown('normal');
                                                        return false;
                                                    },
                                                    onHide : function(colpkr)
                                                    {
                                                        $(colpkr).slideUp('normal');
                                                        return false;
                                                    },
                                                    onChange : function(hsb, hex, rgb)
                                                    {
                                                        $('#txtTextBackColor').val(hex);
                                                        $('#spnTextBackColor').css('backgroundColor', '#' + hex);
                                                    },
                                                    onSubmit : function(hsb, hex, rgb)
                                                    {
                                                        $('#txtTextBackColor').val(hex);
                                                        $('#spnTextBackColor').css('backgroundColor', '#' + hex);
                                                    }
                                                }
                                            )
                            .change (
                                        function()
                                        {
                                            $('#spnTextBackColor').css('backgroundColor', '#' + $(this).val());
                                        }
                                    );

        $('#txtTextForeColor').ColorPicker   (
                                                {
                                                    onBeforeShow : function()
                                                    {
                                                        $(this).ColorPickerSetColor(this.value);
                                                    },
                                                    onShow : function(colpkr)
                                                    {
                                                        $(colpkr).slideDown('normal');
                                                        return false;
                                                    },
                                                    onHide : function(colpkr)
                                                    {
                                                        $(colpkr).slideUp('normal');
                                                        return false;
                                                    },
                                                    onChange : function(hsb, hex, rgb)
                                                    {
                                                        $('#txtTextForeColor').val(hex);
                                                        $('#spnTextForeColor').css('backgroundColor', '#' + hex);
                                                    },
                                                    onSubmit : function(hsb, hex, rgb)
                                                    {
                                                        $('#txtTextForeColor').val(hex);
                                                        $('#spnTextForeColor').css('backgroundColor', '#' + hex);
                                                    }
                                                }
                                            )
                            .change (
                                        function()
                                        {
                                            $('#spnTextForeColor').css('backgroundColor', '#' + $(this).val());
                                        }
                                    );

        $('#txtCountBackColor').ColorPicker   (
                                                {
                                                    onBeforeShow : function()
                                                    {
                                                        $(this).ColorPickerSetColor(this.value);
                                                    },
                                                    onShow : function(colpkr)
                                                    {
                                                        $(colpkr).slideDown('normal');
                                                        return false;
                                                    },
                                                    onHide : function(colpkr)
                                                    {
                                                        $(colpkr).slideUp('normal');
                                                        return false;
                                                    },
                                                    onChange : function(hsb, hex, rgb)
                                                    {
                                                        $('#txtCountBackColor').val(hex);
                                                        $('#spnCountBackColor').css('backgroundColor', '#' + hex);
                                                    },
                                                    onSubmit : function(hsb, hex, rgb)
                                                    {
                                                        $('#txtCountBackColor').val(hex);
                                                        $('#spnCountBackColor').css('backgroundColor', '#' + hex);
                                                    }
                                                }
                                            )
                            .change (
                                        function()
                                        {
                                            $('#spnCountBackColor').css('backgroundColor', '#' + $(this).val());
                                        }
                                    );

        $('#txtCountForeColor').ColorPicker   (
                                                {
                                                    onBeforeShow : function()
                                                    {
                                                        $(this).ColorPickerSetColor(this.value);
                                                    },
                                                    onShow : function(colpkr)
                                                    {
                                                        $(colpkr).slideDown('normal');
                                                        return false;
                                                    },
                                                    onHide : function(colpkr)
                                                    {
                                                        $(colpkr).slideUp('normal');
                                                        return false;
                                                    },
                                                    onChange : function(hsb, hex, rgb)
                                                    {
                                                        $('#txtCountForeColor').val(hex);
                                                        $('#spnCountForeColor').css('backgroundColor', '#' + hex);
                                                    },
                                                    onSubmit : function(hsb, hex, rgb)
                                                    {
                                                        $('#txtCountForeColor').val(hex);
                                                        $('#spnCountForeColor').css('backgroundColor', '#' + hex);
                                                    }
                                                }
                                            )
                            .change (
                                        function()
                                        {
                                            $('#spnCountForeColor').css('backgroundColor', '#' + $(this).val());
                                        }
                                    );
        $('#txtImageCode').click(
                                    function()
                                    {
                                        $(this)[0].select();
                                    }
                                );

        ImageCode.reset();
    },

    reset : function()
    {
        $('#txtBorderColor').val($('#hidBorderColor').val());
        $('#txtTextBackColor').val($('#hidTextBackColor').val());
        $('#txtTextForeColor').val($('#hidTextForeColor').val());
        $('#txtCountBackColor').val($('#hidCountBackColor').val());
        $('#txtCountForeColor').val($('#hidCountForeColor').val());

        $('#spnBorderColor').css('backgroundColor', '#' + $('#hidBorderColor').val());
        $('#spnTextBackColor').css('backgroundColor', '#' + $('#hidTextBackColor').val());
        $('#spnTextForeColor').css('backgroundColor', '#' + $('#hidTextForeColor').val());
        $('#spnCountBackColor').css('backgroundColor', '#' + $('#hidCountBackColor').val());
        $('#spnCountForeColor').css('backgroundColor', '#' + $('#hidCountForeColor').val());

        ImageCode.generateUrl();
    },

    generateUrl : function()
    {
        var borderColor = '';
        var textBackColor = '';
        var textForeColor = '';
        var countBackColor = '';
        var countForeColor = '';

        if ($('#txtBorderColor').val() != $('#hidBorderColor').val())
        {
            borderColor = $('#txtBorderColor').val();
        }

        if ($('#txtTextBackColor').val() != $('#hidTextBackColor').val())
        {
            textBackColor = $('#txtTextBackColor').val();
        }

        if ($('#txtTextForeColor').val() != $('#hidTextForeColor').val())
        {
            textForeColor = $('#txtTextForeColor').val();
        }

        if ($('#txtCountBackColor').val() != $('#hidCountBackColor').val())
        {
            countBackColor = $('#txtCountBackColor').val();
        }

        if ($('#txtCountForeColor').val() != $('#hidCountForeColor').val())
        {
            countForeColor = $('#txtCountForeColor').val();
        }

        var imageUrl =  $('#hidImageUrl').val() + '?url=' + encodeURIComponent($('#hidOriginalUrl').val());

        if (borderColor.length > 0)
        {
            imageUrl += '&borderColor=' + encodeURIComponent(borderColor);
        }

        if (textBackColor.length > 0)
        {
            imageUrl += '&textBackColor=' + encodeURIComponent(textBackColor);
        }

        if (textForeColor.length > 0)
        {
            imageUrl += '&textForeColor=' + encodeURIComponent(textForeColor);
        }

        if (countBackColor.length > 0)
        {
            imageUrl += '&countBackColor=' + encodeURIComponent(countBackColor);
        }

        if (countForeColor.length > 0)
        {
            imageUrl += '&countForeColor=' + encodeURIComponent(countForeColor);
        }

        var url =   '<a rev="vote-for" href=\"' + $('#hidKiggUrl').val() + '\">' +
                    '<img alt=\"' + ImageCode._promoteText + '\" src=\"' + imageUrl + '\" style=\"border:0px\"/>' +
                    '</a>';

        $('#txtImageCode').val(url);

        imageUrl += '&noCache=true';

        $('#imgPreview').attr('src', imageUrl);
    },

    dispose : function()
    {
        $('#a-c').unbind();
        $('#lnkUpdateCode').unbind();
        $('#lnkResetCode').unbind();
        $('#txtBorderColor').unbind();
        $('#txtTextBackColor').unbind();
        $('#txtTextForeColor').unbind();
        $('#txtCountBackColor').unbind();
        $('#txtCountForeColor').unbind();
        $('#txtImageCode').unbind();
    }
};