-- VALIDAR SE É APENAS LIXO
SELECT * FROM TransactionNotMapped WHERE StackTrace IS NULL
--LIMPAR DADOS INUTEIS
DELETE FROM PublishMessage WHERE EntityParentId IS NULL AND ItWasPublished = 1
DELETE FROM PublishMessage WHERE EntityParentId IS NOT NULL AND ItWasPublished = 1
DELETE FROM TelegramMessage WHERE IsDeleted = 1
DELETE FROM TransactionNotMapped WHERE StackTrace IS NULL
SELECT * FROM PublishMessage
--LIMPAR CARTEIRAS COM FALHA NO CARREGAMENTO COM DADOS NO BALANCE
	--DELETE FROM WalletBalanceHistory WHERE WalletId IN(SELECT ID FROM Wallet WHERE IsLoadBalance = 0)
	--DELETE FROM WalletBalance WHERE WalletId IN(SELECT ID FROM Wallet WHERE IsLoadBalance = 0)	
	--SELECT t.ID 
	--    INTO #tmpToken 
	--    FROM Token t
 --  LEFT JOIN WalletBalance wb
 --         ON wb.TokenId = t.ID
	--   WHERE t.Symbol = 'LAZY LOAD'
	--     AND wb.TokenId IS NULL
	--DELETE FROM Token WHERE Id IN(SELECT ID FROM #tmpToken)
	--DROP TABLE #tmpToken 
SELECT * FROM WalletBalanceHistory WHERE WalletId IN(SELECT ID FROM Wallet WHERE IsLoadBalance = 0)
SELECT * FROM WalletBalance WHERE WalletId IN(SELECT ID FROM Wallet WHERE IsLoadBalance = 0)	



      UPDATE RunTimeController SET IsRunning= 0 WHERE IdRuntime IN(7)
	 
	 SELECT * FROM Token WHERE Hash = 'JEFMBtu3rqAeJ21XDzqL9UHr7pswMvm8ogAeTkezpktM'
	 SELECT * FROM Wallet WHERE Id = 'E8ACE934-78D3-4006-A6DE-97DF392253E8'