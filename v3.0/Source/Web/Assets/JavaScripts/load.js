$(document).ready(function () {

    $(".ui-autocomplete li").mouseover(function () {
        $(this).addClass('ui-autocomplete-over');

    });
    $(".ui-autocomplete li").mouseout(function () {
        $(this).removeClass('ui-autocomplete-over');
    });

    $(document).on("click", "a.rozwin", function () {
        event.stopPropagation();
        event.stopImmediatePropagation();
        $(this).html("zwiń");
        $(this).attr("class", "zwin");
        var el = $(this).parents('div.summary').prev().children('div.description'), curHeight = el.height(), autoHeight = el.css('height', 'auto').height() + 15;
        if (window.innerWidth < 768) autoHeight += 115;
        el.height(curHeight).animate({ height: autoHeight }); //.css('height', 'auto');

        var image = $(this).parents('div.summary').prev().children('div.description').children('div.entry-thumb').children('a').children('img.smoothImage');

        if (image.attr("src") === '') {
            image.hide();
            var storyId = image.attr("data-story-id");                      
            $.get("/Story/ThumbnailPath", { storyId: storyId }, function (data) {
                image.attr("src", data);
                image.fadeIn('slow');
            });
        }
    });


    $(document).on("click", "a.zwin", function () {
        event.stopPropagation();
        event.stopImmediatePropagation();
        $(this).html("rozwiń");
        $(this).attr("class", "rozwin");
        $(this).parents('div.summary').prev().children('div.description').animate({ height: '30px' }); //.css('height','30px');
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
    //$("a.hide-sample").live("click", function () {
    //    $(this).html("pokaż podgląd");
    //    $(this).attr("class", "show-sample");
    //    $(".wysiwyg-code").toggle();
    //});
    //$("a.show-sample").live("click", function () {

    //    $(this).html("ukryj podgląd");
    //    $(this).attr("class", "hide-sample");
    //    $(".wysiwyg-code").toggle();


    //});

    $(document).on("click", "a.hide-sample", function () {
        $(this).html("pokaż podgląd");
        $(this).attr("class", "show-sample");
        $(".wysiwyg-code").toggle();
    });

    $(document).on("click", "a.show-sample", function () {
        $(this).html("ukryj podgląd");
        $(this).attr("class", "hide-sample");
        $(".wysiwyg-code").toggle();
    });

    $(".partialContents").each(function (index, item) {
        var url = $(item).data("url");
        if (url && url.length > 0) {
            $(item).load(url);
        }
    });
    //podswietlenie inputów w add-article

    $(".add-article-row input, .add-article-row textarea").focus(function () {
        $(this).css('border-color', '#c2e078');
    });
    $(".add-article-row input, .add-article-row textarea").focusout(function () {
        $(this).css('border-color', '#e1e1e1');
    });
    
});    



                           