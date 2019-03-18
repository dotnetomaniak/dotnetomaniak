CREATE TABLE [dbo].[StoryVote] (
    [StoryId]   UNIQUEIDENTIFIER NOT NULL,
    [UserId]    UNIQUEIDENTIFIER NOT NULL,
    [IPAddress] VARCHAR (15)     NOT NULL,
    [Timestamp] DATETIME         NOT NULL,
    CONSTRAINT [PK_dbo.StoryVote] PRIMARY KEY CLUSTERED ([StoryId] ASC, [UserId] ASC),
    CONSTRAINT [Story_StoryVote] FOREIGN KEY ([StoryId]) REFERENCES [dbo].[Story] ([Id]),
    CONSTRAINT [User_StoryVote] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

