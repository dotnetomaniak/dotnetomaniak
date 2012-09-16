var RichEditor =
{
    create : function(textArea, hidden, previewPane)
    {
        var settings =  {
                            onShiftEnter:{keepDefault:false, openWith:'\n\n'},
                            markupSet: [
                                            {name:'Bold', key:'B', openWith:'**', closeWith:'**'},
                                            {name:'Italic', key:'I', openWith:'_', closeWith:'_'},
                                            {separator:'---------------' },
                                            {name:'Bulleted List', key:'U', openWith:'- ' },
                                            {name:'Numeric List', key:'O', openWith:function(markItUp) { return markItUp.line+'. '; }},
                                            {separator:'---------------' },
                                            {name:'Quotes', key:'.', openWith:'> '},
                                            {name:'Code Block / Code', key:'K', openWith:'(!(\t|!|`)!)', closeWith:'(!(`)!)'},
                                            {separator:'---------------' },
                                            {name:'Link', key:'L', openWith:'[', closeWith:']([![Url:!:http://]!] "[![Title]!]")', placeHolder:'Your text to link here...' }
                                        ]
                        };

        var converter = new Attacklab.showdown.converter();

        textArea.markItUp(settings)
                .blur(
                        function()
                        {
                            update();
                        }
                     );

        setInterval(
                        function()
                        {
                            update();
                        },
                        100
                   );

        function update()
        {
            var value = $.trim(textArea.val());

            previewPane.html('');

            if (value.length > 0)
            {
                value = converter.makeHtml(value);
                hidden.val(value);
                previewPane.html(value);
            }
        }
    }
};