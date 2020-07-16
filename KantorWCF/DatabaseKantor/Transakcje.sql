CREATE TABLE [dbo].[Transakcje]
(
	[id_uzytkownika] INT NOT NULL PRIMARY KEY, 
    [typ] VARCHAR(50) NOT NULL, 
    [data] DATE NOT NULL, 
    [pln] DECIMAL NOT NULL, 
    [waluta] VARCHAR(50) NOT NULL, 
    [kurs] FLOAT NOT NULL, 
    [wynik] DECIMAL NOT NULL
)
