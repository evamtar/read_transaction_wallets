using MediatR;
using SyncronizationBot.Application.Commands.Base;
using SyncronizationBot.Application.Response.SolanaFM;

namespace SyncronizationBot.Application.Commands.SolanaFM
{
    public class RecoveryTransactionsSignatureForAddressCommand : TransactionsCommand, IRequest<RecoveryTransactionsSignatureForAddressCommandResponse>
    {
        
    }
}
