USE KiGG
GO

exec sp_fulltext_database 'enable' 
Go

CREATE FULLTEXT CATALOG [StoryComment] AS DEFAULT;
GO

CREATE FULLTEXT INDEX ON [dbo].[Story](
[TextDescription], 
[Title])
KEY INDEX [PK_Story] ON [StoryComment]
WITH CHANGE_TRACKING AUTO
GO
CREATE FULLTEXT INDEX ON [dbo].[StoryComment](
[TextBody])
KEY INDEX [PK_StoryComment] ON [StoryComment]
WITH CHANGE_TRACKING AUTO
GO

CREATE FUNCTION [dbo].[StorySearch]
      (@query nvarchar(4000))
returns table
as
  return (
			select distinct [Id]
			from containstable(Story,(Title, TextDescription), @query) as ft
			join [Story]
			on [Story].[ID] = ft.[KEY]
		 )
GO

CREATE FUNCTION [dbo].[CommentSearch]
      (@query nvarchar(4000))
returns table
as
  return (
			select distinct [StoryId]
			from containstable(StoryComment,(TextBody), @query) as ft
			join [StoryComment]
			on [StoryComment].[ID] = ft.[KEY]
		 )
GO