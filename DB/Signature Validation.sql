DECLARE @Signature VARCHAR(200) = '3hKSpvPZTTUjhp3Det5F6t32ZBfY2U5HK2U8CCzv1onAt7BWRhJGwXBTXcHvLwT4UTyJPy4hLtgefDQVikdwCCad'
SELECT * FROM Transactions WHERE Signature = @Signature 
SELECT * FROM TransactionNotMapped WHERE Signature = @Signature
SELECT * FROM WalletBalanceHistory WHERE Signature = @Signature 