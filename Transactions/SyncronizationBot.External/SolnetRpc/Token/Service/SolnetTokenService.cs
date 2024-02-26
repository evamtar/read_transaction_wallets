using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Solnet.Rpc;
using Solnet.Rpc.Core.Http;
using Solnet.Rpc.Messages;
using Solnet.Rpc.Models;
using SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Token;
using SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Token.Request;
using SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Token.Response;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Service.CrossCutting.SolnetRpc.Token;

namespace SyncronizationBot.Infra.CrossCutting.SolnetRpc.Token.Service
{
    public class SolnetTokenService : ISolnetTokenService
    {
        private readonly IRpcClient _client;
        private readonly RetryPolicy _retryPolicy;

        public SolnetTokenService()
        {
            this._client = ClientFactory.GetClient(Cluster.MainNet);
            this._retryPolicy = RetryPolicy.Handle<Exception>().Or<Exception>().WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) => { });
        }

        public async Task<TokenRPCResponse> ExecuteRecoveryTokenInfoDetailAsync(TokenRPCRequest request)
        {
            var tokenInfo = (RequestResult<ResponseValue<TokenAccountInfo>>?)null!;
            await this._retryPolicy.Execute(async () => 
            {
                tokenInfo = await _client.GetTokenAccountInfoAsync(request.TokenHash);
            });
            if(tokenInfo != null) 
            {
                if (tokenInfo.Result?.Value?.Data?.Parsed?.Info?.Delegate == null)
                {
                    var parsedObject = JsonConvert.DeserializeObject<JsonRpcResponse>(tokenInfo.RawRpcResponse);
                    return new TokenRPCResponse
                    {
                        Decimals = parsedObject?.Result?.Value?.Data?.Parsed?.Info?.Decimals,
                        Supply = parsedObject?.Result?.Value?.Data?.Parsed?.Info?.SupplyDecimal,
                        MintAuthority = parsedObject?.Result?.Value?.Data?.Parsed?.Info?.MintAuthority,
                        FreezeAuthority = parsedObject?.Result?.Value?.Data?.Parsed?.Info?.FreezeAuthority,
                        FontType = EFontType.SOLANA_RPC,
                        IsSuccess = true
                    };
                }
                else 
                {
                    return new TokenRPCResponse
                    {
                        Decimals = tokenInfo?.Result?.Value.Data.Parsed.Info.DelegatedAmount.Decimals,
                        Supply = tokenInfo?.Result?.Value.Data.Parsed.Info.DelegatedAmount.AmountDecimal,
                        MintAuthority = tokenInfo?.Result?.Value.Data.Parsed.Info.Mint,
                        FontType = EFontType.SOLANA_RPC,
                        IsSuccess = true
                    };
                }
            }
            return new TokenRPCResponse();
        }




    }
}
