DECLARE @Signature VARCHAR(200) = '4JSw65Mz2PwM81hbQqeBwCXcWTSBb5Tf79Mmv4jR1Pn1DVFMzFrQBMVxPD9Kghdg1ojzKw5GLLbR4ooeVkHq5ctk'
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
SELECT * FROM TransactionNotMapped WHERE ERROR IS NOT NULL WHERE Signature = @Signature
SELECT * FROM WalletBalanceHistory WHERE Signature = @Signature
SELECT * FROM TransactionNotMapped WHERE IdWallet = 'C23CAE88-5EE1-48FD-8C4D-08DC1EC6C826'
SELECT * FROM WalletBalanceHistory WHERE IdToken = 'EB90EB08-9AC0-412F-AA3A-08DC1FA76147'
SELECT * FROM TOken WHERE Hash = 'F1TTG9Yttzrdr9KJkXPKzMunnV4xu4gSp6WFW7MSYVW'
SELECT * FROM Wallet
DELETE FROM TransactionNotMapped
https://solscan.io/tx/tMKPmZYjQUZifJJMDPAU9Pe39Qb5Nwqcz2jKXczirSW1jxSgMdisFaanXamUureScTQeDCviyE5UHuUmeLkKTD9
https://solscan.io/tx/tMKPmZYjQUZifJJMDPAU9Pe39Qb5Nwqcz2jKXczirSW1jxSgMdisFaanXamUureScTQeDCviyE5UHuUmeLkKTD9

SELECT COUNT(*) , StatusLoad FROM (
SELECT CASE WHEN IsLoadBalance = 1 THEN
            'Carregada'
	   ELSE
			'Não Carregada'
	   END StatusLoad
	   FROM Wallet)
AS T1
GROUP BY T1.StatusLoad

SELECT * FROM Wallet W 