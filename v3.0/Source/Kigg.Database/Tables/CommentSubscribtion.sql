CREATE TABLE [dbo].[CommentSubscribtion] (
    [StoryId] UNIQUEIDENTIFIER NOT NULL,
    [UserId]  UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.CommentSubscribtion] PRIMARY KEY CLUSTERED ([StoryId] ASC, [UserId] ASC),
    CONSTRAINT [Story_CommentSubscribtion] FOREIGN KEY ([StoryId]) REFERENCES [dbo].[Story] ([Id]),
    CONSTRAINT [User_CommentSubscribtion] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

