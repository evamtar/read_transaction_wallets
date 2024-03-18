using MediatR;
using SyncronizationBot.Application.Commands.Base;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;


namespace SyncronizationBot.Application.Commands.MainCommands.RecoverySave
{
    public class RecoverySaveTransactionsCommand : SearchCommand, IRequest<RecoverySaveTransactionsCommandResponse>
    {
        
    }
}
