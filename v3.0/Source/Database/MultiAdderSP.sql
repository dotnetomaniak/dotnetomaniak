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
CREATE PROCEDURE MultiAdder	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
    SET NOCOUNT ON;

    INSERT INTO UserAchievement (UserId,AchievementId,Displayed,DateAchieved)
	SELECT DISTINCT s.UserId, '8F7A7B7F-6A21-4AC1-B6B7-3B4A51C3F4A2','false',GETUTCDate() 			
			FROM [Story] s 
			JOIN [user] u ON u.Id = UserId			
			WHERE s.UserId NOT IN (SELECT ua.UserId FROM UserAchievement ua WHERE AchievementId = '8F7A7B7F-6A21-4AC1-B6B7-3B4A51C3F4A2')					
			GROUP BY UserId, UserName
			having count(*) >= 256			
    SELECT 1;
END
GO
