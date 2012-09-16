var Faq =
{
    init: function () {
        $('#faq').find('a.q').each(
                                    function () {
                                        $(this).click(
                                                        function () {
                                                            $(this).next('div.ans').toggle('normal');
                                                            if ($(this).parents('li').hasClass('rozwin'))
                                                                $(this).parents('li').removeClass('rozwin');
                                                            else                                                                                                                        
                                                                $(this).parents('li').addClass('rozwin');
                                                        }
                                                    )
                                    }
                       );
    },

    dispose: function () {
    }
};