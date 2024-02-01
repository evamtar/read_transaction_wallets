using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transfers.Request;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Model.Utils.Helpers;
using SyncronizationBot.Domain.Model.Utils.Transfer;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using SyncronizationBot.Service.Base;

namespace SyncronizationBot.Service
{
    public class AlertTesteService : BaseService
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IWalletBalanceHistoryRepository _walletBalanceHistory;
        private readonly ITransfersService _transfersService;
        private readonly IOptions<MappedTokensConfig> _mappedTokensConfig;

        public AlertTesteService(IMediator mediator,
                                         IRunTimeControllerRepository runTimeControllerRepository,
                                         IOptions<SyncronizationBotConfig> syncronizationBotConfig,
                                         ITransfersService transfersService,
                                         ITransactionsRepository transactionsRepository,
                                         IWalletBalanceHistoryRepository walletBalanceHistory,
                                         IOptions<MappedTokensConfig> mappedTokensConfig) : base(mediator, runTimeControllerRepository, ETypeService.Balance, syncronizationBotConfig)
        {
            this._transfersService = transfersService;
            this._transactionsRepository = transactionsRepository;
            this._walletBalanceHistory = walletBalanceHistory;
            this._mappedTokensConfig = mappedTokensConfig;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) 
        {
            base.LogMessage("Iniciando o serviço de teste de alertas");
            var walletId = Guid.Parse("CF4E11EC-081B-491A-BAF5-77962EA6ED28");
            var walletHash = "HwQ9NTLB1QthB3Tsq9eWCXogVHWZSLZrhySiknr2cKFX";
            var signature = "2X24BQ3xArDCfUwbLfsh5WDRXw32sNbaBps7BLfYKCjz9M894jBwXLMJud2NxLoMoNuUsTRafECYPChLc8DUFHmw";
            var balancePosition = new RecoveryAddUpdateBalanceItemCommandResponse
            {
                Quantity = (decimal?)-1032.196408,
                Price = (decimal?)1.4603359759503571,
                PercentModify = (decimal?)-100,
                IsActive = true
            };
            var transactionDetails = await _transfersService.ExecuteRecoveryTransfersAsync(new TransfersRequest { Signature = signature });
            var transferManager = await TransferManagerHelper.GetTransferManager(transactionDetails?.Result?.Data);
            var transferAccount = TransferManagerHelper.GetTransferAccount(walletHash, transactionDetails?.Result.Data[0].Source, transferManager);
            var transferInfo = TransferManagerHelper.GetTransferInfo(transferAccount, _mappedTokensConfig.Value);
            if (transferInfo.TransactionType != ETransactionType.INDEFINED)
            {
                var tokenSended = (RecoverySaveTokenCommandResponse?)null;
                var tokenSendedPool = (RecoverySaveTokenCommandResponse?)null;
                var tokenReceived = (RecoverySaveTokenCommandResponse?)null;
                var tokenReceivedPool = (RecoverySaveTokenCommandResponse?)null;
                var tokenSolForPrice = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = "So11111111111111111111111111111111111111112" });
                if (transferInfo?.TokenSended != null)
                    tokenSended = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferInfo?.TokenSended?.Token });
                if (transferInfo?.TokenSendedPool != null)
                    tokenSendedPool = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferInfo?.TokenSendedPool?.Token });
                if (transferInfo?.TokenReceived != null)
                    tokenReceived = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferInfo.TokenReceived?.Token });
                if (transferInfo?.TokenReceivedPool != null)
                    tokenReceivedPool = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferInfo.TokenReceivedPool?.Token });

                var transactionDB = await this._transactionsRepository.FindFirstOrDefault(x => x.Signature == signature);
                transactionDB.ClassWallet = "TESTE ALERT";
                transactionDB.WalletHash = "TESTE ALERT";
                await this._mediator.Send(new SendTransactionAlertsCommand
                {
                    Parameters = SendTransactionAlertsCommand.GetParameters(new object[]
                                                                    {
                                                                                        transactionDB,
                                                                                        transferInfo!,
                                                                                        new List<RecoverySaveTokenCommandResponse?> { tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool } ,
                                                                                        balancePosition
                                                                    }),
                    IdClassification = 2,
                    WalletId = walletId,
                    Transactions = transactionDB,
                    TokenSendedHash = tokenSended?.Hash,
                    TokenReceivedHash = tokenReceived?.Hash,
                    TokensMapped = this._mappedTokensConfig.Value.Tokens
                });
            }
        }
    }
}
