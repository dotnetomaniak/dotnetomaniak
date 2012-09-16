CREATE function [dbo].[EFSearchComment]
      (
		@storyId uniqueidentifier,
		@query nvarchar(4000)
      )
returns bit
as
BEGIN
			if (EXISTS(select [StoryId]
					   from containstable(StoryComment,(TextBody), @query) as ft
					   join [StoryComment] on [StoryComment].[ID] = ft.[KEY]
					   where [StoryComment].[StoryId] = @storyId))
			BEGIN
				return 1
			END			
				
				return 0
			
		 
END
GO

CREATE function [dbo].[EFSearchStory]
      (
		@storyId uniqueidentifier,
		@query nvarchar(4000)
      )
returns bit
as
BEGIN
			if (EXISTS(select [Id]
						from containstable(Story,(Title, TextDescription), @query) as ft
						join [Story] on [Story].[ID] = ft.[KEY]
						where [Story].[ID] = @storyId))
			BEGIN
				return 1
			END			
				return 0
			
		 
END
GO