var Analytics =
{
    init : function()
    {
        var pageTracker = _gat._getTracker('UA-121414708-1');

        pageTracker._trackPageview();

        $("a[rel*='external']").click(
                                        function()
                                        {
                                            pageTracker._trackPageview('/external/'+ $(this).attr('href'));
                                        }
                                     );
    },

    dispose : function()
    {
    }
};