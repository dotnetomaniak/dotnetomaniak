USE [dotnetomaniak];
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

ALTER TABLE Category ADD [Description] NVARCHAR(200)

BEGIN TRANSACTION;
UPDATE SET [Description] = N'Przydatne narzêdzia dla ka¿dego' 
WHERE UniqueName = N'Narzedzia'

UPDATE SET [Description] = N'RIA od MS. Wszyelkie wersje i dodatki.'
WHERE UniqueName = N'Silverlight'

UPDATE SET [Description] = N'Web Forms, MVC, jQuery i inne przydante rzeczy dla ka¿dego siedz¹cego w Web Development'
WHERE UniqueName = N'ASP.NET'

UPDATE SET [Description] = N'Pozosta³e nigdzie indziej niesklasyfikowane artyku³y' 
WHERE UniqueName = N'Inne'

UPDATE SET [Description] = N'WP7, WindowsMobile, WindowsCE ale tak¿e inne platformy mobilne' 
WHERE UniqueName = N'Mobile development'

UPDATE SET [Description] = N'WinForms, WPF, kontrolki i wszystko inne zwi¹zane z programwoaniem na platformê Windows'
WHERE UniqueName = N'Windows'

UPDATE SET [Description] = N'MSSQL, Oracle, mySQL, PostgreSQL i nie tylko'
WHERE UniqueName = N'Bazy danych i XML'

UPDATE SET [Description] = N'Sharepoint i wszystko co zwi¹zane z programowaniem na tê platformê'
WHERE UniqueName = N'Office'

UPDATE SET [Description] = N'Wszystko co zwi¹zane z rozwojem aplikcji przystosowanych do obs³ugo wiêcej ni¿ 1 maszyny'
WHERE UniqueName = N'Programowanie rozproszone'

UPDATE SET [Description] =  N'Rozwi¹zania na wy¿szym poziomie abstrakcji. Wzorce, dobre praktyki i pomys³y.'
WHERE UniqueName = N'Architecture'
COMMIT;


