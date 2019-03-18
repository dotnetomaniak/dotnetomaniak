CREATE TABLE [dbo].[StoryComment] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [HtmlBody]   NVARCHAR (MAX)   NOT NULL,
    [TextBody]   NVARCHAR (MAX)   NOT NULL,
    [CreatedAt]  DATETIME         NOT NULL,
    [StoryId]    UNIQUEIDENTIFIER NOT NULL,
    [UserId]     UNIQUEIDENTIFIER NOT NULL,
    [IPAddress]  VARCHAR (15)     NOT NULL,
    [IsOffended] BIT              NOT NULL,
    CONSTRAINT [PK_dbo.StoryComment] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Story_StoryComment] FOREIGN KEY ([StoryId]) REFERENCES [dbo].[Story] ([Id]),
    CONSTRAINT [User_StoryComment] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

