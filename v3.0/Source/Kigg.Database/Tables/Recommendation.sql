CREATE TABLE [dbo].[Recommendation] (
    [Id]                  UNIQUEIDENTIFIER NOT NULL,
    [RecommendationLink]  VARCHAR (200)    NOT NULL,
    [RecommendationTitle] VARCHAR (50)     NOT NULL,
    [ImageLink]           VARCHAR (200)    NOT NULL,
    [ImageTitle]          VARCHAR (50)     NOT NULL,
    [CreatedAt]           DATETIME         NOT NULL,
    [StartTime]           DATETIME         NOT NULL,
    [EndTime]             DATETIME         NOT NULL,
    [Position]            INT              NOT NULL,
    [Email]               VARCHAR (50)     NULL,
    [NotificationIsSent]  BIT              NULL,
    [IsBanner]            BIT              NULL,
    CONSTRAINT [PK_dbo.Recommendation] PRIMARY KEY CLUSTERED ([Id] ASC)
);

