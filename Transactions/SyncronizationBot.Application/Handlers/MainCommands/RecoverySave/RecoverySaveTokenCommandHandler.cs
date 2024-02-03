﻿using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenOverview.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenSecurity.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Dexscreener.Token.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Response;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Birdeye;
using SyncronizationBot.Domain.Service.CrossCutting.Dexscreener;



namespace SyncronizationBot.Application.Handlers.MainCommands.RecoverySave
{
    public class RecoverySaveTokenCommandHandler : IRequestHandler<RecoverySaveTokenCommand, RecoverySaveTokenCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITokenOverviewService _tokensOverviewService;
        private readonly ITokenSecurityService _tokenSecurityService;
        private readonly ITokenRepository _tokenRepository;
        private readonly ITokenSecurityRepository _tokenSecurityRepository;
        private readonly IDexScreenerTokenService _dexScreenerTokenService;
        public RecoverySaveTokenCommandHandler(IMediator mediator,
                                               ITokenOverviewService tokensOverviewService,
                                               ITokenSecurityService tokenSecurityService,
                                               ITokenRepository tokenRepository,
                                               ITokenSecurityRepository tokenSecurityRepository,
                                               IDexScreenerTokenService dexScreenerTokenService)
        {
            _mediator = mediator;
            _tokensOverviewService = tokensOverviewService;
            _tokenSecurityService = tokenSecurityService;
            _tokenRepository = tokenRepository;
            _tokenSecurityRepository = tokenSecurityRepository;
            _dexScreenerTokenService = dexScreenerTokenService;
        }
        public async Task<RecoverySaveTokenCommandResponse> Handle(RecoverySaveTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await _tokenRepository.FindFirstOrDefault(x => x.Hash == request.TokenHash);
            var price = await _mediator.Send(new RecoveryPriceCommand { Ids = new List<string> { request.TokenHash! } });
            if (token == null || token.LastUpdate != null && token.LastUpdate.Value.AddDays(7) <= DateTime.Now)
            {
                var tokenResponse = await _tokensOverviewService.ExecuteRecoveryTokenOverviewAsync(new TokenOverviewRequest { TokenHash = request.TokenHash });
                if (tokenResponse.Data == null || tokenResponse.Data.Decimals == null)
                {
                    var tokenSymbol = await _mediator.Send(new RecoveryPriceCommand { Ids = new List<string> { request!.TokenHash! } });
                    if (!tokenSymbol?.Data?.ContainsKey(request!.TokenHash!) ?? false)
                        return null!;
                    var tokenResult = await _dexScreenerTokenService.ExecuteRecoveryTokenAsync(new TokenRequest { TokenHash = request!.TokenHash! });
                    var tokenAdded = await _tokenRepository.Add(new Token
                    {
                        Hash = request.TokenHash,
                        Symbol = tokenSymbol?.Data?[request!.TokenHash!]?.VsTokenSymbol ?? tokenResult?.Pairs?.FirstOrDefault()?.BaseToken?.Symbol,
                        Name = tokenSymbol?.Data?[request!.TokenHash!]?.VsTokenSymbol ?? tokenResult?.Pairs?.FirstOrDefault()?.BaseToken?.Name,
                        Supply = null,
                        MarketCap = tokenResult?.Pairs?.FirstOrDefault()?.Fdv,
                        Liquidity = (decimal?)(tokenResult?.Pairs?.FirstOrDefault()?.Liquidity?.Usd),
                        UniqueWallet24h = null,
                        UniqueWalletHistory24h = null,
                        Decimals = 1,
                        NumberMarkets = null,
                        CreateDate = DateTime.Now,
                        LastUpdate = DateTime.Now
                    });
                    var tokenSecurity = await _tokenSecurityRepository.Add(new TokenSecurity
                    {
                        TokenId = tokenAdded.ID,
                        CreatorAddress = null,
                        CreationTime = null,
                        Top10HolderBalance = null,
                        Top10HolderPercent = null,
                        Top10UserBalance = null,
                        Top10UserPercent = null,
                        IsTrueToken = null,
                        LockInfo = null,
                        Freezeable = null,
                        FreezeAuthority = null,
                        TransferFeeEnable = null,
                        TransferFeeData = null,
                        IsToken2022 = null,
                        NonTransferable = null,
                        MintAuthority = null,
                        IsMutable = null
                    });
                    return new RecoverySaveTokenCommandResponse
                    {
                        TokenId = tokenAdded.ID,
                        Symbol = tokenAdded.Symbol,
                        Name = tokenAdded.Name,
                        Supply = tokenAdded.Supply,
                        MarketCap = tokenAdded.MarketCap,
                        Price = GetPrice(request.TokenHash, price.Data),
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
                    var tokenAdded = await _tokenRepository.Add(new Token
                    {
                        Hash = tokenResponse?.Data?.Address,
                        Symbol = tokenResponse?.Data?.Symbol,
                        Name = tokenResponse?.Data?.Name,
                        Supply = tokenResponse?.Data?.Supply,
                        MarketCap = tokenResponse?.Data?.Mc,
                        Liquidity = tokenResponse?.Data?.Liquidity,
                        UniqueWallet24h = (int?)tokenResponse?.Data?.UniqueWallet24H,
                        UniqueWalletHistory24h = (int?)tokenResponse?.Data?.UniqueWalletHistory24H,
                        Decimals = (int?)tokenResponse?.Data?.Decimals,
                        NumberMarkets = (int?)tokenResponse?.Data?.NumberMarkets,
                        CreateDate = DateTime.Now,
                        LastUpdate = DateTime.Now
                    });
                    var tokenSecurityResponse = await _tokenSecurityService.ExecuteRecoveryTokenCreationAsync(new TokenSecurityRequest { TokenHash = request.TokenHash! });
                    var tokenSecurity = await _tokenSecurityRepository.Add(new TokenSecurity
                    {
                        TokenId = tokenAdded.ID,
                        CreatorAddress = tokenSecurityResponse?.Data?.CreatorAddress,
                        CreationTime = tokenSecurityResponse?.Data?.CreationTime,
                        Top10HolderBalance = tokenSecurityResponse?.Data?.Top10HolderBalance,
                        Top10HolderPercent = tokenSecurityResponse?.Data?.Top10HolderPercent,
                        Top10UserBalance = tokenSecurityResponse?.Data?.Top10UserBalance,
                        Top10UserPercent = tokenSecurityResponse?.Data?.Top10UserPercent,
                        IsTrueToken = tokenSecurityResponse?.Data?.IsTrueToken,
                        LockInfo = (string?)tokenSecurityResponse?.Data?.LockInfo,
                        Freezeable = (string?)tokenSecurityResponse?.Data?.Freezeable,
                        FreezeAuthority = (string?)tokenSecurityResponse?.Data?.FreezeAuthority,
                        TransferFeeEnable = (string?)tokenSecurityResponse?.Data?.TransferFeeEnable,
                        TransferFeeData = (string?)tokenSecurityResponse?.Data?.TransferFeeData,
                        IsToken2022 = tokenSecurityResponse?.Data?.IsToken2022,
                        NonTransferable = (string?)tokenSecurityResponse?.Data?.NonTransferable,
                        MintAuthority = tokenSecurityResponse?.Data?.MintTx,
                        IsMutable = tokenSecurityResponse?.Data?.MutableMetadata
                    });
                    return new RecoverySaveTokenCommandResponse
                    {
                        TokenId = tokenAdded.ID,
                        Hash = tokenAdded?.Hash,
                        Symbol = tokenAdded?.Symbol,
                        Name = tokenAdded?.Name,
                        Supply = tokenAdded?.Supply,
                        MarketCap = tokenAdded?.MarketCap,
                        Price = GetPrice(request.TokenHash, price.Data),
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
                var tokenSecurity = await _tokenSecurityRepository.FindFirstOrDefault(x => x.TokenId == token.ID);
                return new RecoverySaveTokenCommandResponse
                {
                    TokenId = token?.ID,
                    Hash = token?.Hash,
                    Symbol = token?.Symbol,
                    Name = token?.Name,
                    Supply = token?.Supply,
                    MarketCap = token?.MarketCap,
                    Price = GetPrice(request.TokenHash, price.Data),
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

        private decimal? GetPrice(string? tokenHash, Dictionary<string, TokenData>? tokenDatadictionary)
        {
            if (tokenHash != null)
            {
                if (tokenDatadictionary?.ContainsKey(tokenHash) ?? false)
                {
                    return tokenDatadictionary[tokenHash].Price;
                }
            }
            return null;
        }
    }
}
