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
CREATE PROCEDURE GreatStory	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
    SET NOCOUNT ON;

    INSERT INTO UserAchievement (UserId,AchievementId,Displayed,DateAchieved)
	SELECT DISTINCT s.UserId, '4FAFEC17-5069-46BA-A0EB-217D49C889B7','false',GETUTCDate() 
			FROM [StoryVote] sv
			JOIN [Story] s 
			ON s.Id = sv.StoryId  
			WHERE s.UserId NOT IN (SELECT ua.UserId FROM UserAchievement ua WHERE AchievementId = '4FAFEC17-5069-46BA-A0EB-217D49C889B7')		
			GROUP BY sv.StoryId, s.UserId
			HAVING count(*) >= 20
    SELECT 1;
END
GO
