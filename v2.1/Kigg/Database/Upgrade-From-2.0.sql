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

ALTER TABLE [Story]
ADD  
	[ApprovedAt] [datetime] NULL
GO