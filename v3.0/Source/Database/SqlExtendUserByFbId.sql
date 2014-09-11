USE [$(DatabaseName)];
ALTER TABLE [dbo].[User]
    ADD [FbId] NCHAR (50) NULL;