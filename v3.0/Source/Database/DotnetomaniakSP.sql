USE [pawlos_dotnetomaniak]
GO
/****** Object:  StoredProcedure [pawlos_dotnetomaniak].[Dotnetomaniak]    Script Date: 10/01/2012 21:26:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [pawlos_dotnetomaniak].[Dotnetomaniak]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
    SET NOCOUNT ON;
	
    INSERT INTO UserAchievement (AchievementId,Displayed,UserId,DateAchieved)
        SELECT a.Achievement, a.[Type], a.UserId, MIN(a.[TimeStamp]) FROM
(
	select 'C5217D3A-1CA1-4C49-9E4C-990C63CD7176' as Achievement,0 as [Type],UserId, TimeStamp from UserScore where Actiontype = 1
		and UserId NOT IN (SELECT UserId FROM UserAchievement WHERE AchievementId = 'C5217D3A-1CA1-4C49-9E4C-990C63CD7176')
    UNION
    SELECT 'C5217D3A-1CA1-4C49-9E4C-990C63CD7176'as Achievement,0 as [Type],[Id] as UserId ,[CreatedAt] as TimeStamp
        FROM [User]
	WHERE 
		Password IS NULL AND
		IsActive = 1 AND
		Id NOT IN (SELECT UserId FROM UserAchievement WHERE AchievementId = 'C5217D3A-1CA1-4C49-9E4C-990C63CD7176')
) a
GROUP BY Achievement, [Type], [UserId]
    SELECT 1;
END