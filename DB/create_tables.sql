USE [Monitoring]
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TokenAlphaConfiguration')
BEGIN
	DROP TABLE [TokenAlphaConfiguration]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TokenAlphaWallet')
BEGIN
	DROP TABLE [TokenAlphaWallet]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TokenAlpha')
BEGIN
	DROP TABLE [TokenAlpha]
END
GO


IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'WalletBalance')
BEGIN
	DROP TABLE [WalletBalance]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'WalletBalanceSFMCompare')
BEGIN
	DROP TABLE [WalletBalanceSFMCompare]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TransactionsOldForMapping')
BEGIN
	DROP TABLE [TransactionsOldForMapping]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'WalletBalanceHistory')
BEGIN
	DROP TABLE [WalletBalanceHistory]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'Transactions')
BEGIN
	DROP TABLE [Transactions]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TokenSecurity')
BEGIN
	DROP TABLE [TokenSecurity]
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
IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'RunTimeController')
BEGIN
	DROP TABLE [RunTimeController]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'AlertPrice')
BEGIN
	DROP TABLE [AlertPrice]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'AlertParameter')
BEGIN
	DROP TABLE [AlertParameter]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'AlertInformation')
BEGIN
	DROP TABLE [AlertInformation]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'AlertConfiguration')
BEGIN
	DROP TABLE [AlertConfiguration]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TelegramMessage')
BEGIN
	DROP TABLE [TelegramMessage]
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


IF NOT EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'AlertPrice')
BEGIN
	CREATE TABLE AlertPrice(
		ID                UNIQUEIDENTIFIER,
		CreateDate        DATETIME2,
		EndDate           DATETIME2,
		PriceBase         VARCHAR(100),
		TokenHash         VARCHAR(100),
		PriceValue        VARCHAR(100),
		PricePercent      DECIMAL(5,2),
		TypeAlert         INT,
		IsRecurrence      BIT,
		TelegramChannelId UNIQUEIDENTIFIER,   
		PRIMARY KEY(ID),
		FOREIGN KEY (TelegramChannelId) REFERENCES TelegramChannel(ID)
	);
	--DELETE FROM AlertPrice
	DECLARE @TelegramChannelId UNIQUEIDENTIFIER
	SELECT @TelegramChannelId = ID FROM TelegramChannel WHERE ChannelName = 'AlertPriceChange'
	INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '1.958', 'BjBzvw6VX7UJtrC7BaYLG1dHBiwrXP1T9j2YfDEdP4zU', '22.3', null, 1, 0, @TelegramChannelId);
	INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '85.60', 'So11111111111111111111111111111111111111112', '81.58', null, 2, 1, @TelegramChannelId);
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'RunTimeController')
BEGIN
	CREATE TABLE RunTimeController
	(	
		IdRuntime				 INT,
		ConfigurationTimer		 INT,
		TypeService				 INT,
		IsRunning				 BIT,
		IsContingecyTransactions BIT,
		TimesWithoutTransactions INT,
		PRIMARY KEY(IdRuntime)
	);
	INSERT INTO RunTimeController VALUES(1, 1, 1, 0, 0, null);
	INSERT INTO RunTimeController VALUES(2, 1, 2, 0, 0, null);
	INSERT INTO RunTimeController VALUES(3, 1, 3, 0, 0, null);
	INSERT INTO RunTimeController VALUES(4, 15, 4, 0, 0, null);
END
GO 

IF NOT EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TransactionNotMapped')
BEGIN
	CREATE TABLE TransactionNotMapped(
		ID                UNIQUEIDENTIFIER,
		[IdWallet]        UNIQUEIDENTIFIER,
		[Signature]       VARCHAR(150),
		[Link]            VARCHAR(500),
		[Error]           VARCHAR(500),
		[StackTrace]      NVARCHAR(MAX),
		[DateTimeRunner]  DATETIME2,
		PRIMARY KEY (ID)
	);
END
GO 

CREATE TABLE ClassWallet(
	ID               UNIQUEIDENTIFIER,
	IdClassification INT,
	[Description]    VARCHAR(200),
	PRIMARY KEY (ID)
);
GO
INSERT INTO ClassWallet VALUES(NEWID(), 1, 'Premier');
INSERT INTO ClassWallet VALUES(NEWID(), 2, 'Whales');
INSERT INTO ClassWallet VALUES(NEWID(), 3, 'MM');
INSERT INTO ClassWallet VALUES(NEWID(), 4, 'Whales Top Gainers');
INSERT INTO ClassWallet VALUES(NEWID(), 5, 'Arbitradores');
INSERT INTO ClassWallet VALUES(NEWID(), 6, 'BIG BIG Whale');
GO
CREATE TABLE Wallet(
	ID                   UNIQUEIDENTIFIER,
	[Hash]               VARCHAR(50),
	IdClassWallet        UNIQUEIDENTIFIER,
	UnixTimeSeconds      DECIMAL(20,0),
	IsLoadBalance        BIT,
	DateLoadBalance      DATETIME2,
	OldTransactionStared DATETIME2,
	OldTransactionHours  INT,
	IsActive         BIT,
	LastUpdate       DATETIME2, 
	PRIMARY KEY (ID),
	FOREIGN KEY (IdClassWallet) REFERENCES ClassWallet(ID)
);

CREATE TABLE Token(
	ID                     UNIQUEIDENTIFIER,
	[Hash]                 VARCHAR(50),
	Symbol                 VARCHAR(50),
	[Name]			       VARCHAR(200),
	Supply                 VARCHAR(150),
	MarketCap              VARCHAR(150),
	Liquidity              VARCHAR(150),
	UniqueWallet24h        INT,
	UniqueWalletHistory24h INT,
	Decimals               INT,
	NumberMarkets          INT,
	CreateDate             DATETIME,
	LastUpdate             DATETIME,
	PRIMARY KEY (ID)
);

CREATE TABLE TokenSecurity(
    ID                 UNIQUEIDENTIFIER,
	IdToken            UNIQUEIDENTIFIER,
	CreatorAddress     VARCHAR(100),
	CreationTime       DECIMAL(20,0),
	Top10HolderBalance VARCHAR(150),
	Top10HolderPercent VARCHAR(150),
	Top10UserBalance   VARCHAR(150),
	Top10UserPercent   VARCHAR(150),
	IsTrueToken        BIT,
	LockInfo		   VARCHAR(100),
	Freezeable		   VARCHAR(100),
	FreezeAuthority    VARCHAR(100),
	TransferFeeEnable  VARCHAR(100),
	TransferFeeData    VARCHAR(100),
	IsToken2022        BIT,
	NonTransferable    VARCHAR(100),
	MintAuthority      VARCHAR(100),
	IsMutable          BIT,
	PRIMARY KEY (ID),
	FOREIGN KEY (IdToken) REFERENCES Token(ID)
);

CREATE TABLE Transactions
(
	ID                           UNIQUEIDENTIFIER,
	[Signature]                  VARCHAR(150),
	DateOfTransaction            DATETIME2,
	AmountValueSource            VARCHAR(150),
	AmountValueSourcePool        VARCHAR(150),
	AmountValueDestination       VARCHAR(150),
	AmountValueDestinationPool   VARCHAR(150),
	FeeTransaction               VARCHAR(150),
	MtkcapTokenSource            VARCHAR(150),
	MtkcapTokenSourcePool        VARCHAR(150),
	MtkcapTokenDestination       VARCHAR(150),
	MtkcapTokenDestinationPool   VARCHAR(150),
	PriceTokenSourceUSD          VARCHAR(150),
	PriceTokenSourcePoolUSD      VARCHAR(150),
	PriceTokenDestinationUSD     VARCHAR(150),
	PriceTokenDestinationPoolUSD VARCHAR(150),
	PriceSol				     VARCHAR(150),
	TotalTokenSource             VARCHAR(150),
	TotalTokenSourcePool         VARCHAR(150),
	TotalTokenDestination        VARCHAR(150),
	TotalTokenDestinationPool    VARCHAR(150),
	IdTokenSource                UNIQUEIDENTIFIER,
	IdTokenSourcePool            UNIQUEIDENTIFIER,
	IdTokenDestination           UNIQUEIDENTIFIER,
	IdTokenDestinationPool       UNIQUEIDENTIFIER,
	IdWallet                     UNIQUEIDENTIFIER,
	TypeOperation                INT, -- 1 For Buy, 2 For Sell, 3 For Transfer, 4 For Received, 5 SWAP, 6 POOL CREATE, 7 POOL FINALIZED
	PRIMARY KEY (ID),
	FOREIGN KEY (IdTokenSource) REFERENCES Token(ID),
	FOREIGN KEY (IdTokenSourcePool) REFERENCES Token(ID),
	FOREIGN KEY (IdTokenDestination) REFERENCES Token(ID),
	FOREIGN KEY (IdTokenDestinationPool) REFERENCES Token(ID),
	FOREIGN KEY (IdWallet) REFERENCES Wallet(ID),
);
GO

CREATE TABLE TransactionsOldForMapping
(
	ID                           UNIQUEIDENTIFIER,
	[Signature]                  VARCHAR(150),
	DateOfTransaction            DATETIME2,
	IdWallet                     UNIQUEIDENTIFIER,
	CreateDate				     DATETIME2,
	IsIntegrated				 BIT,
	PRIMARY KEY (ID),
	FOREIGN KEY (IdWallet) REFERENCES Wallet(ID),
);
GO
DECLARE @IdClassWallet UNIQUEIDENTIFIER
SELECT @IdClassWallet = ID FROM ClassWallet WHERE IdClassification = 1
INSERT INTO Wallet VALUES (NEWID(),'3oc7EzM8UWf4o3MJYvt52uEL4GnTEGK72tYwGq5eskzS', @IdClassWallet, null, 0, null, null, 1, 1, null); 
INSERT INTO Wallet VALUES (NEWID(),'FZNrSiYifncDHTRNB6L8AyGX3sQu4T5Jb9k56S1zgTsz', @IdClassWallet, null, 0, null, null, 1, 1, null);
SELECT @IdClassWallet = ID FROM ClassWallet WHERE IdClassification = 2
INSERT INTO Wallet VALUES (NEWID(),'HwQ9NTLB1QthB3Tsq9eWCXogVHWZSLZrhySiknr2cKFX', @IdClassWallet, null, 0, null, null, 1, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'DUHbm9JZ9D82h1pmRZYZAMA9U44hS4D7z6ZxyEjbMYNn', @IdClassWallet, null, 0, null, null, 1, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'EgZNycuVcr4YWxgjoDK3METamtSDjrPnCUs7jWgmgYSq', @IdClassWallet, null, 0, null, null, 1, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'GZR6XTytmQwa2goHtq4D6F5FSJRDvA477gdC7jCrt7Qc', @IdClassWallet, null, 0, null, null, 1, 1, null);


--SELECT @IdClassWallet = ID FROM ClassWallet WHERE IdClassification = 2
--INSERT INTO Wallet VALUES (NEWID(), 'GhuBeitd7eh8KwCurXy1tFCRxGphpVxa8X4rUX8dQxHc', @IdClassWallet, null, 0, null, null, 1, 1, null);

--SELECT @IdClassWallet = ID FROM ClassWallet WHERE IdClassification = 3
--INSERT INTO Wallet VALUES (NEWID(),'HzoNzi7mLVCxsa9EkdBmoob75rkXCjWLHy131ch1oEbX', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'43QLsYdyomxCyoZiz1W18LaZaY3tevxLM4KyJWnVeFaB', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'8usu4t61wPgUjwU1qohajaYGDjqz43Jp8oGMJ4Sa82A7', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'CGLBeFjXSAeKGKGPKjT8E8sHcDD72SCAWEV4jgXpY4aK', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'2bAVD7hHAoLJV939Goo5id6uguHXHaQDbqqsFijwcPEC', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'CLee88KHw1tBdpY1hkxDgendy7UjCD3PMGDY4xbLTbys', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'2cSsWeb3v7uD4dGgcqoCNWBwBcos9iK7jEEfyVTH2LSZ', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'D7Lwt6j2hzDhy14oC5YdfXh4pPeoAGbaNWQZX2UCPUKZ', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'Erja5wNDfvvWWvYe9sHYgjxBKpqxaK2Uk33mEV3Ts5du', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'6hqR9urgXPXnPFYybvwYdhLZ7TRKS4NcLcNivJhRr7Jf', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'BsgFVpmEBB3Ps39aNQJPXt5mndbEQ7aP3fLXNRL8BTxW', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'DqFEdkAjcqqPV7C1kmjhyCayxCaJsdjb7JWHUKZk7hYX', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'CxaA6Dmhq8xf9dfo3SuvupHB2mCPQLc1K6wkv8i6us6d', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'HzMZ89kGbgfut7KgyFk1GXdWibUK4owwXZsoKDnuqNQL', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'9UG3uahH8ejWKM7VwMTQtTHm6YERyPPuWtJNUHfCyabV', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'Asyuz72ArUZ1yMW2WQtdwjCaN7uJN4iD41jKm6SY8e65', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'HSAxeaos35XJAAeua67Bdw3rnSCecPziCokXSGWR4fQD', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'DEPErBeK3Ej3C5VRncMQJkmCcn7bCxKLHqEDSMxui3Lh', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'4bce4DGEBmgL5cuQUSs5CEp9PBMFQPWCZNkpfKmWytHo', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'9Ltbz7RQr7Bd3dCD3qmhkWNwR2fAfKAn6NXEjPrd9UUE', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'6C8qysda5dg8WpPJoj6iMLELhtLzQe2A7nnSA1A7ttpF', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'7B7XQbX4ZigQgP28sEHQwicAmfjgRD47pPkKcfPhMeA5', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'CMup3m7gDxMTCbMu1xQVtjRmNRH2kTJESPPhkD3j7XUk', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'A5f6HGc2yjEDV9C4kMj3XLLsHvkVqduiV1CFvnafer7j', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'GoKi3UyMz7nELSGnqPa68KgDbV4cW4eJTPKMkrPugQpL', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'22PwbCzNaxKAbMBB3BYAN8rUXu4TARsRbEyemFqpSejz', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'BgwNWZAnejxMcCBBcSfwgTZLt1XzHd7pL3f7EHQxURNy', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'BuxtrMH8FVGENMYKn6LFzHw5tyCq5CrTQzJyC3HRCSBy', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'ufQ6EY7bzKdWNpC8hZH86WEzX6zkgqYd2eGaKbDyTU3',  @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'3gtDsHFSAN3JrBhW9gMiksZVHHZNUsz4AnoQcLqkTGnm', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'6iK4DpQd48qW65Kav1zrs3uYAtpcZFDZLE9jafYTf35C', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'6ZzWpyjbieCqzCDyaeYZKZmT9C3tcprDNfQR6xpzB1yr', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'8XqHwUHZWA7iwFw3Jydd4duiEF2bGdtSmnbPMjpYDS2n', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'2FMPgFspCouJTcifuYwF1AdDKa2poH7Tqm6HSDSiGrGo', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'97kcP9Ss2YnkwEA2ovT1C4UdAJ3mQ1hJgVPZxAwvDrKj', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'ECL1PSED78CqHB7DqVk73bEC3PyDSmKNvq85Y8Xsehwd', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'FkYptuxkuz7CSrjNMesdD3SqPHjGrVJB1YUxhKA3HGxA', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'3UUdqsU7gajKwL58mwRBPFsXoxRakLPY63HcEGj2W2MD', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'HYrAcw6FwNVgu7CJ6SecS1rxJdDnRuZhEtUCTQKcVVuT', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'AnqkPLfaRX4H69mT4dmoH2V4T4VzcHzoTkPqPib15Hwx', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'GqLsGpJyvPJBKPpm87zXHUKh4ywQC7cUghbkXwdGbvdT', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'CnHCkUwcdZ3mgf7vXoiKMejaKFUF8tnA9VXL2CuxMaXt', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'9z3yhh9nTToHTuRJrk9485ozXb5EPbSALrKd9pRNCF5q', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'BpjaWFG3Q7ia19jmZbPsWB47YGHQWGwDBYHoCGnLD3WT', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'DdymDLAwbsn5R3cN26d2NdmhiKv9sAGDFnPBbsPT9ut1', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'FTQWvJFb5tBhEQTtyTxzJdV3QvTsJh14ofRC9S6DE5Lq', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'fstYfhMw3Petz3df1j9yY8E5nHbkkpexqCnpFahzUSg',  @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'31HWL9p9k4E8D3KgW34YWzu8rw6aLjevpd449zUTG6cc', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'636ZbmWmrg2Dgpi29qncZNARUFNXvVHwoqwmRZtrfMXS', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'3xWhH42D1skKHhpanvBdn6zvoPiXoThRcuaMEjqjzmmA', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'AR1Eb6GjQScNWc8HBvzE19TMksu4ambaP87wppQ4g7vz', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'8ZPqRatB9U43DYoaVk9YZKZTnjNQR9uK1HaGaPKgvKzZ', @IdClassWallet, null, 0, null, null, 1, 1, null);
--INSERT INTO Wallet VALUES (NEWID(),'7s3MckqAqdci8b1y8QZ4oYL1PDz1yPBmb1PeZfBTVnac', @IdClassWallet, null, 0, null, null, 1, 1, null);

--SELECT @IdClassWallet = ID FROM ClassWallet WHERE IdClassification = 5
--INSERT INTO Wallet VALUES (NEWID(),'F3SexpfyD785mndnj38EGoxZdAAQEMxBmJCvNe15rz5B', @IdClassWallet, null, 0, null, null, 1, 1, null);
CREATE TABLE WalletBalance
(
	ID            UNIQUEIDENTIFIER,
	IdWallet      UNIQUEIDENTIFIER,
	IdToken       UNIQUEIDENTIFIER,
	TokenHash     VARCHAR(100),
	Quantity      VARCHAR(100),
	Price         VARCHAR(100),
	TotalValueUSD VARCHAR(100),
	IsActive      BIT,
	LastUpdate    DATETIME2,
	PRIMARY KEY (ID),
	FOREIGN KEY (IdWallet) REFERENCES Wallet(ID),
	FOREIGN KEY(IdToken) REFERENCES Token(ID),
);

CREATE TABLE WalletBalanceSFMCompare(
	ID            UNIQUEIDENTIFIER,
	IdWallet      UNIQUEIDENTIFIER,
	IdToken       UNIQUEIDENTIFIER,
	TokenHash     VARCHAR(100),
	Quantity      VARCHAR(100),
	Price         VARCHAR(100),
	TotalValueUSD VARCHAR(100),
	IsActive      BIT,
	LastUpdate    DATETIME2,
	PRIMARY KEY (ID),
	FOREIGN KEY (IdWallet) REFERENCES Wallet(ID),
	FOREIGN KEY(IdToken) REFERENCES Token(ID),
);

CREATE TABLE WalletBalanceHistory
(
	ID                    UNIQUEIDENTIFIER,
	IDWalletBalance       UNIQUEIDENTIFIER,
	IdWallet              UNIQUEIDENTIFIER,
	IdToken               UNIQUEIDENTIFIER,
	TokenHash             VARCHAR(100),
	OldQuantity           VARCHAR(100),
	NewQuantity           VARCHAR(100),
	RequestQuantity       VARCHAR(100),
	PercentageCalculated  VARCHAR(100),
	Price                 VARCHAR(100),
	TotalValueUSD         VARCHAR(100),
	[Signature]           VARCHAR(150),
	FontType			  INT,
	CreateDate            DATETIME2,
	LastUpdate            DATETIME2,
	PRIMARY KEY (ID)
);
GO

-- Telegram Messages
CREATE TABLE TelegramMessage
(
	ID                 UNIQUEIDENTIFIER,
	MessageId          BIGINT,
	TelegramChannelId  UNIQUEIDENTIFIER,
	DateSended         DATETIME2,
	IsDeleted		   BIT
	PRIMARY KEY (ID),
	FOREIGN KEY(TelegramChannelId) REFERENCES TelegramChannel(ID)
);
GO

-- ALERTS
CREATE TABLE AlertConfiguration(
	ID                    UNIQUEIDENTIFIER,
	[Name]                VARCHAR(200),
	TypeAlert             INT, -- 1 BUY, 2 - REBUY, 3 - SELL, 4 - SWAP, 5 - POOL CREATE, 6 - POOL FINISH
	TelegramChannelId     UNIQUEIDENTIFIER,
	IsActive              BIT,
	CreateDate            DATETIME2,
	LastUpdate            DATETIME2,
	PRIMARY KEY (ID),
	FOREIGN KEY (TelegramChannelId) REFERENCES TelegramChannel(ID)
);

DECLARE @IdTelegramChannel UNIQUEIDENTIFIER;
SELECT @IdTelegramChannel = ID FROM TelegramChannel WHERE ChannelName = 'CallSolanaLog';
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Log Execute', -1, @IdTelegramChannel, 1, GETDATE(), GETDATE());
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Log Service Running', -2, @IdTelegramChannel, 1, GETDATE(), GETDATE());
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Log Error', -3, @IdTelegramChannel, 1, GETDATE(), GETDATE());
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Log Service Lost Configuration', -4, @IdTelegramChannel, 1, GETDATE(), GETDATE());
SELECT @IdTelegramChannel = ID FROM TelegramChannel WHERE ChannelName = 'CallSolana';
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Buy', 1, @IdTelegramChannel, 1, GETDATE(), GETDATE());
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Rebuy', 2, @IdTelegramChannel,  1, GETDATE(), GETDATE());
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Sell', 3, @IdTelegramChannel, 1, GETDATE(), GETDATE());
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Swap', 4, @IdTelegramChannel, 1, GETDATE(), GETDATE());
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Pool Create', 5, @IdTelegramChannel, 1, GETDATE(), GETDATE());
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Pool Finish', 6, @IdTelegramChannel, 1, GETDATE(), GETDATE());
SELECT @IdTelegramChannel = ID FROM TelegramChannel WHERE ChannelName = 'AlertPriceChange';
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Price', 7, @IdTelegramChannel, 1, GETDATE(), GETDATE());
CREATE TABLE AlertInformation(
	ID                    UNIQUEIDENTIFIER,
	[Message]             VARCHAR(4000) COLLATE Latin1_General_100_CI_AI_SC_UTF8,
	IdClassification      INT,
	AlertConfigurationId  UNIQUEIDENTIFIER,
	PRIMARY KEY (ID),
	FOREIGN KEY (AlertConfigurationId) REFERENCES AlertConfiguration(ID),
	
);

CREATE TABLE AlertParameter(
	ID                    UNIQUEIDENTIFIER,
	[Name]                VARCHAR(200),
	AlertInformationId    UNIQUEIDENTIFIER,
	[Class]               VARCHAR(255),
	[Parameter]           VARCHAR(255),
	[FixValue]            VARCHAR(200),
	[DefaultValue]        VARCHAR(200),
	[HasAdjustment]       BIT,
	IsIcon				  BIT,
	IsImage               BIT
	PRIMARY KEY (ID),
	FOREIGN KEY (AlertInformationId) REFERENCES AlertInformation(ID),
);


CREATE TABLE TokenAlphaConfiguration
(
	ID            UNIQUEIDENTIFIER,
	MaxMarketcap  VARCHAR(100),
	DateOfLaunch  DATETIME2,
);
GO

CREATE TABLE TokenAlpha(
	ID          UNIQUEIDENTIFIER,
	TokenId     UNIQUEIDENTIFIER,
	CallNumber  INT,
	Marketcap   VARCHAR(100),
	Price       VARCHAR(100),
	CreateDate  DATETIME2,
	LastUpdate  DATETIME2,
	PRIMARY KEY (ID),
	FOREIGN KEY (TokenId) REFERENCES Token(ID),
);
GO
CREATE TABLE TokenAlphaWallet(
	ID            UNIQUEIDENTIFIER,
	TokenAlphaId  UNIQUEIDENTIFIER,
	WalletId      UNIQUEIDENTIFIER,
	PRIMARY KEY (ID),
	FOREIGN KEY (TokenAlphaId) REFERENCES TokenAlpha(ID),
	FOREIGN KEY (WalletId) REFERENCES Wallet(ID)
);
GO


DECLARE @IdAlertConfiguration UNIQUEIDENTIFIER;
DECLARE @IdAlertInformation UNIQUEIDENTIFIER;
SELECT @IdAlertInformation = NEWID();
-- LOG SUCESSO
SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = -1;
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>Execução do serviço {{ServiceName}} de call solana</b>{{NEWLINE}}<b>Data Execução: </b>{{DateTimeNow}}.{{NEWLINE}}<i><b>Proxima execução</b> no período timer de --> {{TimerExecute}}</i>{{NEWLINE}}', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ServiceName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'ServiceName', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{DateTimeNow}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'DateExecuted', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TimerExecute}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'Timer', NULL, NULL, 0, 0, 0);
-- LOG RUNNING
SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = -2;
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>O serviço {{ServiceName}} está rodando.</b>{{NEWLINE}}<i><b>Não irá efetuar essa execução:</b> {{DateTimeNow}}</i>.{{NEWLINE}}', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ServiceName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'ServiceName', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{DateTimeNow}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'DateExecuted', NULL, NULL, 0, 0, 0);
-- LOG ERROR
SELECT @IdAlertConfiguration= ID FROM AlertConfiguration WHERE TypeAlert = -3;
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>O serviço {{ServiceName}} suspendeu a execução.</b>{{NEWLINE}}<i><b>Mensagem de erro:</b> {{ErrorMessage}}</i>.{{NEWLINE}}StackTrace: {{StackTrace}}{{NEWLINE}}<i><b>Proxima execução</b> no período timer de --> {{TimerExecute}}.{{NEWLINE}}<b>Dev''s Favor verificar</b> Cc:@evandrotartari , @euRodrigo</i>{{NEWLINE}}', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ServiceName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'ServiceName', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{DateTimeNow}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'DateExecuted', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ErrorMessage}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'Exception.Message', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{StackTrace}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'Exception.StackTrace', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TimerExecute}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'Timer', NULL, NULL, 0, 0, 0);
-- LOG LOST CONFIGURATION
SELECT @IdAlertConfiguration= ID FROM AlertConfiguration WHERE TypeAlert = -4;
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>Timer do serviço {{ServiceName}} está nulo ou não configurado.</b>{{NEWLINE}}<i><b>Não irá efetuar essa execução:</b> {{DateTimeNow}}</i>.{{NEWLINE}}', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ServiceName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'ServiceName', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{DateTimeNow}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'DateExecuted', NULL, NULL, 0, 0, 0);

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 1; --BUY
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** NEW BUY ALERT ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>🟢🟢🟢🟢🟢🟢🟢🟢🟢🟢🟢</tg-emoji>{{NEWLINE}}<s>Signature:</s> {{Signature}}{{NEWLINE}}<s>WalletHash:</s> {{WalletHash}{{NEWLINE}}<s>ClassWallet:</s> {{ClassWallet}} {{NEWLINE}}<s>Token:</s> {{Token}}{{NEWLINE}}<s>Ca:</s> {{Ca}}<pre>{{NEWLINE}}</pre><s>Minth Authority:</s>{{MinthAuthority}}{{NEWLINE}}<s>Freeze Authority:</s> {{FreezeAuthority}}{{NEWLINE}}<s>Is Mutable:</s>{{IsMutable}}<s>Quantity:</s> {{Quantity}} {{QuantitySymbol}} {{NEWLINE}}<s>Value Spent:</s> {{ValueSpent}} {{ValueSpentSymbol}}{{NEWLINE}}<s>Date:</s> {{Date}}{{NEWLINE}}<s>Position Increase</s> {{PositionIncrease}} % {{NEWLINE}}<a href=''https://birdeye.so/token/{{Ca}}?chain=solana''>Chart</a>', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Signature}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'Signature', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{WalletHash}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'WalletHash', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ClassWallet}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'ClassWallet', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Token}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Name', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Ca}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Hash', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{MinthAuthority}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].MintAuthority', NULL, 'NO', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{FreezeAuthority}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].FreezeAuthority', NULL, 'NO', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{IsMutable}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].IsMutable', NULL, 'NO', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Quantity}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueDestination', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantitySymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Symbol', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSpent}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', 'AmountValueSource', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSpentSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[0].Symbol', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Date}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Utils.Transfer.TransferInfo', 'DataOfTransfer', NULL, NULL, 1, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{PositionIncrease}}', @IdAlertInformation, 'SyncronizationBot.Application.Response.MainCommands.AddUpdate.RecoveryAddUpdateBalanceItemCommandResponse', 'PercentModify', NULL, NULL, 0, 0, 0);

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 2; --REBUY
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** NEW REBUY ALERT ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>🔵🔵🔵🔵🔵🔵🔵🔵🔵🔵🔵</tg-emoji>{{NEWLINE}}<s>Signature:</s> {{Signature}}{{NEWLINE}}<s>WalletHash:</s> {{WalletHash}}{{NEWLINE}}<s>ClassWallet:</s> {{ClassWallet}}{{NEWLINE}}<s>Token:</s> {{Token}}{{NEWLINE}}<s>Ca:</s> <pre>{{Ca}}</pre>{{NEWLINE}}<s>Minth Authority:</s> {{MinthAuthority}}{{NEWLINE}}<s>Freeze Authority:</s> {{FreezeAuthority}}{{NEWLINE}}<s>Is Mutable:</s> {{IsMutable}}{{NEWLINE}}<s>Quantity:</s> {{Quantity}} {{QuantitySymbol}}{{NEWLINE}}<s>Value Spent:</s>{{ValueSpent}} {{ValueSpentSymbol}}{{NEWLINE}}<s>Date:</s>{{Date}}{{NEWLINE}}<s>Position Increase</s> {{PositionIncrease}}% {{NEWLINE}}<a href=''https://birdeye.so/token/{{Ca}}?chain=solana''>Chart</a>', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Signature}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'Signature', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{WalletHash}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'WalletHash', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ClassWallet}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'ClassWallet', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Token}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Name', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Ca}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Hash', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{MinthAuthority}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].MintAuthority', NULL, 'NO', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{FreezeAuthority}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].FreezeAuthority', NULL, 'NO', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{IsMutable}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].IsMutable', NULL, 'NO', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Quantity}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueDestination', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantitySymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Symbol', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSpent}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueSource', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSpentSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[0].Symbol', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Date}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Utils.Transfer.TransferInfo', 'DataOfTransfer', NULL, NULL, 1, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{PositionIncrease}}', @IdAlertInformation, 'SyncronizationBot.Application.Response.MainCommands.AddUpdate.RecoveryAddUpdateBalanceItemCommandResponse', 'PercentModify', NULL, NULL, 0, 0, 0);

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 3; --SELL
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** NEW SELL ALERT ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>🔴🔴🔴🔴🔴🔴🔴🔴🔴🔴🔴</tg-emoji>{{NEWLINE}}<s>Signature:</s> {{Signature}}{{NEWLINE}}<s>WalletHash:</s> {{WalletHash}}{{NEWLINE}}<s>ClassWallet:</s> {{ClassWallet}}{{NEWLINE}}<s>Token:</s> {{Token}}{{NEWLINE}}<s>Quantity:</s> {{Quantity}} {{QuantitySymbol}}{{NEWLINE}}<s>Value Received:</s> {{ValueReceived}} {{ValueReceivedSymbol}}{{NEWLINE}}<s>Date:</s> {{Date}}{{NEWLINE}}<s>Position Sell:</s> {{PositionSell}}%{{NEWLINE}}<a href=''https://birdeye.so/token/{{Token}}?chain=solana''>Chart</a>', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Signature}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'Signature', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{WalletHash}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'WalletHash', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ClassWallet}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'ClassWallet', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Token}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[0].Hash', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Quantity}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueSource', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantitySymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[0].Symbol', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueReceived}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueDestination', NULL, NULL, 0, 0, 0); 
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueReceivedSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Symbol', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Date}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Utils.Transfer.TransferInfo', 'DataOfTransfer', NULL, NULL, 1, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{PositionSell}}', @IdAlertInformation, 'SyncronizationBot.Application.Response.MainCommands.AddUpdate.RecoveryAddUpdateBalanceItemCommandResponse', 'PercentModify', NULL, NULL, 0, 0, 0);

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 4; --SWAP
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** SWAP ALERT ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>🔄🔄🔄🔄🔄🔄🔄🔄🔄🔄🔄</tg-emoji>{{NEWLINE}}<s>Signature:</s> {{Signature}}{{NEWLINE}}<s>WalletHash:</s> {{WalletHash}}{{NEWLINE}}<s>ClassWallet:</s> {{ClassWallet}}{{NEWLINE}}<s>Token Change:</s> {{TokenChange}}  {{TokenChangeSymbol}}{{NEWLINE}}Token Received:</s> {{TokenReceived}}{{NEWLINE}}Ca:</s> <pre>{{Ca}}</pre>{{NEWLINE}}Date:</s> {{Date}}{{NEWLINE}}Position Swap:</s> {{PositionSwap}} %{{NEWLINE}}<a href=''https://birdeye.so/token/{{TokenReceivedHash}}?chain=solana''>Chart1</a>{{NEWLINE}}<a href=''https://birdeye.so/token/{{TokenSendedHash}}?chain=solana''>Chart2</a>', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Signature}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'Signature', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{WalletHash}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'WalletHash', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ClassWallet}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'ClassWallet', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenChange}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueSource', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenChangeSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[0].Symbol', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenReceived}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueDestination', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenReceivedSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Symbol', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Ca}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Hash', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Date}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Utils.Transfer.TransferInfo', 'DataOfTransfer', NULL, NULL, 1, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{PositionSwap}}', @IdAlertInformation, 'SyncronizationBot.Application.Response.MainCommands.AddUpdate.RecoveryAddUpdateBalanceItemCommandResponse', 'PercentModify', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenReceivedHash}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Hash', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenSendedHash}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[0].Hash', NULL, NULL, 0, 0, 0);

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 5; -- POOL CREATED
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** POOL CREATED ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>🌊🌊🌊🌊🌊🌊🌊🌊🌊🌊🌊</tg-emoji>{{NEWLINE}}<s>Signature:</s>{{Signature}}{{NEWLINE}}<s>WalletHash:</s>{{WalletHash}}{{NEWLINE}}<s>ClassWallet:</s>{{ClassWallet}}{{NEWLINE}}<s>Amount Pool:</s>{{QuantitySend}}  {{QuantitySendSymbol}}{{NEWLINE}}<s>Amount Pool:</s>{{QuantitySendPool}} {{QuantitySendPoolSymbol}}{{NEWLINE}}<s>Ca Token Pool:</s> {{CaSended}}{{NEWLINE}}<s>Ca Token Pool:</s> {{CaSendedPool}}{{NEWLINE}}<s>Date:</s>{{Date}}{{NEWLINE}}<a href=''https://birdeye.so/token/{{CaSended}}?chain=solana''> Chart1</a>}{{NEWLINE}}<a href=''https://birdeye.so/token/{{CaSendedPool}}?chain=solana''> Chart2</a>', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Signature}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'Signature', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{WalletHash}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'WalletHash', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ClassWallet}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'ClassWallet', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantitySend}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueSource', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantitySendSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[0].Symbol', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantitySendPool}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueSourcePool', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantitySendPoolSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[1].Symbol', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{CaSended}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[0].Hash', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{CaSendedPool}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[1].Hash', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Date}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Utils.Transfer.TransferInfo', 'DataOfTransfer', NULL, NULL, 1, 0, 0);

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 6; -- POOL FINISH
INSERT INTO AlertInformation VALUES(NEWID(), N'<b>*** POOL FINALIZED ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>❌❌❌❌❌❌❌❌❌❌❌</tg-emoji>{{NEWLINE}}<s>Signature:</s>{{Signature}}{{NEWLINE}}<s>WalletHash:</s>{{WalletHash}}{{NEWLINE}}<s>ClassWallet:</s>{{ClassWallet}}{{NEWLINE}}<s>Amount Pool:</s> {{QuantityReceived}} {{QuantityReceivedSymbol}}{{NEWLINE}}<s>Amount Pool:</s> {{QuantityReceivedPool}} {{QuantityReceivedPoolSymbol}}{{NEWLINE}}<s>Ca Token Pool:</s>{{CaReceived}}{{NEWLINE}}<s>Ca Token Pool:</s>{{CaReceivedPool}}{{NEWLINE}}<s>Date:</s>{{Date}}{{NEWLINE}}<a href=''https://birdeye.so/token/{{CaReceived}}?chain=solana''>Chart1</a>{{NEWLINE}}<a href=''https://birdeye.so/token/{{CaReceivedPool}}?chain=solana''>Chart2</a>', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Signature}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'Signature', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{WalletHash}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'WalletHash', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ClassWallet}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'ClassWallet', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityReceived}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueDestination', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityReceivedSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Symbol', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityReceivedPool}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueDestinationPool', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityReceivedPoolSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[3].Symbol', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{CaReceived}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Hash', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{CaReceivedPool}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[3].Hash', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Date}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Utils.Transfer.TransferInfo', 'DataOfTransfer', NULL, NULL, 1, 0, 0);

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 7; -- Alert Price
-- ALERT PRICE UP
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** PRICE UP ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>🔥🔥🔥🔥🔥🔥🔥🔥🔥🔥🔥</tg-emoji>{{NEWLINE}}<s>Token Hash:</s>{{TokenHash}}{{NEWLINE}}<s>Token Name:</s>{{TokenName}}{{NEWLINE}}<s>New Price Change:</s>{{PriceChance}}{{NEWLINE}}<s>Is Recurrency Alert:</s> {{IsRecurrencyAlert}}{{NEWLINE}}', 1, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenHash}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.AlertPrice', 'TokenHash', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenName}}', @IdAlertInformation, 'SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse', 'Name', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{PriceChance}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Response.TokenData', 'Price', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{IsRecurrencyAlert}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.AlertPrice', 'IsRecurrence', NULL, NULL, 0, 0, 0);
-- ALERT PRICE DOWN
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** PRICE DOWN ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>🚨🚨🚨🚨🚨🚨🚨🚨🚨🚨🚨</tg-emoji>{{NEWLINE}}<s>Token Hash:</s>{{TokenHash}}{{NEWLINE}}<s>Token Name:</s>{{TokenName}}{{NEWLINE}}<s>New Price Change:</s>{{PriceChance}}{{NEWLINE}}<s>Is Recurrency Alert:</s> {{IsRecurrencyAlert}}{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>💸💸💸💸💸💸💸💸💸💸💸</tg-emoji>{{NEWLINE}}', 2, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenHash}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.AlertPrice', 'TokenHash', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenName}}', @IdAlertInformation, 'SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse', 'Name', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{PriceChance}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Response.TokenData', 'Price', NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{IsRecurrencyAlert}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.AlertPrice', 'IsRecurrence', NULL, NULL, 0, 0, 0);

------------------------------------------------------------

SELECT * FROM TelegramMessage Order By MessageId
UPDATE RunTimeController SET IsRunning = 0 WHERE TypeService = 4

SELECT COUNT(*) , StatusLoad FROM (
SELECT CASE WHEN IsLoadBalance = 1 THEN
            'Carregada'
	   ELSE
			'Não Carregada'
	   END StatusLoad
	   FROM Wallet)
AS T1
GROUP BY T1.StatusLoad

