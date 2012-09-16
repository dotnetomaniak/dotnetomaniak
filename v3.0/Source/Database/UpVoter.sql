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
CREATE PROCEDURE UpVoter	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
    SET NOCOUNT ON;

    INSERT INTO UserAchievement (UserId,AchievementId,Displayed,DateAchieved)
	SELECT DISTINCT u.Id, '76ACDC60-4D9F-4633-96CC-683CA6105F17','false',GETUTCDate() 
		FROM [User] u 
		JOIN UserScore us
		ON us.UserId = u.Id		
		WHERE u.Id NOT IN (SELECT ua.UserId FROM UserAchievement ua WHERE AchievementId = '76ACDC60-4D9F-4633-96CC-683CA6105F17')		
		AND us.ActionType IN (4,5)
    SELECT 1;
END
GO
