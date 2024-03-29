USE [Monitoring]
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TransactionsRPCRecovery')
BEGIN
	DROP TABLE [TransactionsRPCRecovery]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'PublishMessage')
BEGIN
	DROP TABLE [PublishMessage]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TokenAlphaProfit')
BEGIN
	DROP TABLE [TokenAlphaProfit]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TokenAlphaWalletHistory')
BEGIN
	DROP TABLE [TokenAlphaWalletHistory]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TokenAlphaWallet')
BEGIN
	DROP TABLE [TokenAlphaWallet]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TokenAlphaHistory')
BEGIN
	DROP TABLE [TokenAlphaHistory]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TokenAlpha')
BEGIN
	DROP TABLE [TokenAlpha]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TokenAlphaConfiguration')
BEGIN
	DROP TABLE [TokenAlphaConfiguration]
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

IF NOT EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TelegramChannel')
BEGIN
	CREATE TABLE TelegramChannel(
		ID           UNIQUEIDENTIFIER,
		ChannelId    DECIMAL(30,0),
		ChannelName  VARCHAR(50),
		PRIMARY KEY (ID)
	);
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
	DELETE FROM AlertPrice
	DECLARE @TelegramChannelId UNIQUEIDENTIFIER
	SELECT @TelegramChannelId = ID FROM TelegramChannel WHERE ChannelName = 'AlertPriceChange'
	INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '0.008302', '5jcDWDV3HYeFvDBGEfPtk68WNmV7ZoLU8QUvDAnACpnE', '1.3', null, 1, 0, @TelegramChannelId);
	INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '12.180', '3QYAWuowfaLC1CqYKx2eTe1SwV9MqAe1dUZT3NPt3srQ', '23.4', null, 1, 0, @TelegramChannelId);
	INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '12.180', '3QYAWuowfaLC1CqYKx2eTe1SwV9MqAe1dUZT3NPt3srQ', '9.2', null, 2, 0, @TelegramChannelId);
	INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '85.60', 'So11111111111111111111111111111111111111112', '81.58', null, 2, 1, @TelegramChannelId);
	INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '0.000000100380', '2PCegSVAesdb8ffZViieXUC3gH2wku7ieXRteB8Az7o6', '0.000000989687', null, 1, 0, @TelegramChannelId);
	INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '0.000000100380', '2PCegSVAesdb8ffZViieXUC3gH2wku7ieXRteB8Az7o6', '0.000001977806', null, 1, 0, @TelegramChannelId);
	INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '0.000000100380', '2PCegSVAesdb8ffZViieXUC3gH2wku7ieXRteB8Az7o6', '0.000010002757', null, 1, 0, @TelegramChannelId);
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'RunTimeController')
BEGIN
	CREATE TABLE RunTimeController
	(	
		IdRuntime				 INT,
		ConfigurationTimer		 VARCHAR(100),
		TypeService				 INT,
		IsRunning				 BIT,
		IsContingecyTransactions BIT,
		TimesWithoutTransactions INT,
		JobName                  VARCHAR(200),
		PRIMARY KEY(IdRuntime)
	);
	INSERT INTO RunTimeController VALUES(1, '1', 1, 0, 0, null, 'Alerta de Transações');
	INSERT INTO RunTimeController VALUES(2, '1', 2, 0, 0, null, 'Carregar balanços das wallets');
	INSERT INTO RunTimeController VALUES(3, '1', 3, 0, 0, null, 'Alerta de preços');
	INSERT INTO RunTimeController VALUES(4, '4', 4, 0, 0, null, 'Excluir mensagens de log antigas');
	INSERT INTO RunTimeController VALUES(5, '1', 5, 0, 0, null, 'Alerta de Token Alpha');
	INSERT INTO RunTimeController VALUES(6, '1', 6, 0, 0, null, 'Transacões Antigas para Mapear');
	INSERT INTO RunTimeController VALUES(7, '1', 6, 0, 0, null, 'Carregar Listagem de Novos Tokens');
END
GO 

IF NOT EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TransactionNotMapped')
BEGIN
	CREATE TABLE TransactionNotMapped(
		ID                 UNIQUEIDENTIFIER,
		[WalletId]         UNIQUEIDENTIFIER,
		[Signature]        VARCHAR(150),
		[Link]             VARCHAR(500),
		[Error]            VARCHAR(500),
		[StackTrace]       NVARCHAR(MAX),
		[DateTimeRunner]   DATETIME2
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


GO

CREATE TABLE Wallet(
	ID                   UNIQUEIDENTIFIER,
	[Hash]               VARCHAR(50),
	ClassWalletId        UNIQUEIDENTIFIER,
	UnixTimeSeconds      DECIMAL(20,0),
	IsLoadBalance        BIT,
	DateLoadBalance      DATETIME2,
	OldTransactionStared DATETIME2,
	OldTransactionHours  INT,
	IsActive             BIT,
	LastUpdate           DATETIME2, 
	IsRunningProcess     BIT
	PRIMARY KEY (ID),
	FOREIGN KEY (ClassWalletId) REFERENCES ClassWallet(ID)
);
IF NOT EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'Token')
BEGIN
	CREATE TABLE Token(
		ID                     UNIQUEIDENTIFIER,
		[Hash]                 VARCHAR(50),
		Symbol                 VARCHAR(500),
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
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TokenSecurity')
BEGIN
	CREATE TABLE TokenSecurity(
		ID                 UNIQUEIDENTIFIER,
		TokenId            UNIQUEIDENTIFIER,
		CreatorAddress     VARCHAR(100),
		CreationTime       BIGINT,
		Top10HolderBalance VARCHAR(150),
		Top10HolderPercent VARCHAR(150),
		Top10UserBalance   VARCHAR(150),
		Top10UserPercent   VARCHAR(150),
		IsTrueToken        BIT,
		LockInfo		   NVARCHAR(MAX),
		Freezeable		   BIT,
		FreezeAuthority    VARCHAR(100),
		TransferFeeEnable  VARCHAR(100),
		TransferFeeData    NVARCHAR(MAX),
		IsToken2022        BIT,
		NonTransferable    VARCHAR(100),
		MintAuthority      VARCHAR(100),
		IsMutable          BIT,
		PRIMARY KEY (ID),
		FOREIGN KEY (TokenId) REFERENCES Token(ID)
	);
END
GO

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
	TokenSourceId                UNIQUEIDENTIFIER,
	TokenSourcePoolId            UNIQUEIDENTIFIER,
	TokenDestinationId           UNIQUEIDENTIFIER,
	TokenDestinationPoolId       UNIQUEIDENTIFIER,
	WalletId                     UNIQUEIDENTIFIER,
	TypeOperation                INT, -- 1 For Buy, 2 For Sell, 3 For Transfer, 4 For Received, 5 SWAP, 6 POOL CREATE, 7 POOL FINALIZED
	PRIMARY KEY (ID),
	FOREIGN KEY (TokenSourceId) REFERENCES Token(ID),
	FOREIGN KEY (TokenSourcePoolId) REFERENCES Token(ID),
	FOREIGN KEY (TokenDestinationId) REFERENCES Token(ID),
	FOREIGN KEY (TokenDestinationPoolId) REFERENCES Token(ID),
	FOREIGN KEY (WalletId) REFERENCES Wallet(ID),
);
GO

CREATE TABLE TransactionsOldForMapping
(
	ID                           UNIQUEIDENTIFIER,
	[Signature]                  VARCHAR(150),
	DateOfTransaction            DATETIME2,
	WalletId                     UNIQUEIDENTIFIER,
	CreateDate				     DATETIME2,
	IsIntegrated				 BIT,
	PRIMARY KEY (ID),
	FOREIGN KEY (WalletId) REFERENCES Wallet(ID),
);
GO

CREATE TABLE TransactionsRPCRecovery
(
	ID                           UNIQUEIDENTIFIER,
	[Signature]                  VARCHAR(150),
	[BlockTime]                  VARCHAR(100),
	DateOfTransaction            DATETIME2,
	WalletId                     UNIQUEIDENTIFIER,
	CreateDate				     DATETIME2,
	IsIntegrated				 BIT,
	PRIMARY KEY (ID),
	FOREIGN KEY (WalletId) REFERENCES Wallet(ID),
);
GO

CREATE TABLE WalletBalance
(
	ID            UNIQUEIDENTIFIER,
	WalletId      UNIQUEIDENTIFIER,
	TokenId       UNIQUEIDENTIFIER,
	TokenHash     VARCHAR(100),
	Quantity      VARCHAR(100),
	Price         VARCHAR(100),
	TotalValueUSD VARCHAR(100),
	IsActive      BIT,
	LastUpdate    DATETIME2,
	PRIMARY KEY (ID),
	FOREIGN KEY (WalletId) REFERENCES Wallet(ID),
	FOREIGN KEY(TokenId) REFERENCES Token(ID),
);

CREATE TABLE WalletBalanceSFMCompare(
	ID            UNIQUEIDENTIFIER,
	WalletId      UNIQUEIDENTIFIER,
	TokenId       UNIQUEIDENTIFIER,
	TokenHash     VARCHAR(100),
	Quantity      VARCHAR(100),
	Price         VARCHAR(100),
	TotalValueUSD VARCHAR(100),
	IsActive      BIT,
	LastUpdate    DATETIME2,
	PRIMARY KEY (ID),
	FOREIGN KEY (WalletId) REFERENCES Wallet(ID),
	FOREIGN KEY(TokenId) REFERENCES Token(ID),
);

CREATE TABLE WalletBalanceHistory
(
	ID                    UNIQUEIDENTIFIER,
	WalletBalanceId       UNIQUEIDENTIFIER,
	WalletId              UNIQUEIDENTIFIER,
	TokenId               UNIQUEIDENTIFIER,
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

IF NOT EXISTS (SELECT 1 FROM sys.TABLES WHERE NAME = 'TelegramMessage')
BEGIN
	-- Telegram Messages
	CREATE TABLE TelegramMessage
	(
		ID                 UNIQUEIDENTIFIER,
		MessageId          BIGINT,
		TelegramChannelId  UNIQUEIDENTIFIER,
		EntityId		   UNIQUEIDENTIFIER,
		DateSended         DATETIME2,
		IsDeleted		   BIT
		PRIMARY KEY (ID),
		FOREIGN KEY(TelegramChannelId) REFERENCES TelegramChannel(ID)
	);
END
GO


-- Token Alpha
CREATE TABLE TokenAlphaConfiguration
(
	ID                   UNIQUEIDENTIFIER,
	Name                 VARCHAR(300),
	Ordernation          INT,
	MaxMarketcap         VARCHAR(100),
	MaxDateOfLaunchDays  INT,
	PRIMARY KEY (ID)
);
GO

INSERT INTO TokenAlphaConfiguration VALUES(NEWID(), 'Alpha Creation Until 5 days ago', 1, '2000000', 5);
INSERT INTO TokenAlphaConfiguration VALUES(NEWID(), 'Alpha Creation Until 15 days ago', 4, '2000000', 15);
INSERT INTO TokenAlphaConfiguration VALUES(NEWID(), 'Maybe Alpha Creation Until 30 days ago', 7, '2000000', 30);
INSERT INTO TokenAlphaConfiguration VALUES(NEWID(), 'Maybe it''s shitcoin Creation Until 2 months ago', 10, '2000000', 60);
INSERT INTO TokenAlphaConfiguration VALUES(NEWID(), 'Alpha Mid Mktcap Creation Until 5 days ago', 2, '5000000', 5);
INSERT INTO TokenAlphaConfiguration VALUES(NEWID(), 'Alpha Mid Mktcap Creation Until 15 days ago', 5, '5000000', 15);
INSERT INTO TokenAlphaConfiguration VALUES(NEWID(), 'Maybe Alpha Mid Mktcap Creation Until 30 days ago', 8, '5000000', 30);
INSERT INTO TokenAlphaConfiguration VALUES(NEWID(), 'Maybe it''s Mid Mktcap shitcoin Creation Until 2 months ago', 11, '5000000', 60);
INSERT INTO TokenAlphaConfiguration VALUES(NEWID(), 'Alpha Big Mktcap Creation Until 5 days ago', 3, '10000000', 5);
INSERT INTO TokenAlphaConfiguration VALUES(NEWID(), 'Alpha Big Mktcap Creation Until 15 days ago', 6, '10000000', 15);
INSERT INTO TokenAlphaConfiguration VALUES(NEWID(), 'Maybe Alpha Big Mktcap Creation Until 30 days ago', 9, '10000000', 30);
INSERT INTO TokenAlphaConfiguration VALUES(NEWID(), 'Maybe it''s Big Mktcap shitcoin Creation Until 2 months ago', 12, '10000000', 60);

CREATE TABLE TokenAlpha(
	ID                        UNIQUEIDENTIFIER,
	CallNumber                INT,
	InitialMarketcap          VARCHAR(100),
	ActualMarketcap           VARCHAR(100),
	InitialPrice              VARCHAR(100),
	ActualPrice               VARCHAR(100),
	CreateDate                DATETIME2,
	LastUpdate                DATETIME2,
	TokenId                   UNIQUEIDENTIFIER,
	TokenHash                 VARCHAR(50),
	TokenSymbol               VARCHAR(500),
	TokenName		          VARCHAR(200),
	TokenAlphaConfigurationId UNIQUEIDENTIFIER,
	PRIMARY KEY (ID),
	FOREIGN KEY (TokenId) REFERENCES Token(ID),
	FOREIGN KEY (TokenAlphaConfigurationId) REFERENCES TokenAlphaConfiguration(ID)
);
GO

CREATE TABLE TokenAlphaHistory(
	ID                        UNIQUEIDENTIFIER,
	TokenAlphaId              UNIQUEIDENTIFIER,
	CallNumber                INT,
	InitialMarketcap          VARCHAR(100),
	ActualMarketcap           VARCHAR(100),
	InitialPrice              VARCHAR(100),
	ActualPrice               VARCHAR(100),
	RequestMarketCap          VARCHAR(100),
	RequestPrice              VARCHAR(100),
	CreateDate                DATETIME2,
	LastUpdate                DATETIME2,
	TokenId                   UNIQUEIDENTIFIER,
	TokenHash                 VARCHAR(50),
	TokenSymbol               VARCHAR(500),
	TokenName		          VARCHAR(200),
	TokenAlphaConfigurationId UNIQUEIDENTIFIER,
	PRIMARY KEY (ID)
);
GO

CREATE TABLE TokenAlphaWallet(
	ID                      UNIQUEIDENTIFIER,
	NumberOfBuys            INT,
	ValueSpentSol           VARCHAR(100),
	ValueSpentUSDC          VARCHAR(100),
	ValueSpentUSDT          VARCHAR(100),
	QuantityToken           VARCHAR(100),
	NumberOfSells           INT,
	ValueReceivedSol        VARCHAR(100),
	ValueReceivedUSDC       VARCHAR(100),
	ValueReceivedUSDT       VARCHAR(100),
	QuantityTokenSell       VARCHAR(100),
	TokenAlphaId            UNIQUEIDENTIFIER,
	WalletId                UNIQUEIDENTIFIER,
	WalletHash              VARCHAR(50),
	ClassWalletDescription  VARCHAR(200),
	PRIMARY KEY (ID),
	FOREIGN KEY (TokenAlphaId) REFERENCES TokenAlpha(ID),
	FOREIGN KEY (WalletId) REFERENCES Wallet(ID)
);
GO
CREATE TABLE TokenAlphaWalletHistory(
	ID                      UNIQUEIDENTIFIER,
	TokenAlphaWalletId      UNIQUEIDENTIFIER,
	NumberOfBuys	        INT,
	ValueSpentSol           VARCHAR(100),
	ValueSpentUSDC          VARCHAR(100),
	ValueSpentUSDT		    VARCHAR(100),
	QuantityToken           VARCHAR(100),
	NumberOfSells           INT,
	ValueReceivedSol        VARCHAR(100),
	ValueReceivedUSDC       VARCHAR(100),
	ValueReceivedUSDT       VARCHAR(100),
	QuantityTokenSell       VARCHAR(100),
	RequestValueInSol       VARCHAR(100),
	RequestValueInUSDC      VARCHAR(100),
	RequestValueInUSDT      VARCHAR(100),
	RequestQuantityToken    VARCHAR(100),
	TokenAlphaId		    UNIQUEIDENTIFIER,
	WalletId			    UNIQUEIDENTIFIER,
	WalletHash              VARCHAR(50),
	ClassWalletDescription  VARCHAR(200),
	PRIMARY KEY (ID)
);
GO
CREATE TABLE TokenAlphaProfit
(
	ID						    UNIQUEIDENTIFIER,
	TokenAlphaWalletId          UNIQUEIDENTIFIER,
	OperationType               INT, -- 1 BUY, 2 - SELL, 3 - SWAP
	AmountToken                 VARCHAR(100),
	ValueOperationInSol         VARCHAR(100),
	ValueOperationInUSD         VARCHAR(100),
	ProfitCalculatedInOperation VARCHAR(100), --(TOTAL BUY  / TOTAL TOKEN = PRICE PER TOKEN | TOTAL SELL / TOTAL TOKEN = PRICE PER TOKEN)
	PRIMARY KEY (ID),
	FOREIGN KEY (TokenAlphaWalletId) REFERENCES TokenAlphaWallet(ID)
);
GO

CREATE TABLE PublishMessage(
	ID                  UNIQUEIDENTIFIER,
	EntityId			UNIQUEIDENTIFIER,
	Entity				VARCHAR(500),
	JsonValue           NVARCHAR(MAX),
	ItWasPublished      BIT,
	EntityParentId      UNIQUEIDENTIFIER,
	PRIMARY KEY (ID),
	FOREIGN KEY (EntityParentId) REFERENCES PublishMessage(ID)
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
SELECT @IdTelegramChannel = ID FROM TelegramChannel WHERE ChannelName = 'TokenAlpha';
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Token Alpha', 8, @IdTelegramChannel, 1, GETDATE(), GETDATE());
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
	[FormatValue]		  VARCHAR(50),
	[HasAdjustment]       BIT,
	IsIcon				  BIT,
	IsImage               BIT
	PRIMARY KEY (ID),
	FOREIGN KEY (AlertInformationId) REFERENCES AlertInformation(ID),
);

DECLARE @IdAlertConfiguration UNIQUEIDENTIFIER;
DECLARE @IdAlertInformation UNIQUEIDENTIFIER;
SELECT @IdAlertInformation = NEWID();
-- LOG SUCESSO
SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = -1;
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>Execução do serviço {{ServiceName}} de call solana</b>{{NEWLINE}}<b>Data Execução: </b>{{DateTimeNow}}.{{NEWLINE}}<i><b>Proxima execução</b> no período timer de --> {{TimerExecute}}</i>{{NEWLINE}}', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ServiceName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'ServiceName', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{DateTimeNow}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'DateExecuted', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TimerExecute}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'Timer', NULL, NULL, NULL, 0, 0, 0);
-- LOG RUNNING
SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = -2;
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>O serviço {{ServiceName}} está rodando.</b>{{NEWLINE}}<i><b>Não irá efetuar essa execução:</b> {{DateTimeNow}}</i>.{{NEWLINE}}', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ServiceName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'ServiceName', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{DateTimeNow}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'DateExecuted', NULL, NULL, NULL, 0, 0, 0);
-- LOG ERROR
SELECT @IdAlertConfiguration= ID FROM AlertConfiguration WHERE TypeAlert = -3;
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>O serviço {{ServiceName}} suspendeu a execução.</b>{{NEWLINE}}<i><b>Mensagem de erro:</b> {{ErrorMessage}}</i>.{{NEWLINE}}StackTrace: {{StackTrace}}{{NEWLINE}}<i><b>Proxima execução</b> no período timer de --> {{TimerExecute}}.{{NEWLINE}}<b>Dev''s Favor verificar</b> Cc:@evandrotartari , @euRodrigo</i>{{NEWLINE}}', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ServiceName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'ServiceName', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{DateTimeNow}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'DateExecuted', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ErrorMessage}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'Exception.Message', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{StackTrace}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'Exception.StackTrace', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TimerExecute}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'Timer', NULL, NULL, NULL, 0, 0, 0);
-- LOG LOST CONFIGURATION
SELECT @IdAlertConfiguration= ID FROM AlertConfiguration WHERE TypeAlert = -4;
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>Timer do serviço {{ServiceName}} está nulo ou não configurado.</b>{{NEWLINE}}<i><b>Não irá efetuar essa execução:</b> {{DateTimeNow}}</i>.{{NEWLINE}}', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ServiceName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'ServiceName', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{DateTimeNow}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'DateExecuted', NULL, NULL, NULL, 0, 0, 0);

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 1; --BUY
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** NEW BUY ALERT ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>🟢🟢🟢🟢🟢🟢🟢🟢🟢🟢🟢</tg-emoji>{{NEWLINE}}🖌 <b>Signature:</b> {{Signature}}{{NEWLINE}}💼 <b>WalletHash:</b> {{WalletHash}}{{NEWLINE}}📰 <b>ClassWallet:</b> {{ClassWallet}} {{NEWLINE}}🪙 <b>Token:</b> {{Token}}{{NEWLINE}}🔒 <b>Ca:</b> {{Ca}}{{NEWLINE}}🚨 <b>Minth Authority:</b>{{MinthAuthority}}{{NEWLINE}}🚨 <b>Freeze Authority:</b> {{FreezeAuthority}}{{NEWLINE}}🚨 <b>Is Mutable:</b>{{IsMutable}}{{NEWLINE}}🪙 <b>Quantity:</b> {{Quantity}} {{QuantitySymbol}} {{NEWLINE}}💸 <b>Value Spent:</b> {{ValueSpent}} {{ValueSpentSymbol}}{{NEWLINE}}📆 <b>Date:</b> {{Date}}{{NEWLINE}}⬆ <b>Position Increase</b> {{PositionIncrease}} % {{NEWLINE}}📊 <a href=''https://birdeye.so/token/{{Ca}}?chain=solana''>Chart</a>', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Signature}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'Signature', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{WalletHash}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'WalletHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ClassWallet}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'ClassWallet', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Token}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Name', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Ca}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Hash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{MinthAuthority}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].MintAuthority', NULL, 'NO', NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{FreezeAuthority}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].FreezeAuthority', NULL, 'NO', NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{IsMutable}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].IsMutable', NULL, 'NO', NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Quantity}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueDestination', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantitySymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Symbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSpent}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueSource', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSpentSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[0].Symbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Date}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Utils.Transfer.TransferInfo', 'DataOfTransfer', NULL, NULL, NULL, 1, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{PositionIncrease}}', @IdAlertInformation, 'SyncronizationBot.Application.Response.MainCommands.AddUpdate.RecoveryAddUpdateBalanceItemCommandResponse', 'PercentModify', NULL, NULL, NULL, 0, 0, 0);

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 2; --REBUY
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** NEW REBUY ALERT ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>🔵🔵🔵🔵🔵🔵🔵🔵🔵🔵🔵</tg-emoji>{{NEWLINE}}🖌 <b>Signature:</b> {{Signature}}{{NEWLINE}}💼 <b>WalletHash:</b> {{WalletHash}}{{NEWLINE}}📰 <b>ClassWallet:</b> {{ClassWallet}}{{NEWLINE}}🪙 <b>Token:</b> {{Token}}{{NEWLINE}}🔒 <b>Ca:</b> {{Ca}} {{NEWLINE}}🚨 <b>Minth Authority:</b> {{MinthAuthority}}{{NEWLINE}}🚨 <b>Freeze Authority:</b> {{FreezeAuthority}}{{NEWLINE}}🚨 <b>Is Mutable:</b> {{IsMutable}}{{NEWLINE}}🪙 <b>Quantity:</b> {{Quantity}} {{QuantitySymbol}}{{NEWLINE}}💸 <b>Value Spent:</b>{{ValueSpent}} {{ValueSpentSymbol}}{{NEWLINE}}📆 <b>Date:</b>{{Date}}{{NEWLINE}}⬆ <b>Position Increase</b> {{PositionIncrease}}% {{NEWLINE}}📊 <a href=''https://birdeye.so/token/{{Ca}}?chain=solana''>Chart</a>', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Signature}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'Signature', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{WalletHash}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'WalletHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ClassWallet}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'ClassWallet', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Token}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Name', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Ca}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Hash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{MinthAuthority}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].MintAuthority', NULL, 'NO', NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{FreezeAuthority}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].FreezeAuthority', NULL, 'NO', NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{IsMutable}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].IsMutable', NULL, 'NO', NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Quantity}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueDestination', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantitySymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Symbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSpent}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueSource', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSpentSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[0].Symbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Date}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Utils.Transfer.TransferInfo', 'DataOfTransfer', NULL, NULL, NULL, 1, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{PositionIncrease}}', @IdAlertInformation, 'SyncronizationBot.Application.Response.MainCommands.AddUpdate.RecoveryAddUpdateBalanceItemCommandResponse', 'PercentModify', NULL, NULL, NULL, 0, 0, 0);

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 3; --SELL
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** NEW SELL ALERT ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>🔴🔴🔴🔴🔴🔴🔴🔴🔴🔴🔴</tg-emoji>{{NEWLINE}}🖌 <b>Signature:</b> {{Signature}}{{NEWLINE}}💼 <b>WalletHash:</b> {{WalletHash}}{{NEWLINE}}📰 <b>ClassWallet:</b> {{ClassWallet}}{{NEWLINE}}🪙 <b>Token:</b> {{Token}}{{NEWLINE}}🪙 <b>Quantity:</b> {{Quantity}} {{QuantitySymbol}}{{NEWLINE}}💰 <b>Value Received:</b> {{ValueReceived}} {{ValueReceivedSymbol}}{{NEWLINE}}📆 <b>Date:</b> {{Date}}{{NEWLINE}}⬇ <b>Position Sell:</b> {{PositionSell}}%{{NEWLINE}}📊 <a href=''https://birdeye.so/token/{{Token}}?chain=solana''>Chart</a>', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Signature}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'Signature', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{WalletHash}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'WalletHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ClassWallet}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'ClassWallet', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Token}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[0].Hash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Quantity}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueSource', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantitySymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[0].Symbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueReceived}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueDestination', NULL, NULL, NULL, 0, 0, 0); 
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueReceivedSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Symbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Date}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Utils.Transfer.TransferInfo', 'DataOfTransfer', NULL, NULL, NULL, 1, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{PositionSell}}', @IdAlertInformation, 'SyncronizationBot.Application.Response.MainCommands.AddUpdate.RecoveryAddUpdateBalanceItemCommandResponse', 'PercentModify', NULL, NULL, NULL, 0, 0, 0);

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 4; --SWAP
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** SWAP ALERT ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>🔄🔄🔄🔄🔄🔄🔄🔄🔄🔄🔄</tg-emoji>{{NEWLINE}}🖌 <b>Signature:</b> {{Signature}}{{NEWLINE}}💼 <b>WalletHash:</b> {{WalletHash}}{{NEWLINE}}📰 <b>ClassWallet:</b> {{ClassWallet}}{{NEWLINE}}⬇ <b>Token Change:</b> {{TokenChange}}  {{TokenChangeSymbol}}{{NEWLINE}}⬆ <b>Token Received:</b> {{TokenReceived}} {{TokenReceivedSymbol}}{{NEWLINE}}🔒 <b>Ca:</b> {{Ca}}{{NEWLINE}}📆 <b>Date:</b> {{Date}}{{NEWLINE}}🔁 <b>Position Swap:</b> {{PositionSwap}} %{{NEWLINE}}⬇📊 <a href=''https://birdeye.so/token/{{TokenReceivedHash}}?chain=solana''>Chart</a>{{NEWLINE}}⬆📊 <a href=''https://birdeye.so/token/{{TokenSendedHash}}?chain=solana''>Chart</a>', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Signature}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'Signature', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{WalletHash}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'WalletHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ClassWallet}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'ClassWallet', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenChange}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueSource', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenChangeSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[0].Symbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenReceived}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueDestination', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenReceivedSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Symbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Ca}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Hash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Date}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Utils.Transfer.TransferInfo', 'DataOfTransfer', NULL, NULL, NULL, 1, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{PositionSwap}}', @IdAlertInformation, 'SyncronizationBot.Application.Response.MainCommands.AddUpdate.RecoveryAddUpdateBalanceItemCommandResponse', 'PercentModify', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenReceivedHash}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Hash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenSendedHash}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[0].Hash', NULL, NULL, NULL, 0, 0, 0);

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 5; -- POOL CREATED
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** POOL CREATED ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>🌊🌊🌊🌊🌊🌊🌊🌊🌊🌊🌊</tg-emoji>{{NEWLINE}}🖌 <b>Signature:</b>{{Signature}}{{NEWLINE}}💼 <b>WalletHash:</b>{{WalletHash}}{{NEWLINE}}📰 <b>ClassWallet:</b>{{ClassWallet}}{{NEWLINE}}💰 <b>Amount Pool:</b>{{QuantitySend}}  {{QuantitySendSymbol}}{{NEWLINE}}💰 <b>Amount Pool:</b>{{QuantitySendPool}} {{QuantitySendPoolSymbol}}{{NEWLINE}}🔒 <b>Ca Token Pool:</b> {{CaSended}}{{NEWLINE}}🔒 <b>Ca Token Pool:</b> {{CaSendedPool}}{{NEWLINE}}📆 <b>Date:</b>{{Date}}{{NEWLINE}}📊 <a href=''https://birdeye.so/token/{{CaSended}}?chain=solana''> Chart Sended</a>{{{NEWLINE}}📊 <a href=''https://birdeye.so/token/{{CaSendedPool}}?chain=solana''> Chart Sended 2</a>', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Signature}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'Signature', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{WalletHash}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'WalletHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ClassWallet}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'ClassWallet', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantitySend}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueSource', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantitySendSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[0].Symbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantitySendPool}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueSourcePool', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantitySendPoolSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[1].Symbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{CaSended}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[0].Hash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{CaSendedPool}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[1].Hash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Date}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Utils.Transfer.TransferInfo', 'DataOfTransfer', NULL, NULL, NULL, 1, 0, 0);

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 6; -- POOL FINISH
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** POOL FINALIZED ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>❌❌❌❌❌❌❌❌❌❌❌</tg-emoji>{{NEWLINE}}🖌 <b>Signature:</b>{{Signature}}{{NEWLINE}}💼 <b>WalletHash:</b>{{WalletHash}}{{NEWLINE}}📰 <b>ClassWallet:</b>{{ClassWallet}}{{NEWLINE}}💰 <b>Amount Pool:</b> {{QuantityReceived}} {{QuantityReceivedSymbol}}{{NEWLINE}}💰 <b>Amount Pool:</b> {{QuantityReceivedPool}} {{QuantityReceivedPoolSymbol}}{{NEWLINE}}🔒 <b>Ca Token Pool:</b>{{CaReceived}}{{NEWLINE}}🔒 <b>Ca Token Pool:</b>{{CaReceivedPool}}{{NEWLINE}}📆 <b>Date:</b>{{Date}}{{NEWLINE}}📊 <a href=''https://birdeye.so/token/{{CaReceived}}?chain=solana''>Chart Received</a>{{NEWLINE}}📊 <a href=''https://birdeye.so/token/{{CaReceivedPool}}?chain=solana''>Chart Received 2</a>', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Signature}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'Signature', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{WalletHash}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'WalletHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ClassWallet}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'ClassWallet', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityReceived}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueDestination', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityReceivedSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Symbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityReceivedPool}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueDestinationPool', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityReceivedPoolSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[3].Symbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{CaReceived}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Hash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{CaReceivedPool}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[3].Hash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Date}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Utils.Transfer.TransferInfo', 'DataOfTransfer', NULL, NULL, NULL, 1, 0, 0);

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 7; -- Alert Price
-- ALERT PRICE UP
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** PRICE UP ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>🔥🔥🔥🔥🔥🔥🔥🔥🔥🔥🔥</tg-emoji>{{NEWLINE}}🔒 <b>Token Hash:</b> {{TokenHash}}{{NEWLINE}}⚠ <b>Token Name:</b> {{TokenName}}{{NEWLINE}}💲 <b>New Price Change:</b> {{PriceChance}}{{NEWLINE}}🔁 <b>Is Recurrency Alert:</b>  {{IsRecurrencyAlert}}{{NEWLINE}}', 1, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenHash}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.AlertPrice', 'TokenHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenName}}', @IdAlertInformation, 'SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse', 'Name', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{PriceChance}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Response.TokenData', 'Price', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{IsRecurrencyAlert}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.AlertPrice', 'IsRecurrence', NULL, NULL, NULL, 0, 0, 0);
-- ALERT PRICE DOWN
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** PRICE DOWN ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>🚨🚨🚨🚨🚨🚨🚨🚨🚨🚨🚨</tg-emoji>{{NEWLINE}}🔒 <b>Token Hash:</b> {{TokenHash}}{{NEWLINE}}⚠ <b>Token Name:</b> {{TokenName}}{{NEWLINE}}💲 <b>New Price Change:</b> {{PriceChance}}{{NEWLINE}}🔁 <b>Is Recurrency Alert:</b>  {{IsRecurrencyAlert}}{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>💸💸💸💸💸💸💸💸💸💸💸</tg-emoji>{{NEWLINE}}', 2, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenHash}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.AlertPrice', 'TokenHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenName}}', @IdAlertInformation, 'SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse', 'Name', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{PriceChance}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Response.TokenData', 'Price', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{IsRecurrencyAlert}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.AlertPrice', 'IsRecurrence', NULL, NULL, NULL, 0, 0, 0);

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 8; -- Alert Token Alpha
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** POTENCIAL ALPHA INFORMATION ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>✴✴✴✴✴✴✴✴✴✴✴</tg-emoji>{{NEWLINE}}📰 <b>Alpha Classification:</b> {{AlphaRange}}{{NEWLINE}}☎ <b>CallNumber:</b> {{CallNumber}}{{NEWLINE}}🔒 <b>Token Ca:</b> {{TokenCa}}{{NEWLINE}}⚠ <b>Name:</b> {{TokenName}}{{NEWLINE}}🪙 <b>Symbol:</b> {{TokenSymbol}}{{NEWLINE}}💰 <b>MarketCap:</b> {{MarketCap}}{{NEWLINE}}💲 <b>Price:</b> {{Price}}{{NEWLINE}}💰 <b>Actual MarketCap:</b> {{ActualMarketCap}}{{NEWLINE}}💲 <b>Actual Price:</b> {{ActualPrice}}{{NEWLINE}}💼 <b>TotalWalletsBuy:</b> {{TotalWalletsBuy}}{{NEWLINE}}💲 <b>ValueBuyInSol:</b> {{ValueBuyInSol}}{{NEWLINE}}💲 <b>ValueBuyInUSD:</b> {{ValueBuyInUSD}}{{NEWLINE}}*⃣  <b>Quanity Token:</b> {{QuantityToken}}{{NEWLINE}}💲 <b>ValueReceivedInSol:</b> {{ValueSellInSol}}{{NEWLINE}}💲 <b>ValueReceivedInUSD:</b> {{ValueSellInUSD}}{{NEWLINE}}*⃣  <b>Quanity Token Sell:</b> {{QuantityTokenSell}}{{NEWLINE}}💼 <b>Wallets:</b> {{NEWLINE}}{{RangeWallets}}🔎 <b>Classifications:</b>{{NEWLINE}}{{Classifications}}{{NEWLINE}}📊 <a href=''https://birdeye.so/token/{{TokenCa}}?chain=solana''>Chart</a>', 1, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{AlphaRange}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlphaConfiguration', 'Name', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{CallNumber}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'CallNumber', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenCa}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'TokenHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'TokenName', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenSymbol}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'TokenSymbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{MarketCap}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'InitialMarketcap', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Price}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'InitialPrice', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ActualMarketCap}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'ActualMarketcap', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ActualPrice}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'ActualPrice', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TotalWalletsBuy}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Count', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueBuyInSol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueSpentSol', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueBuyInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueSpentUSDC', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityToken}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|QuantityToken', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInSol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedSol', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedUSDC', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityTokenSell}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|QuantityTokenSell', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{RangeWallets}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'RANGE-ALL|WalletHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Classifications}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'RANGE-ALL|ClassWalletDescription', NULL, NULL, NULL, 0, 0, 0);

SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** POTENCIAL ALPHA INFORMATION ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>✴✴✴✴✴✴✴✴✴✴✴</tg-emoji>{{NEWLINE}}📰 <b>Alpha Classification:</b> {{AlphaRange}}{{NEWLINE}}☎ <b>CallNumber:</b> {{CallNumber}}{{NEWLINE}}🔒 <b>Token Ca:</b> {{TokenCa}}{{NEWLINE}}⚠ <b>Name:</b> {{TokenName}}{{NEWLINE}}🪙 <b>Symbol:</b> {{TokenSymbol}}{{NEWLINE}}💰 <b>MarketCap:</b> {{MarketCap}}{{NEWLINE}}💲 <b>Price:</b> {{Price}}{{NEWLINE}}💰 <b>Actual MarketCap:</b> {{ActualMarketCap}}{{NEWLINE}}💲 <b>Actual Price:</b> {{ActualPrice}}{{NEWLINE}}💼 <b>TotalWalletsBuy:</b> {{TotalWalletsBuy}}{{NEWLINE}}💲 <b>ValueBuyInSol:</b> {{ValueBuyInSol}}{{NEWLINE}}💲 <b>ValueBuyInUSD:</b> {{ValueBuyInUSD}}{{NEWLINE}}*⃣  <b>Quanity Token:</b> {{QuantityToken}}{{NEWLINE}}💲 <b>ValueReceivedInSol:</b> {{ValueSellInSol}}{{NEWLINE}}💲 <b>ValueReceivedInUSD:</b> {{ValueSellInUSD}}{{NEWLINE}}*⃣  <b>Quanity Token Sell:</b> {{QuantityTokenSell}}{{NEWLINE}}💼 <b>Wallets:</b> {{NEWLINE}}{{RangeWallets}}🔎 <b>Classifications:</b>{{NEWLINE}}{{Classifications}}{{NEWLINE}}📊 <a href=''https://birdeye.so/token/{{TokenCa}}?chain=solana''>Chart</a>', 2, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{AlphaRange}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlphaConfiguration', 'Name', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{CallNumber}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'CallNumber', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenCa}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'TokenHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'TokenName', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenSymbol}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'TokenSymbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{MarketCap}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'InitialMarketcap', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Price}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'InitialPrice', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ActualMarketCap}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'ActualMarketcap', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ActualPrice}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'ActualPrice', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TotalWalletsBuy}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Count', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueBuyInSol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueSpentSol', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueBuyInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueSpentUSDC', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityToken}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|QuantityToken', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInSol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedSol', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedUSDC', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityTokenSell}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|QuantityTokenSell', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{RangeWallets}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'RANGE-ALL|WalletHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Classifications}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'RANGE-ALL|ClassWalletDescription', NULL, NULL, NULL, 0, 0, 0);


SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** DETECTED ALPHA INFORMATION ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>⚠⚠⚠⚠⚠⚠⚠⚠⚠⚠⚠⚠</tg-emoji>{{NEWLINE}}📰 <b>Alpha Classification:</b> {{AlphaRange}}{{NEWLINE}}☎ <b>CallNumber:</b> {{CallNumber}}{{NEWLINE}}🔒 <b>Token Ca:</b> {{TokenCa}}{{NEWLINE}}⚠ <b>Name:</b> {{TokenName}}{{NEWLINE}}🪙 <b>Symbol:</b> {{TokenSymbol}}{{NEWLINE}}💰 <b>MarketCap:</b> {{MarketCap}}{{NEWLINE}}💲 <b>Price:</b> {{Price}}{{NEWLINE}}💰 <b>Actual MarketCap:</b> {{ActualMarketCap}}{{NEWLINE}}💲 <b>Actual Price:</b> {{ActualPrice}}{{NEWLINE}}💼 <b>TotalWalletsBuy:</b> {{TotalWalletsBuy}}{{NEWLINE}}💲 <b>ValueBuyInSol:</b> {{ValueBuyInSol}}{{NEWLINE}}💲 <b>ValueBuyInUSD:</b> {{ValueBuyInUSD}}{{NEWLINE}}*⃣  <b>Quanity Token:</b> {{QuantityToken}}{{NEWLINE}}💲 <b>ValueReceivedInSol:</b> {{ValueSellInSol}}{{NEWLINE}}💲 <b>ValueReceivedInUSD:</b> {{ValueSellInUSD}}{{NEWLINE}}*⃣  <b>Quanity Token Sell:</b> {{QuantityTokenSell}}{{NEWLINE}}💼 <b>Wallets:</b> {{NEWLINE}}{{RangeWallets}}🔎 <b>Classifications:</b>{{NEWLINE}}{{Classifications}}{{NEWLINE}}📊 <a href=''https://birdeye.so/token/{{TokenCa}}?chain=solana''>Chart</a>', 3, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{AlphaRange}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlphaConfiguration', 'Name', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{CallNumber}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'CallNumber', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenCa}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'TokenHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'TokenName', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenSymbol}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'TokenSymbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{MarketCap}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'InitialMarketcap', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Price}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'InitialPrice', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ActualMarketCap}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'ActualMarketcap', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ActualPrice}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'ActualPrice', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TotalWalletsBuy}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Count', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueBuyInSol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueSpentSol', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueBuyInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueSpentUSDC', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityToken}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|QuantityToken', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInSol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedSol', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedUSDC', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityTokenSell}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|QuantityTokenSell', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{RangeWallets}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'RANGE-ALL|WalletHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Classifications}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'RANGE-ALL|ClassWalletDescription', NULL, NULL, NULL, 0, 0, 0);

SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** CONFIRMED ALPHA INFORMATION ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>✅✅✅✅✅✅✅✅✅✅✅</tg-emoji>{{NEWLINE}}📰 <b>Alpha Classification:</b> {{AlphaRange}}{{NEWLINE}}☎ <b>CallNumber:</b> {{CallNumber}}{{NEWLINE}}🔒 <b>Token Ca:</b> {{TokenCa}}{{NEWLINE}}⚠ <b>Name:</b> {{TokenName}}{{NEWLINE}}🪙 <b>Symbol:</b> {{TokenSymbol}}{{NEWLINE}}💰 <b>MarketCap:</b> {{MarketCap}}{{NEWLINE}}💲 <b>Price:</b> {{Price}}{{NEWLINE}}💰 <b>Actual MarketCap:</b> {{ActualMarketCap}}{{NEWLINE}}💲 <b>Actual Price:</b> {{ActualPrice}}{{NEWLINE}}💼 <b>TotalWalletsBuy:</b> {{TotalWalletsBuy}}{{NEWLINE}}💲 <b>ValueBuyInSol:</b> {{ValueBuyInSol}}{{NEWLINE}}💲 <b>ValueBuyInUSD:</b> {{ValueBuyInUSD}}{{NEWLINE}}*⃣  <b>Quanity Token:</b> {{QuantityToken}}{{NEWLINE}}💲 <b>ValueReceivedInSol:</b> {{ValueSellInSol}}{{NEWLINE}}💲 <b>ValueReceivedInUSD:</b> {{ValueSellInUSD}}{{NEWLINE}}*⃣  <b>Quanity Token Sell:</b> {{QuantityTokenSell}}{{NEWLINE}}💼 <b>Wallets:</b> {{NEWLINE}}{{RangeWallets}}🔎 <b>Classifications:</b>{{NEWLINE}}{{Classifications}}{{NEWLINE}}📊 <a href=''https://birdeye.so/token/{{TokenCa}}?chain=solana''>Chart</a>', 4, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{AlphaRange}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlphaConfiguration', 'Name', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{CallNumber}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'CallNumber', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenCa}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'TokenHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'TokenName', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenSymbol}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'TokenSymbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{MarketCap}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'InitialMarketcap', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Price}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'InitialPrice', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ActualMarketCap}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'ActualMarketcap', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ActualPrice}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'ActualPrice', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TotalWalletsBuy}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Count', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueBuyInSol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueSpentSol', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueBuyInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueSpentUSDC', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityToken}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|QuantityToken', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInSol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedSol', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedUSDC', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityTokenSell}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|QuantityTokenSell', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{RangeWallets}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'RANGE-ALL|WalletHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Classifications}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'RANGE-ALL|ClassWalletDescription', NULL, NULL, NULL, 0, 0, 0);

SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** TOKEN ALPHA INFORMATION ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>✅✅⚠⚠💲💲💲⚠⚠✅✅</tg-emoji>{{NEWLINE}}📰 <b>Alpha Classification:</b> {{AlphaRange}}{{NEWLINE}}☎ <b>CallNumber:</b> {{CallNumber}}{{NEWLINE}}🔒 <b>Token Ca:</b> {{TokenCa}}{{NEWLINE}}⚠ <b>Name:</b> {{TokenName}}{{NEWLINE}}🪙 <b>Symbol:</b> {{TokenSymbol}}{{NEWLINE}}💰 <b>MarketCap:</b> {{MarketCap}}{{NEWLINE}}💲 <b>Price:</b> {{Price}}{{NEWLINE}}💰 <b>Actual MarketCap:</b> {{ActualMarketCap}}{{NEWLINE}}💲 <b>Actual Price:</b> {{ActualPrice}}{{NEWLINE}}💼 <b>TotalWalletsBuy:</b> {{TotalWalletsBuy}}{{NEWLINE}}💲 <b>ValueBuyInSol:</b> {{ValueBuyInSol}}{{NEWLINE}}💲 <b>ValueBuyInUSD:</b> {{ValueBuyInUSD}}{{NEWLINE}}*⃣  <b>Quanity Token:</b> {{QuantityToken}}{{NEWLINE}}💲 <b>ValueReceivedInSol:</b> {{ValueSellInSol}}{{NEWLINE}}💲 <b>ValueReceivedInUSD:</b> {{ValueSellInUSD}}{{NEWLINE}}*⃣  <b>Quanity Token Sell:</b> {{QuantityTokenSell}}{{NEWLINE}}💼 <b>Wallets:</b> {{NEWLINE}}{{RangeWallets}}🔎 <b>Classifications:</b>{{NEWLINE}}{{Classifications}}{{NEWLINE}}📊 <a href=''https://birdeye.so/token/{{TokenCa}}?chain=solana''>Chart</a>', 5, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{AlphaRange}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlphaConfiguration', 'Name', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{CallNumber}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'CallNumber', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenCa}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'TokenHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'TokenName', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenSymbol}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'TokenSymbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{MarketCap}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'InitialMarketcap', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Price}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'InitialPrice', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ActualMarketCap}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'ActualMarketcap', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ActualPrice}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.TokenAlpha', 'ActualPrice', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TotalWalletsBuy}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Count', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueBuyInSol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueSpentSol', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueBuyInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueSpentUSDC', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityToken}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|QuantityToken', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInSol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedSol', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedUSDC', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityTokenSell}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|QuantityTokenSell', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{RangeWallets}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'RANGE-ALL|WalletHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Classifications}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'RANGE-ALL|ClassWalletDescription', NULL, NULL, NULL, 0, 0, 0);
------------------------------------------------------------
