--SELECT * FROM TypeOperation
--UPDATE RunTimeController SET IsRunning = 0 WHERE TypeService = 4
--SELECT * FROM RunTimeController WHERE TypeService = 4
--DELETE FROM RunTimeController
/*********** RUNTIME CONTROLLER ***********/ 
INSERT INTO RunTimeController VALUES(1, '1', 'Serviço de transações das carteiras mapeadas', 'Recuperar transações e colocar na fila de processamento', 1, 0, 1);
INSERT INTO RunTimeController VALUES(2, '1', 'Serviço de balanço', 'Carregar Tokens das Wallets', 2, 0, 1);
INSERT INTO RunTimeController VALUES(3, '60', 'Serviço de balanço', 'Atualizar os saldos das Wallets', 3, 0, 1);
INSERT INTO RunTimeController VALUES(4, '1', 'Serviço de alerta de preços', 'Enviar alerta de preços para canal', 4, 0, 1);
INSERT INTO RunTimeController VALUES(5, '1', 'Serviço de exclusão de mensagens', 'Exclusão de mensagens antigas dos canais', 5, 0, 1);
INSERT INTO RunTimeController VALUES(6, '30', 'Serviço de transações antigas', 'Processamento de transações antigas apenas para histórico', 6, 0, 1);
/*********** TYPE OPERATION ***********/ 

/***** SPECIAL TYPE OPERATION *****/ 
INSERT INTO TypeOperation VALUES(NEWID(), 'Log Execute', 0, 1);
INSERT INTO TypeOperation VALUES(NEWID(), 'Log Error', 0, 2);
INSERT INTO TypeOperation VALUES(NEWID(), 'Log App Running', 0, 3);
INSERT INTO TypeOperation VALUES(NEWID(), 'Log Lost Configuration', 0, 4);
INSERT INTO TypeOperation VALUES(NEWID(), 'Price Alert', 0, 5);
INSERT INTO TypeOperation VALUES(NEWID(), 'Token Alpha', 0, 6);

/***** NORMAL TYPE OPERATION *****/ 
INSERT INTO TypeOperation VALUES(NEWID(), 'Buy Operation', 1, 1);
INSERT INTO TypeOperation VALUES(NEWID(), 'ReBuy Operation', 1, 2);
INSERT INTO TypeOperation VALUES(NEWID(), 'Sell Operation', 2, NULL);
INSERT INTO TypeOperation VALUES(NEWID(), 'Arbitration Operation', 3, NULL);
INSERT INTO TypeOperation VALUES(NEWID(), 'Transfer Operation', 4, NULL);
INSERT INTO TypeOperation VALUES(NEWID(), 'Received Operation', 5, NULL);
INSERT INTO TypeOperation VALUES(NEWID(), 'Swap Operation', 6, NULL);
INSERT INTO TypeOperation VALUES(NEWID(), 'Pool Create', 7, NULL);
INSERT INTO TypeOperation VALUES(NEWID(), 'Pool finalized', 8, NULL);
/***** CREATE FOR REBUY TYPE OPERATION *****/ 


/*********** ALPHA CONFIGURATION ***********/ 
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

--/*********** ALERT PRICE TESTE ***********/ 
--DELETE FROM AlertPrice
--DECLARE @TelegramChannelId UNIQUEIDENTIFIER
--SELECT @TelegramChannelId = ID FROM TelegramChannel WHERE ChannelName = 'AlertPriceChange'
--INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '0.008302', '5jcDWDV3HYeFvDBGEfPtk68WNmV7ZoLU8QUvDAnACpnE', '1.3', null, 1, 0, @TelegramChannelId);
--INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '12.180', '3QYAWuowfaLC1CqYKx2eTe1SwV9MqAe1dUZT3NPt3srQ', '23.4', null, 1, 0, @TelegramChannelId);
--INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '12.180', '3QYAWuowfaLC1CqYKx2eTe1SwV9MqAe1dUZT3NPt3srQ', '9.2', null, 2, 0, @TelegramChannelId);
--INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '85.60', 'So11111111111111111111111111111111111111112', '81.58', null, 2, 1, @TelegramChannelId);
--INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '0.000000100380', '2PCegSVAesdb8ffZViieXUC3gH2wku7ieXRteB8Az7o6', '0.000000989687', null, 1, 0, @TelegramChannelId);
--INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '0.000000100380', '2PCegSVAesdb8ffZViieXUC3gH2wku7ieXRteB8Az7o6', '0.000001977806', null, 1, 0, @TelegramChannelId);
--INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '0.000000100380', '2PCegSVAesdb8ffZViieXUC3gH2wku7ieXRteB8Az7o6', '0.000010002757', null, 1, 0, @TelegramChannelId);
--INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '0.000005438', '2PCegSVAesdb8ffZViieXUC3gH2wku7ieXRteB8Az7o6', '0.000003438', null, 2, 0, @TelegramChannelId);
--INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '0.000005438', '2PCegSVAesdb8ffZViieXUC3gH2wku7ieXRteB8Az7o6', '0.000010438', null, 1, 0, @TelegramChannelId);
--INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '0.000005438', '2PCegSVAesdb8ffZViieXUC3gH2wku7ieXRteB8Az7o6', '0.00010438', null, 1, 0, @TelegramChannelId);
--INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '0.000005438', '2PCegSVAesdb8ffZViieXUC3gH2wku7ieXRteB8Az7o6', '0.0010438', null, 1, 0, @TelegramChannelId);
--INSERT INTO AlertPrice VALUES(NEWID(), GETDATE(), NULL, '0.000005438', '2PCegSVAesdb8ffZViieXUC3gH2wku7ieXRteB8Az7o6', '0.010438', null, 1, 0, @TelegramChannelId);
--/*********** ALERT CONFIGURATION ***********/ 
DECLARE @TelegramChannelId UNIQUEIDENTIFIER;
SELECT @TelegramChannelId = ID FROM TelegramChannel WHERE ChannelName = 'CallSolanaLog';
DECLARE @TypeOperationId UNIQUEIDENTIFIER;
/***** Messages For Log *****/
DECLARE @IdAlertConfiguration UNIQUEIDENTIFIER;
DECLARE @IdAlertInformation UNIQUEIDENTIFIER;


SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 0 AND IdSubLevel = 4;
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Log Service Lost Configuration', @TypeOperationId, @TelegramChannelId, 4, GETDATE(), GETDATE());

-- LOG LOST CONFIGURATION
SELECT @IdAlertConfiguration= ID FROM AlertConfiguration WHERE TypeOperationId = @TypeOperationId;
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>Timer do serviço {{ServiceName}} está nulo ou não configurado.</b>{{NEWLINE}}<i><b>Não irá efetuar essa execução:</b> {{DateTimeNow}}</i>.{{NEWLINE}}', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ServiceName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'ServiceName', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{DateTimeNow}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'DateExecuted', NULL, NULL, NULL, 0, 0, 0);

-- LOG RUNNING
SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 0  AND IdSubLevel = 3;
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Log Service Running', @TypeOperationId, @TelegramChannelId, 1, GETDATE(), GETDATE());

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeOperationId = @TypeOperationId;
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>O serviço {{ServiceName}} está rodando.</b>{{NEWLINE}}<i><b>Não irá efetuar essa execução:</b> {{DateTimeNow}}</i>.{{NEWLINE}}', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ServiceName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'ServiceName', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{DateTimeNow}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'DateExecuted', NULL, NULL, NULL, 0, 0, 0);

-- LOG ERROR
SELECT @IdAlertInformation = NEWID();
SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 0 AND IdSubLevel = 2;
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Log Error', @TypeOperationId, @TelegramChannelId, 1, GETDATE(), GETDATE());

SELECT @IdAlertConfiguration= ID FROM AlertConfiguration WHERE TypeOperationId = @TypeOperationId;
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>O serviço {{ServiceName}} suspendeu a execução.</b>{{NEWLINE}}<i><b>Mensagem de erro:</b> {{ErrorMessage}}</i>.{{NEWLINE}}StackTrace: {{StackTrace}}{{NEWLINE}}<i><b>Proxima execução</b> no período timer de --> {{TimerExecute}}.{{NEWLINE}}<b>Dev''s Favor verificar</b> Cc:@evandrotartari , @euRodrigo</i>{{NEWLINE}}', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ServiceName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'ServiceName', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{DateTimeNow}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'DateExecuted', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ErrorMessage}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'Exception.Message', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{StackTrace}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'Exception.StackTrace', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TimerExecute}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'Timer', NULL, NULL, NULL, 0, 0, 0);


-- LOG EXECUTE
SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 0 AND IdSubLevel = 1;
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Log Execute', @TypeOperationId, @TelegramChannelId, 1, GETDATE(), GETDATE());

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeOperationId = @TypeOperationId;
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>Execução do serviço {{ServiceName}} de call solana</b>{{NEWLINE}}<b>Data Execução: </b>{{DateTimeNow}}.{{NEWLINE}}<i><b>Proxima execução</b> no período timer de --> {{TimerExecute}}</i>{{NEWLINE}}', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ServiceName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'ServiceName', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{DateTimeNow}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'DateExecuted', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TimerExecute}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'Timer', NULL, NULL, NULL, 0, 0, 0);



/***** Messages For Price Alert *****/
SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 0 AND IdSubLevel = 5;
SELECT @TelegramChannelId = ID FROM TelegramChannel WHERE ChannelName = 'AlertPriceChange';
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Price', @TypeOperationId, @TelegramChannelId, 1, GETDATE(), GETDATE());

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeOperationiD = @TypeOperationId; -- Alert Price
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


/***** Messages For Token Alpha *****/
SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 0 AND IdSubLevel = 6;
SELECT @TelegramChannelId = ID FROM TelegramChannel WHERE ChannelName = 'TokenAlpha';
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Token Alpha', @TypeOperationId, @TelegramChannelId, 1, GETDATE(), GETDATE());

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeOperationiD = @TypeOperationId; -- Alert Token Alpha
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
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueBuyInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueSpentUSD', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityToken}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|QuantityToken', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInSol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedSol', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedUSD', NULL, NULL, 'N2', 0, 0, 0);
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
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueBuyInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueSpentUSD', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityToken}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|QuantityToken', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInSol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedSol', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedUSD', NULL, NULL, 'N2', 0, 0, 0);
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
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueBuyInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueSpentUSD', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityToken}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|QuantityToken', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInSol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedSol', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedUSD', NULL, NULL, 'N2', 0, 0, 0);
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
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueBuyInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueSpentUSD', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityToken}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|QuantityToken', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInSol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedSol', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedUSD', NULL, NULL, 'N2', 0, 0, 0);
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
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueBuyInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueSpentUSD', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityToken}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|QuantityToken', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInSol}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedSol', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ValueSellInUSD}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|ValueReceivedUSD', NULL, NULL, 'N2', 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{QuantityTokenSell}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'Invoke-Sum|QuantityTokenSell', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{RangeWallets}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'RANGE-ALL|WalletHash', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{Classifications}}', @IdAlertInformation, 'System.Collections.Generic.List`1[SyncronizationBot.Domain.Model.Database.TokenAlphaWallet]', 'RANGE-ALL|ClassWalletDescription', NULL, NULL, NULL, 0, 0, 0);

/***** Messages For Transactions *****/
SELECT @TelegramChannelId = ID FROM TelegramChannel WHERE ChannelName = 'CallSolana';
SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 1 AND IdSubLevel = 1;
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Buy', @TypeOperationId, @TelegramChannelId, 1, GETDATE(), GETDATE());

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeOperationiD = @TypeOperationId;
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

--REBUY
SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 1 AND IdSubLevel = 2;
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Rebuy', @TypeOperationId, @TelegramChannelId,  1, GETDATE(), GETDATE());

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeOperationiD = @TypeOperationId; --REBUY
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

--SELL
SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 2;
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Sell', @TypeOperationId, @TelegramChannelId, 1, GETDATE(), GETDATE());

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeOperationiD = @TypeOperationId; --SELL
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


SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 3;
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Arbitration', @TypeOperationId, @TelegramChannelId, 1, GETDATE(), GETDATE());
--TODO

SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 4;
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert Transfer Sended', @TypeOperationId, @TelegramChannelId, 1, GETDATE(), GETDATE());
--TODO

SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 5;
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert Transfer Received', @TypeOperationId, @TelegramChannelId, 1, GETDATE(), GETDATE());
--TODO

SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 6;
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Swap', @TypeOperationId, @TelegramChannelId, 1, GETDATE(), GETDATE());

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeOperationiD = @TypeOperationId; --SWAP
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

SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 7;

INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Pool Create', @TypeOperationId, @TelegramChannelId, 1, GETDATE(), GETDATE());
SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 8;

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeOperationiD = @TypeOperationId; -- POOL CREATED
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

INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Pool Finish', @TypeOperationId, @TelegramChannelId, 1, GETDATE(), GETDATE());

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeOperationiD = @TypeOperationId; -- POOL FINISH
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

/************************** WALLETS FOR TEST *****************************/
DECLARE @ClassWalletId UNIQUEIDENTIFIER = NEWID()
INSERT INTO ClassWallet VALUES(@ClassWalletId, 1, 'TESTE WALLETS')
INSERT INTO Wallet VALUES(NEWID(), '5niysgHXFoa8apmrgeBNRXJ6yPiz4WnMnVnAobUXoaMh', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), '6r9xLdcMrYGKFnksQd752HBuvNDcb2ZeHMBwzjAyUqrL', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), 'Bo1d3W7F4Jk6yAxAtkCanZMTfBPVA13N1oC73XvPiAmT', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), 'HNkZxJUCAAcF1QbAi6nrughS4U6iTzbhwcqoo4pPBRNH', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), 'ATomG2gRJcB2jNvwcjo2zBMoyLsHZzLgwqy651zzYoCq', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), '75RBnyF168q4txJcURgE3eVqyazPaoderqv18LWETvM6', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), 'ZG98FUCjb8mJ824Gbs6RsgVmr1FhXb2oNiJHa2dwmPd',  @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), 'DCAK8tuwzsNowVA6eSojLHhHSDQMyECuSbu7KovyvYbm', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), 'C2X8MT6MZfM2GMLeoaJvdnMCqbSQDJ2kHk3vsCHrZ2H5', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), 'EccxYg7rViwYfn9EMoNu7sUaV82QGyFt6ewiQaH1GYjv', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), '3sf1qf2pGyL6uvz2bHFMo52wGhevCdF5XwBtwom4pQNA', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), 's98WxZtDFKL7D1RtKnmJMavvvUQJ2ikDbkZ7XeFhAmz',  @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), '7EcSiGfi21fSKhawxK4xWf5CLZX54W5Bk7UMuzX7N16P', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), 'DXgCVkmVC1PDfmbKGKi7rwVUzKfJ6BLpmXHhxoDX3fKp', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), '2VeBttDPvHUaeFTSzsWLrSvdcqKkAZCu3PHVHJtdG5ch', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), '9Ak6WuHsCcQeSvZ2XN43M2QddcLqkxZSosSbbUVzwYoj', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), '48tnujTTbc4h7Wkr6FjubF1NvaETWGAFjUEPCFfqfk23', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), '5tUJdqCfo812RSJHkLVvAQDm8xoRfqr2X8T7nxThuqe6', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), '9NuRc1sG9MdCfqjTVmmEb9E2CvndeWSpaw52opk1zftz', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), 'EYy9PNdpJmB9FTYWdPGb7L9HoZUDJxVFSLphYXaMgLXo', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), '9NrSmuETVePLbcQNDeSwNxBFzpeyF6EfHv8YB2CgdWZM', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), 'AX3kwXuXTbmJYaS6CtezARDj7Kqht5dYx8JV4vLj6xBJ', @ClassWalletId, 0, NULL, 1, NULL);
INSERT INTO Wallet VALUES(NEWID(), 'H7d3HRfSG6iAePUXX24gKQgHkiooaSvcPeDzXvmGvy6M', @ClassWalletId, 0, NULL, 1, NULL);

--DECLARE @SqlStatement NVARCHAR(MAX)
--SELECT @SqlStatement = 
--    COALESCE(@SqlStatement, N'') + N'DROP TABLE [DBO].' + QUOTENAME(TABLE_NAME) + N';' + CHAR(13)
--FROM INFORMATION_SCHEMA.TABLES
--WHERE TABLE_SCHEMA = 'DBO' and TABLE_TYPE = 'BASE TABLE'

--PRINT @SqlStatement