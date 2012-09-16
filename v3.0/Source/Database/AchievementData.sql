SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[Achievement]([Id], [Name], [Description], [Type])
SELECT N'2e8f0603-0eb3-4412-a864-854448b894be', N'StoryAdderBadge', N'Doda³ przynajmniej jeden artyku³', 0 UNION ALL
SELECT N'1fd420ba-3104-436e-a7c8-87897dcfe954', N'NightOwlBadge', N'Odwiedzi³ serwis pomiêdzy 2 a 3 w nocy', 0 UNION ALL
SELECT N'f5cc5705-5559-4818-a440-8c77f7d0b30c', N'EarlyBirdBadge', N'Odwiedzi³ serwis pomiêdzy 4 a 5 rano', 0 UNION ALL
SELECT N'77287de2-5f75-483f-be1f-a544d266cfd8', N'_1kPointsBadge', N'Uzyska³ ponad 1k punktów', 0 UNION ALL
SELECT N'0ac5293b-55f4-48f0-a8d5-ce427406ba2b', N'_10kPointsBadge', N'Uzyska³ ponad 10k punktów', 0
COMMIT;
RAISERROR (N'[dbo].[Achievement]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO

