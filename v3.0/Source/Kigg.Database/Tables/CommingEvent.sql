CREATE TABLE [dbo].[CommingEvent] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [EventLink]  VARCHAR (200)    NOT NULL,
    [EventName]  VARCHAR (50)     NOT NULL,
    [EventDate]  DATETIME         NOT NULL,
    [EventPlace] VARCHAR (50)     NULL,
    [EventLead]  VARCHAR (500)    NULL,
    [CreatedAt]  DATETIME         NOT NULL,
    [Email]      VARCHAR (50)     NULL,
    [IsApproved] BIT              NULL,
    CONSTRAINT [PK_dbo.CommingEvent] PRIMARY KEY CLUSTERED ([Id] ASC)
);

