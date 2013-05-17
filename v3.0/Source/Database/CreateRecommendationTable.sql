USE [pawlos_dotnetomaniak]
GO

/****** Object:  Table [dbo].[Recommendation]    Script Date: 2013-05-17 11:38:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Recommendation](
	[Id] [uniqueidentifier] NOT NULL,
	[RecommendationLink] [varchar](200) NOT NULL,
	[RecommendationTitle] [varchar](50) NOT NULL,
	[ImageLink] [varchar](200) NOT NULL,
	[ImageTitle] [varchar](50) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[Position] [int] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Recommendation] ADD  CONSTRAINT [DF_Recommendation_Position]  DEFAULT ((999)) FOR [Position]
GO