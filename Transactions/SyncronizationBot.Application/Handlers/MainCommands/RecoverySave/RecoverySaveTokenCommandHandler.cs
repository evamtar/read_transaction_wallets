using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenOverview.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenOverview.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenSecurity.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Dexscreener.Token.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Response;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Service.CrossCutting.Birdeye;
using SyncronizationBot.Domain.Service.CrossCutting.Dexscreener;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Domain.Repository.UnitOfWork;



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
                                               IUnitOfWorkSqlServer unitOfWorkSqlServer,
                                               IDexScreenerTokenService dexScreenerTokenService)
        {
            _mediator = mediator;
            _tokensOverviewService = tokensOverviewService;
            _tokenSecurityService = tokenSecurityService;
            _tokenRepository = unitOfWorkSqlServer.TokenRepository;
            _tokenSecurityRepository = unitOfWorkSqlServer.TokenSecurityRepository;
            _dexScreenerTokenService = dexScreenerTokenService;
        }
        public async Task<RecoverySaveTokenCommandResponse> Handle(RecoverySaveTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await this._tokenRepository.FindFirstOrDefaultAsync(x => x.Hash == request.TokenHash);
            if(request.LazyLoad ?? false) 
            {
                if (token == null) 
                {
                    token = await _tokenRepository.AddAsync(new Token
                    {
                        Hash = request.TokenHash,
                        Symbol = LAZY_LOAD,
                        Name = LAZY_LOAD,
                        Supply = null,
                        Decimals = 1,
                        CreateDate = DateTime.Now,
                        LastUpdate = DateTime.Now
                    });
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
                    tokenSecurity = await this._tokenSecurityRepository.FindFirstOrDefaultAsync(x => x.TokenId == token.ID);
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
                            tokenSecurity = await this._tokenSecurityRepository.FindFirstOrDefaultAsync(x => x.TokenId == token.ID);
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
                    Decimals = token?.Decimals,
                    DateCreation = tokenSecurity?.CreationTimeDate,
                    FreezeAuthority = tokenSecurity?.FreezeAuthority,
                    MintAuthority = tokenSecurity?.MintAuthority,
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
            var token = await _tokenRepository.AddAsync(new Token
            {
                Hash = tokenResponse?.Data?.Address,
                Symbol = tokenResponse?.Data?.Symbol,
                Name = tokenResponse?.Data?.Name,
                Supply = tokenResponse?.Data?.Supply,
                Decimals = (int?)tokenResponse?.Data?.Decimals,
                CreateDate = DateTime.Now,
                LastUpdate = DateTime.Now
            });
            var tokenSecurity = await AddTokenSecurityAsync(request, token, false);
            return (token, tokenSecurity);
        }

        private async Task<(Token Token, TokenSecurity TokenSecurity)> AddTokenContingencyAsync(RecoverySaveTokenCommand request) 
        {
            var tokenSymbol = await _mediator.Send(new RecoveryPriceCommand { Ids = new List<string> { request!.TokenHash! } });
            if (!tokenSymbol?.Data?.ContainsKey(request!.TokenHash!) ?? false)
                return (null!, null!);
            var tokenResult = await this._dexScreenerTokenService.ExecuteRecoveryTokenAsync(new TokenRequest { TokenHash = request!.TokenHash! });
            var token = await _tokenRepository.AddAsync(new Token
            {
                Hash = request.TokenHash,
                Symbol = tokenSymbol?.Data?[request!.TokenHash!]?.VsTokenSymbol ?? tokenResult?.Pairs?.FirstOrDefault()?.BaseToken?.Symbol,
                Name = tokenSymbol?.Data?[request!.TokenHash!]?.VsTokenSymbol ?? tokenResult?.Pairs?.FirstOrDefault()?.BaseToken?.Name,
                Supply = null,
                Decimals = 1,
                CreateDate = DateTime.Now,
                LastUpdate = DateTime.Now
            });
            var tokenSecurity = await AddTokenSecurityAsync(request, token, true);
            return (token, tokenSecurity);
        }

        private async Task<TokenSecurity> AddTokenSecurityAsync(RecoverySaveTokenCommand request, Token token, bool forceContingency = false) 
        {
            var tokenSecurity = (TokenSecurity?)null;
            if (forceContingency)
            {
                tokenSecurity = await _tokenSecurityRepository.AddAsync(new TokenSecurity
                {
                    TokenId = token.ID,
                    CreationTime = null,
                    FreezeAuthority = null,
                    MintAuthority = null,
                });
            }
            else 
            {
                var tokenSecurityResponse = await _tokenSecurityService.ExecuteRecoveryTokenCreationAsync(new TokenSecurityRequest { TokenHash = request.TokenHash! });
                if (tokenSecurityResponse.Data != null || (tokenSecurityResponse.Data?.CreationTime != null || tokenSecurityResponse.Data?.TotalSupply != null))
                {
                    tokenSecurity = await _tokenSecurityRepository.AddAsync(new TokenSecurity
                    {
                        TokenId = token.ID,
                        CreationTime = tokenSecurityResponse?.Data?.CreationTime,
                        FreezeAuthority = tokenSecurityResponse?.Data?.FreezeAuthority,
                        MintAuthority = tokenSecurityResponse?.Data?.MintTx,
                    });
                }
                else
                {
                    tokenSecurity = await _tokenSecurityRepository.AddAsync(new TokenSecurity
                    {
                        TokenId = token.ID,
                        CreationTime = null,
                        FreezeAuthority = null,
                        MintAuthority = null,
                    });
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
            token!.Decimals = (int?)tokenResponse?.Data?.Decimals;
            token!.LastUpdate = DateTime.Now;
            this._tokenRepository.Update(token);
            var tokenSecurity = await this._tokenSecurityRepository.FindFirstOrDefaultAsync(x => x.TokenId == token.ID);
            return (token, tokenSecurity);
        }

        private async Task<(Token Token, TokenSecurity? TokenSecurity)> UpdateTokenContingencyASync(RecoverySaveTokenCommand request, Token token) 
        {
            var tokenSecurity = (TokenSecurity?)null;
            //Recupera preço da Jupiter
            var tokenSymbol = await _mediator.Send(new RecoveryPriceCommand { Ids = new List<string> { request!.TokenHash! } });
            if (tokenSymbol?.Data?.ContainsKey(request!.TokenHash!) ?? false)
            {
                token!.LastUpdate = DateTime.Now;
                this._tokenRepository.Update(token);
                tokenSecurity = await this._tokenSecurityRepository.FindFirstOrDefaultAsync(x => x.TokenId == token.ID);
            }
            else
            {
                //Recupera price da dexscreener
                var tokenResult = await this._dexScreenerTokenService.ExecuteRecoveryTokenAsync(new TokenRequest { TokenHash = request!.TokenHash! });
                if (tokenResult != null)
                {
                    token!.LastUpdate = DateTime.Now;
                    this._tokenRepository.Update(token);
                    tokenSecurity = await this._tokenSecurityRepository.FindFirstOrDefaultAsync(x => x.TokenId == token.ID);
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
