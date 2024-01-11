USE [Monitoring]
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'WalletBalance')
BEGIN
	DROP TABLE [WalletBalance]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'RunTimeController')
BEGIN
	DROP TABLE [RunTimeController]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'Transactions')
BEGIN
	DROP TABLE [Transactions]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'Token')
BEGIN
	DROP TABLE [Token]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'Wallet')
BEGIN
	DROP TABLE [Wallet]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'ClassWallet')
BEGIN
	DROP TABLE [ClassWallet]
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TelegramChannel')
BEGIN
	CREATE TABLE TelegramChannel(
		ID           UNIQUEIDENTIFIER,
		ChannelId    DECIMAL(30,0),
		ChannelName  VARCHAR(50),
		PRIMARY KEY (ID)
	)
END
GO 
CREATE TABLE ClassWallet(
	ID               UNIQUEIDENTIFIER,
	IdClassification INT,
	[Description]    VARCHAR(200),
	PRIMARY KEY (ID)
);
GO
INSERT INTO ClassWallet VALUES(NEWID(), 1, 'Whales');
INSERT INTO ClassWallet VALUES(NEWID(), 2, 'MM');
INSERT INTO ClassWallet VALUES(NEWID(), 3, 'Asians');
INSERT INTO ClassWallet VALUES(NEWID(), 4, 'Arbitradores');
GO
CREATE TABLE Wallet(
	ID               UNIQUEIDENTIFIER,
	[Hash]           VARCHAR(50),
	IdClassWallet    UNIQUEIDENTIFIER,
	IsActive         BIT,
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
	ID                         UNIQUEIDENTIFIER,
	[Signature]                VARCHAR(150),
	DateOfTransaction          DATETIME2,
	AmountValueSource          DECIMAL(38,18),
	AmountValueSourcePool      DECIMAL(38,18),
	AmountValueDestination     DECIMAL(38,18),
	AmountValueDestinationPool DECIMAL(38,18),
	IdTokenSource              UNIQUEIDENTIFIER,
	IdTokenSourcePool          UNIQUEIDENTIFIER,
	IdTokenDestination         UNIQUEIDENTIFIER,
	IdTokenDestinationPool     UNIQUEIDENTIFIER,
	IdWallet                   UNIQUEIDENTIFIER,
	TypeOperation              INT, -- 1 For Buy, 2 For Sell, 3 For Transfer, 4 For Received, 5 SWAP, 6 POOL CREATE, 7 POOL FINALIZED
	PRIMARY KEY (ID),
	FOREIGN KEY (IdTokenSource) REFERENCES Token(ID),
	FOREIGN KEY (IdTokenSourcePool) REFERENCES Token(ID),
	FOREIGN KEY (IdTokenDestination) REFERENCES Token(ID),
	FOREIGN KEY (IdTokenDestinationPool) REFERENCES Token(ID),
	FOREIGN KEY (IdWallet) REFERENCES Wallet(ID),
);
GO
DECLARE @IdClassWallet UNIQUEIDENTIFIER
SELECT @IdClassWallet = ID FROM ClassWallet WHERE IdClassification = 1

INSERT INTO Wallet VALUES (NEWID(), 'FZNrSiYifncDHTRNB6L8AyGX3sQu4T5Jb9k56S1zgTsz', @IdClassWallet, 1);
INSERT INTO Wallet VALUES (NEWID(), 'HwQ9NTLB1QthB3Tsq9eWCXogVHWZSLZrhySiknr2cKFX', @IdClassWallet, 1);
INSERT INTO Wallet VALUES (NEWID(), '6hqR9urgXPXnPFYybvwYdhLZ7TRKS4NcLcNivJhRr7Jf', @IdClassWallet, 1);
INSERT INTO Wallet VALUES (NEWID(), 'DUHbm9JZ9D82h1pmRZYZAMA9U44hS4D7z6ZxyEjbMYNn', @IdClassWallet, 1);
INSERT INTO Wallet VALUES (NEWID(), '3oc7EzM8UWf4o3MJYvt52uEL4GnTEGK72tYwGq5eskzS', @IdClassWallet, 1);
INSERT INTO Wallet VALUES (NEWID(), 'EgZNycuVcr4YWxgjoDK3METamtSDjrPnCUs7jWgmgYSq', @IdClassWallet, 1);
INSERT INTO Wallet VALUES (NEWID(), 'GZR6XTytmQwa2goHtq4D6F5FSJRDvA477gdC7jCrt7Qc', @IdClassWallet, 1);

SELECT @IdClassWallet = ID FROM ClassWallet WHERE IdClassification = 2
INSERT INTO Wallet VALUES (NEWID(), 'GhuBeitd7eh8KwCurXy1tFCRxGphpVxa8X4rUX8dQxHc', @IdClassWallet, 1);

CREATE TABLE RunTimeController
(	
	IdRuntime INT,
	UnixTimeSeconds DECIMAL(20,0),
	IsRunning BIT,
	PRIMARY KEY(IdRuntime)
);

CREATE TABLE WalletBalance
(
	ID         UNIQUEIDENTIFIER,
	DateUpdate DATETIME2,
	IdWallet   UNIQUEIDENTIFIER,
	IdToken    UNIQUEIDENTIFIER,
	Quantity   DECIMAL(38,18),
	PRIMARY KEY (ID),
	FOREIGN KEY (IdWallet) REFERENCES Wallet(ID),
	FOREIGN KEY(IdToken) REFERENCES Token(ID),
);

INSERT INTO RunTimeController VALUES(1, 1703976485, 0);