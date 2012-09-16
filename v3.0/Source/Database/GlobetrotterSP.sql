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
ALTER PROCEDURE Globetrotter	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	CREATE TABLE #Tmp (IpAddress binary(4), UserId uniqueidentifier)
	INSERT INTO #Tmp
		SELECT cast(CAST(PARSENAME(IPAddress, 4) as tinyint) as varbinary(1)) +
			cast(CAST(PARSENAME(IPAddress, 3) as tinyint) as varbinary(1)) +
			cast(CAST(PARSENAME(IPAddress, 2) as tinyint) as varbinary(1)) +
			cast(CAST(PARSENAME(IPAddress, 1) as tinyint) as varbinary(1)), UserId
		FROM [Story]
		UNION ALL
		SELECT cast(CAST(PARSENAME(IPAddress, 4) as tinyint) as varbinary(1)) +
			cast(CAST(PARSENAME(IPAddress, 3) as tinyint) as varbinary(1)) +
			cast(CAST(PARSENAME(IPAddress, 2) as tinyint) as varbinary(1)) +
			cast(CAST(PARSENAME(IPAddress, 1) as tinyint) as varbinary(1)), UserId
		FROM [StoryVote]
		UNION ALL
		SELECT cast(CAST(PARSENAME(IPAddress, 4) as tinyint) as varbinary(1)) +
			cast(CAST(PARSENAME(IPAddress, 3) as tinyint) as varbinary(1)) +
			cast(CAST(PARSENAME(IPAddress, 2) as tinyint) as varbinary(1)) +
			cast(CAST(PARSENAME(IPAddress, 1) as tinyint) as varbinary(1)), UserId
		FROM [StoryComment]

;WITH UserAddrs(UserId, CountryName) AS
(
		SELECT DISTINCT UserId, 
		(SELECT Country FROM IpToGeo WHERE IpAddress  BETWEEN IpFrom AND IpTo)
		FROM #Tmp sv      
)

INSERT INTO UserAchievement (UserId,AchievementId,Displayed,DateAchieved)
	SELECT UserId,'C264F476-ECF0-4CEE-91FF-7B08642A56AC','false',GETUTCDate() FROM UserAddrs
	WHERE UserId NOT IN (SELECT ua.UserId FROM UserAchievement ua WHERE AchievementId = 'C264F476-ECF0-4CEE-91FF-7B08642A56AC')
	GROUP BY UserId
	HAVING count(*) >= 3

DROP TABLE #Tmp
END