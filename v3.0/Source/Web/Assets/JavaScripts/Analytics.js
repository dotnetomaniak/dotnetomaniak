var Analytics =
{
    init : function()
    {
        var pageTracker = _gat._getTracker('UA-5894416-2');

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