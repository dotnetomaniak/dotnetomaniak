CREATE TABLE [dbo].[Tag] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [UniqueName] NVARCHAR (64)    NOT NULL,
    [Name]       NVARCHAR (64)    NOT NULL,
    [CreatedAt]  DATETIME         NOT NULL,
    CONSTRAINT [PK_dbo.Tag] PRIMARY KEY CLUSTERED ([Id] ASC)
);

