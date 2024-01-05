using MediatR;
using ReadTransactionsWallets.Application.Response;

namespace ReadTransactionsWallets.Application.Commands
{
    public class ReadWalletsCommand : IRequest<ReadWalletsCommandResponse>
    {
        public decimal InitialTicks { get; set; }
        public decimal FinalTicks { get; set; }
    }
}
