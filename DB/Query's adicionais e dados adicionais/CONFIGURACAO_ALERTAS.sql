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
INSERT INTO AlertParameter VALUES (NEWID(), '{{PriceChance}}', @IdAlertInformation, 'SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse', 'Price', NULL, NULL, NULL, 0, 0, 0);
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
INSERT INTO AlertParameter VALUES (NEWID(), '{{Classifications}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'AGGREGATE|ClassWalletDescription', NULL, NULL, NULL, 0, 0, 0);

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
INSERT INTO AlertParameter VALUES (NEWID(), '{{Classifications}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'AGGREGATE|ClassWalletDescription', NULL, NULL, NULL, 0, 0, 0);


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
INSERT INTO AlertParameter VALUES (NEWID(), '{{Classifications}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'AGGREGATE|ClassWalletDescription', NULL, NULL, NULL, 0, 0, 0);

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
INSERT INTO AlertParameter VALUES (NEWID(), '{{Classifications}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'AGGREGATE|ClassWalletDescription', NULL, NULL, NULL, 0, 0, 0);

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
INSERT INTO AlertParameter VALUES (NEWID(), '{{Classifications}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'AGGREGATE|ClassWalletDescription', NULL, NULL, NULL, 0, 0, 0);

DELETE FROM AlertParameter WHERE AlertInformationId IN(SELECT ID FROM AlertInformation WHERE AlertConfigurationId IN(SELECT ID FROM AlertConfiguration WHERE TypeAlert = 9))
DELETE FROM AlertInformation WHERE AlertConfigurationId IN(SELECT ID FROM AlertConfiguration WHERE TypeAlert = 9)

DECLARE @IdAlertConfiguration UNIQUEIDENTIFIER;
DECLARE @IdAlertInformation UNIQUEIDENTIFIER;
SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeAlert = 9; 
SELECT @IdAlertInformation = NEWID();

INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>*** ALERTA DE TRANSACAO DE WHALE PUMP ***</b>{{NEWLINE}}<tg-emoji emoji-id=''5368324170671202286''>🐳🐳🐳🐳🐳🐳🐳🐳🐳🐳🐳</tg-emoji>{{NEWLINE}}📰 🖌 <b>Signature:</b> {{Signature}}{{NEWLINE}} 💼 <b>WalletHash:</b> {{WalletHash}}{{NEWLINE}}🔒 <b>Token Ca:</b> {{TokenCa}}{{NEWLINE}}⚠ <b>Name:</b> {{TokenName}}{{NEWLINE}}🪙 <b>Symbol:</b> {{TokenSymbol}}{{NEWLINE}}💰 <b>MarketCap:</b> {{MarketCap}}{{NEWLINE}}💲 <b>Price:</b> {{Price}}{{NEWLINE}}💰 TransactionValue: {{TransactionValue}} - {{TransactionSymbol}} {{NEWLINE}}📊 <a href=''https://birdeye.so/token/{{TokenCa}}?chain=solana''>Chart</a>', 1, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Signature}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'Signature', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{WalletHash}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'WalletHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenCa}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Hash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenName}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Name', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TokenSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[2].Symbol', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{MarketCap}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'MtkcapTokenDestination', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Price}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'PriceTokenDestinationUSD', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TransactionValue}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Database.Transactions', 'AmountValueSource', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TransactionSymbol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Application.Response.MainCommands.RecoverySave.RecoverySaveTokenCommandResponse]', '[0].Symbol', NULL, NULL, NULL, 0, 0, 0);