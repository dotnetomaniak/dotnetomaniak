/****** Object:  Table [dbo].[UserAchievement]    Script Date: 05/21/2012 22:11:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserAchievement](
	[UserId] [uniqueidentifier] NOT NULL,
	[AchievementId] [uniqueidentifier] NOT NULL,
	[Displayed] [bit] NULL,
	[DateAchieved] [datetime] NOT NULL,
 CONSTRAINT [PK_UserAchievement] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[AchievementId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserAchievement]  WITH CHECK ADD  CONSTRAINT [FK_UserAchievement_Achievements] FOREIGN KEY([AchievementId])
REFERENCES [dbo].[Achievement] ([Id])
GO

ALTER TABLE [dbo].[UserAchievement] CHECK CONSTRAINT [FK_UserAchievement_Achievements]
GO

ALTER TABLE [dbo].[UserAchievement]  WITH CHECK ADD  CONSTRAINT [FK_UserAchievement_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO

ALTER TABLE [dbo].[UserAchievement] CHECK CONSTRAINT [FK_UserAchievement_User]
GO


