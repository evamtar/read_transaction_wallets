UPDATE RunTimeController SET IsRunning = 0
SELECT * FROM RunTimeController WHERE TypeService = 2
/*********** RUNTIME CONTROLLER ***********/ 
--INSERT INTO RunTimeController VALUES(1, '1', 'Main Job Controller', 'Serviço de controle de job''s', '', 1, 0, 0, 1, null);
--INSERT INTO RunTimeController VALUES(1, '1', 'Alerta de Transações', 'Serviço de alerta de transações', '', 2, 0, 0, 0, 1, null);
--INSERT INTO RunTimeController VALUES(1, '1', 'Carregar Tokens das Wallets e Atualizar saldo', 'Serviço de balanço e saldos', '', 2, 0, 0, 0, 1, null);
INSERT INTO RunTimeController VALUES(1, '1', 'Serviço de balanço e saldos', 'Carregar Tokens das Wallets e Atualizar saldo', '', 2, 0, 0, 0, 1, null);
--INSERT INTO RunTimeController VALUES(3, '1', 3, 0, 0, null, 'Alerta de preços');
--INSERT INTO RunTimeController VALUES(4, '4', 4, 0, 0, null, 'Excluir mensagens de log antigas');
--INSERT INTO RunTimeController VALUES(5, '1', 5, 0, 0, null, 'Alerta de Token Alpha');
--INSERT INTO RunTimeController VALUES(6, '1', 6, 0, 0, null, 'Transacões Antigas para Mapear');
--INSERT INTO RunTimeController VALUES(7, '1', 6, 0, 0, null, 'Carregar Listagem de Novos Tokens');

/*********** TYPE OPERATION ***********/ 
/***** SPECIAL TYPE OPERATION *****/ 
INSERT INTO TypeOperation VALUES(NEWID(), 'Log Lost Configuration', -5);
INSERT INTO TypeOperation VALUES(NEWID(), 'Log App Running', -4);
INSERT INTO TypeOperation VALUES(NEWID(), 'Log Error', -3);
INSERT INTO TypeOperation VALUES(NEWID(), 'Log Execute', -2);
INSERT INTO TypeOperation VALUES(NEWID(), 'Price Alert', -1);
INSERT INTO TypeOperation VALUES(NEWID(), 'Token Alpha', 0);
/***** NORMAL TYPE OPERATION *****/ 
INSERT INTO TypeOperation VALUES(NEWID(), 'Buy Operation', 1);
INSERT INTO TypeOperation VALUES(NEWID(), 'Sell Operation', 2);
INSERT INTO TypeOperation VALUES(NEWID(), 'Arbitration Operation', 3);
INSERT INTO TypeOperation VALUES(NEWID(), 'Transfer Operation', 4);
INSERT INTO TypeOperation VALUES(NEWID(), 'Received Operation', 5);
INSERT INTO TypeOperation VALUES(NEWID(), 'Swap Operation', 6);
INSERT INTO TypeOperation VALUES(NEWID(), 'Pool Create', 7);
INSERT INTO TypeOperation VALUES(NEWID(), 'Pool finalized', 8);
/***** CREATE FOR REBUY TYPE OPERATION *****/ 
INSERT INTO TypeOperation VALUES(NEWID(), 'ReBuy Operation', 9);

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

SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = -5;
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Log Service Lost Configuration', @TypeOperationId, @TelegramChannelId, 1, GETDATE(), GETDATE());

-- LOG LOST CONFIGURATION
SELECT @IdAlertConfiguration= ID FROM AlertConfiguration WHERE TypeOperationId = @TypeOperationId;
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>Timer do serviço {{ServiceName}} está nulo ou não configurado.</b>{{NEWLINE}}<i><b>Não irá efetuar essa execução:</b> {{DateTimeNow}}</i>.{{NEWLINE}}', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ServiceName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'ServiceName', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{DateTimeNow}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'DateExecuted', NULL, NULL, NULL, 0, 0, 0);

SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = -4;
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Log Service Running', @TypeOperationId, @TelegramChannelId, 1, GETDATE(), GETDATE());

-- LOG RUNNING
SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeOperationId = @TypeOperationId;
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>O serviço {{ServiceName}} está rodando.</b>{{NEWLINE}}<i><b>Não irá efetuar essa execução:</b> {{DateTimeNow}}</i>.{{NEWLINE}}', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ServiceName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'ServiceName', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{DateTimeNow}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'DateExecuted', NULL, NULL, NULL, 0, 0, 0);

-- LOG SUCESSO
SELECT @IdAlertInformation = NEWID();
SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = -3;
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Log Error', @TypeOperationId, @TelegramChannelId, 1, GETDATE(), GETDATE());

SELECT @IdAlertConfiguration= ID FROM AlertConfiguration WHERE TypeOperationId = @TypeOperationId;
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>O serviço {{ServiceName}} suspendeu a execução.</b>{{NEWLINE}}<i><b>Mensagem de erro:</b> {{ErrorMessage}}</i>.{{NEWLINE}}StackTrace: {{StackTrace}}{{NEWLINE}}<i><b>Proxima execução</b> no período timer de --> {{TimerExecute}}.{{NEWLINE}}<b>Dev''s Favor verificar</b> Cc:@evandrotartari , @euRodrigo</i>{{NEWLINE}}', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ServiceName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'ServiceName', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{DateTimeNow}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'DateExecuted', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ErrorMessage}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'Exception.Message', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{StackTrace}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'Exception.StackTrace', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TimerExecute}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'Timer', NULL, NULL, NULL, 0, 0, 0);

-- LOG ERROR
SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = -2;
INSERT INTO AlertConfiguration VALUES(NEWID(), 'Alert For Log Execute', @TypeOperationId, @TelegramChannelId, 1, GETDATE(), GETDATE());

SELECT @IdAlertConfiguration = ID FROM AlertConfiguration WHERE TypeOperationId = @TypeOperationId;
SELECT @IdAlertInformation = NEWID();
INSERT INTO AlertInformation VALUES(@IdAlertInformation, N'<b>Execução do serviço {{ServiceName}} de call solana</b>{{NEWLINE}}<b>Data Execução: </b>{{DateTimeNow}}.{{NEWLINE}}<i><b>Proxima execução</b> no período timer de --> {{TimerExecute}}</i>{{NEWLINE}}', null, @IdAlertConfiguration);
INSERT INTO AlertParameter VALUES (NEWID(), '{{ServiceName}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'ServiceName', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{DateTimeNow}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'DateExecuted', NULL, NULL, NULL, 0, 0, 0);
INSERT INTO AlertParameter VALUES (NEWID(), '{{TimerExecute}}', @IdAlertInformation, 'SyncronizationBot.Domain.Model.Alerts.LogExecute', 'Timer', NULL, NULL, NULL, 0, 0, 0);


/***** Messages For Price Alert *****/
SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = -1;
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
SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 0;
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

/***** Messages For Transactions *****/
SELECT @TelegramChannelId = ID FROM TelegramChannel WHERE ChannelName = 'CallSolana';
SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 1;
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

SELECT @TypeOperationId = ID FROM TypeOperation WHERE IdTypeOperation = 9;
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