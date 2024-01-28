USE [Monitoring]
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'WalletBalance')
BEGIN
	DROP TABLE [WalletBalance]
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
		IdRuntime INT,
		ConfigurationTimer INT,
		TypeService INT,
		IsRunning BIT,
		PRIMARY KEY(IdRuntime)
	);
	INSERT INTO RunTimeController VALUES(1, 1, 1, 0);
	INSERT INTO RunTimeController VALUES(2, 1, 2, 0);
	INSERT INTO RunTimeController VALUES(3, 1, 3, 0);
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
INSERT INTO ClassWallet VALUES(NEWID(), 1, 'Whales');
INSERT INTO ClassWallet VALUES(NEWID(), 2, 'MM');
INSERT INTO ClassWallet VALUES(NEWID(), 3, 'Whales Top Gainers');
INSERT INTO ClassWallet VALUES(NEWID(), 4, 'Arbitradores');
INSERT INTO ClassWallet VALUES(NEWID(), 5, 'BIG BIG Whale');
GO
CREATE TABLE Wallet(
	ID               UNIQUEIDENTIFIER,
	[Hash]           VARCHAR(50),
	IdClassWallet    UNIQUEIDENTIFIER,
	UnixTimeSeconds  DECIMAL(20,0),
	IsLoadBalance    BIT,
	DateLoadBalance  DATETIME2,
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

INSERT INTO Wallet VALUES (NEWID(), 'HwQ9NTLB1QthB3Tsq9eWCXogVHWZSLZrhySiknr2cKFX', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(), 'DUHbm9JZ9D82h1pmRZYZAMA9U44hS4D7z6ZxyEjbMYNn', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(), 'EgZNycuVcr4YWxgjoDK3METamtSDjrPnCUs7jWgmgYSq', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(), 'GZR6XTytmQwa2goHtq4D6F5FSJRDvA477gdC7jCrt7Qc', @IdClassWallet, null, 0, null, 1, null);

SELECT @IdClassWallet = ID FROM ClassWallet WHERE IdClassification = 2
INSERT INTO Wallet VALUES (NEWID(), 'GhuBeitd7eh8KwCurXy1tFCRxGphpVxa8X4rUX8dQxHc', @IdClassWallet, null, 0, null, 1, null);

SELECT @IdClassWallet = ID FROM ClassWallet WHERE IdClassification = 3
INSERT INTO Wallet VALUES (NEWID(),'HzoNzi7mLVCxsa9EkdBmoob75rkXCjWLHy131ch1oEbX', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'43QLsYdyomxCyoZiz1W18LaZaY3tevxLM4KyJWnVeFaB', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'8usu4t61wPgUjwU1qohajaYGDjqz43Jp8oGMJ4Sa82A7', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'CGLBeFjXSAeKGKGPKjT8E8sHcDD72SCAWEV4jgXpY4aK', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'FZNrSiYifncDHTRNB6L8AyGX3sQu4T5Jb9k56S1zgTsz', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'2bAVD7hHAoLJV939Goo5id6uguHXHaQDbqqsFijwcPEC', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'CLee88KHw1tBdpY1hkxDgendy7UjCD3PMGDY4xbLTbys', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'2cSsWeb3v7uD4dGgcqoCNWBwBcos9iK7jEEfyVTH2LSZ', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'D7Lwt6j2hzDhy14oC5YdfXh4pPeoAGbaNWQZX2UCPUKZ', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'Erja5wNDfvvWWvYe9sHYgjxBKpqxaK2Uk33mEV3Ts5du', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'6hqR9urgXPXnPFYybvwYdhLZ7TRKS4NcLcNivJhRr7Jf', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'BsgFVpmEBB3Ps39aNQJPXt5mndbEQ7aP3fLXNRL8BTxW', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'3oc7EzM8UWf4o3MJYvt52uEL4GnTEGK72tYwGq5eskzS', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'DqFEdkAjcqqPV7C1kmjhyCayxCaJsdjb7JWHUKZk7hYX', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'CxaA6Dmhq8xf9dfo3SuvupHB2mCPQLc1K6wkv8i6us6d', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'HzMZ89kGbgfut7KgyFk1GXdWibUK4owwXZsoKDnuqNQL', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'9UG3uahH8ejWKM7VwMTQtTHm6YERyPPuWtJNUHfCyabV', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'Asyuz72ArUZ1yMW2WQtdwjCaN7uJN4iD41jKm6SY8e65', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'HSAxeaos35XJAAeua67Bdw3rnSCecPziCokXSGWR4fQD', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'DEPErBeK3Ej3C5VRncMQJkmCcn7bCxKLHqEDSMxui3Lh', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'4bce4DGEBmgL5cuQUSs5CEp9PBMFQPWCZNkpfKmWytHo', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'9Ltbz7RQr7Bd3dCD3qmhkWNwR2fAfKAn6NXEjPrd9UUE', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'6C8qysda5dg8WpPJoj6iMLELhtLzQe2A7nnSA1A7ttpF', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'7B7XQbX4ZigQgP28sEHQwicAmfjgRD47pPkKcfPhMeA5', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'CMup3m7gDxMTCbMu1xQVtjRmNRH2kTJESPPhkD3j7XUk', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'A5f6HGc2yjEDV9C4kMj3XLLsHvkVqduiV1CFvnafer7j', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'GoKi3UyMz7nELSGnqPa68KgDbV4cW4eJTPKMkrPugQpL', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'22PwbCzNaxKAbMBB3BYAN8rUXu4TARsRbEyemFqpSejz', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'BgwNWZAnejxMcCBBcSfwgTZLt1XzHd7pL3f7EHQxURNy', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'BuxtrMH8FVGENMYKn6LFzHw5tyCq5CrTQzJyC3HRCSBy', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'ufQ6EY7bzKdWNpC8hZH86WEzX6zkgqYd2eGaKbDyTU3',  @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'3gtDsHFSAN3JrBhW9gMiksZVHHZNUsz4AnoQcLqkTGnm', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'6iK4DpQd48qW65Kav1zrs3uYAtpcZFDZLE9jafYTf35C', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'6ZzWpyjbieCqzCDyaeYZKZmT9C3tcprDNfQR6xpzB1yr', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'8XqHwUHZWA7iwFw3Jydd4duiEF2bGdtSmnbPMjpYDS2n', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'2FMPgFspCouJTcifuYwF1AdDKa2poH7Tqm6HSDSiGrGo', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'97kcP9Ss2YnkwEA2ovT1C4UdAJ3mQ1hJgVPZxAwvDrKj', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'ECL1PSED78CqHB7DqVk73bEC3PyDSmKNvq85Y8Xsehwd', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'FkYptuxkuz7CSrjNMesdD3SqPHjGrVJB1YUxhKA3HGxA', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'3UUdqsU7gajKwL58mwRBPFsXoxRakLPY63HcEGj2W2MD', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'HYrAcw6FwNVgu7CJ6SecS1rxJdDnRuZhEtUCTQKcVVuT', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'AnqkPLfaRX4H69mT4dmoH2V4T4VzcHzoTkPqPib15Hwx', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'GqLsGpJyvPJBKPpm87zXHUKh4ywQC7cUghbkXwdGbvdT', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'CnHCkUwcdZ3mgf7vXoiKMejaKFUF8tnA9VXL2CuxMaXt', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'9z3yhh9nTToHTuRJrk9485ozXb5EPbSALrKd9pRNCF5q', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'BpjaWFG3Q7ia19jmZbPsWB47YGHQWGwDBYHoCGnLD3WT', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'DdymDLAwbsn5R3cN26d2NdmhiKv9sAGDFnPBbsPT9ut1', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'FTQWvJFb5tBhEQTtyTxzJdV3QvTsJh14ofRC9S6DE5Lq', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'fstYfhMw3Petz3df1j9yY8E5nHbkkpexqCnpFahzUSg',  @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'31HWL9p9k4E8D3KgW34YWzu8rw6aLjevpd449zUTG6cc', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'636ZbmWmrg2Dgpi29qncZNARUFNXvVHwoqwmRZtrfMXS', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'3xWhH42D1skKHhpanvBdn6zvoPiXoThRcuaMEjqjzmmA', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'AR1Eb6GjQScNWc8HBvzE19TMksu4ambaP87wppQ4g7vz', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'8ZPqRatB9U43DYoaVk9YZKZTnjNQR9uK1HaGaPKgvKzZ', @IdClassWallet, null, 0, null, 1, null);
INSERT INTO Wallet VALUES (NEWID(),'7s3MckqAqdci8b1y8QZ4oYL1PDz1yPBmb1PeZfBTVnac', @IdClassWallet, null, 0, null, 1, null);

SELECT @IdClassWallet = ID FROM ClassWallet WHERE IdClassification = 5
INSERT INTO Wallet VALUES (NEWID(),'F3SexpfyD785mndnj38EGoxZdAAQEMxBmJCvNe15rz5B', @IdClassWallet, null, 0, null, 1, null);
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
	CreateDate            DATETIME2,
	LastUpdate            DATETIME2,
	PRIMARY KEY (ID)
);

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
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Log Error', -2, @IdTelegramChannel, 1, GETDATE(), GETDATE());
SELECT @IdTelegramChannel = ID FROM TelegramChannel WHERE ChannelName = 'CallSolana';
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Buy', 1, @IdTelegramChannel, 1, GETDATE(), GETDATE());
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Rebuy', 2, @IdTelegramChannel,  1, GETDATE(), GETDATE());
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Sell', 3, @IdTelegramChannel, 1, GETDATE(), GETDATE());
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Swap', 4, @IdTelegramChannel, 1, GETDATE(), GETDATE());
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Pool Create', 5, @IdTelegramChannel, 1, GETDATE(), GETDATE());
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Pool Finish', 6, @IdTelegramChannel, 1, GETDATE(), GETDATE());

CREATE TABLE AlertInformation(
	ID                    UNIQUEIDENTIFIER,
	[Message]             VARCHAR(500),
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
	IsIcon				  BIT,
	IsImage               BIT
	PRIMARY KEY (ID),
	FOREIGN KEY (AlertInformationId) REFERENCES AlertInformation(ID),
);

DECLARE @IdAlertConfiguration UNIQUEIDENTIFIER;
DECLARE @IdAlertInformation UNIQUEIDENTIFIER;
SELECT @IdAlertInformation = NEWID();
SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = -1;
INSERT INTO AlertInformation VALUES(@IdAlertInformation, '<b>Execução do serviço {{ServiceName}} de call solana</b>{{NEWLINE}}<b>Data Execução: </b>{{DateTimeNow}}.{{NEWLINE}}<i><b>Proxima execução</b> no período timer de --> {{TimerExecute}}</i>{{NEWLINE}}', @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ServiceName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'ServiceName', NULL, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{DateTimeNow}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'DateExecuted', NULL, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TimerExecute}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'Timer', NULL, 0, 0);

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = -2;
INSERT INTO AlertInformation VALUES(NEWID(), '<b>O serviço {{ServiceName}} está rodando.</b>{{NEWLINE}}<i><b>Não irá efetuar essa execução:</b> {{DateTimeNow}}</i>.{{NEWLINE}}', @IdAlertConfiguration);


--SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 1;
--INSERT INTO AlertInformation VALUES(NEWID(), @IdTElegramChannel, '', @IdAlertConfiguration);

--SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 2;
--INSERT INTO AlertInformation VALUES(NEWID(), @IdTElegramChannel, '', @IdAlertConfiguration);

--SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 3;
--INSERT INTO AlertInformation VALUES(NEWID(), @IdTElegramChannel, '', @IdAlertConfiguration);

--SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 4;
--INSERT INTO AlertInformation VALUES(NEWID(), @IdTElegramChannel, '', @IdAlertConfiguration);

--SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 5;
--INSERT INTO AlertInformation VALUES(NEWID(), @IdTElegramChannel, '', @IdAlertConfiguration);

--SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 6;
--INSERT INTO AlertInformation VALUES(NEWID(), @IdTElegramChannel, '', @IdAlertConfiguration);

------------------------------------------------------------

UPDATE RunTimeController
SET IsRunning = 0

177.170.8.206