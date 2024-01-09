using MediatR;
using ReadTransactionsWallets.Application.Response;

namespace ReadTransactionsWallets.Application.Commands
{
    public class ReadWalletsBalanceCommand: IRequest<ReadWalletsBalanceCommandResponse>
    {
    }
}
