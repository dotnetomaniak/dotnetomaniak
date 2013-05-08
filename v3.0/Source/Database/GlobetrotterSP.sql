USE [pawlos_dotnetomaniak]
GO
/****** Object:  StoredProcedure [pawlos_dotnetomaniak].[Globetrotter]    Script Date: 2013-05-08 16:52:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [pawlos_dotnetomaniak].[Globetrotter]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

INSERT INTO 
		UserAchievement (UserId,AchievementId,Displayed,DateAchieved)
	SELECT 
		UserId, 'C264F476-ECF0-4CEE-91FF-7B08642A56AC','false',GETUTCDate() 
	FROM 
		(
		SELECT UserId, IpCountry FROM [Story] WHERE IpCountry IS NOT NULL
		UNION 
		SELECT UserId, IpCountry FROM [StoryComment] WHERE IpCountry IS NOT NULL
		UNION 
		SELECT UserId, IpCountry FROM [StoryVote] WHERE IpCountry IS NOT NULL
		) 
		AS
			uc		
		WHERE 
			UserId NOT IN (SELECT ua.UserId FROM UserAchievement ua WHERE AchievementId = 'C264F476-ECF0-4CEE-91FF-7B08642A56AC')
		GROUP BY 
			UserId
		HAVING count(*) >= 3

END