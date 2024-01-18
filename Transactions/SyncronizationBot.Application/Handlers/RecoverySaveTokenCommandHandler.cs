using MediatR;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenOverview.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenSecurity.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Accounts.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Birdeye;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;



namespace SyncronizationBot.Application.Handlers
{
    public class RecoverySaveTokenCommandHandler : IRequestHandler<RecoverySaveTokenCommand, RecoverySaveTokenCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITokenOverviewService _tokensOverviewService;
        private readonly ITokenSecurityService _tokenSecurityService;
        private readonly ITokenRepository _tokenRepository;
        private readonly ITokenSecurityRepository _tokenSecurityRepository;
        private readonly IAccountsService _accountsService;
        public RecoverySaveTokenCommandHandler(IMediator mediator,
                                               ITokenOverviewService tokensOverviewService,
                                               ITokenSecurityService tokenSecurityService,
                                               ITokenRepository tokenRepository,
                                               ITokenSecurityRepository tokenSecurityRepository,
                                               IAccountsService accountsService)
        {
            this._mediator = mediator;
            this._tokensOverviewService = tokensOverviewService;
            this._tokenSecurityService = tokenSecurityService;
            this._tokenRepository = tokenRepository;
            this._tokenSecurityRepository = tokenSecurityRepository;
            this._accountsService = accountsService;
        }
        public async Task<RecoverySaveTokenCommandResponse> Handle(RecoverySaveTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await this._tokenRepository.FindFirstOrDefault(x => x.Hash == request.TokenHash);
            if (token == null || (token.LastUpdate != null && token.LastUpdate.Value.AddDays(7) <= DateTime.Now))
            {
                var tokenResponse = await this._tokensOverviewService.ExecuteRecoveryTokenOverviewAsync(new TokenOverviewRequest { TokenHash = request.TokenHash });
                if (tokenResponse.Data == null || tokenResponse.Data.Decimals == null)
                {
                    var tokenAccount = await this._accountsService.ExecuteRecoveryAccountAsync(new AccountsRequest { AccountHashes = new List<string> { request!.TokenHash! } });
                    var tokenSymbol = await this._mediator.Send(new RecoveryPriceCommand { Ids = new List<string> { request!.TokenHash! } });
                    var tokenAdded = await this._tokenRepository.Add(new Token
                    {
                        Hash = request.TokenHash,
                        Symbol = tokenSymbol?.Data?[request!.TokenHash!]?.VsTokenSymbol ?? tokenAccount?.Result?.FirstOrDefault()?.Onchain?.Owner,
                        Name = tokenSymbol?.Data?[request!.TokenHash!]?.VsTokenSymbol ?? tokenAccount?.Result?.FirstOrDefault()?.Onchain?.Owner,
                        Supply = decimal.Parse(tokenAccount?.Result?.FirstOrDefault()?.Onchain?.Data?.Parsed?.Info?.Supply ?? (10 * tokenAccount?.Result?.FirstOrDefault()?.Onchain?.Data?.Parsed?.Info?.Decimals?? 1).ToString()),
                        MarketCap = decimal.Parse(tokenAccount?.Result?.FirstOrDefault()?.Onchain?.Data?.Parsed?.Info?.Supply ?? (10 * tokenAccount?.Result?.FirstOrDefault()?.Onchain?.Data?.Parsed?.Info?.Decimals ?? 1).ToString()) * (tokenSymbol?.Data?[request!.TokenHash!]?.Price ?? 0),
                        Liquidity = null,
                        UniqueWallet24h = null,
                        UniqueWalletHistory24h = null,
                        Decimals = tokenAccount?.Result?.FirstOrDefault()?.Onchain?.Data?.Parsed?.Info?.Decimals,
                        NumberMarkets = null,
                        CreateDate = DateTime.Now,
                        LastUpdate = DateTime.Now
                    });
                    var tokenSecurity = await this._tokenSecurityRepository.Add(new TokenSecurity 
                    { 
                        IdToken = tokenAdded.ID,
                        CreatorAddress = null,
                        CreationTime = null,
                        Top10HolderBalance = null,
                        Top10HolderPercent = null,
                        Top10UserBalance = null,
                        Top10UserPercent = null,
                        IsTrueToken = null,
                        LockInfo = null,
                        Freezeable = null,
                        FreezeAuthority = tokenAccount?.Result?.FirstOrDefault()?.Onchain?.Data?.Parsed?.Info?.FreezeAuthority,
                        TransferFeeEnable = null,
                        TransferFeeData = null,
                        IsToken2022 = null,
                        NonTransferable = null,
                        MintAuthority = tokenAccount?.Result?.FirstOrDefault()?.Onchain?.Data?.Parsed?.Info?.MintAuthority,
                        IsMutable = null
                    });
                    return new RecoverySaveTokenCommandResponse
                    {
                        TokenId = tokenAdded.ID,
                        Symbol = tokenAdded.Symbol,
                        Name = tokenAdded.Name,
                        Supply = tokenAdded.Supply,
                        MarketCap = tokenAdded.MarketCap,
                        Liquidity = tokenAdded.Liquidity,
                        UniqueWallet24h = tokenAdded.UniqueWallet24h,
                        UniqueWalletHistory24h = tokenAdded.UniqueWalletHistory24h,
                        Decimals = tokenAdded.Decimals,
                        NumberMarkets = tokenAdded.NumberMarkets,
                        DateCreation = tokenSecurity?.CreationTimeDate,
                        FreezeAuthority = tokenSecurity?.FreezeAuthority,
                        MintAuthority = tokenSecurity?.MintAuthority,
                        IsMutable = null,
                    };
                }
                else 
                {
                    var tokenAdded = await this._tokenRepository.Add(new Token
                    {
                        Hash = tokenResponse?.Data?.Address,
                        Symbol = tokenResponse?.Data?.Symbol,
                        Name = tokenResponse?.Data?.Name,
                        Supply = tokenResponse?.Data?.Supply,
                        MarketCap = tokenResponse?.Data?.Mc,
                        Liquidity = tokenResponse?.Data?.Liquidity,
                        UniqueWallet24h = ((int?)tokenResponse?.Data?.UniqueWallet24H),
                        UniqueWalletHistory24h = ((int?)tokenResponse?.Data?.UniqueWalletHistory24H),
                        Decimals = (int?)tokenResponse?.Data?.Decimals,
                        NumberMarkets = (int?)tokenResponse?.Data?.NumberMarkets,
                        CreateDate = DateTime.Now,
                        LastUpdate = DateTime.Now
                    });
                    var tokenSecurityResponse = await this._tokenSecurityService.ExecuteRecoveryTokenCreationAsync(new TokenSecurityRequest { TokenHash = request.TokenHash! });
                    var tokenSecurity = await this._tokenSecurityRepository.Add(new TokenSecurity
                    {
                        IdToken = tokenAdded.ID,
                        CreatorAddress = tokenSecurityResponse?.TokenData?.CreatorAddress,
                        CreationTime = tokenSecurityResponse?.TokenData?.CreationTime,
                        Top10HolderBalance = tokenSecurityResponse?.TokenData?.Top10HolderBalance,
                        Top10HolderPercent = tokenSecurityResponse?.TokenData?.Top10HolderPercent,
                        Top10UserBalance = tokenSecurityResponse?.TokenData?.Top10UserBalance,
                        Top10UserPercent = tokenSecurityResponse?.TokenData?.Top10UserPercent,
                        IsTrueToken = tokenSecurityResponse?.TokenData?.IsTrueToken,
                        LockInfo = (string?)tokenSecurityResponse?.TokenData?.LockInfo,
                        Freezeable = (string?)tokenSecurityResponse?.TokenData?.Freezeable,
                        FreezeAuthority = (string?)tokenSecurityResponse?.TokenData?.FreezeAuthority,
                        TransferFeeEnable = (string?)tokenSecurityResponse?.TokenData?.TransferFeeEnable,
                        TransferFeeData = (string?)tokenSecurityResponse?.TokenData?.TransferFeeData,
                        IsToken2022 = (bool?)tokenSecurityResponse?.TokenData?.IsToken2022,
                        NonTransferable = (string?)tokenSecurityResponse?.TokenData?.NonTransferable,
                        MintAuthority = (string?)tokenSecurityResponse?.TokenData?.MintTx,
                        IsMutable = tokenSecurityResponse?.TokenData?.MutableMetadata
                    });
                    return new RecoverySaveTokenCommandResponse
                    {
                        TokenId = tokenAdded.ID,
                        Hash = tokenAdded?.Hash,
                        Symbol = tokenAdded?.Symbol,
                        Name = tokenAdded?.Name,
                        Supply = tokenAdded?.Supply,
                        MarketCap = tokenAdded?.MarketCap,
                        Liquidity = tokenAdded?.Liquidity,
                        UniqueWallet24h = tokenAdded?.UniqueWallet24h,
                        UniqueWalletHistory24h = tokenAdded?.UniqueWalletHistory24h,
                        Decimals = tokenAdded?.Decimals,
                        NumberMarkets = tokenAdded?.NumberMarkets,
                        DateCreation = tokenSecurity?.CreationTimeDate,
                        FreezeAuthority = tokenSecurity?.FreezeAuthority,
                        MintAuthority = tokenSecurity?.MintAuthority,
                        IsMutable = tokenSecurity?.IsMutable,
                    };
                }
            }
            else 
            {
                var tokenSecurity = await this._tokenSecurityRepository.FindFirstOrDefault(x => x.IdToken == token.ID);
                return new RecoverySaveTokenCommandResponse 
                {
                    TokenId = token?.ID,
                    Hash = token?.Hash,
                    Symbol = token?.Symbol,
                    Name = token?.Name,
                    Supply = token?.Supply,
                    MarketCap = token?.MarketCap,
                    Liquidity = token?.Liquidity,
                    UniqueWallet24h = token?.UniqueWallet24h,
                    UniqueWalletHistory24h = token?.UniqueWalletHistory24h,
                    Decimals = token?.Decimals,
                    NumberMarkets = token?.NumberMarkets,
                    DateCreation = tokenSecurity?.CreationTimeDate,
                    FreezeAuthority = tokenSecurity?.FreezeAuthority,
                    MintAuthority = tokenSecurity?.MintAuthority,
                    IsMutable = tokenSecurity?.IsMutable,
                };
            }
        }
    }
}
