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
CREATE PROCEDURE GoodStory	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
    SET NOCOUNT ON;

    INSERT INTO UserAchievement (UserId,AchievementId,Displayed,DateAchieved)
	SELECT DISTINCT s.UserId, '134B2022-274F-46AB-98F5-2BE0CBC972AC','false',GETUTCDate() 
			FROM [StoryVote] sv
			JOIN [Story] s 
			ON s.Id = sv.StoryId  
			WHERE s.UserId NOT IN (SELECT ua.UserId FROM UserAchievement ua WHERE AchievementId = '134B2022-274F-46AB-98F5-2BE0CBC972AC')		
			GROUP BY sv.StoryId, s.UserId
			HAVING count(*) >= 10
    SELECT 1;
END
GO
