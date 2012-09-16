USE KiGG
GO

EXEC sp_rename 'StoryComment.Timestamp', 'CreatedAt', 'COLUMN'
GO

ALTER TABLE [Category]
DROP COLUMN [Version]
GO

ALTER TABLE [CommentSubscribtion]
DROP COLUMN [Version]
GO

ALTER TABLE [Story]
DROP COLUMN [Version]
GO

ALTER TABLE [StoryComment]
DROP COLUMN [Version]
GO

ALTER TABLE [StoryMarkAsSpam]
DROP COLUMN [Version]
GO

ALTER TABLE [StoryTag]
DROP COLUMN [Version]
GO

ALTER TABLE [StoryView]
DROP COLUMN [Version]
GO

ALTER TABLE [StoryVote]
DROP COLUMN [Version]
GO

ALTER TABLE [Tag]
DROP COLUMN [Version]
GO

ALTER TABLE [User]
DROP COLUMN [Version]
GO

ALTER TABLE [UserScore]
DROP COLUMN [Version]
GO

ALTER TABLE [UserTag]
DROP COLUMN [Version]
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