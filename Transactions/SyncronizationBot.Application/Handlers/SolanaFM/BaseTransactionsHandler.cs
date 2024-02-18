using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Response;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServer;

namespace SyncronizationBot.Application.Handlers.SolanaFM
{
    public class BaseTransactionsHandler
    {
        private readonly ITransactionOldForMappingRepository _transactionsOldForMappingRepository;
        private readonly IOptions<SyncronizationBotConfig> _syncronizationBotConfig;
        public BaseTransactionsHandler(ITransactionOldForMappingRepository transactionsOldForMappingRepository,
                                       IOptions<SyncronizationBotConfig> syncronizationBotConfig)
        {
            this._transactionsOldForMappingRepository = transactionsOldForMappingRepository;
            this._syncronizationBotConfig = syncronizationBotConfig;
        }

        protected async Task SaveTransactionsOldForMapping(TransactionInfoResponse? transactions, Guid? walletId)
        {
            var exists = await this._transactionsOldForMappingRepository.FindFirstOrDefault(x => x.Signature == transactions!.Signature);
            if (exists == null)
            {
                await this._transactionsOldForMappingRepository.Add(new TransactionOldForMapping
                {
                    Signature = transactions?.Signature,
                    DateOfTransaction = transactions?.DateOfTransaction,
                    CreateDate = DateTime.Now,
                    WalletId = walletId,
                    IsIntegrated = false,
                });
            }
        }

        protected async Task SaveTransactionsOldForMapping(TransactionResponse? transactions, Guid? walletId)
        {
            var exists = this._transactionsOldForMappingRepository.FindFirstOrDefault(x => x.Signature == transactions!.Signature);
            if (exists == null)
            {
                await this._transactionsOldForMappingRepository.Add(new TransactionOldForMapping
                {
                    Signature = transactions?.Signature,
                    DateOfTransaction = transactions?.DateOfTransaction,
                    CreateDate = DateTime.Now,
                    WalletId = walletId,
                    IsIntegrated = false,
                });
            }
        }

        protected DateTime AdjustDateTimeToPtBR(DateTime? dateTime)
        {
            return dateTime?.AddHours(this._syncronizationBotConfig.Value.GTMHoursAdjust ?? 0) ?? DateTime.MinValue;
        }
    }
}
