Create Table Administrator(
    ID int PRIMARY KEY IDENTITY(1,1),
    Name Varchar(200) not null,
    PhoneNumber VARCHAR(40) not null,
    Email VARCHAR(50) not null,
    Pwd VARCHAR(50) not NULL,
    AdminType VARCHAR(30) Not null,
    RegDate DateTime not null DEFAULT GETDATE()
)