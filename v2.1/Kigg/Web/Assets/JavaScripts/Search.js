var Search =
{
    init : function()
    {
        var defaultSearchText = 'enter your keyword';
        var defaultColor = '#5a5a5a';

        var txtSearch = $('#txtSearch');

        if ($.trim(txtSearch[0].value).length == 0)
        {
            txtSearch.val(defaultSearchText).css({color: defaultColor});
        }

        txtSearch.focus (
                            function()
                            {
                                this.style.color = '';

                                if (this.value == defaultSearchText)
                                {
                                    this.value = '';
                                }
                            }
                        )
                .blur (
                        function()
                        {
                            if (this.value.length == 0)
                            {
                                this.style.color = defaultColor;
                                this.value = defaultSearchText;
                            }
                        }
                      );

        $('#frmSearch').submit  (
                                    function(e)
                                    {
                                        var q = $.trim(txtSearch[0].value);

                                        if (q.length == 0)
                                        {
                                            txtSearch[0].focus();
                                            e.preventDefault();
                                        }

                                        if (q == defaultSearchText)
                                        {
                                            e.preventDefault();
                                        }
                                    }
                                );
    },

    dispose : function()
    {
        $('#txtSearch').unbind();
        $('#frmSearch').unbind();
    }
};