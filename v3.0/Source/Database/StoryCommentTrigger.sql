USE [pawlos_dotnetomaniak]
GO
/****** Object:  Trigger [dbo].[StoryCommentTrigger]    Script Date: 2013-05-08 16:56:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[StoryCommentTrigger] ON [dbo].[StoryComment] AFTER INSERT 
AS 
UPDATE 
	dbo.StoryComment
SET 
	IpCountry = (
		SELECT 
			Country 
		FROM 
			IpToGeo 
		WHERE (SELECT 
				cast(CAST(PARSENAME(INSERTED.IPAddress, 4) as tinyint) as varbinary(1)) +
				cast(CAST(PARSENAME(INSERTED.IPAddress, 3) as tinyint) as varbinary(1)) +
				cast(CAST(PARSENAME(INSERTED.IPAddress, 2) as tinyint) as varbinary(1)) +
				cast(CAST(PARSENAME(INSERTED.IPAddress, 1) as tinyint) as varbinary(1)) 					
				FROM INSERTED
			   )
		BETWEEN 
			IpFrom 
		AND 
			IpTo					 

		)
WHERE Id IN( SELECT Id FROM INSERTED )
