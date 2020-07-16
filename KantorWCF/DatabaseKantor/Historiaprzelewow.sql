CREATE TABLE [dbo].[Historiaprzelewow]
(
	[id_przelewu] INT          IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [id_uzytkownika] INT NOT NULL, 
    [typ] VARCHAR(50) NOT NULL, 
    [waluta] VARCHAR(50) NOT NULL, 
    [kwota] DECIMAL NOT NULL, 
    [data] DATE NOT NULL
)
