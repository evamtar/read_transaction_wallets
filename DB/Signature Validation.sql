DECLARE @Signature VARCHAR(200) = '5ABMoD4VCBJJuxnSNVLgYd2FGrHpKwKaFpHcnsmUHL6yVXaurKjjTsGEmHE9oxkB3FRt1jdrdi51HFsbRpViVwQH'
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