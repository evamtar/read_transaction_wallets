-- VALIDAR SE � APENAS LIXO
SELECT * FROM TransactionNotMapped WHERE StackTrace IS NULL
--LIMPAR DADOS INUTEIS
DELETE FROM PublishMessage WHERE ItWasPublished = 1
DELETE FROM TelegramMessage WHERE IsDeleted = 1
DELETE FROM TransactionNotMapped WHERE StackTrace IS NULL