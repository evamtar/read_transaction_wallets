DECLARE @Signature VARCHAR(200) = '3PxGMzrvqLNzb26fq61yLLMaocRWZsm6QXHzzbFyKfn2j75dw3HwmmCtRr8K6kWrvyiW5R7Y2TZ7JbhCRCDFYDiP'
SELECT * FROM Transactions WHERE Signature = @Signature 
SELECT * FROM TransactionNotMapped WHERE Signature = @Signature