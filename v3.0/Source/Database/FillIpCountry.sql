UPDATE 
	dbo.Story
SET 
	IpCountry = (
		SELECT 
			Country 
		FROM 
			IpToGeo 
		WHERE  
				cast(CAST(PARSENAME(IPAddress, 4) as tinyint) as varbinary(1)) +
				cast(CAST(PARSENAME(IPAddress, 3) as tinyint) as varbinary(1)) +
				cast(CAST(PARSENAME(IPAddress, 2) as tinyint) as varbinary(1)) +
				cast(CAST(PARSENAME(IPAddress, 1) as tinyint) as varbinary(1)) 					
		BETWEEN 
			IpFrom 
		AND 
			IpTo					 

		)

GO

UPDATE 
	dbo.StoryComment
SET 
	IpCountry = (
		SELECT 
			Country 
		FROM 
			IpToGeo 
		WHERE  
				cast(CAST(PARSENAME(IPAddress, 4) as tinyint) as varbinary(1)) +
				cast(CAST(PARSENAME(IPAddress, 3) as tinyint) as varbinary(1)) +
				cast(CAST(PARSENAME(IPAddress, 2) as tinyint) as varbinary(1)) +
				cast(CAST(PARSENAME(IPAddress, 1) as tinyint) as varbinary(1)) 					
		BETWEEN 
			IpFrom 
		AND 
			IpTo					 

		)

GO

UPDATE 
	dbo.StoryVote
SET 
	IpCountry = (
		SELECT 
			Country 
		FROM 
			IpToGeo 
		WHERE  
				cast(CAST(PARSENAME(IPAddress, 4) as tinyint) as varbinary(1)) +
				cast(CAST(PARSENAME(IPAddress, 3) as tinyint) as varbinary(1)) +
				cast(CAST(PARSENAME(IPAddress, 2) as tinyint) as varbinary(1)) +
				cast(CAST(PARSENAME(IPAddress, 1) as tinyint) as varbinary(1)) 					
		BETWEEN 
			IpFrom 
		AND 
			IpTo					 

		)

GO