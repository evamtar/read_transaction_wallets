using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Delete;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Commands.MainCommands.Triggers;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transfers.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Model.Utils.Helpers;
using SyncronizationBot.Domain.Model.Utils.Transfer;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using SyncronizationBot.Service.Base;


namespace SyncronizationBot.Service
{
    public class TestService : BaseService
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IWalletBalanceHistoryRepository _walletBalanceHistory;
        private readonly ITransfersService _transfersService;
        private readonly ITokenRepository _tokenRepository;
        private readonly ITokenAlphaRepository _tokenAlphaRepository;
        private readonly ITokenAlphaConfigurationRepository _tokenAlphaConfigurationRepository;
        private readonly ITokenAlphaWalletRepository _tokenAlphaWalletRepository;
        private readonly IOptions<MappedTokensConfig> _mappedTokensConfig;

        public TestService(IMediator mediator,
                                         IRunTimeControllerRepository runTimeControllerRepository,
                                         IOptions<SyncronizationBotConfig> syncronizationBotConfig,
                                         ITransfersService transfersService,
                                         ITransactionsRepository transactionsRepository,
                                         IWalletBalanceHistoryRepository walletBalanceHistory,
                                         ITokenAlphaRepository tokenAlphaRepository,
                                         ITokenAlphaConfigurationRepository tokenAlphaConfigurationRepository,
                                         ITokenAlphaWalletRepository tokenAlphaWalletRepository,
                                         IOptions<MappedTokensConfig> mappedTokensConfig) : base(mediator, runTimeControllerRepository, ETypeService.Balance, syncronizationBotConfig)
        {
            this._transfersService = transfersService;
            this._transactionsRepository = transactionsRepository;
            this._walletBalanceHistory = walletBalanceHistory;
            this._mappedTokensConfig = mappedTokensConfig;
            this._tokenAlphaRepository = tokenAlphaRepository;
            this._tokenAlphaConfigurationRepository = tokenAlphaConfigurationRepository;
            this._tokenAlphaWalletRepository = tokenAlphaWalletRepository;
            base.LogMessage("Iniciando o serviço de teste de alertas");
        }

        protected override async Task DoExecute(PeriodicTimer timer, CancellationToken stoppingToken)
        {
            //await this.TesteEnvioAlerta();
            //await this.TesteAlphaVerification();
            await this.TestAlertAlphaValidation();
        }

        private async Task TestAlertAlphaValidation() 
        {
            var tokenAlpha = await this._tokenAlphaRepository.FindFirstOrDefault(x => x.IsCalledInChannel == true);
            var hasNext = tokenAlpha != null;
            while (hasNext)
            {
                var tokenAlphaConfiguration = await this._tokenAlphaConfigurationRepository.FindFirstOrDefault(x => x.ID == tokenAlpha!.TokenAlphaConfigurationId);
                var tokensAlphaWalletsToAlert = await this._tokenAlphaWalletRepository.Get(x => x.TokenAlphaId == tokenAlpha!.ID);
                //Limpar mensagens de calls anteriores do mesmo token
                await this._mediator.Send(new DeleteOldCallsCommand
                {
                    EntityId = tokenAlpha!.ID
                });
                await this._mediator.Send(new SendAlertMessageCommand
                {
                    IdClassification = this.GetClassificationAlert(tokensAlphaWalletsToAlert),
                    EntityId = tokenAlpha?.ID,
                    Parameters = SendAlertMessageCommand.GetParameters(new object[]
                    {
                        tokenAlpha!,
                        tokenAlphaConfiguration!,
                        tokensAlphaWalletsToAlert
                    }),
                    TypeAlert = ETypeAlert.ALERT_TOKEN_ALPHA
                });
                tokenAlpha!.IsCalledInChannel = true;
                tokenAlpha!.LastUpdate = DateTime.Now;
                await this._tokenAlphaRepository.Edit(tokenAlpha);
                await this._tokenAlphaRepository.DetachedItem(tokenAlpha);
                tokenAlpha = await this._tokenAlphaRepository.FindFirstOrDefault(x => x.IsCalledInChannel == false);
                hasNext = tokenAlpha != null;
            }
        }
        private async Task TesteAlphaVerification() 
        {
            ///DEPRECATE-
            await this._mediator.Send(new VerifyAddTokenAlphaCommand
            {
                TokenId = Guid.Parse("A9AFF2C0-4D2E-429C-2C00-08DC289BD3E8"),
                WalletId = Guid.Parse("42A905A6-CE0D-49AD-B1E8-20567BDA41AE"),
                LaunchDate = DateTime.Now.AddHours(-10),
                MarketCap = (decimal?)2000001.00,
                ValueBuySol = (decimal?)(250.00000000 / 98.8967628),
                ValueBuyUSDC = (decimal?)250,
                ValueBuyUSDT = (decimal?)250,
                Signature = "5zietZumjqQiwj6TgR5UPc7pXYfgHuMZK5DxYa4DqZPDC7S8wVhJrLsFaDtMtb2y5KhLtaTW7aWmyyVp8rPHFuEG",
                Price = (decimal?)0.000000214747
            });
        }
        private async Task TesteEnvioAlerta() 
        {
            
            var walletId = Guid.Parse("684B5A8C-5078-41C8-9D49-31DEEDC938C7");
            var walletHash = "HwQ9NTLB1QthB3Tsq9eWCXogVHWZSLZrhySiknr2cKFX";
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
                //if (transactionDB?.TypeOperation == ETypeOperation.BUY || transactionDB?.TypeOperation == ETypeOperation.SWAP)
                //{
                //    await this._mediator.Send(new VerifyAddTokenAlphaCommand
                //    {
                //        WalletId = walletId,
                //        TokenId = transactionDB?.TokenDestinationId,
                //        ValueBuySol = this.CalculatedTotalSol(transferInfo?.TokenSended?.Token, transactionDB?.AmountValueSource, tokenSolForPrice.Price, tokenSended?.Price, transactionDB?.TypeOperation),
                //        ValueBuyUSDC = this.CalculatedTotalUSD(transferInfo?.TokenSended?.Token, transactionDB?.AmountValueSource, tokenSolForPrice.Price, tokenSended?.Price, transactionDB?.TypeOperation),
                //        ValueBuyUSDT = this.CalculatedTotalUSD(transferInfo?.TokenSended?.Token, transactionDB?.AmountValueSource, tokenSolForPrice.Price, tokenSended?.Price, transactionDB?.TypeOperation),
                //        Signature = transactionDB?.Signature,
                //        MarketCap = transactionDB?.MtkcapTokenDestination ?? (decimal?)644100.28,
                //        Price = transactionDB?.PriceTokenDestinationUSD ?? (decimal?)644100.28 / 1000000,
                //        LaunchDate = tokenReceived?.DateCreation ?? DateTime.Now,
                //    });
                //}
                await this._mediator.Send(new SendTransactionAlertsCommand
                {
                    Parameters = SendTransactionAlertsCommand.GetParameters(new object[]
                                                                    {
                                                                                        transactionDB!,
                                                                                        transferInfo!,
                                                                                        new List<RecoverySaveTokenCommandResponse?> { tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool } ,
                                                                                        balancePosition
                                                                    }),
                    IdClassification = 1,
                    WalletId = walletId,
                    Transactions = transactionDB,
                    TokenSendedHash = tokenSended?.Hash,
                    TokenReceivedHash = tokenReceived?.Hash,
                    TokensMapped = this._mappedTokensConfig.Value.Tokens
                });
            }
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

        private int GetClassificationAlert(List<TokenAlphaWallet> tokensAlphaWalletsToAlert)
        {
            if (tokensAlphaWalletsToAlert.Count() > 4)
                return 4;
            return tokensAlphaWalletsToAlert.Count();
        }
    }
}
