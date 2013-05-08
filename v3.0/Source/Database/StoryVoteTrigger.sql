USE [pawlos_dotnetomaniak]
GO
/****** Object:  Trigger [dbo].[StoryVoteTrigger]    Script Date: 2013-05-08 16:55:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[StoryVoteTrigger] ON [dbo].[StoryVote] AFTER INSERT 
AS 
UPDATE 
	dbo.StoryVote
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
WHERE StoryId IN( SELECT StoryId FROM INSERTED ) AND UserId IN( SELECT UserId FROM INSERTED )
