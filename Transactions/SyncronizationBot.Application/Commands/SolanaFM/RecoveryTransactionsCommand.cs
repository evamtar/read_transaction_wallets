using MediatR;
using SyncronizationBot.Application.Commands.Base;
using SyncronizationBot.Application.Response.SolanaFM;

namespace SyncronizationBot.Application.Commands.SolanaFM
{
    public class RecoveryTransactionsCommand : TransactionsCommand, IRequest<RecoveryTransactionsCommandResponse>
    {
        
    }
}
