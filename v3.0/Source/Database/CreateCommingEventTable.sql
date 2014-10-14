CREATE TABLE [dbo].[CommingEvent](
	[Id] [uniqueidentifier] NOT NULL,
	[EventLink] [varchar](200) NOT NULL,
	[EventName] [varchar](50) NOT NULL,
	[EventDate] [datetime] NOT NULL,
 CONSTRAINT [PK_CommingEvent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
