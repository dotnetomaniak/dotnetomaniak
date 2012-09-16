var Moderation =
{
    _markCommentAsOffendedUrl : '',
    _spamCommentUrl : '',
    _deleteStoryUrl : '',
    _spamStoryUrl : '',
    _getStoryUrl : '',
    _centerMeTimer : null,

    set_markCommentAsOffendedUrl : function(value)
    {
        Moderation._markCommentAsOffendedUrl = value;
    },

    set_spamCommentUrl : function(value)
    {
        Moderation._spamCommentUrl = value;
    },

    set_deleteStoryUrl : function(value)
    {
        Moderation._deleteStoryUrl = value;
    },

    set_spamStoryUrl : function(value)
    {
        Moderation._spamStoryUrl = value;
    },

    set_getStoryUrl : function(value)
    {
        Moderation._getStoryUrl = value;
    },

    init : function()
    {
        $('#storyEditorBox').keydown  (
                                            function(e)
                                            {
                                                if (e.keyCode === 27) // EscapeKey
                                                {
                                                    Moderation._hide();
                                                }
                                            }
                                        );

        $('#storyEditorClose').click(function() { Moderation._hide(); });

        $('#frmStoryUpdate').validate(
                                        {
                                            rules: {
                                                        name : 'required',
                                                        createdAt: 'required',
                                                        description :   {
                                                                                required: true,
                                                                                rangelength: [8, 2048]
                                                                        },
                                                        category : 'required'
                                                    },
                                            messages : {
                                                            name : 'Name cannot be blank.',
                                                            createdAt: 'Created At cannot be blank.',
                                                            title :    {
                                                                                required: 'Title cannot be blank.',
                                                                                maxlength: 'Title cannot be more than 256 character.'
                                                                       },
                                                            description :  {
                                                                                    required: 'Description cannot be blank.',
                                                                                    rangelength: 'Description must be between 8 to 2048 character.'
                                                                            },
                                                            category : 'Category must be selected.'
                                                        },
                                            submitHandler : function(form)
                                            {
                                                var options =   {
                                                                    dataType : 'json',
                                                                    beforeSubmit : function(values, form, options)
                                                                    {
                                                                        $('#updateStoryMessage').text('').css('color', '').hide();
                                                                        $U.disableInputs('#frmStoryUpdate', true);
                                                                        $U.showProgress('Updating Story...', '#btnUpdateStory');
                                                                    },
                                                                    success : function(result)
                                                                    {
                                                                        $U.disableInputs('#frmStoryUpdate', false);
                                                                        $U.hideProgress();

                                                                        if (result.isSuccessful)
                                                                        {
                                                                            Moderation._hide();
                                                                        }
                                                                        else
                                                                        {
                                                                            $('#updateStoryMessage').text(result.errorMessage).css({color : '#ff0000', display : 'block'});
                                                                        }
                                                                    }
                                                                };

                                                $(form).ajaxSubmit(options);
                                            },
                                            errorPlacement : function(error, element)
                                            {
                                                element.parents('p:first').find('span.error').text(error.text());
                                            },
                                            highlight : function(element, errorClass)
                                            {
                                                $(element).parents('p:first').find('span.error').show();
                                            },
                                            unhighlight : function(element, errorClass)
                                            {
                                                $(element).parents('p:first').find('span.error').hide();
                                            }
                                        }
                                    );
    },

    dispose : function()
    {
    },

    editStory : function(storyId)
    {
        var data = 'id=' + encodeURIComponent(storyId);

        $U.disableInputs('#frmStoryUpdate', true);

        $.ajax  (
                    {
                        url : Moderation._getStoryUrl,
                        type : 'POST',
                        dataType : 'json',
                        data : data,
                        beforeSend :    function()
                                        {
                                            $U.showProgress('Loading Story...');
                                        },
                        success :   function(result)
                                    { 
                                        $U.hideProgress();
                                        $U.disableInputs('#frmStoryUpdate', false);

                                        if (result.errorMessage)
                                        {
                                            $U.messageBox('Error', result.errorMessage, true);
                                        }
                                        else
                                        {
                                            $U.blockUI();

                                            var modal = $('#storyEditorBox');

                                            $('span.error').hide();
                                            $('span.message').hide();

                                            $('#hidStoryId').val(result.id);
                                            $('#txtStoryName').val(result.name);
                                            $('#txtStoryCreatedAt').val(result.createdAt);
                                            $('#txtStoryTitle').val(result.title);
                                            $('#txtStoryDescription').val(result.description);
                                            $('#txtStoryTags').val(result.tags);

                                            $('#frmStoryUpdate input:radio').each(
                                                                                            function()
                                                                                            {
                                                                                                var rdo = $(this)[0];

                                                                                                if (rdo.value == result.category)
                                                                                                {
                                                                                                    rdo.checked = true;
                                                                                                }
                                                                                            }
                                                                                        );
                                            $U.center(modal);
                                            modal.fadeIn();
                                            Moderation._centerMe();
                                        }
                                    }
                    }
                );
    },

    confirmSpamStory : function(storyId)
    {
        function submit()
        {
            var data = 'id=' + encodeURIComponent(storyId);

            $.ajax  (
                        {
                            url : Moderation._spamStoryUrl,
                            type : 'POST',
                            dataType : 'json',
                            data : data,
                            beforeSend :    function()
                                            {
                                                $U.showProgress('Approving Spam...');
                                            },
                            success :   function(result)
                                        {
                                            $U.hideProgress();

                                            if (result.isSuccessful)
                                            {
                                                $('#t-' + storyId).fadeOut('normal', function(){$(this).remove();});
                                            }
                                            else
                                            {
                                                $U.messageBox('Error', result.errorMessage, true);
                                            }
                                        }
                        }
                    );
        }

        $U.confirm('Approve Spam Story?', 'Are you sure you want to approve this story as spam?', submit);
    },

    deleteStory : function(storyId)
    {
        function submit()
        {
            var data = 'id=' + encodeURIComponent(storyId);

            $.ajax  (
                        {
                            url : Moderation._deleteStoryUrl,
                            type : 'POST',
                            dataType : 'json',
                            data : data,
                            beforeSend :    function()
                                            {
                                                $U.showProgress('Deleting Story...');
                                            },
                            success :   function(result)
                                        {
                                            $U.hideProgress();

                                            if (result.isSuccessful)
                                            {
                                                $('#t-' + storyId).fadeOut('normal', function(){$(this).remove();});
                                            }
                                            else
                                            {
                                                $U.messageBox('Error', result.errorMessage, true);
                                            }
                                        }
                        }
                    );
        }

        $U.confirm('Delete Story?', 'Are you sure you want to delete this story?', submit);
    },

    confirmSpamComment : function(storyId, commentId)
    {
        function submit()
        {
            var data = {
                            storyId : storyId,
                            commentId : commentId
                       };

            $.ajax  (
                        {
                            url : Moderation._spamCommentUrl,
                            type : 'POST',
                            dataType : 'json',
                            data : data,
                            beforeSend :    function()
                                            {
                                                $U.showProgress('Approving Spam...');
                                            },
                            success :   function(result)
                                        {
                                            $U.hideProgress();

                                            if (result.isSuccessful)
                                            {
                                                $('#li-' + commentId).fadeOut('normal', function(){$(this).remove();});
                                            }
                                            else
                                            {
                                                $U.messageBox('Error', result.errorMessage, true);
                                            }
                                        }
                        }
                    );
        }

        $U.confirm('Approve Spam Comment?', 'Are you sure you want to approve this comment as spam?', submit);
    },

    markCommentAsOffended : function(storyId, commentId)
    {
        function submit()
        {
            var data = {
                            storyId : storyId,
                            commentId : commentId
                       };

            $.ajax  (
                        {
                            url : Moderation._markCommentAsOffendedUrl,
                            type : 'POST',
                            dataType : 'json',
                            data : data,
                            beforeSend :    function()
                                            {
                                                $U.showProgress('Marking Offended...');
                                            },
                            success :   function(result)
                                        {
                                            $U.hideProgress();

                                            if (result.isSuccessful)
                                            {
                                                $U.messageBox('Success', 'Comment has been marked as offended.', false);
                                            }
                                            else
                                            {
                                                $U.messageBox('Error', result.errorMessage, true);
                                            }
                                        }
                        }
                    );
        }

        $U.confirm('Mark Offended?', 'Are you sure you want to mark this comment as offended?', submit);
    },

    _hide : function()
    {
        $U.disableInputs('#storyEditorBox', false);
        $U.hideProgress();
        Moderation._clearCenterMeTimer();

        $('#storyEditorBox').fadeOut(    'normal',
                                        function()
                                        {
                                            $U.unblockUI();
                                        }
                                     );
    },

    _centerMe : function()
    {
        var e = $('#storyEditorBox');

        $U.center(e);
        Moderation._clearCenterMeTimer();
        Moderation._centerMeTimer = setInterval(
                                                    function()
                                                    {
                                                        Moderation._centerMe();
                                                    },
                                                    2000
                                                );
    },

    _clearCenterMeTimer : function()
    {
        if (Moderation._centerMeTimer != null)
        {
            clearInterval(Moderation._centerMeTimer);
            Moderation._centerMeTimer = null;
        }
    }
};