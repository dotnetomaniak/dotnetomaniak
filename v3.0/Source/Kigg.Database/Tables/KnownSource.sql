CREATE TABLE [dbo].[KnownSource] (
    [Url]   NVARCHAR (450) NOT NULL,
    [Grade] INT            NOT NULL,
    CONSTRAINT [PK_dbo.KnownSource] PRIMARY KEY CLUSTERED ([Url] ASC)
);

