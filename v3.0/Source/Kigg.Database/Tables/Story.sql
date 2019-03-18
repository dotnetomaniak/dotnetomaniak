CREATE TABLE [dbo].[Story] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [UniqueName]      NVARCHAR (256)   NOT NULL,
    [Title]           NVARCHAR (256)   NOT NULL,
    [HtmlDescription] NVARCHAR (MAX)   NOT NULL,
    [TextDescription] NVARCHAR (MAX)   NOT NULL,
    [Url]             NVARCHAR (2048)  NOT NULL,
    [UrlHash]         NCHAR (24)       NOT NULL,
    [CategoryId]      UNIQUEIDENTIFIER NOT NULL,
    [UserId]          UNIQUEIDENTIFIER NOT NULL,
    [IPAddress]       VARCHAR (15)     NOT NULL,
    [CreatedAt]       DATETIME         NOT NULL,
    [LastActivityAt]  DATETIME         NOT NULL,
    [ApprovedAt]      DATETIME         NULL,
    [PublishedAt]     DATETIME         NULL,
    [Rank]            INT              NULL,
    [LastProcessedAt] DATETIME         NULL,
    CONSTRAINT [PK_dbo.Story] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Category_Story] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([Id]),
    CONSTRAINT [User_Story] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

