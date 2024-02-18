using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Commands.SolanaFM;
using SyncronizationBot.Application.Handlers.Base;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Application.Response.SolanaFM;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.AccountInfo.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.AccountInfo.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.TokensAccountsByOwner.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.TokensAccountsByOwner.Response;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository.SQLServer;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;

namespace SyncronizationBot.Application.Handlers.SolanaFM
{
    public class RecoverySaveBalanceSFMCommandHandler : BaseWalletHandler, IRequestHandler<RecoverySaveBalanceSFMCommand, RecoverySaveBalanceSFMCommandResponse>
    {
        private readonly IWalletBalanceRepository _walletBalanceRepository;
        private readonly IWalletBalanceHistoryRepository _walletBalanceHistoryRepository;
        private readonly IAccountInfoService _accountInfoService;
        private readonly ITokensAccountsByOwnerService _tokensAccountsByOwnerService;
        public RecoverySaveBalanceSFMCommandHandler(IMediator mediator,
                                                        IWalletRepository walletRepository,
                                                        IOptions<SyncronizationBotConfig> config,
                                                        IWalletBalanceRepository walletBalanceRepository,
                                                        IWalletBalanceHistoryRepository walletBalanceHistoryRepository,
                                                        IAccountInfoService accountInfoService,
                                                        ITokensAccountsByOwnerService tokensAccountsByOwnerService) : base(mediator, walletRepository, EFontType.SOLANA_FM, config)
        {
            this._walletBalanceRepository = walletBalanceRepository;
            this._walletBalanceHistoryRepository = walletBalanceHistoryRepository;
            this._accountInfoService = accountInfoService;
            this._tokensAccountsByOwnerService = tokensAccountsByOwnerService; 
        }
        public async Task<RecoverySaveBalanceSFMCommandResponse> Handle(RecoverySaveBalanceSFMCommand request, CancellationToken cancellationToken)
        {
            // Recovery Total Amount in SOL
            var accountInfo = await this._accountInfoService.ExecuteRecoveryAccountInfoAsync(new AccountInfoRequest { WalletHash = request.WalletHash });
            await this.SaveBalance(request, accountInfo, DateTime.Now);
            // Recovery Total Amount in another tokens
            var tokensAccountsByOwners = await this._tokensAccountsByOwnerService.ExecuteRecoveryTokensAccountsByOwnerAsync(new TokensAccountsByOwnerRequest { WalletPublicKeyHash = request.WalletHash });
            var dateLoadBalance = DateTime.Now;
            if (tokensAccountsByOwners.Any())
            {
                foreach (var tokenAccountsByOwner in tokensAccountsByOwners)
                {
                    if (tokenAccountsByOwner?.Result?.Value?.Any() ?? false)
                    {
                        //Tras apenas os tokens com valores
                        var filteredTokenWithAmount = from resultValue in tokenAccountsByOwner.Result.Value
                                                      where resultValue.Account?.Data?.Parsed?.Info?.TokenAmount?.Amount > 0 select resultValue;
                        foreach (var tokenResultResponse in filteredTokenWithAmount)
                            await this.SaveBalance(request, tokenResultResponse, dateLoadBalance);
                    }
                }
            }
            return new RecoverySaveBalanceSFMCommandResponse { DateLoadBalance = (base.IsSaveBalance() ?? false)? dateLoadBalance : null };
        }

        private async Task SaveBalance(RecoverySaveBalanceSFMCommand request, AccountInfoResponse accountInfo, DateTime? dateLoadBalance)
        {
            var balance = (WalletBalance)null!;
            var token = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = "So11111111111111111111111111111111111111112", LazyLoad = true });
            if (accountInfo != null && accountInfo.Result?.Value?.Lamports > 0)
            {
                if (base.IsSaveBalance() ?? false)
                {
                    balance = await this._walletBalanceRepository.AddAsync(new WalletBalance
                    {
                        WalletId = request.WalletId,
                        TokenId = token?.TokenId,
                        TokenHash = "So11111111111111111111111111111111111111112",
                        Quantity = accountInfo.Result?.Value?.Lamports / this.GetDivisor(token?.Decimals),
                        Price = token?.MarketCap / token?.Supply,
                        TotalValueUSD = (accountInfo.Result?.Value?.Lamports / this.GetDivisor(token?.Decimals)) * (token?.MarketCap / token?.Supply),
                        IsActive = accountInfo.Result?.Value?.Lamports > 0,
                        LastUpdate = dateLoadBalance
                    });
                    await this._walletBalanceHistoryRepository.AddAsync(new WalletBalanceHistory
                    {
                        WalletBalanceId = balance?.ID,
                        WalletId = balance?.WalletId,
                        TokenId = balance?.TokenId,
                        TokenHash = balance?.TokenHash,
                        OldQuantity = (decimal?)0,
                        NewQuantity = balance?.Quantity,
                        RequestQuantity = balance?.Quantity,
                        PercentageCalculated = 100,
                        Price = balance?.Price,
                        TotalValueUSD = balance?.TotalValueUSD,
                        Signature = "CREATE BALANCE",
                        FontType = base._fontType,
                        CreateDate = DateTime.Now,
                        LastUpdate = balance?.LastUpdate
                    });
                }
                else
                {
                    await this._walletBalanceHistoryRepository.AddAsync(new WalletBalanceHistory
                    {
                        WalletBalanceId = null,
                        WalletId = request.WalletId,
                        TokenId = token?.TokenId,
                        TokenHash = token?.Hash,
                        OldQuantity = (decimal?)0,
                        NewQuantity = accountInfo.Result?.Value?.Lamports / this.GetDivisor(token?.Decimals),
                        RequestQuantity = accountInfo.Result?.Value?.Lamports / this.GetDivisor(token?.Decimals),
                        PercentageCalculated = 100,
                        Price = token?.MarketCap / token?.Supply,
                        TotalValueUSD = (accountInfo.Result?.Value?.Lamports / this.GetDivisor(token?.Decimals)) * (token?.MarketCap / token?.Supply),
                        Signature = "CREATE BALANCE",
                        FontType = base._fontType,
                        CreateDate = DateTime.Now,
                        LastUpdate = dateLoadBalance
                    });
                }
            }
        }

        private async Task SaveBalance(RecoverySaveBalanceSFMCommand request, TokenAccountResponse tokenAccount, DateTime? dateLoadBalance)
        {
            var balance = (WalletBalance)null!;
            var token = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = tokenAccount.Pubkey, LazyLoad = true });
            if (tokenAccount != null && tokenAccount.Account?.Data?.Parsed?.Info?.TokenAmount!= null)
            {
                if (base.IsSaveBalance() ?? false)
                {
                    balance = await this._walletBalanceRepository.AddAsync(new WalletBalance
                    {
                        WalletId = request.WalletId,
                        TokenId = token?.TokenId,
                        TokenHash = tokenAccount.Pubkey,
                        Quantity = tokenAccount.Account?.Data?.Parsed?.Info?.TokenAmount?.Amount / this.GetDivisor(tokenAccount.Account?.Data?.Parsed?.Info?.TokenAmount?.Decimals ?? token?.Decimals),
                        Price = token?.MarketCap / token?.Supply,
                        TotalValueUSD = (tokenAccount.Account?.Data?.Parsed?.Info?.TokenAmount?.Amount / this.GetDivisor(tokenAccount.Account?.Data?.Parsed?.Info?.TokenAmount?.Decimals ?? token?.Decimals)) * (token?.MarketCap / token?.Supply),
                        IsActive = tokenAccount.Account?.Data?.Parsed?.Info?.TokenAmount?.Amount > 0,
                        LastUpdate = dateLoadBalance
                    });
                    await this._walletBalanceHistoryRepository.AddAsync(new WalletBalanceHistory
                    {
                        WalletBalanceId = balance?.ID,
                        WalletId = balance?.WalletId,
                        TokenId = balance?.TokenId,
                        TokenHash = balance?.TokenHash,
                        OldQuantity = (decimal?)0,
                        NewQuantity = balance?.Quantity,
                        RequestQuantity = balance?.Quantity,
                        PercentageCalculated = 100,
                        Price = balance?.Price,
                        TotalValueUSD = balance?.TotalValueUSD,
                        Signature = "CREATE BALANCE",
                        FontType = base._fontType,
                        CreateDate = DateTime.Now,
                        LastUpdate = balance?.LastUpdate
                    });
                }
                else
                {
                    var quantity = tokenAccount.Account?.Data?.Parsed?.Info?.TokenAmount?.Amount / this.GetDivisor(tokenAccount.Account?.Data?.Parsed?.Info?.TokenAmount?.Decimals ?? token?.Decimals);
                    await this._walletBalanceHistoryRepository.AddAsync(new WalletBalanceHistory
                    {
                        WalletBalanceId = null,
                        WalletId = request.WalletId,
                        TokenId = token?.TokenId,
                        TokenHash = tokenAccount.Pubkey,
                        OldQuantity = (decimal?)0,
                        NewQuantity = quantity,
                        RequestQuantity = quantity,
                        PercentageCalculated = 100,
                        Price = token?.MarketCap / token?.Supply,
                        TotalValueUSD = quantity * (token?.MarketCap / token?.Supply),
                        Signature = "CREATE BALANCE",
                        FontType = base._fontType,
                        CreateDate = DateTime.Now,
                        LastUpdate = dateLoadBalance
                    });
                }
            }
        }
    }
}
