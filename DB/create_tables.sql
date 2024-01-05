USE [Monitoring]
GO

CREATE TABLE ClassWallet(
	ID               UNIQUEIDENTIFIER,
	IdClassification INT,
	[Description]    VARCHAR(200),
	PRIMARY KEY (ID)
);
GO
INSERT INTO ClassWallet VALUES(NEWID(), 1, 'Top Gainer Whales');
INSERT INTO ClassWallet VALUES(NEWID(), 2, 'Second Class Of Whales');
INSERT INTO ClassWallet VALUES(NEWID(), 3, 'Buy Many things and anyone right');
INSERT INTO ClassWallet VALUES(NEWID(), 4, 'Buy Anything and less right');
GO
CREATE TABLE Wallet(
	ID               UNIQUEIDENTIFIER,
	[Hash]           VARCHAR(50),
	IdClassWallet    UNIQUEIDENTIFIER,
	PRIMARY KEY (ID),
	FOREIGN KEY (IdClassWallet) REFERENCES ClassWallet(ID)
);

CREATE TABLE Token(
	ID               UNIQUEIDENTIFIER,
	[Hash]           VARCHAR(50),
	TokenAlias       VARCHAR(100),
	Symbol			 VARCHAR(50),
	TokenType        VARCHAR(100),
	FreezeAuthority  VARCHAR(100),
	MintAuthority    VARCHAR(100),
	IsMutable        BIT,
	Decimals         INT,
	PRIMARY KEY (ID)
);

CREATE TABLE Transactions
(
	ID                  UNIQUEIDENTIFIER,
	[Signature]         VARCHAR(150),
	DateOfTransaction   DateTime2,
	AmountValue         decimal(38,20),
	IdToken             UNIQUEIDENTIFIER,
	IdWallet            UNIQUEIDENTIFIER,
	TypeOperation       INT, -- 1 For Buy, 2 For Sell, 3 For Transfer
	jsonResponse        NVARCHAR(MAX),
	PRIMARY KEY (ID),
	FOREIGN KEY (IdToken) REFERENCES Token(ID),
	FOREIGN KEY (IdWallet) REFERENCES Wallet(ID),
);
GO
DECLARE @IdClassWallet UNIQUEIDENTIFIER
SELECT @IdClassWallet = ID FROM ClassWallet WHERE IdClassification = 1

INSERT INTO Wallet VALUES (NEWID(), 'FZNrSiYifncDHTRNB6L8AyGX3sQu4T5Jb9k56S1zgTsz', @IdClassWallet);
INSERT INTO Wallet VALUES (NEWID(), 'HwQ9NTLB1QthB3Tsq9eWCXogVHWZSLZrhySiknr2cKFX', @IdClassWallet);
INSERT INTO Wallet VALUES (NEWID(), '6hqR9urgXPXnPFYybvwYdhLZ7TRKS4NcLcNivJhRr7Jf', @IdClassWallet);
INSERT INTO Wallet VALUES (NEWID(), 'DUHbm9JZ9D82h1pmRZYZAMA9U44hS4D7z6ZxyEjbMYNn', @IdClassWallet);
INSERT INTO Wallet VALUES (NEWID(), '3oc7EzM8UWf4o3MJYvt52uEL4GnTEGK72tYwGq5eskzS', @IdClassWallet);
INSERT INTO Wallet VALUES (NEWID(), 'EgZNycuVcr4YWxgjoDK3METamtSDjrPnCUs7jWgmgYSq', @IdClassWallet);
INSERT INTO Wallet VALUES (NEWID(), 'GZR6XTytmQwa2goHtq4D6F5FSJRDvA477gdC7jCrt7Qc', @IdClassWallet);


CREATE TABLE RunTimeController
(	
	IdRuntime INT,
	UnixTimeSeconds DECIMAL(20,0),
	IsRunning BIT,
	PRIMARY KEY(IdRuntime)
)

INSERT INTO RunTimeController VALUES(1, 1703976485, 0);