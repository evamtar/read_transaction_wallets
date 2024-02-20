IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TransactionRPCRecovery')
BEGIN
	DROP TABLE [TransactionRPCRecovery]
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

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TransactionOldForMapping')
BEGIN
	DROP TABLE [TransactionOldForMapping]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'WalletBalanceHistory')
BEGIN
	DROP TABLE [WalletBalanceHistory]
END
GO
IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TransactionToken')
BEGIN
	DROP TABLE [TransactionToken]
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

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'Transactions')
BEGIN
	DROP TABLE [Transactions]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TypeOperation')
BEGIN
	DROP TABLE [TypeOperation]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TokenPriceHistory')
BEGIN
	DROP TABLE [TokenPriceHistory]
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

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TelegramMessage')
BEGIN
	DROP TABLE [TelegramMessage]
END
GO

IF EXISTS(SELECT 1 FROM SYS.TABLES WHERE NAME = 'TelegramChannel')
BEGIN
	DROP TABLE [TelegramChannel]
END
GO


CREATE TABLE TelegramChannel(
	ID               UNIQUEIDENTIFIER,
	ChannelId        DECIMAL(30,0),
	ChannelName      VARCHAR(50),
	TimeBeforeDelete INT,
	PRIMARY KEY (ID)
);
GO 

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
GO

CREATE TABLE RunTimeController
(	
	RuntimeId				 INT,
	ConfigurationTimer		 VARCHAR(100),
	JobName                  VARCHAR(200),
	JobDescription		     VARCHAR(500),
	FullClassName		     VARCHAR(1000),
	TypeService				 INT,
	TimesWithoutTransaction  INT,
	IsRunning				 BIT,
	IsContingecyTransaction  BIT,
	IsActive		         BIT,
	PRIMARY KEY(RuntimeId)
);
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

CREATE TABLE Wallet(
	ID                   UNIQUEIDENTIFIER,
	[Hash]               VARCHAR(50),
	ClassWalletId        UNIQUEIDENTIFIER,
	IsLoadBalance        BIT,
	DateLoadBalance      DATETIME2,
	IsActive             BIT,
	LastUpdate           DATETIME2
	PRIMARY KEY (ID),
	FOREIGN KEY (ClassWalletId) REFERENCES ClassWallet(ID)
);

CREATE TABLE Token(
	ID                     UNIQUEIDENTIFIER,
	[Hash]                 VARCHAR(50),
	Symbol                 VARCHAR(500),
	[Name]			       VARCHAR(200),
	Supply                 VARCHAR(150),
	Decimals               INT,
	CreateDate             DATETIME,
	LastUpdate             DATETIME,
	IsLazyLoad             BIT,
	PRIMARY KEY (ID)
);
GO

CREATE TABLE TokenPriceHistory(
	ID                     UNIQUEIDENTIFIER,
	TokenId                UNIQUEIDENTIFIER,
	MarketCap              VARCHAR(150),
	Liquidity              VARCHAR(150),
	UniqueWallet24h        INT,
	UniqueWalletHistory24h INT,
	NumberMarkets          INT,
	CreateDate             DATETIME,
	PRIMARY KEY (ID),
	FOREIGN KEY (TokenId) REFERENCES Token(ID)
);
GO

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
GO

CREATE TABLE TypeOperation(
	ID                           UNIQUEIDENTIFIER,
	Name						 VARCHAR(100),
	IdTypeOperation			     INT,
	PRIMARY KEY (ID)
);
GO

CREATE TABLE Transactions
(
	ID                           UNIQUEIDENTIFIER,
	[Signature]                  VARCHAR(150),
	DateTransactionUTC           DATETIME2,
	FeeTransaction               VARCHAR(150),
	PriceSol				     VARCHAR(150),
	TotalOperationSol			 VARCHAR(150),
	WalletId                     UNIQUEIDENTIFIER,
	TypeOperationId              UNIQUEIDENTIFIER,
	PRIMARY KEY (ID),
	FOREIGN KEY (WalletId) REFERENCES Wallet(ID),
	FOREIGN KEY (TypeOperationId) REFERENCES TypeOperation(ID),
);
GO

CREATE TABLE TransactionToken(
	ID                           UNIQUEIDENTIFIER,
	AmountValue				     VARCHAR(150),
	MtkcapToken                  VARCHAR(150),
	TotalToken					 VARCHAR(150),
	TypeTokenTransaction		 INT, --SENDED (1), RECEIVED(2)
	IsArbitrationOperation       BIT,
	IsPoolOperation				 BIT,
	IsSwapOperation              BIT,
	TokenId			             UNIQUEIDENTIFIER,
	TransactionsId               UNIQUEIDENTIFIER,
    PRIMARY KEY (ID),
	FOREIGN KEY (TokenId) REFERENCES Token(ID),
	FOREIGN KEY (TransactionsId) REFERENCES Transactions(ID),
);

CREATE TABLE TransactionOldForMapping
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

CREATE TABLE TransactionRPCRecovery
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

CREATE TABLE TelegramMessage
(
	ID                 UNIQUEIDENTIFIER,
	MessageId          BIGINT,
	TelegramChannelId  UNIQUEIDENTIFIER,
	EntityId		   UNIQUEIDENTIFIER,
	DateSended         DATETIME2,
	IsDeleted		   BIT,
	TryDeleted         INT,
	PRIMARY KEY (ID),
	FOREIGN KEY(TelegramChannelId) REFERENCES TelegramChannel(ID)
);
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
	TypeOperationId       UNIQUEIDENTIFIER, 
	TelegramChannelId     UNIQUEIDENTIFIER,
	IsActive              BIT,
	CreateDate            DATETIME2,
	LastUpdate            DATETIME2,
	PRIMARY KEY (ID),
	FOREIGN KEY (TelegramChannelId) REFERENCES TelegramChannel(ID),
	FOREIGN KEY (TypeOperationId) REFERENCES TypeOperation(ID)
);

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

------------------------------------------------------------
