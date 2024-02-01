SELECT WB.ID,
			   WB.IDWalletBalance,
			   WB.IdWallet,
			   w.Hash AS PublicKey,
			   WB.IdToken,
			   WB.TokenHash,
			   CAST(WB.OldQuantity AS DECIMAL(38,2)) AS OldQuantity,
			   CAST(WB.NewQuantity AS DECIMAL(38,2)) AS NewQuantity,
			   CAST(WB.RequestQuantity AS DECIMAL(38,2)) AS RequestQuantity,
			   CAST(WB.PercentageCalculated AS DECIMAL(38,2)) AS PercentageCalculated,
			   CAST(WB.Price AS DECIMAL(38,2)) AS Price,
			   CAST(WB.TotalValueUSD AS DECIMAL(38,2)) AS TotalValueUSD,
			   WB.Signature,
			   CASE 
					WHEN WB.FontType = 1 THEN
					  'Byrdeye'
					ELSE
					  'SolanaFM'
					END AS FontBalance,
			   WB.CreateDate,
			   WB.LastUpdate
		FROM WalletBalanceHistory WB
  INNER JOIN Wallet w
          ON w.ID = WB.IdWallet

SELECT DISTINCT
			   w.Hash AS PublicKey
		FROM WalletBalanceHistory WB
  INNER JOIN Wallet w
          ON w.ID = WB.IdWallet