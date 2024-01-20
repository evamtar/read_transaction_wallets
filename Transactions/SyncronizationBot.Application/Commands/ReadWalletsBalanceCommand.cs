using MediatR;
using SyncronizationBot.Application.Response;

namespace SyncronizationBot.Application.Commands
{
    public class ReadWalletsBalanceCommand: IRequest<ReadWalletsBalanceCommandResponse>
    {
    }
}
