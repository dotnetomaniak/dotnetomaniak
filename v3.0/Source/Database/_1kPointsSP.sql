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
CREATE PROCEDURE _1kPoints 	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
    SET NOCOUNT ON;

    INSERT INTO UserAchievement (UserId,AchievementId,Displayed,DateAchieved)
	SELECT u.Id, '77287de2-5f75-483f-be1f-a544d266cfd8','false',GETUTCDate() 
		FROM [User] u 
		JOIN UserScore us
		ON us.UserId = u.Id	
		WHERE u.Id NOT IN (SELECT ua.UserId FROM UserAchievement ua WHERE AchievementId = '77287de2-5f75-483f-be1f-a544d266cfd8')
		GROUP BY u.Id,u.UserName
		HAVING SUM(us.Score) > 1*1024            
    SELECT 1;
END
GO
