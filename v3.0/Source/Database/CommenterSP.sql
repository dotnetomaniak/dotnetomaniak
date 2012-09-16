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
CREATE PROCEDURE Commenter 	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
    SET NOCOUNT ON;

    INSERT INTO UserAchievement (UserId,AchievementId,Displayed,DateAchieved)
	SELECT DISTINCT u.Id, 'F23FF0BA-37F1-433B-B405-7427D0F4ED2E','false',GETUTCDate() 
		FROM [User] u 
		JOIN StoryComment sc
		ON sc.UserId = u.Id		
		WHERE u.Id NOT IN (SELECT ua.UserId FROM UserAchievement ua WHERE AchievementId = 'F23FF0BA-37F1-433B-B405-7427D0F4ED2E')		
    SELECT 1;
END
GO
