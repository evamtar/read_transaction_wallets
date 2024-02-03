using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Commands.MainCommands.Triggers;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.Triggers;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transfers.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Model.Utils.Helpers;
using SyncronizationBot.Domain.Model.Utils.Transfer;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using SyncronizationBot.Service.Base;
using System.Diagnostics;

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
            //await this.TesteVerificacaoAlpha();
            await this.TesteEnvioAlerta();
        }

        private async Task TesteEnvioAlerta() 
        {
            base.LogMessage("Iniciando o serviço de teste de alertas");
            var walletId = Guid.Parse("1C7D55B4-6289-4106-A684-AC5853531129");
            var walletHash = "ufQ6EY7bzKdWNpC8hZH86WEzX6zkgqYd2eGaKbDyTU3";
            /*
             * "5zietZumjqQiwj6TgR5UPc7pXYfgHuMZK5DxYa4DqZPDC7S8wVhJrLsFaDtMtb2y5KhLtaTW7aWmyyVp8rPHFuEG"; CALL 1
             * "66Nvub4hvrDp7Q6X5vFsTkFQQMHoqrybZS7BkJyPHyF7pyVbzWPcxjpGg9pyfG2m8AFwW7JSjKFSrzoWEni9qiT1"; CALL 2
             */
            var signature = "66Nvub4hvrDp7Q6X5vFsTkFQQMHoqrybZS7BkJyPHyF7pyVbzWPcxjpGg9pyfG2m8AFwW7JSjKFSrzoWEni9qiT1";
            var balancePosition = new RecoveryAddUpdateBalanceItemCommandResponse
            {
                Quantity = (decimal?)-1032.196408,
                Price = (decimal?)1.4603359759503571,
                PercentModify = (decimal?)-100,
                IsActive = true
            };
            var transactionDetails = await _transfersService.ExecuteRecoveryTransfersAsync(new TransfersRequest { Signature = signature });
            var transferManager = await TransferManagerHelper.GetTransferManager(transactionDetails?.Result?.Data);
            var transferAccount = TransferManagerHelper.GetTransferAccount(walletHash, transactionDetails?.Result?.Data?[0].Source, transferManager);
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
                transactionDB!.ClassWallet = "TESTE ALERT";
                transactionDB!.WalletHash = "TESTE ALERT";
                if (transactionDB?.TypeOperation == ETypeOperation.BUY || transactionDB?.TypeOperation == ETypeOperation.SWAP)
                {
                    await this._mediator.Send(new VerifyAddTokenAlphaCommand
                    {
                        WalletId = walletId,
                        TokenId = transactionDB?.TokenDestinationId,
                        ValueBuySol = this.CalculatedTotalSol(transferInfo?.TokenSended?.Token, transactionDB?.AmountValueSource, tokenSolForPrice.Price, tokenSended?.Price, transactionDB?.TypeOperation),
                        ValueBuyUSDC = this.CalculatedTotalUSD(transferInfo?.TokenSended?.Token, transactionDB?.AmountValueSource, tokenSolForPrice.Price, tokenSended?.Price, transactionDB?.TypeOperation),
                        ValueBuyUSDT = this.CalculatedTotalUSD(transferInfo?.TokenSended?.Token, transactionDB?.AmountValueSource, tokenSolForPrice.Price, tokenSended?.Price, transactionDB?.TypeOperation),
                        Signature = transactionDB?.Signature,
                        MarketCap = transactionDB?.MtkcapTokenDestination,
                        Price = transactionDB?.PriceTokenDestinationUSD,
                        LaunchDate = tokenReceived?.DateCreation ?? DateTime.Now,
                    });
                }
                await this._mediator.Send(new SendTransactionAlertsCommand
                {
                    Parameters = SendTransactionAlertsCommand.GetParameters(new object[]
                                                                    {
                                                                                        transactionDB!,
                                                                                        transferInfo!,
                                                                                        new List<RecoverySaveTokenCommandResponse?> { tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool } ,
                                                                                        balancePosition
                                                                    }),
                    IdClassification = 3,
                    WalletId = walletId,
                    Transactions = transactionDB,
                    TokenSendedHash = tokenSended?.Hash,
                    TokenReceivedHash = tokenReceived?.Hash,
                    TokensMapped = this._mappedTokensConfig.Value.Tokens
                });
            }
        }
        private async Task TesteVerificacaoAlpha()
        {
            await this._mediator.Send(new VerifyAddTokenAlphaCommand
            {
                TokenId = Guid.Parse("090B6F31-D20C-4484-1DA5-08DC242D3536"),
                WalletId = Guid.Parse("1C7D55B4-6289-4106-A684-AC5853531129"),
                LaunchDate = DateTime.Now,
                MarketCap = (decimal?)333877.000000000,
                ValueBuySol = (decimal?)(250.00000000 / 98.8967628),
                ValueBuyUSDC = (decimal?)250,
                ValueBuyUSDT = (decimal?)250,
                Signature = "5zietZumjqQiwj6TgR5UPc7pXYfgHuMZK5DxYa4DqZPDC7S8wVhJrLsFaDtMtb2y5KhLtaTW7aWmyyVp8rPHFuEG",
                Price = (decimal?)0.000332494
            });
        }

        private decimal? CalculatedTotalSol(string? tokenHash, decimal? amountSource, decimal? solPrice, decimal? tokenPrice, ETypeOperation? typeOperation)
        {
            switch (typeOperation)
            {
                case ETypeOperation.BUY:
                    if (tokenHash == "So11111111111111111111111111111111111111112")
                        return Math.Abs(amountSource ?? 0);
                    else
                        return Math.Abs(amountSource / solPrice?? 0);
                case ETypeOperation.SWAP:
                    return Math.Abs((amountSource * tokenPrice) / solPrice ?? 0);
                default:
                    break;
            }
            return null;
        }
        private decimal? CalculatedTotalUSD(string? tokenHash, decimal? amountSource, decimal? solPrice, decimal? tokenPrice, ETypeOperation? typeOperation)
        {
            switch (typeOperation)
            {
                case ETypeOperation.BUY:
                    if (tokenHash != "So11111111111111111111111111111111111111112")
                        return Math.Abs(amountSource ?? 0);
                    else
                        return Math.Abs(amountSource * solPrice ?? 0);
                case ETypeOperation.SWAP:
                    return Math.Abs(amountSource * tokenPrice ?? 0);
                default:
                    break;
            }
            return null;
        }
    }
}
