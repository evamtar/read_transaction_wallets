using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;


namespace SyncronizationBot.Application.Handlers.MainCommands.RecoverySave
{
    public class RecoverySaveNewsTokensCommandHandler : IRequestHandler<RecoverySaveNewsTokensCommand, RecoverySaveNewsTokensCommandResponse>
    {
        public RecoverySaveNewsTokensCommandHandler()
        {
            
        }

        public Task<RecoverySaveNewsTokensCommandResponse> Handle(RecoverySaveNewsTokensCommand request, CancellationToken cancellationToken)
        {
            //var rpcClient = ClientFactory.GetClient(Cluster.MainNet);
            /*
                //Tras todas transações (limit 1000)
                var transactions = await rpcClient.GetSignaturesForAddressAsync("HDMfkW4Ft1rqS7pUkGYsBNrq9JsYhCW9ze75eeqFmMtu");
                foreach (var transaction in transactions.Result)
                {
                    DateTimeTicks.Instance.ConvertTicksToDateTime((long)(transaction.BlockTime ?? 0));
                }


                
            */
            /*
             
            // balance é apenas para o valor que a conta possui em SOL
                //{"jsonrpc":"2.0","result":{"context":{"apiVersion":"1.17.21","slot":247983600},"value":1082851562},"id":1}
                var balance = await rpcClient.GetBalanceAsync("5NmQDaY3w3Gp8BfWXPkrCxTBPrP4zaMrTPPz96J44BG4");
                //{"jsonrpc":"2.0","result":{"context":{"apiVersion":"1.17.21","slot":247983635},"value":{"data":["","base64"],"executable":false,"lamports":1082851562,"owner":"11111111111111111111111111111111","rentEpoch":18446744073709551615,"space":0}},"id":2}
                var accountInfo = await rpcClient.GetAccountInfoAsync("5NmQDaY3w3Gp8BfWXPkrCxTBPrP4zaMrTPPz96J44BG4");

             //  TRAS TODOS OS TOKENS QUE A WALLET JÁ POSSUIU
                var tokens = TokenMintResolver.Load();//Solnet.EXTENSION -- TokenMintResolver
                TokenWallet tokenWallet = TokenWallet.Load(rpcClient, tokens, "5NmQDaY3w3Gp8BfWXPkrCxTBPrP4zaMrTPPz96J44BG4");
                var balances = tokenWallet.Balances();
            */

            //var tokens = TokenMintResolver.Load();//Solnet.EXTENSION -- TokenMintResolver
            //TokenWallet tokenWallet = await TokenWallet.LoadAsync(rpcClient, tokens, "5NmQDaY3w3Gp8BfWXPkrCxTBPrP4zaMrTPPz96J44BG4");
            //var balances = tokenWallet.Balances();


            ////var balance = await rpcClient.GetBalanceAsync("AYyYgh3i43s1QSpvG4vwhJ6s3gewfN7uteFwYrswgMGw");
            //var programAccountsConfig = await rpcClient.GetProgramAccountsAsync("AYyYgh3i43s1QSpvG4vwhJ6s3gewfN7uteFwYrswgMGw");
            //var tokenBalance = await rpcClient.GetTokenAccountBalanceAsync("AYyYgh3i43s1QSpvG4vwhJ6s3gewfN7uteFwYrswgMGw");
            ////{"jsonrpc":"2.0","result":{"context":{"apiVersion":"1.17.21","slot":247982761},"value":{"data":{"parsed":{"info":{"decimals":9,"freezeAuthority":null,"isInitialized":true,"mintAuthority":null,"supply":"999717986436501"},"type":"mint"},"program":"spl-token","space":82},"executable":false,"lamports":301461600,"owner":"TokenkegQfeZyiNwAJbNbGKPFXCWuBvf9Ss623VQ5DA","rentEpoch":18446744073709551615,"space":82}},"id":8}
            //var tokenInfo = await rpcClient.GetTokenAccountInfoAsync("AYyYgh3i43s1QSpvG4vwhJ6s3gewfN7uteFwYrswgMGw");
            ////{"jsonrpc":"2.0","result":{"context":{"apiVersion":"1.17.21","slot":247982859},"value":{"data":{"parsed":{"info":{"decimals":9,"freezeAuthority":null,"isInitialized":true,"mintAuthority":null,"supply":"999717986436501"},"type":"mint"},"program":"spl-token","space":82},"executable":false,"lamports":301461600,"owner":"TokenkegQfeZyiNwAJbNbGKPFXCWuBvf9Ss623VQ5DA","rentEpoch":18446744073709551615,"space":82}},"id":9}
            //var tokenMintInfo = await rpcClient.GetTokenMintInfoAsync("AYyYgh3i43s1QSpvG4vwhJ6s3gewfN7uteFwYrswgMGw");
            //Owner que volta do token info / tokenMintInfo
            //try
            //{
            //    //AAAAAKFjGeYjDtRZfDLN/fzYsPLZtBgesr8tadgJ5R/3prlIlQV1
            //    //AYyYgh3i43s1QSpvG4vwhJ6s3gewfN7uteFwYrswgMGw
            //    var delegateKey = new PublicKey("AYyYgh3i43s1QSpvG4vwhJ6s3gewfN7uteFwYrswgMGw");
            //    var delegateTokenAccounts = rpcClient.GetAccountInfoAsync(delegateKey).GetAwaiter().GetResult();
            //    var mintPublicKey = TokenProgram.SyncNative(delegateKey);
            //    var teste = new PublicKey(delegateTokenAccounts.Result.Value.Data[0]);
            //    byte[] zstdEncodedBytes = Encoding.UTF8.GetBytes(delegateTokenAccounts.Result.Value.Data[0]);

            //    // Descomprime os bytes usando Zstandard
            //    byte[] decompressedBytes = new ZstdNet.Decompressor().Unwrap(zstdEncodedBytes);

            //    // Converte os bytes descomprimidos para uma string
            //    string resultString = Encoding.UTF8.GetString(decompressedBytes);
                
            //    //foreach (var account in delegateTokenAccounts.Result)
            //    //{
            //    //    Console.WriteLine(account.Account.Data.Parsed.Info.DelegatedAmount == null ?
            //    //        $"Account: {account.PublicKey} - Mint: {account.Account.Data.Parsed.Info.Mint} - TokenBalance: {account.Account.Data.Parsed.Info.TokenAmount.UiAmountString}" :
            //    //        $"Account: {account.PublicKey} - Mint: {account.Account.Data.Parsed.Info.Mint} - TokenBalance: {account.Account.Data.Parsed.Info.TokenAmount.UiAmountString}" +
            //    //        $" - Delegate: {account.Account.Data.Parsed.Info.Delegate} - DelegatedBalance: {account.Account.Data.Parsed.Info.DelegatedAmount.UiAmountString}");
            //    //}
            //}
            //catch 
            //{ 
            //}
            
            return null;
        }
    }
}
