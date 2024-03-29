﻿using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenOverview.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenOverview.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenSecurity.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Dexscreener.Token.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Response;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Birdeye;
using SyncronizationBot.Domain.Service.CrossCutting.Dexscreener;
using System.Xml.Linq;
using System;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using SyncronizationBot.Domain.Model.CrossCutting.Dexscreener.Token.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Tokens.Response;



namespace SyncronizationBot.Application.Handlers.MainCommands.RecoverySave
{
    public class RecoverySaveTokenCommandHandler : IRequestHandler<RecoverySaveTokenCommand, RecoverySaveTokenCommandResponse>
    {
        private const string LAZY_LOAD = "LAZY LOAD";
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
            var token = await this._tokenRepository.FindFirstOrDefault(x => x.Hash == request.TokenHash);
            if(request.LazyLoad ?? false) 
            {
                if (token == null) 
                {
                    token = await _tokenRepository.Add(new Token
                    {
                        Hash = request.TokenHash,
                        Symbol = LAZY_LOAD,
                        Name = LAZY_LOAD,
                        Supply = null,
                        MarketCap = null,
                        Liquidity = null,
                        UniqueWallet24h = null,
                        UniqueWalletHistory24h = null,
                        Decimals = 1,
                        NumberMarkets = null,
                        CreateDate = DateTime.Now,
                        LastUpdate = DateTime.Now
                    });
                    await _tokenRepository.DetachedItem(token);
                }
                return new RecoverySaveTokenCommandResponse
                {
                    TokenId = token?.ID,
                    Hash = token?.Hash,
                    Symbol = LAZY_LOAD,
                    Name = LAZY_LOAD,
                    Supply = null,
                    MarketCap = null,
                    Price = null,
                    Liquidity = null,
                    UniqueWallet24h = null,
                    UniqueWalletHistory24h = null,
                    Decimals = 1,
                    NumberMarkets = null,
                    DateCreation = null,
                    FreezeAuthority = null,
                    MintAuthority = null,
                    IsMutable = null,
                };
            }
            else
            {
                var price = await _mediator.Send(new RecoveryPriceCommand { Ids = new List<string> { request.TokenHash! } });
                var tokenSecurity = (TokenSecurity?)null;
                if (token == null)
                {
                    var response = await AddTokenAsync(request);
                    token = response.Token;
                    tokenSecurity = response.TokenSecurity;
                    if (token == null && tokenSecurity == null)
                        return null!;
                }
                else
                {
                    tokenSecurity = await this._tokenSecurityRepository.FindFirstOrDefault(x => x.TokenId == token.ID);
                    if (tokenSecurity == null && (token.Symbol == LAZY_LOAD && !(request.LazyLoad ?? false)))
                    {
                        var response = await UpdateTokenAsync(request, token!);
                        token = response.Token;
                        if (response.TokenSecurity == null)
                            tokenSecurity = await AddTokenSecurityAsync(request, token);
                        else
                            tokenSecurity = response.TokenSecurity;
                    }
                    else 
                    {
                        if (token?.LastUpdate > DateTime.Now.AddMinutes(-3))
                        {
                            //Se atualizou a menos de 3 minutos não bate na API
                            tokenSecurity = await this._tokenSecurityRepository.FindFirstOrDefault(x => x.TokenId == token.ID);
                            await this._tokenSecurityRepository.DetachedItem(tokenSecurity!);
                        }
                        else
                        {
                            //Get WITH Price update
                            var response = await UpdateTokenAsync(request, token!);
                            token = response.Token;
                            tokenSecurity = response.TokenSecurity;
                        }
                    }
                }
                return new RecoverySaveTokenCommandResponse
                {
                    TokenId = token?.ID,
                    Hash = token?.Hash,
                    Symbol = token?.Symbol,
                    Name = token?.Name,
                    Supply = token?.Supply,
                    MarketCap = GetMarketCap(request.TokenHash, price.Data, token?.Supply, token?.MarketCap),
                    Price = GetPrice(request.TokenHash, price.Data, token?.Supply, token?.MarketCap),
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

        #region Add Token

        private async Task<(Token Token, TokenSecurity? TokenSecurity)> AddTokenAsync(RecoverySaveTokenCommand request) 
        {
            //Add From birdeye's
            var tokenResponse = await _tokensOverviewService.ExecuteRecoveryTokenOverviewAsync(new TokenOverviewRequest { TokenHash = request.TokenHash });
            if (tokenResponse.Data != null || tokenResponse.Data?.Decimals != null)
                return await AddTokenFromBirdeyeExternalFontAsync(request, tokenResponse);
            else
            {
                //Contigencia
                return await AddTokenContingencyAsync(request);
            }
        }

        private async Task<(Token Token, TokenSecurity TokenSecurity)> AddTokenFromBirdeyeExternalFontAsync(RecoverySaveTokenCommand request, TokenOverviewResponse tokenResponse) 
        {
            var token = await _tokenRepository.Add(new Token
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
            await _tokenRepository.DetachedItem(token);
            var tokenSecurity = await AddTokenSecurityAsync(request, token, false);
            return (token, tokenSecurity);
        }

        private async Task<(Token Token, TokenSecurity TokenSecurity)> AddTokenContingencyAsync(RecoverySaveTokenCommand request) 
        {
            var tokenSymbol = await _mediator.Send(new RecoveryPriceCommand { Ids = new List<string> { request!.TokenHash! } });
            if (!tokenSymbol?.Data?.ContainsKey(request!.TokenHash!) ?? false)
                return (null!, null!);
            var tokenResult = await this._dexScreenerTokenService.ExecuteRecoveryTokenAsync(new TokenRequest { TokenHash = request!.TokenHash! });
            var token = await _tokenRepository.Add(new Token
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
            await _tokenRepository.DetachedItem(token);
            var tokenSecurity = await AddTokenSecurityAsync(request, token, true);
            return (token, tokenSecurity);
        }

        private async Task<TokenSecurity> AddTokenSecurityAsync(RecoverySaveTokenCommand request, Token token, bool forceContingency = false) 
        {
            var tokenSecurity = (TokenSecurity?)null;
            if (forceContingency)
            {
                tokenSecurity = await _tokenSecurityRepository.Add(new TokenSecurity
                {
                    TokenId = token.ID,
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
                await _tokenSecurityRepository.DetachedItem(tokenSecurity);
            }
            else 
            {
                var tokenSecurityResponse = await _tokenSecurityService.ExecuteRecoveryTokenCreationAsync(new TokenSecurityRequest { TokenHash = request.TokenHash! });
                if (tokenSecurityResponse.Data != null || (tokenSecurityResponse.Data?.CreationTime != null || tokenSecurityResponse.Data?.TotalSupply != null))
                {
                    tokenSecurity = await _tokenSecurityRepository.Add(new TokenSecurity
                    {
                        TokenId = token.ID,
                        CreatorAddress = tokenSecurityResponse?.Data?.CreatorAddress,
                        CreationTime = tokenSecurityResponse?.Data?.CreationTime,
                        Top10HolderBalance = tokenSecurityResponse?.Data?.Top10HolderBalance,
                        Top10HolderPercent = tokenSecurityResponse?.Data?.Top10HolderPercent,
                        Top10UserBalance = tokenSecurityResponse?.Data?.Top10UserBalance,
                        Top10UserPercent = tokenSecurityResponse?.Data?.Top10UserPercent,
                        IsTrueToken = tokenSecurityResponse?.Data?.IsTrueToken,
                        LockInfo = this.ConvertObjectToString(tokenSecurityResponse?.Data?.LockInfo),
                        Freezeable = tokenSecurityResponse?.Data?.Freezeable,
                        FreezeAuthority = tokenSecurityResponse?.Data?.FreezeAuthority,
                        TransferFeeEnable = this.ConvertObjectToString(tokenSecurityResponse?.Data?.TransferFeeEnable),
                        TransferFeeData = this.ConvertObjectToString(tokenSecurityResponse?.Data?.TransferFeeData),
                        IsToken2022 = tokenSecurityResponse?.Data?.IsToken2022,
                        NonTransferable = this.ConvertObjectToString(tokenSecurityResponse?.Data?.NonTransferable),
                        MintAuthority = tokenSecurityResponse?.Data?.MintTx,
                        IsMutable = tokenSecurityResponse?.Data?.MutableMetadata
                    });
                    await _tokenSecurityRepository.DetachedItem(tokenSecurity);
                }
                else
                {
                    tokenSecurity = await _tokenSecurityRepository.Add(new TokenSecurity
                    {
                        TokenId = token.ID,
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
                    await _tokenSecurityRepository.DetachedItem(tokenSecurity);
                }
            }
            return tokenSecurity;
        }

        #endregion

        #region Update Token

        private async Task<(Token Token, TokenSecurity? TokenSecurity)> UpdateTokenAsync(RecoverySaveTokenCommand request, Token token) 
        {
            var tokenResponse = await _tokensOverviewService.ExecuteRecoveryTokenOverviewAsync(new TokenOverviewRequest { TokenHash = request.TokenHash });
            if (tokenResponse != null)
                return await UpdateTokenFromBirdeyeExternalFontASync(token!, tokenResponse);
            else
                return await UpdateTokenContingencyASync(request, token!);
        }

        private async Task<(Token Token, TokenSecurity? TokenSecurity)> UpdateTokenFromBirdeyeExternalFontASync(Token token, TokenOverviewResponse tokenResponse) 
        {
            token!.Hash = tokenResponse?.Data?.Address;
            token!.Symbol = tokenResponse?.Data?.Symbol;
            token!.Name = tokenResponse?.Data?.Name;
            token!.Supply = tokenResponse?.Data?.Supply;
            token!.MarketCap = tokenResponse?.Data?.Mc;
            token!.Liquidity = tokenResponse?.Data?.Liquidity;
            token!.UniqueWallet24h = (int?)tokenResponse?.Data?.UniqueWallet24H;
            token!.UniqueWalletHistory24h = (int?)tokenResponse?.Data?.UniqueWalletHistory24H;
            token!.Decimals = (int?)tokenResponse?.Data?.Decimals;
            token!.NumberMarkets = (int?)tokenResponse?.Data?.NumberMarkets;
            token!.LastUpdate = DateTime.Now;
            await this._tokenRepository.Edit(token);
            await this._tokenRepository.DetachedItem(token);
            var tokenSecurity = await this._tokenSecurityRepository.FindFirstOrDefault(x => x.TokenId == token.ID);
            await this._tokenSecurityRepository.DetachedItem(tokenSecurity!);
            return (token, tokenSecurity);
        }

        private async Task<(Token Token, TokenSecurity? TokenSecurity)> UpdateTokenContingencyASync(RecoverySaveTokenCommand request, Token token) 
        {
            var tokenSecurity = (TokenSecurity?)null;
            //Recupera preço da Jupiter
            var tokenSymbol = await _mediator.Send(new RecoveryPriceCommand { Ids = new List<string> { request!.TokenHash! } });
            if (tokenSymbol?.Data?.ContainsKey(request!.TokenHash!) ?? false)
            {
                token!.MarketCap = tokenSymbol?.Data?[request!.TokenHash!].Price * token.Supply;
                token!.LastUpdate = DateTime.Now;
                await this._tokenRepository.Edit(token);
                await this._tokenRepository.DetachedItem(token);
                tokenSecurity = await this._tokenSecurityRepository.FindFirstOrDefault(x => x.TokenId == token.ID);
                await this._tokenSecurityRepository.DetachedItem(tokenSecurity!);
            }
            else
            {
                //Recupera price da dexscreener
                var tokenResult = await this._dexScreenerTokenService.ExecuteRecoveryTokenAsync(new TokenRequest { TokenHash = request!.TokenHash! });
                if (tokenResult != null)
                {
                    token!.MarketCap = tokenResult?.Pairs?.FirstOrDefault()?.Fdv;
                    token!.Liquidity = (decimal?)(tokenResult?.Pairs?.FirstOrDefault()?.Liquidity?.Usd);
                    token!.LastUpdate = DateTime.Now;
                    await this._tokenRepository.Edit(token);
                    await this._tokenRepository.DetachedItem(token);
                    tokenSecurity = await this._tokenSecurityRepository.FindFirstOrDefault(x => x.TokenId == token.ID);
                    await this._tokenSecurityRepository.DetachedItem(tokenSecurity!);
                }
            }
            return (token, tokenSecurity);
        }

        #endregion

        #region Commom Methods

        private decimal? GetMarketCap(string? tokenHash, Dictionary<string, TokenData>? tokenDatadictionary, decimal? supply, decimal? marketcap)
        {
            if (marketcap.HasValue) return marketcap;
            else 
            {
                var price = this.GetPrice(tokenHash, tokenDatadictionary, supply, marketcap);
                if(price.HasValue) 
                    return supply * price;
            }
            return null;
        }

        private decimal? GetPrice(string? tokenHash, Dictionary<string, TokenData>? tokenDatadictionary, decimal? supply, decimal? marketcap)
        {
            if (tokenHash != null)
            {
                if (tokenDatadictionary?.ContainsKey(tokenHash) ?? false)
                    return tokenDatadictionary[tokenHash].Price;
                else
                    return marketcap / supply;
            }
            return null;
        }

        private string? ConvertObjectToString(object? objectValue) 
        { 
            if(objectValue?.GetType() == typeof(JsonObject))
                return JsonConvert.SerializeObject(objectValue) ?? null;
            return objectValue?.ToString();

        }

        #endregion

    }
}
