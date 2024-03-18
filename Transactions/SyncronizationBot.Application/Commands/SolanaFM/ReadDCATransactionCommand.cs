using MediatR;
using SyncronizationBot.Application.Response.SolanaFM;

namespace SyncronizationBot.Application.Commands.SolanaFM
{
    public class ReadDCATransactionCommand : IRequest<ReadDCATransactionCommandResponse>
    {
    }
}
