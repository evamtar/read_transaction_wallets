using Solnet.Extensions;
using Solnet.Rpc;
using SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Balance.Request;
using SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Balance.Response;
using SyncronizationBot.Domain.Service.CrossCutting.SolnetRpc.Balance;

namespace SyncronizationBot.Infra.CrossCutting.SolnetRpc.Balance.Service
{
    public class SolnetBalanceService : ISolnetBalanceService
    {
        private DateTime? ExecuteDateTime { get; set; }

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
            //GetSolValue
            listBalances.Add(await this.GetAmountBalanceInSol(request));
            //GetAnotherTokens
            listBalances.AddRange(await this.GetBalanceResult(request));
            return new SolnetBalanceResponse { IsSuccess = true, DateLoadBalance = this.ExecuteDateTime, Result = listBalances };
        }

        private async Task<BalanceResponse> GetAmountBalanceInSol(SolnetBalanceRequest request) 
        {
            var balance = await this._client.GetBalanceAsync(request?.WalletHash ?? string.Empty);
            return new BalanceResponse
            {
                Amount = balance.Result.Value,
                Token = new TokenResponse
                {
                    Hash = "So11111111111111111111111111111111111111112",
                    Decimals = 9,
                    Name = "Solana",
                    Symbol = "SOL"
                }
            };
        }

        private async Task<List<BalanceResponse>> GetBalanceResult(SolnetBalanceRequest request)
        {
            var listBalances = new List<BalanceResponse>();
            TokenWallet tokenWallet = TokenWallet.Load(this._client, this._tokens, request?.WalletHash ?? string.Empty);
            var tokens = tokenWallet.TokenAccounts();
            this.ExecuteDateTime = DateTime.Now;
            var balances = tokenWallet.Balances();
            foreach (var balance in balances)
            {
                if ((request?.IgnoreAmountValueZero ?? false) && balance.QuantityDecimal == 0)
                    continue;
                listBalances.Add(new BalanceResponse
                {
                    Amount = balance.QuantityDecimal,
                    Token = new TokenResponse
                    {
                        Hash = balance.TokenMint,
                        Symbol = balance.Symbol,
                        Name = balance.TokenName,
                        Decimals = balance.DecimalPlaces
                    }
                });
            }
            return listBalances;
        }
    }
}
