CREATE TABLE [dbo].[Category] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [UniqueName]  NVARCHAR (64)    NOT NULL,
    [Name]        NVARCHAR (64)    NOT NULL,
    [CreatedAt]   DATETIME         NOT NULL,
    [Description] NVARCHAR (4000)  NOT NULL,
    [IsActive]    BIT              NOT NULL,
    CONSTRAINT [PK_dbo.Category] PRIMARY KEY CLUSTERED ([Id] ASC)
);

