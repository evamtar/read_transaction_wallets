using MediatR;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Info.Command;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Info.Response;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Price.Command;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Price.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenOverview.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Tokens.Request;
using SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Token.Request;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Service.CrossCutting.Birdeye;
using SyncronizationBot.Domain.Service.CrossCutting.Dexscreener;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using SyncronizationBot.Domain.Service.CrossCutting.SolnetRpc.Token;

namespace SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Info.Handler
{
    public class ReadTokenInfoCommandHandler : IRequestHandler<ReadTokenInfoCommand, ReadTokenInfoCommandResponse>
    {
        private readonly ISolnetTokenService _solnetTokenService;
        private readonly ITokensService _tokensService;
        private readonly ITokenOverviewService _tokensOverviewService;
        public ReadTokenInfoCommandHandler(ISolnetTokenService solnetTokenService,
                                           ITokensService tokensService,
                                           ITokenOverviewService tokensOverviewService)
        {
            this._solnetTokenService = solnetTokenService;
            this._tokensService = tokensService;
            this._tokensOverviewService = tokensOverviewService;
        }
        
        public async Task<ReadTokenInfoCommandResponse> Handle(ReadTokenInfoCommand request, CancellationToken cancellationToken)
        {
            var response = await TryRecoveryTokenInfoSolnetRPC(request);
            if (response.IsSuccess) return response;
            response = await TryRecoveryTokenInfoFromSolanaFM(request);
            if (response.IsSuccess) return response;
            response = await TryRecoveryTokenInfoFromBirdeye(request);
            if (response.IsSuccess) return response;
            return new ReadTokenInfoCommandResponse { IsSuccess = false };
        }


        #region SolnetRPC

        private async Task<ReadTokenInfoCommandResponse> TryRecoveryTokenInfoSolnetRPC(ReadTokenInfoCommand request)
        {
            try
            {
                var response = await this._solnetTokenService.ExecuteRecoveryTokenInfoDetailAsync(new TokenRPCRequest { TokenHash = request.TokenHash });
                if(response.IsSuccess) 
                {
                    return new ReadTokenInfoCommandResponse
                    {
                        Supply = response.Supply,
                        Decimals = response.Decimals,
                        MintAuthority = response.MintAuthority,
                        FreezeAuthority = response.FreezeAuthority,
                        FontType = EFontType.SOLANA_RPC,
                        IsSuccess = true
                    };
                }
            }
            finally{ }
            return new ReadTokenInfoCommandResponse { IsSuccess = false };
        }

        #endregion

        #region Solana FM

        private async Task<ReadTokenInfoCommandResponse> TryRecoveryTokenInfoFromSolanaFM(ReadTokenInfoCommand request) 
        {
            try
            {
                var response = await this._tokensService.ExecuteRecoveryTokensAsync(new TokensRequest 
                { 
                    TokenHash = request.TokenHash
                });
                if (response != null && response.Decimals > -1)
                {
                    return new ReadTokenInfoCommandResponse
                    {
                        Name = response?.TokenMetadata?.OnChainInfo?.Name,
                        Symbol = response?.TokenMetadata?.OnChainInfo?.Symbol,
                        Decimals = response?.Decimals,
                        MintAuthority = response?.MintAuthority,
                        FreezeAuthority = response?.FreezeAuthority,
                        FontType = EFontType.SOLANA_FM,
                        IsSuccess = true
                    };
                }
            }
            finally { }
            return new ReadTokenInfoCommandResponse { IsSuccess = false };
        }

        #endregion

        #region BirdEye

        private async Task<ReadTokenInfoCommandResponse> TryRecoveryTokenInfoFromBirdeye(ReadTokenInfoCommand request)
        {
            var response = await _tokensOverviewService.ExecuteRecoveryTokenOverviewAsync(new TokenOverviewRequest { TokenHash = request.TokenHash });
            if (response?.Data != null)
            {

                return new ReadTokenInfoCommandResponse
                {
                    Name = response.Data.Name,
                    Symbol = response.Data.Symbol,
                    Supply = response.Data.Supply,
                    Decimals = response.Data.Decimals,
                    IsSuccess = true,
                    FontType = EFontType.BIRDEYE
                };
            }
            return new ReadTokenInfoCommandResponse { IsSuccess = false };
        }

        #endregion
    }
}
