using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.AddUpdate;
using SyncronizationBot.Application.Commands.MainCommands.Delete;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Commands.MainCommands.Triggers;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transfers.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Model.Utils.Helpers;
using SyncronizationBot.Domain.Model.Utils.Transfer;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using SyncronizationBot.Service.Base;
using System.Data.Common;
using System.Diagnostics;
using System.Transactions;


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
        private readonly IPublishMessageRepository _publishMessageRepository;
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
                                         IPublishMessageRepository publishMessageRepositor,
                                         IOptions<MappedTokensConfig> mappedTokensConfig) : base(mediator, runTimeControllerRepository, ETypeService.Balance, syncronizationBotConfig)
        {
            this._transfersService = transfersService;
            this._transactionsRepository = transactionsRepository;
            this._walletBalanceHistory = walletBalanceHistory;
            this._mappedTokensConfig = mappedTokensConfig;
            this._tokenAlphaRepository = tokenAlphaRepository;
            this._tokenAlphaConfigurationRepository = tokenAlphaConfigurationRepository;
            this._tokenAlphaWalletRepository = tokenAlphaWalletRepository;
            this._publishMessageRepository = publishMessageRepositor;
            base.LogMessage("Iniciando o serviço de teste de alertas");
        }

        protected override async Task DoExecute(PeriodicTimer timer, CancellationToken stoppingToken)
        {
            //await this.TestePublicMessage();
            await this.TesteAdicionarAlpha();
            //await this.RepublishTokenAlpha();
        }

        private async Task RepublishTokenAlpha() 
        {
            var tokensAlpha = await this._tokenAlphaRepository.GetAll();
            if(tokensAlpha != null) 
            {
                foreach (var tokenAlpha in tokensAlpha)
                {
                    var tokenAlphaConfiguration = await this._tokenAlphaConfigurationRepository.FindFirstOrDefault(x => x.ID == tokenAlpha.TokenAlphaConfigurationId);
                    await PublishMessage(tokenAlpha!, tokenAlphaConfiguration!);
                }
            }
            

        }
        private async Task PublishMessage(TokenAlpha tokenAlpha, TokenAlphaConfiguration tokenAlphaConfiguration)
        {
            var listTokenAlphaWalletsIds = new List<Guid?>();
            var tokenAlphaWallet = await this._tokenAlphaWalletRepository.FindFirstOrDefault(x => x.TokenAlphaId == tokenAlpha.ID);
            var hasNext = tokenAlphaWallet != null;
            var publishMessageAlpha = await this.SavePublishMessage(tokenAlpha, null);
            await this.SavePublishMessage(tokenAlphaConfiguration, publishMessageAlpha.ID);
            while (hasNext)
            {
                listTokenAlphaWalletsIds.Add(tokenAlphaWallet!.ID);
                await this.SavePublishMessage(tokenAlphaWallet, publishMessageAlpha.ID);
                tokenAlphaWallet = await this._tokenAlphaWalletRepository.FindFirstOrDefault(x => x.TokenAlphaId == tokenAlpha.ID && !listTokenAlphaWalletsIds.Contains(x.ID));
                hasNext = tokenAlphaWallet != null;
            }
        }
        private async Task<PublishMessage> SavePublishMessage<T>(T entity, Guid? parentId) where T : Entity
        {
            var publishMessage = await this._publishMessageRepository.Add(new PublishMessage
            {
                EntityId = entity.ID,
                Entity = typeof(T).ToString(),
                JsonValue = entity.JsonSerialize(),
                ItWasPublished = false,
                EntityParentId = parentId,
            });
            await this._publishMessageRepository.DetachedItem(publishMessage);
            return publishMessage;
        }

        private async Task TestePublicMessage()
        {
            await this._mediator.Send(new SendAlertTokenAlphaCommand
            { });
        }

        private async Task TesteAdicionarAlpha() 
        {
            var signature = "UjnNkQ3kNmR6t1Xk1TNvPVw7XdtdgiNbuxmcayUjrX6N1DUjRvxGsH568UtPGrEZcNWuGca2ymRcMZXyFfEJbaA";
            var walletHash = "3ivNHNjUhAgEHWCZsMPicX9QMKvgu9BDK1DdH57n5RfK";
            var classWallet = "Insiders";
            var tokenHash = "7obg932wg2A1oTZhrPzR4DSdhKbjXzKSfHTBo2Hu5EbP";
            var tokenName = "Catfish";
            var tokenSymbol = "LEH";
            var transactionDetails = await _transfersService.ExecuteRecoveryTransfersAsync(new TransfersRequest { Signature = signature });
            if (transactionDetails?.Result != null && transactionDetails.Result.Data?.Count > 0)
            {
                var transferManager = await TransferManagerHelper.GetTransferManager(transactionDetails?.Result?.Data);
                var transferAccount = TransferManagerHelper.GetTransferAccount(walletHash, transactionDetails?.Result.Data[0].Source, transferManager);
                var transferInfo = TransferManagerHelper.GetTransferInfo(transferAccount, _mappedTokensConfig.Value);
                var transactionDB = await _transactionsRepository.FindFirstOrDefault(x => x.Signature == signature);
                await this._transactionsRepository.DetachedItem(transactionDB!);
                transactionDB!.WalletHash = walletHash;
                transactionDB!.ClassWallet = classWallet;
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
                var balancePosition = new RecoveryAddUpdateBalanceItemCommandResponse
                {
                    PercentModify = 1000,
                    Price = (decimal)0.0001225,
                    Quantity = (decimal)10000000000,
                    IsActive = true
                };
                if (transactionDB?.TypeOperation == ETypeOperation.BUY || transactionDB?.TypeOperation == ETypeOperation.SWAP)
                {
                    await this._mediator.Send(new VerifyAddTokenAlphaCommand
                    {
                        WalletId = transactionDB?.WalletId,
                        WalletHash = transactionDB?.WalletHash,
                        ClassWalletDescription = transactionDB?.ClassWallet,
                        TokenId = transactionDB?.TokenDestinationId,
                        TokenHash = tokenHash,
                        TokenName = tokenName,
                        TokenSymbol = tokenSymbol,
                        ValueBuySol = this.CalculatedTotalSol(transferInfo?.TokenSended?.Token, transactionDB?.AmountValueSource, tokenSolForPrice.Price, tokenSended?.Price, transactionDB?.TypeOperation),
                        ValueBuyUSDC = this.CalculatedTotalUSD(transferInfo?.TokenSended?.Token, transactionDB?.AmountValueSource, tokenSolForPrice.Price, tokenSended?.Price, transactionDB?.TypeOperation),
                        ValueBuyUSDT = this.CalculatedTotalUSD(transferInfo?.TokenSended?.Token, transactionDB?.AmountValueSource, tokenSolForPrice.Price, tokenSended?.Price, transactionDB?.TypeOperation),
                        QuantityTokenReceived = transactionDB?.AmountValueDestination,
                        Signature = transactionDB?.Signature,
                        MarketCap = transactionDB?.MtkcapTokenDestination,
                        Price = tokenReceived?.Price,
                        LaunchDate = tokenReceived?.DateCreation ?? DateTime.Now,
                    });
                }
            }
            
        }

        private decimal? CalculatedTotalSol(string? tokenHash, decimal? amountSource, decimal? solPrice, decimal? tokenPrice, ETypeOperation? typeOperation)
        {
            switch (typeOperation)
            {
                case ETypeOperation.BUY:
                case ETypeOperation.SELL:
                    if (tokenHash == "So11111111111111111111111111111111111111112")
                        return Math.Abs(amountSource ?? 0);
                    else
                        return Math.Abs(amountSource / solPrice ?? 0);
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
                case ETypeOperation.SELL:
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
