var SmoothImage =
{
    show : function(img)
    {
        var ie = /MSIE (\d+\.\d+);/.test(navigator.userAgent);

        var OPACITY = {IMG : img};

        var timerid = setInterval(  function()
                                    {
                                        try 
                                        {
                                            var count = (ie) ? OPACITY.IMG.filters.alpha.opacity : (OPACITY.IMG.style.opacity * 100);

                                            count += 5;

                                            if (count <= 100)
                                            {
                                                if (ie)
                                                {
                                                    OPACITY.IMG.filters.alpha.opacity = count;
                                                }
                                                else
                                                {
                                                    OPACITY.IMG.style.opacity = (count/100);
                                                }
                                            }
                                            else
                                            {
                                                clearInterval(OPACITY.TIMER);
                                            }
                                        }
                                        catch (e)
                                        {
                                        }
                                    }
                                    ,
                                    25
                                );

        OPACITY.TIMER = timerid;
    }
};