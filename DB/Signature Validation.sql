DECLARE @Signature VARCHAR(200) = '4WWDy6FjBcDiAf93dcY3ZVgbRmqJ35NPM4sbNDXMRmaanrtoHpAjYYRbtfRX38kCAbHnjpyLodcoZc1QuLBaPwqE'
	SELECT tsource.Symbol TokenSource, 
		   tsourcepool.Symbol TokenSourcePool, 
		   tdestination.Symbol TokenDestination, 
		   tdestinationpool.Symbol TokenDestinationPool, 
		   t.*  
	  FROM Transactions t
 LEFT JOIN Token tsource
        ON tsource.ID = t.IdTokenSource
 LEFT JOIN Token tsourcepool
        ON tsourcepool.ID = t.IdTokenSourcePool
 LEFT JOIN Token tdestination
        ON tdestination.ID = t.IdTokenDestination
 LEFT JOIN Token tdestinationpool
        ON tdestinationpool.ID = t.IdTokenDestinationPool
	 WHERE t.[Signature] = @Signature 
SELECT * FROM TransactionNotMapped WHERE Signature = @Signature
SELECT * FROM WalletBalanceHistory WHERE Signature = @Signature
SELECT * FROM TransactionNotMapped WHERE IdWallet = 'C23CAE88-5EE1-48FD-8C4D-08DC1EC6C826'
SELECT * FROM WalletBalanceHistory WHERE IdWallet = '281EFDEA-9EFE-41F3-B190-1A3925653435'	AND IdToken = 'C23CAE88-5EE1-48FD-8C4D-08DC1EC6C826'

