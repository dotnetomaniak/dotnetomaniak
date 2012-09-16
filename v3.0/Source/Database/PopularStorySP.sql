-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE PopularStory	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
    SET NOCOUNT ON;

    INSERT INTO UserAchievement (UserId,AchievementId,Displayed,DateAchieved)
	SELECT DISTINCT s.UserId, '0412DD1F-0F6A-417E-B72F-36F2E5D9DDE4','false',GETUTCDate() 
			FROM [StoryView] sv
			JOIN [Story] s 
			ON s.Id = sv.StoryId  
			WHERE s.UserId NOT IN (SELECT ua.UserId FROM UserAchievement ua WHERE AchievementId = '0412DD1F-0F6A-417E-B72F-36F2E5D9DDE4')		
			GROUP BY sv.StoryId, s.UserId
			HAVING count(*) >= 512
    SELECT 1;
END
GO
