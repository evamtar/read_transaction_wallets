-- VALIDAR SE É APENAS LIXO
SELECT * FROM TransactionNotMapped WHERE StackTrace IS NULL
--LIMPAR DADOS INUTEIS
DELETE FROM PublishMessage WHERE ItWasPublished = 1
DELETE FROM TelegramMessage WHERE IsDeleted = 1
DELETE FROM TransactionNotMapped WHERE StackTrace IS NULL

--LIMPAR CARTEIRAS COM FALHA NO CARREGAMENTO COM DADOS NO BALANCE
	--DELETE FROM WalletBalanceHistory WHERE WalletId IN(SELECT ID FROM Wallet WHERE IsLoadBalance = 0)
	--DELETE FROM WalletBalance WHERE WalletId IN(SELECT ID FROM Wallet WHERE IsLoadBalance = 0)	
SELECT * FROM WalletBalanceHistory WHERE WalletId IN(SELECT ID FROM Wallet WHERE IsLoadBalance = 0)
SELECT * FROM WalletBalance WHERE WalletId IN(SELECT ID FROM Wallet WHERE IsLoadBalance = 0)	


	 