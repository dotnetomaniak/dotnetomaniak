-- nowa tabelki na potrzeby konkursów

CREATE TABLE [dbo].[Quiz] (
  [Id][uniqueidentifier] NOT NULL,
  [Title] [nvarchar](256) NOT NULL,
  [DateStart] [datetime] NOT NULL,
  [DateEnd] [datetime] NOT NULL  
  CONSTRAINT [PK_Quiz] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[QuizContestants] (	
	[Id][uniqueidentifier] NOT NULL,
	[QuizId][uniqueidentifier] NOT NULL,
	[UserId][uniqueidentifier] NOT NULL,
	[RegisteredAt][datetime] NOT NULL,
	CONSTRAINT [PK_QuizContestants] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[QuizContestants]  WITH CHECK ADD  CONSTRAINT [FK_QuizContestants_Quiz] FOREIGN KEY([QuizId])
REFERENCES [dbo].[Quiz] ([Id])
GO

ALTER TABLE [dbo].[QuizContestants]  WITH CHECK ADD  CONSTRAINT [FK_QuizContestants_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO