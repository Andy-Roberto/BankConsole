Create DATABASE master;
Use master;
DROP DATABASE master;
USE Bank;
GO
ALTER DATABASE master 
SET SINGLE_USER 
WITH ROLLBACK IMMEDIATE;
GO
DROP DATABASE master;
use Bank;
CREATE TABLE Client(
    ID Int PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(200) NOT NULL,
    PhoneNumber VARCHAR(40) NOT NULL,
    Email VARCHAR(50),
    Balance DECIMAL(10,2)
)


ALTER TABLE Client
DROP COLUMN Balance;

ALTER TABLE Client
ADD RegDate DateTime DEFAULT GETDATE();

ALTER TABLE Client
ALTER COLUMN RegDate DATETIME NOT NULL;

Insert into Client (Name,PhoneNumber,Email)
VALUES ('Pedro','17317313','pedrito@gmail.com')


Insert into Client (Name,PhoneNumber)
VALUES ('Ana','17317313')

-- Insert into Client (PhoneNumber)
-- VALUES ('17317313')

UPDATE Client SET Email = 'pedro@gmail.com' WHERE ID = 1;
UPDATE Client

DELETE From Client
WHERE ID = 1;

CREATE TABLE AccountType(
    ID INT PRIMARY KEY IDENTITY(1,1),
    NAME VARCHAR(100) NOT NULL,
    RegDate DATETIME Not Null DEFAULT GETDATE()
);
CREATE TABLE TransactionType(
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) Not NULL,
    RegDate DATETIME Not Null DEFAULT GETDATE()

);

CREATE TABLE Account(
    ID INT PRIMARY KEY IDENTITY(1,1),
    AccountType INT NOT NULL FOREIGN KEY REFERENCES AccountType(ID),
    ClientID Int Not NULL FOREIGN KEY REFERENCES Client(ID),
    Balance DECIMAL(10,2) not null,
    RegDate DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE BankTransaction
(
    ID INT PRIMARY KEY IDENTITY(1,1),
    AccountID INT NOT NULL FOREIGN KEY REFERENCES Account(ID),
    TransactionType INT NOT NULL FOREIGN KEY REFERENCES TransactionType(ID),
    ExternalAccount INT NULL,
    RegDate DATETIME not null DEFAULT GETDATE()
)
INSERT INTO AccountType (NAME)
VALUES ('Personal'),('Nómina'),('Ahorro');

INSERT INTO TransactionType ( NAME)
Values ('Depósito en Efectivo'),('Retiro en Efectivo'),
('Depósito vía Transferencia'),('Retiro vía Transferencia');


DROP TABLE Account;
DROP TABLE TransactionType;
DROP TABLE AccountType;
INSERT INTO Account(AccountType,ClientID,Balance)
VALUES (1,1,5000),(2,1,10000),(1,2,3000),(2,2,14000);

ALTER TABLE BankTransaction
ADD  Amount DECIMAL(10,2) NOT NULL;

INSERT INTO BankTransaction(AccountID,TransactionType,ExternalAccount,Amount)
VALUES (1,1,NULL,100),(1,3,123456,200),(3,1,NULL,100),(3,3,454545,250);

SELECT a.ID, acc.Name AS AccountName, c.Name as ClientName, a.Balance, a.RegDate
From Account a
inner join Client c on a.ClientID = c.ID
INNER JOIN AccountType acc on a.AccountType = acc.ID;

SELECT b.ID, C.Name as ClientName, t.Name as typeOfTransaction, b.Amount, b.ExternalAccount 
From BankTransaction b
JOIN Account a on b.AccountID = a.ID
JOIN Client c on a.ClientID = c.ID
JOIN TransactionType t on b.TransactionType = t.ID;

Alter PROCEDURE SelectAccount
    @ClientID INT = NULL
AS
    If @ClientID is NULL
    BEGIN 
        SELECT a.ID, acc.Name as AccountName, c.Name as ClientName, a.Balance, a.RegDate
        From Account a
        JOIN AccountType acc on a.AccountType = acc.ID
        lEFT JOIN Client c on a.ClientID = c.ID;
    END
    ELSE
    BEGIN
        SELECT a.ID, acc.Name as AccountName, c.Name as ClientName, a.Balance, a.RegDate
        From Account a
        JOIN AccountType acc on a.AccountType = acc.ID
        LEFT JOIN Client c on a.ClientID = c.ID
        WHERE a.ClientID = @ClientID;
    END
GO

EXEC SelectAccount @ClientID =1;

Alter PROCEDURE InsertClient
    @Name VARCHAR(200),
    @PhoneNumber VARCHAR(40),
    @Email VARCHAR(50) = NULL,
    @retorno VARCHAR(100) out
AS 
    DECLARE @Correo VARCHAR(50)
    Set @Correo = (SELECT Top 1 Email From Client WHERE Email = @Email)
    IF @Correo != @Email 
        INSERT INTO Client(Name,PhoneNumber, Email)
        VALUES (@Name,@PhoneNumber,@Email);
    ELSE 
        SET @retorno= 'Un usuario con ese correo ya existe';
        
GO
DECLARE @retorno VARCHAR(100);
 Exec InsertClient 'Alex', '1313231','pedro@gmail.com',@retorno OUTPUT;
PRINT @retorno
SELECT * FRom Client;

CREATE TRIGGER ClientAfterInsert
on Client 
After INSERT
AS  
    DECLARE @NewClientID INT;
    Set @NewClientID = (SELECT ID FROM inserted)
    INSERT INTO Account ( AccountType, ClientID, Balance)
    VALUES (1,@NewClientID,0);
GO

ALTER TABLE Account
Alter COLUMN ClientID INT null;

DECLARE @retorno VARCHAR(100)
 Exec InsertClient @Name = 'Alex', @PhoneNumber = '1313231', @Email = 'pedro@gmail.com',@;
 PRINT @retorno

SELECT * FRom Client;
CREATE TRIGGER ClientInsteadOfDelete
on Client
INSTEAD of DELETE
AS
    DECLARE @DeletedID int;
    Set @DeletedID= (SELECT ID FROM deleted)

    UPDATE Account set ClientID = null
    WHERE ClientID = @DeletedID;

    DELETE FROM Client WHERE ID = @DeletedID;
Go
DELETE From Client where ID = 3; 
Exec SelectAccount;

ALTER PROCEDURE InsertBankTransaction
    @AccountID INT,
    @TransactionType INT,
    @Amount DECIMAL(10,2),
    @ExternalAccount int = NULL,
    @retorno VARCHAR(100) out
AS
    DECLARE @CurrentBalance DECIMAL(10,2), @NewBalance DECIMAL(10,2);
    BEGIN TRANSACTION;
    Set @CurrentBalance = (SELECT Balance FROM Account where ID = @AccountID);
    IF @TransactionType = 2 or @TransactionType =4
        set @NewBalance = @CurrentBalance - @Amount;
    ELSE
        SET @NewBalance = @CurrentBalance + @Amount;
    UPDATE Account Set Balance = @NewBalance WHERE ID = @AccountID;

    INSERT INTO BankTransaction (AccountID,TransactionType, Amount, ExternalAccount)
    VALUES (@AccountID,@TransactionType, @Amount, @ExternalAccount)

    IF @NewBalance >=0
        COMMIT TRANSACTION;
    ELSE
        BEGIN
        ROLLBACK TRANSACTION;
        set @retorno= 'El valor de la transaccion es mayor del que hay actualmente en la cuenta';
        END
GO
DECLARE @retorno VARCHAR (100) =NULL
EXEC InsertBankTransaction  1,2,2000,NULL,@retorno Output;
Print @retorno
Exec SelectAccount;
SELECT * FROM Account;
SELECT * FROM BankTransaction;
SELECT * from TransactionType;
SELECT * from AccountType;
SELECT * FROM Client;

Create PROCEDURE SelectClient
    @ClientID INT = NULL
AS
    If @ClientID is NULL
    BEGIN 
        SELECT ID, Name, PhoneNumber,Email,RegDate From Client;
    END
    ELSE
    BEGIN
        SELECT ID, Name, PhoneNumber,Email,RegDate From Client
        WHERE ID = @ClientID;
    END
GO
Exec SelectClient @ClientID = NULL;
DELETE From Client where ID=3;



BACKUP DATABASE Bank to disk ='H:\Bank1231.bak';