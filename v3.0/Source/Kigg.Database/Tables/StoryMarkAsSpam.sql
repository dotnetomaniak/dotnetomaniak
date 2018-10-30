CREATE TABLE [dbo].[StoryMarkAsSpam] (
    [StoryId]   UNIQUEIDENTIFIER NOT NULL,
    [UserId]    UNIQUEIDENTIFIER NOT NULL,
    [IPAddress] VARCHAR (15)     NOT NULL,
    [Timestamp] DATETIME         NOT NULL,
    CONSTRAINT [PK_dbo.StoryMarkAsSpam] PRIMARY KEY CLUSTERED ([StoryId] ASC, [UserId] ASC),
    CONSTRAINT [Story_StoryMarkAsSpam] FOREIGN KEY ([StoryId]) REFERENCES [dbo].[Story] ([Id]),
    CONSTRAINT [User_StoryMarkAsSpam] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

