CREATE TABLE [dbo].[UserTag] (
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [TagId]  UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.UserTag] PRIMARY KEY CLUSTERED ([UserId] ASC, [TagId] ASC),
    CONSTRAINT [Tag_UserTag] FOREIGN KEY ([TagId]) REFERENCES [dbo].[Tag] ([Id]),
    CONSTRAINT [User_UserTag] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

