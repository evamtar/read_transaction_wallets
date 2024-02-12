using MediatR;
using Solnet.Programs;
using Solnet.Rpc;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;

namespace SyncronizationBot.Application.Handlers.MainCommands.RecoverySave
{
    public class RecoverySaveNewsTokensCommandHandler : IRequestHandler<RecoverySaveNewsTokensCommand, RecoverySaveNewsTokensCommandResponse>
    {
        public RecoverySaveNewsTokensCommandHandler()
        {
            
        }

        public async Task<RecoverySaveNewsTokensCommandResponse> Handle(RecoverySaveNewsTokensCommand request, CancellationToken cancellationToken)
        {
            var rpcClient = ClientFactory.GetClient("https://api.mainnet-beta.solana.com/");
            var tokenList = await rpcClient.GetProgramAccountsAsync(TokenProgram.ProgramIdKey);
            return new RecoverySaveNewsTokensCommandResponse { };
        }
    }
}
