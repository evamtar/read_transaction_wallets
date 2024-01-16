using MediatR;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Accounts.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Tokens.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;


namespace SyncronizationBot.Application.Handlers
{
    public class RecoverySaveTokenCommandHandler : IRequestHandler<RecoverySaveTokenCommand, RecoverySaveTokenCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITokensService _tokensService;
        private readonly ITokenRepository _tokenRepository;
        private readonly IAccountsService _accountsService;
        public RecoverySaveTokenCommandHandler(IMediator mediator, 
                                               ITokensService tokensService,
                                               ITokenRepository tokenRepository,
                                               IAccountsService accountsService)
        {
            this._mediator = mediator;
            this._tokensService = tokensService;
            this._tokenRepository = tokenRepository;
            this._accountsService = accountsService;
        }
        public async Task<RecoverySaveTokenCommandResponse> Handle(RecoverySaveTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await this._tokenRepository.FindFirstOrDefault(x => x.Hash == request.TokenHash);
            if (token == null)
            {
                var tokenResponse = await this._tokensService.ExecuteRecoveryTokensAsync(new TokensRequest { TokenHash = request.TokenHash });
                if (tokenResponse.Decimals == null && tokenResponse.TokenType == null && tokenResponse.TokenList?.Name == null)
                {
                    var tokenAccount = await this._accountsService.ExecuteRecoveryAccountAsync(new AccountsRequest { AccountHashes = new List<string> { request!.TokenHash! } });
                    var tokenAdded = await this._tokenRepository.Add(new Token
                    {
                        Hash = request.TokenHash,
                        TokenAlias = tokenAccount?.Result?.FirstOrDefault()?.Onchain?.Owner,
                        Symbol = tokenAccount?.Result?.FirstOrDefault()?.Onchain?.Data?.Program,
                        TokenType = tokenAccount?.Result?.FirstOrDefault()?.Onchain?.Data?.Parsed?.Type,
                        FreezeAuthority = tokenAccount?.Result?.FirstOrDefault()?.Onchain?.Data?.Parsed?.Info?.FreezeAuthority,
                        MintAuthority = tokenAccount?.Result?.FirstOrDefault()?.Onchain?.Data?.Parsed?.Info?.MintAuthority,
                        IsMutable = null,
                        Decimals = tokenAccount?.Result?.FirstOrDefault()?.Onchain?.Data?.Parsed?.Info?.Decimals
                    });
                    return new RecoverySaveTokenCommandResponse
                    {
                        TokenId = tokenAdded.ID,
                        Decimals = tokenAdded.Decimals,
                        TokenAlias = tokenAdded.TokenAlias,
                        TokenHash = request.TokenHash,
                        FreezeAuthority = tokenAdded.FreezeAuthority,
                        MintAuthority = tokenAdded.MintAuthority,
                        IsMutable = tokenAdded.IsMutable,
                    };
                }
                else 
                {
                    var tokenAdded = await this._tokenRepository.Add(new Token
                    {
                        Hash = request.TokenHash,
                        TokenAlias = tokenResponse.TokenList?.Name ?? tokenResponse.TokenMetadata?.OnChainInfo?.Name,
                        Symbol = tokenResponse.TokenList?.Symbol ?? tokenResponse.TokenMetadata?.OnChainInfo?.Symbol,
                        TokenType = tokenResponse.TokenType,
                        FreezeAuthority = tokenResponse.FreezeAuthority,
                        MintAuthority = tokenResponse.MintAuthority,
                        IsMutable = tokenResponse.TokenMetadata?.OnChainInfo?.IsMutable,
                        Decimals = tokenResponse.Decimals
                    });
                    return new RecoverySaveTokenCommandResponse
                    {
                        TokenId = tokenAdded.ID,
                        Decimals = tokenAdded.Decimals,
                        TokenAlias = tokenAdded.TokenAlias,
                        TokenHash = request.TokenHash,
                        FreezeAuthority = tokenAdded.FreezeAuthority,
                        MintAuthority = tokenAdded.MintAuthority,
                        IsMutable = tokenAdded.IsMutable,
                    };
                }
            }
            else 
            {
                return new RecoverySaveTokenCommandResponse 
                {
                    TokenId = token.ID,
                    Decimals = token.Decimals,
                    TokenAlias = token.TokenAlias,
                    TokenHash = request.TokenHash,
                    FreezeAuthority = token.FreezeAuthority,
                    MintAuthority = token.MintAuthority,
                    IsMutable = token.IsMutable,
                };
            }
        }
    }
}
