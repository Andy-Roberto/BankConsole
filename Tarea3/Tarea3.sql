Alter PROCEDURE InsertClient
    @Name VARCHAR(200),
    @PhoneNumber VARCHAR(40),
    @Email VARCHAR(50) = NULL,
    @retorno VARCHAR(100) out
AS 
    DECLARE @Correo VARCHAR(50)
    Set @Correo = (SELECT Top 1 Email From Client WHERE Email = @Email)
    IF @Correo is NULL 
        INSERT INTO Client(Name,PhoneNumber, Email)VALUES (@Name,@PhoneNumber,@Email);
    ELSE 
        SET @retorno= 'Un usuario con ese correo ya existe';
        
GO

DECLARE @retorno VARCHAR(100);
 Exec InsertClient 'Alexis', '1313231','Alex@gmail.com',@retorno OUTPUT;
PRINT @retorno
SELECT * FRom Client;

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
EXEC InsertBankTransaction  1,2,500,NULL,@retorno Output;
Print @retorno
Exec SelectAccount;


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
Exec SelectClient @ClientID = 10;

BACKUP DATABASE Bank to disk ='H:\Bank1231.bak';