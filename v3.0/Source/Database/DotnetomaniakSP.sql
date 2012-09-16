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
CREATE PROCEDURE Dotnetomaniak	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
    SET NOCOUNT ON;
	
    INSERT INTO UserAchievement (AchievementId,Displayed,UserId,DateAchieved)
        select 'C5217D3A-1CA1-4C49-9E4C-990C63CD7176',0,UserId, TimeStamp from UserScore where Actiontype = 1
	and UserId NOT IN (SELECT UserId FROM UserAchievement WHERE AchievementId = 'C5217D3A-1CA1-4C49-9E4C-990C63CD7176')
     UNION
        SELECT 'C5217D3A-1CA1-4C49-9E4C-990C63CD7176',0,[Id] ,[CreatedAt]
        FROM [User]
	WHERE 
		Password IS NULL AND
		IsActive = 1 AND
		Id NOT IN (SELECT UserId FROM UserAchievement WHERE AchievementId = 'C5217D3A-1CA1-4C49-9E4C-990C63CD7176')
    SELECT 1;
END
GO