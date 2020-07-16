CREATE TABLE [dbo].[Uzytkownicy]
(
	[id_user] INT NOT NULL PRIMARY KEY, 
    [imie] VARCHAR(50) NOT NULL, 
    [nazwisko] VARCHAR(50) NOT NULL, 
    [email] VARCHAR(50) NOT NULL, 
    [haslo] VARCHAR(50) NOT NULL
)
