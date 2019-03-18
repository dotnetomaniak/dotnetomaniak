CREATE TABLE [dbo].[StoryView] (
    [Id]        BIGINT           IDENTITY (1, 1) NOT NULL,
    [StoryId]   UNIQUEIDENTIFIER NOT NULL,
    [Timestamp] DATETIME         NOT NULL,
    [IPAddress] VARCHAR (15)     NOT NULL,
    CONSTRAINT [PK_dbo.StoryView] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Story_StoryView] FOREIGN KEY ([StoryId]) REFERENCES [dbo].[Story] ([Id])
);

