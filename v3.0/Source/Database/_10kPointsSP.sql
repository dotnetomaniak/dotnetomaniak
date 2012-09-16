-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
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
CREATE PROCEDURE _10kPoints 	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
    SET NOCOUNT ON;

    INSERT INTO UserAchievement (UserId,AchievementId,Displayed,DateAchieved)
	SELECT u.Id, '0AC5293B-55F4-48F0-A8D5-CE427406BA2B','false',GETUTCDate() 
		FROM [User] u 
		JOIN UserScore us
		ON us.UserId = u.Id	
		WHERE u.Id NOT IN (SELECT ua.UserId FROM UserAchievement ua WHERE AchievementId = '0AC5293B-55F4-48F0-A8D5-CE427406BA2B')
		GROUP BY u.Id,u.UserName
		HAVING SUM(us.Score) > 10*1024            
    SELECT 1;
END
GO
