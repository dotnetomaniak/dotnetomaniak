USE [pawlos_dotnetomaniak]
GO
/****** Object:  StoredProcedure [pawlos_dotnetomaniak].[UpVoter]    Script Date: 18/07/2014 14:43:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [pawlos_dotnetomaniak].[Facebook]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
    SET NOCOUNT ON;

    INSERT INTO UserAchievement (UserId,AchievementId,Displayed,DateAchieved)
	SELECT DISTINCT u.Id, '856E4164-AB34-4221-8E23-100B0C6BE576','false',GETUTCDate() 
		FROM [User] u 
		WHERE u.Id NOT IN (SELECT ua.UserId FROM UserAchievement ua WHERE AchievementId = '856E4164-AB34-4221-8E23-100B0C6BE576')		
		AND u.FbId IS NOT NULL
    SELECT 1;
END
