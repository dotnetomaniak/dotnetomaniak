window.onerror = function(message, url, lineNumber)
{
    /*
    var isJs2Loaded = (typeof (Story) == 'function');

    var msg =   sJs2Loaded ?
                'Hold on cowboy, the site is not fully loaded! Give it few moments to breathe.' +
                'It usually happens when you are accessing the site with a low bandwidth internet connection.' :

                'Something really went wrong!. Try redo your last operation, if the problem persist, reload ' +
                'the site from your keyboard by pressing Ctrl+F5. If the problem still persist then clear your internet ' +
                'cache and revisit. We are extremely sorry for this inconvenience.';

    if (isJs2Loaded)
    {
        $U.messageBox('Oops', msg, true);
    }
    else
    {
        alert(msg);
    }
    */

    return true;
};