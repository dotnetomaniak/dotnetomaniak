CREATE TABLE [dbo].[StoryTag] (
    [StoryId] UNIQUEIDENTIFIER NOT NULL,
    [TagId]   UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.StoryTag] PRIMARY KEY CLUSTERED ([StoryId] ASC, [TagId] ASC),
    CONSTRAINT [Story_StoryTag] FOREIGN KEY ([StoryId]) REFERENCES [dbo].[Story] ([Id]),
    CONSTRAINT [Tag_StoryTag] FOREIGN KEY ([TagId]) REFERENCES [dbo].[Tag] ([Id])
);

