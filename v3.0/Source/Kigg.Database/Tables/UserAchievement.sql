CREATE TABLE [dbo].[UserAchievement] (
    [DateAchieved]  DATETIME         NOT NULL,
    [AchievementId] UNIQUEIDENTIFIER NOT NULL,
    [UserId]        UNIQUEIDENTIFIER NOT NULL,
    [Displayed]     BIT              NOT NULL,
    CONSTRAINT [PK_dbo.UserAchievement] PRIMARY KEY CLUSTERED ([AchievementId] ASC, [UserId] ASC),
    CONSTRAINT [Achievement_UserAchievement] FOREIGN KEY ([AchievementId]) REFERENCES [dbo].[Achievement] ([Id]),
    CONSTRAINT [User_UserAchievement] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

