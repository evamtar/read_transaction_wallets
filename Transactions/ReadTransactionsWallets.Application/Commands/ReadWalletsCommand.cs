using MediatR;
using ReadTransactionsWallets.Application.Commands.Base;
using ReadTransactionsWallets.Application.Response;

namespace ReadTransactionsWallets.Application.Commands
{
    public class ReadWalletsCommand : SearchCommand, IRequest<ReadWalletsCommandResponse>
    {

    }
}
