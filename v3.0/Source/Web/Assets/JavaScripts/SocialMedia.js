var SocialMedia =
    {
        init: function () {
            var mainHeight = $('#main').height();
            var subHeight = $('#sub').height();
            var subTotalHeight = subHeight + 200;
            if (mainHeight > subTotalHeight) {
                $('#sub').height(mainHeight);
                $('#content').height(mainHeight + 303)
            }
            else {
                $('#sub').height(subTotalHeight);
                $('#content').height(subTotalHeight + 303)
            }
        }
};