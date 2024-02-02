DECLARE @Signature VARCHAR(200) = '51uTzee2ArxvB84Ah8nvT5Sc7HxmEySvuwkFjjXTkBqKXT8Bc7vBpkHagb5ohar4fBz2T3x7AhMqKWgRQZvgzHHs'
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
-- History
	SELECT wb.IDWalletBalance,
		   wb.IdWallet,
		   w.Hash,
		   wb.IdToken,
		   t.Symbol,
		   wb.TokenHash,
		   wb.OldQuantity,
		   wb.NewQuantity,
		   wb.RequestQuantity,
		   wb.PercentageCalculated,
		   wb.Price,
		   wb.TotalValueUSD,
		   wb.Signature,
		   wb.FontType,
		   wb.CreateDate,
		   wb.LastUpdate
	  FROM WalletBalanceHistory wb
 LEFT JOIN Token t
        ON t.ID = wb.IdToken
 LEFT JOIN Wallet w
        ON w.ID = wb.IdWallet
 	 WHERE Signature = @Signature
-- Transactions Not Mapped
SELECT * FROM TransactionNotMapped WHERE Signature = @Signature
-- Transactions Old For Mapping
SELECT * FROM TransactionsOldForMapping WHERE Signature = @Signature
