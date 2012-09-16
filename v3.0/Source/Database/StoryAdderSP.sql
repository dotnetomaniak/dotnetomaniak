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
CREATE PROCEDURE StoryAdder 	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
    SET NOCOUNT ON;

    INSERT INTO UserAchievement (UserId,AchievementId,Displayed,DateAchieved)
	SELECT DISTINCT u.Id, '2E8F0603-0EB3-4412-A864-854448B894BE','false',GETUTCDate() 
		FROM [User] u 
		JOIN Story s
		ON s.UserId = u.Id		
		WHERE u.Id NOT IN (SELECT ua.UserId FROM UserAchievement ua WHERE AchievementId = '2E8F0603-0EB3-4412-A864-854448B894BE')		
    SELECT 1;
END
GO
