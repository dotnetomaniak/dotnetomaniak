CREATE TABLE [dbo].[Achievement] (
    [Name]        NVARCHAR (4000)  NOT NULL,
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Description] NTEXT            NOT NULL,
    [Type]        INT              NOT NULL,
    CONSTRAINT [PK_dbo.Achievement] PRIMARY KEY CLUSTERED ([Id] ASC)
);

