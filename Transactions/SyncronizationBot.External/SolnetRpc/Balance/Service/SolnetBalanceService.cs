using Solana.Unity.Extensions;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Core.Http;
using Solana.Unity.Rpc.Messages;
using SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Balance.Request;
using SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Balance.Response;
using SyncronizationBot.Domain.Service.CrossCutting.SolnetRpc.Balance;

namespace SyncronizationBot.Infra.CrossCutting.SolnetRpc.Balance.Service
{
    public class SolnetBalanceService : ISolnetBalanceService
    {
        private readonly IRpcClient _client;
        private readonly ITokenMintResolver _tokens;

        public SolnetBalanceService()
        {
            this._client = ClientFactory.GetClient(Cluster.MainNet);
            this._tokens = TokenMintResolver.Load();
        }

        public async Task<SolnetBalanceResponse> ExecuteRecoveryWalletBalanceAsync(SolnetBalanceRequest request)
        {
            var listBalances = new List<BalanceResponse>();
            
            var balance = await this._client.GetBalanceAsync(request?.WalletHash ?? string.Empty);
            var accountInfo = await this._client.GetAccountInfoAsync(request?.WalletHash ?? string.Empty);
            listBalances.Add(await this.GetBalanceResult(balance, accountInfo));

            TokenWallet tokenWallet = await TokenWallet.LoadAsync(this._client, this._tokens, request?.WalletHash ?? string.Empty);
            var balances = tokenWallet.Balances();
            foreach (var tokenBalance in balances)
                listBalances.Add(await this.GetBalanceResult(tokenBalance));

            return new SolnetBalanceResponse { IsSuccess = true, ExecutionDate = DateTime.Now, Result = listBalances };
        }

        private Task<BalanceResponse> GetBalanceResult(RequestResult<ResponseValue<ulong>> balance, RequestResult<ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>> accountInfo) 
        {
            //TODO
            //balance - {"jsonrpc":"2.0","result":{"context":{"apiVersion":"1.17.21","slot":247983600},"value":1082851562},"id":1}
            //accountInfo - {"jsonrpc":"2.0","result":{"context":{"apiVersion":"1.17.21","slot":247983635},"value":{"data":["","base64"],"executable":false,"lamports":1082851562,"owner":"11111111111111111111111111111111","rentEpoch":18446744073709551615,"space":0}},"id":2}
            return null!;
        }

        private Task<BalanceResponse> GetBalanceResult(TokenWalletBalance tokenBalance)
        {
            //TODO
            return null!;
        }
    }
}
