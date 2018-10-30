CREATE TABLE [dbo].[User] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [UserName]       NVARCHAR (256)   NOT NULL,
    [Password]       NVARCHAR (64)    NULL,
    [Email]          NVARCHAR (256)   NOT NULL,
    [IsActive]       BIT              NOT NULL,
    [IsLockedOut]    BIT              NOT NULL,
    [Role]           INT              NOT NULL,
    [LastActivityAt] DATETIME         NOT NULL,
    [CreatedAt]      DATETIME         NOT NULL,
    [FbId]           NVARCHAR (256)   NULL,
    CONSTRAINT [PK_dbo.[User]]] PRIMARY KEY CLUSTERED ([Id] ASC)
);

