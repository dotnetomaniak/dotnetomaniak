$(document).ready(function () {
    jQuery('.ui-autocomplete li').live('mouseenter', function () {
        jQuery(this).addClass('ui-autocomplete-over');
    }).live('mouseleave',
    function () {
        jQuery(this).removeClass('ui-autocomplete-over');
    });

    $("a.rozwin").live("click", function () {
        $(this).html("zwiń");
        $(this).attr("class", "zwin");
        var el = $(this).parents('div.summary').prev(), curHeight = el.height(), autoHeight = el.css('height', 'auto').height();
        el.height(curHeight).animate({ height: autoHeight }); //.css('height', 'auto');
    });
    $("a.zwin").live("click", function () {

        $(this).html("rozwiń");
        $(this).attr("class", "rozwin");
        $(this).parents('div.summary').prev().animate({ height: '30px' }); //.css('height','30px');


    });
    $(".categories-container").mouseover(function () {
        $("div.categories").show();
        $("li#categories a").addClass('cat-active');

    });
    $(".categories-container").mouseout(function () {
        $("div.categories").hide();
        $("li#categories a").removeClass('cat-active');
    });
    $(".about-container").mouseover(function () {
        $("div.about").show();
        $("li#about a").addClass('cat-active');

    });
    $(".about-container").mouseout(function () {
        $("div.about").hide();
        $("li#about a").removeClass('cat-active');

    });

    //Tabs
    $("ul.tabs-list li a").click(function () {

        var id = $(this).attr('id');

        $(".tab-content").each(function () {
            $(this).hide();
        });
        $(".tabs-list li a").each(function () {
            $(this).removeClass("active");
        });
        $(this).addClass("active");

        $("#" + id + "-content").show();
    });

    //podglad wysiwyg
    $("a.hide-sample").live("click", function () {
        $(this).html("pokaż podgląd");
        $(this).attr("class", "show-sample");
        $(".wysiwyg-code").toggle();
    });
    $("a.show-sample").live("click", function () {

        $(this).html("ukryj podgląd");
        $(this).attr("class", "hide-sample");
        $(".wysiwyg-code").toggle();


    });

    //podswietlenie inputów w add-article

    $(".add-article-row input, .add-article-row textarea").focus(function () {
        $(this).css('border-color', '#c2e078');
    });
    $(".add-article-row input, .add-article-row textarea").focusout(function () {
        $(this).css('border-color', '#e1e1e1');
    });

});    



                           