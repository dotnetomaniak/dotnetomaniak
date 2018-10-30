CREATE TABLE [dbo].[UserScore] (
    [Id]         BIGINT           IDENTITY (1, 1) NOT NULL,
    [UserId]     UNIQUEIDENTIFIER NOT NULL,
    [Timestamp]  DATETIME         NOT NULL,
    [ActionType] INT              NOT NULL,
    [Score]      DECIMAL (5, 2)   NOT NULL,
    CONSTRAINT [PK_dbo.UserScore] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [User_UserScore] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

