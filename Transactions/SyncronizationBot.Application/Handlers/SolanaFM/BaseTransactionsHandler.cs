using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Response;


namespace SyncronizationBot.Application.Handlers.SolanaFM
{
    public class BaseTransactionsHandler
    {
        private readonly IOptions<SyncronizationBotConfig> _syncronizationBotConfig;
        public BaseTransactionsHandler(IOptions<SyncronizationBotConfig> syncronizationBotConfig)
        {
            this._syncronizationBotConfig = syncronizationBotConfig;
        }

        protected async Task SaveTransactionsOldForMapping(TransactionInfoResponse? transactions, Guid? walletId)
        {
            //var exists = await this._transactionsOldForMappingRepository.FindFirstOrDefaultAsync(x => x.Signature == transactions!.Signature);
            //if (exists == null)
            //{
            //    await this._transactionsOldForMappingRepository.AddAsync(new TransactionOldForMapping
            //    {
            //        Signature = transactions?.Signature,
            //        DateOfTransaction = transactions?.DateOfTransaction,
            //        CreateDate = DateTime.Now,
            //        WalletId = walletId,
            //        IsIntegrated = false,
            //    });
            //}
        }

        protected async Task SaveTransactionsOldForMapping(TransactionResponse? transactions, Guid? walletId)
        {
            //var exists = this._transactionsOldForMappingRepository.FindFirstOrDefaultAsync(x => x.Signature == transactions!.Signature);
            //if (exists == null)
            //{
            //    await this._transactionsOldForMappingRepository.AddAsync(new TransactionOldForMapping
            //    {
            //        Signature = transactions?.Signature,
            //        DateOfTransaction = transactions?.DateOfTransaction,
            //        CreateDate = DateTime.Now,
            //        WalletId = walletId,
            //        IsIntegrated = false,
            //    });
            //}
        }

        protected DateTime AdjustDateTimeToPtBR(DateTime? dateTime)
        {
            return dateTime?.AddHours(this._syncronizationBotConfig.Value.GTMHoursAdjust ?? 0) ?? DateTime.MinValue;
        }
    }
}
