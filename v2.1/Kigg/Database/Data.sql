USE Kigg
GO

DELETE [Tag]
GO

INSERT INTO Tag([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'CSharp', 'C#', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Tag([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'FSharp', 'F#', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Tag([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'VB.NET', 'VB.NET', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Tag([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'WPF', 'WPF', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Tag([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'WCF', 'WCF', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Tag([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'WF', 'WF', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Tag([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'Linq', 'Linq', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Tag([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'XLinq', 'XLinq', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Tag([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'CLR', 'CLR', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Tag([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'Iron-Ruby', 'Iron Ruby', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Tag([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'DLR', 'DLR', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Tag([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'IoC-DI', 'IoC/DI', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Tag([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'NUnit', 'NUnit', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Tag([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'MBUnit', 'MBUnit', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Tag([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'xunit', 'xunit', getutcdate())
WAITFOR DELAY '00:00:01'
GO

GO

DELETE [Category]
GO

INSERT INTO Category([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'ASP.NET', 'ASP.NET', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Category([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'Ajax', 'Ajax', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Category([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'Silverlight', 'Silverlight', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Category([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'Smart-Client', 'Smart Client', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Category([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'UX', 'UX', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Category([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'Web-Service', 'Web Service', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Category([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'SQL', 'SQL', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Category([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'Foundation', 'Foundation', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Category([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'Architecture', 'Architecture', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Category([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'Agile', 'Agile', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Category([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'Podcast', 'Podcast', getutcdate())
WAITFOR DELAY '00:00:01'
GO

INSERT INTO Category([ID], [UniqueName], [Name], [CreatedAt])
VALUES (newid(), 'Screencast', 'Screencast', getutcdate())
WAITFOR DELAY '00:00:01'
GO