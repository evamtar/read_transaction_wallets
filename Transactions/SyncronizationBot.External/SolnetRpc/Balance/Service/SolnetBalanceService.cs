using Polly;
using Polly.Retry;
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

        public SolnetBalanceResponse ExecuteRecoveryWalletBalanceAsync(SolnetBalanceRequest request)
        {
            var listBalances = this.GetBalanceResult(request);
            return new SolnetBalanceResponse { IsSuccess = true, DateLoadBalance = this.ExecuteDateTime, Result = listBalances };
        }

        private List<BalanceResponse> GetBalanceResult(SolnetBalanceRequest request)
        {
            var listBalances = new List<BalanceResponse>();
            var policy = RetryPolicy.Handle<TokenWalletException>()
                    .Or<Exception>()
                    .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) => { }
                );
            TokenWallet tokenWallet = null!;
            policy.Execute(() =>
            {
                tokenWallet = TokenWallet.Load(this._client, this._tokens, request?.WalletHash ?? string.Empty);
                this.ExecuteDateTime = DateTime.Now;
            });
            listBalances.Add(new BalanceResponse
            {
                Amount = tokenWallet.Sol,
                Token = new TokenResponse
                {
                    Hash = "So11111111111111111111111111111111111111112",
                    Decimals = 9,
                    Name = "Solana",
                    Symbol = "SOL"
                }
            });
            var balances = tokenWallet.Balances().ToList();
            balances.ForEach(balance =>
            {
                if ((balance.QuantityDecimal == 0 && request?.IgnoreAmountValueZero == false) || balance.QuantityDecimal > 0)
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
            });
            return listBalances;
        }
    }
}
